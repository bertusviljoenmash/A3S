/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using za.co.grindrodbank.a3sidentityserver.Extensions;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using za.co.grindrodbank.a3s.Managers;
using za.co.grindrodbank.a3s.Models;
using za.co.grindrodbank.a3sidentityserver.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using za.co.grindrodbank.a3s.Repositories;
using za.co.grindrodbank.a3s.Services;
using za.co.grindrodbank.a3s.Stores;
using za.co.grindrodbank.a3s.Helpers;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using Microsoft.AspNetCore.Http;
using za.co.grindrodbank.a3s.ConnectionClients;
using Microsoft.AspNetCore.HttpOverrides;

namespace za.co.grindrodbank.a3sidentityserver
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        private const string CONFIG_SCHEMA = "_ids4";

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<A3SContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<UserModel, IdentityRole>()
                .AddEntityFrameworkStores<A3SContext>()
                .AddUserManager<CustomUserManager>()
                .AddUserStore<CustomUserStore>()
                .AddDefaultTokenProviders();

            // Configure cookie policy to cater for older user agents that do not support the new SameSite cookie property functionality
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });

            // Register own SignInManager to handle Just-In-Time LDAP Auth
            services.AddScoped<SignInManager<UserModel>, CustomSignInManager<UserModel>>();

            services.AddControllersWithViews()
               .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                   options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
               })
               .AddRazorRuntimeCompilation();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // Configure the Identity Server to run behind a reverse proxy if configured.
            if (Configuration.GetValue<bool>("RunningBehindReverseProxy"))
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders =
                        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                });
            }

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.AccessTokenJwtType = "JWT";
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = configurationBuilder =>
                    configurationBuilder.UseNpgsql(
                        Configuration.GetConnectionString("DefaultConnection")
                        );

                options.DefaultSchema = CONFIG_SCHEMA;
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = configurationBuilder =>
                    configurationBuilder.UseNpgsql(
                        Configuration.GetConnectionString("DefaultConnection")
                        );

                options.DefaultSchema = CONFIG_SCHEMA;
            })
            .AddAspNetIdentity<UserModel>()
            .AddProfileService<IdentityWithAdditionalClaimsProfileService>()
            .LoadSigningCredentialFrom(Configuration["certificates:signing"], Configuration["certificates:signingPassword"], Environment);

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILdapAuthenticationModeRepository, LdapAuthenticationModeRepository>();
            services.AddScoped<ITermsOfServiceRepository, TermsOfServiceRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IApplicationDataPolicyRepository, ApplicationDataPolicyRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();

            // Register services
            services.AddScoped<ILdapConnectionService, LdapConnectionService>();
            services.AddScoped<ISafeRandomizerService, SafeRandomizerService>();

            // Register Helpers
            services.AddScoped<IArchiveHelper, ArchiveHelper>();

            // Register Connection Clients
            services.AddScoped<ILdapConnectionClient, LdapConnectionClient>();
        }

        public void Configure(IApplicationBuilder app)
        {
            // Configure cookie policy to cater for older user agents that do not support the new SameSite cookie property functionality
            app.UseCookiePolicy();

            // Configure the Identity Server to run behind a reverse proxy if configured.
            if (Configuration.GetValue<bool>("RunningBehindReverseProxy"))
            {
                app.UseForwardedHeaders();
            }

            // Configure the Identity Server to use HTTPS schema as it is running behind a TLS terminated Ingress.
            if (Configuration.GetValue<bool>("RunningBehindTlsTermintedIngress"))
            {
                app.Use((context, next) =>
                {
                    context.Request.Scheme = "https";
                    return next();
                });
            }

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // Initialiaze the configuration database and seed entries from the Config file.
                InitializeConfigurationDatabase(app);

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
            });
        }

        private void InitializeConfigurationDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApis())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }

        private void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

                if (DisallowsSameSiteNone(userAgent, httpContext.Request.IsHttps))
                    options.SameSite = SameSiteMode.Unspecified;

                if (SaveCookiesAsSecure(userAgent, httpContext.Request.IsHttps))
                    options.Secure = true;
            }
        }

        private bool DisallowsSameSiteNone(string userAgent, bool isHttps)
        {
            // Cover all iOS based browsers here. This includes:
            // - Safari on iOS 12 for iPhone, iPod Touch, iPad
            // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the iOS networking stack
            if (userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12"))
                return true;

            // Cover Mac OS X based browsers that use the Mac OS networking stack. This includes:
            // - Safari on Mac OS X.
            // This does not include:
            // - Chrome on Mac OS X
            // Because they do not use the Mac OS networking stack.
            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
                userAgent.Contains("Version/") && userAgent.Contains("Safari"))
                return true;

            // Cover Chrome 50-69, because some versions are broken by SameSite=None, 
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions, 
            // but pre-Chromium Edge does not require SameSite=None.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
                return true;

            // Cover Chrome 80+ with http only, to cater for quickstarts running A3S in http only mode.
            if (userAgent.Contains("Chrome/"))
            {
                Version agentVersion = GetAgentVersion(userAgent);

                if (agentVersion.Major >= 80 && !isHttps)
                    return true;
            }

            return false;
        }

        private bool SaveCookiesAsSecure(string userAgent, bool isHttps)
        {
            if (!isHttps)
                return false;

            Version agentVersion = GetAgentVersion(userAgent);

            if (agentVersion == null)
                return false;

            // Cover Chrome 80, where SameSite=None must now be set with Secure.
            if (userAgent.Contains("Chrome/") && agentVersion.Major >= 80)
                return true;
            
            return false;
        }

        private Version GetAgentVersion(string userAgent)
        {
            string agentName = string.Empty;

            if (userAgent.Contains("Chrome"))
                agentName = "Chrome";

            int versionTextStart = userAgent.IndexOf($"{agentName}/") + agentName.Length + 1;
            int versionTextEnd = userAgent.IndexOf(' ', versionTextStart);
            if (versionTextEnd == -1)
                versionTextEnd = userAgent.Length - 1;

            string versionText = userAgent.Substring(versionTextStart, (versionTextEnd - versionTextStart));

            if (versionText.Length > 0)
            {
                if (Version.TryParse(versionText, out Version agentVersion))
                    return agentVersion;
                else
                    return null;
            }

            return null;
        }
    }
}

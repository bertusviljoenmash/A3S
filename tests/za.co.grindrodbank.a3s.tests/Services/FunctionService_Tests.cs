/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using za.co.grindrodbank.a3s.MappingProfiles;
using za.co.grindrodbank.a3s.Models;
using za.co.grindrodbank.a3s.Repositories;
using za.co.grindrodbank.a3s.Services;
using AutoMapper;
using NSubstitute;
using Xunit;
using za.co.grindrodbank.a3s.A3SApiResources;
using za.co.grindrodbank.a3s.Exceptions;

namespace za.co.grindrodbank.a3s.tests.Services
{
    public class FunctionService_Tests
    {
        private readonly IMapper mapper;
        private readonly FunctionModel mockedFunctionModel;
        private readonly Guid guid;
        private readonly FunctionSubmit mockedFunctionSubmitModel;

        private readonly IFunctionRepository mockedFunctionRepository;
        private readonly IPermissionRepository mockedPermissionRepository;
        private readonly IApplicationRepository mockedApplicationRepository;
        private readonly ISubRealmRepository mockedSubRealmRepository;
        private readonly IFunctionTransientRepository mockedFunctionTransientRepository;

        public FunctionService_Tests()
        {
            mockedFunctionRepository = Substitute.For<IFunctionRepository>();
            mockedPermissionRepository = Substitute.For<IPermissionRepository>();
            mockedApplicationRepository = Substitute.For<IApplicationRepository>();
            mockedSubRealmRepository = Substitute.For<ISubRealmRepository>();
            mockedFunctionTransientRepository = Substitute.For<IFunctionTransientRepository>();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FunctionResourceFunctionModelProfile());
                cfg.AddProfile(new PermissionResourcePermisionModelProfile());
                cfg.AddProfile(new ApplicationResourceApplicationModelProfile());
            });

            mapper = config.CreateMapper();

            guid = Guid.NewGuid();
            var applicationGuid = Guid.NewGuid();
            var permissionsGuid = Guid.NewGuid();

            mockedFunctionModel = new FunctionModel();
            mockedFunctionModel.Id = guid;
            mockedFunctionModel.Name = "Test function name";
            mockedFunctionModel.Description = "Test description";
            mockedFunctionModel.Application = new ApplicationModel
            {
                Name = "Test Application",
                Id = applicationGuid
            };

            mockedFunctionModel.FunctionPermissions = new List<FunctionPermissionModel>
            {
                new FunctionPermissionModel
                {
                    Function = mockedFunctionModel,
                    Permission = new PermissionModel
                    {
                        Name = "Test permission",
                        Description = "Test permissions description",
                        Id = permissionsGuid,
                        ApplicationFunctionPermissions = new List<ApplicationFunctionPermissionModel>()
                        {
                            new ApplicationFunctionPermissionModel()
                            {
                                ApplicationFunctionId = mockedFunctionModel.Application.Id,
                                PermissionId = permissionsGuid,
                                ApplicationFunction = new ApplicationFunctionModel()
                                {
                                    Application = mockedFunctionModel.Application
                                }
                            }
                        }
                    }
                }
            };

            mockedFunctionSubmitModel = new FunctionSubmit()
            {
                Uuid = mockedFunctionModel.Id,
                Name = mockedFunctionModel.Name,
                ApplicationId = mockedFunctionModel.Application.Id,
                Permissions = new List<Guid>()
            };

            foreach (var permission in mockedFunctionModel.FunctionPermissions)
                mockedFunctionSubmitModel.Permissions.Add(permission.PermissionId);
        }

        [Fact]
        public async Task GetById_GivenGuid_ReturnsFunctionResource()
        {
            mockedFunctionRepository.GetByIdAsync(guid).Returns(mockedFunctionModel);

            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);
            var functionResource = await functionService.GetByIdAsync(guid);

            Assert.NotNull(functionResource);
            Assert.True(functionResource.Name == "Test function name", $"Function Resource name: '{functionResource.Name}' not the expected value: 'Test Function name'");
            Assert.True(functionResource.Uuid == guid, $"Function Resource UUId: '{functionResource.Uuid}' not the expected value: '{guid}'");
        }

        [Fact]
        public async Task CreateAsync_GivenUnfindableApplication_ThrowsItemNotFoundException()
        {
            mockedPermissionRepository.GetByIdWithApplicationAsync(mockedFunctionModel.FunctionPermissions[0].PermissionId)
                .Returns(mockedFunctionModel.FunctionPermissions[0].Permission);

            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);

            // Act
            Exception caughEx = null;

            try
            {
                var functionResource = await functionService.CreateAsync(mockedFunctionSubmitModel, Guid.NewGuid());
            }
            catch (Exception ex)
            {
                caughEx = ex;
            }

            // Assert
            Assert.True(caughEx is ItemNotFoundException, "Unfindable Applications must throw an ItemNotFoundException");
        }

        [Fact]
        public async Task CreateAsync_GivenUnfindablePermission_ThrowsItemNotFoundException()
        {
            // Arrange
            mockedApplicationRepository.GetByIdAsync(mockedFunctionModel.Application.Id)
                .Returns(mockedFunctionModel.Application);

            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);

            // Act
            Exception caughEx = null;

            try
            {
                var functionResource = await functionService.CreateAsync(mockedFunctionSubmitModel, Guid.NewGuid());
            }
            catch (Exception ex)
            {
                caughEx = ex;
            }

            // Assert
            Assert.True(caughEx is ItemNotFoundException, "Unfindable Permissions must throw an ItemNotFoundException.");
        }

        [Fact]
        public async Task CreateAsync_GivenUnlinkedPermissionAndApplication_ThrowsItemNotProcessableException()
        {
            // Arrange

            // Change ApplicationId to break link between the permission and Application
            mockedFunctionModel.FunctionPermissions[0].Permission.ApplicationFunctionPermissions[0].ApplicationFunction.Application.Id = Guid.NewGuid();

            mockedApplicationRepository.GetByIdAsync(Arg.Any<Guid>())
                .Returns(mockedFunctionModel.Application);
            mockedPermissionRepository.GetByIdWithApplicationAsync(mockedFunctionModel.FunctionPermissions[0].PermissionId)
                .Returns(mockedFunctionModel.FunctionPermissions[0].Permission);
            mockedFunctionRepository.CreateAsync(Arg.Any<FunctionModel>()).Returns(mockedFunctionModel);

            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);

            // Act
            Exception caughEx = null;
            try
            {
                var functionResource = await functionService.CreateAsync(mockedFunctionSubmitModel, Guid.NewGuid());
            }
            catch (Exception ex)
            {
                caughEx = ex;
            }

            // Assert
            Assert.True(caughEx is ItemNotProcessableException, "Unlinked permissions and applications must throw an ItemNotProcessableException.");
        }

        [Fact]
        public async Task CreateAsync_GivenFullProcessableModel_ReturnsCreatedModel()
        {
            // Arrange
            mockedApplicationRepository.GetByIdAsync(mockedFunctionModel.Application.Id)
                .Returns(mockedFunctionModel.Application);
            mockedPermissionRepository.GetByIdWithApplicationAsync(mockedFunctionModel.FunctionPermissions[0].PermissionId)
                .Returns(mockedFunctionModel.FunctionPermissions[0].Permission);
            mockedFunctionRepository.CreateAsync(Arg.Any<FunctionModel>()).Returns(mockedFunctionModel);

            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);

            // Act
            var functionResource = await functionService.CreateAsync(mockedFunctionSubmitModel, Guid.NewGuid());

            // Assert
            Assert.NotNull(functionResource);
            Assert.True(functionResource.Name == mockedFunctionSubmitModel.Name, $"Function Resource name: '{functionResource.Name}' not the expected value: '{mockedFunctionSubmitModel.Name}'");
            Assert.True(functionResource.Application.Uuid == mockedFunctionSubmitModel.ApplicationId, $"Function Resource name: '{functionResource.Application.Uuid}' not the expected value: '{mockedFunctionSubmitModel.ApplicationId}'");
            Assert.True(functionResource.Permissions.Count == mockedFunctionSubmitModel.Permissions.Count, $"Function Resource Permission Count: '{functionResource.Permissions.Count}' not the expected value: '{mockedFunctionSubmitModel.Permissions.Count}'");
        }

        [Fact]
        public async Task CreateAsync_GivenAlreadyUsedName_ThrowsItemNotProcessableException()
        {
            mockedApplicationRepository.GetByIdAsync(mockedFunctionModel.Application.Id)
                .Returns(mockedFunctionModel.Application);
            mockedPermissionRepository.GetByIdWithApplicationAsync(mockedFunctionModel.FunctionPermissions[0].PermissionId)
                .Returns(mockedFunctionModel.FunctionPermissions[0].Permission);
            mockedFunctionRepository.GetByIdAsync(mockedFunctionModel.Id).Returns(mockedFunctionModel);
            mockedFunctionRepository.GetByNameAsync(mockedFunctionSubmitModel.Name).Returns(mockedFunctionModel);
            mockedFunctionRepository.CreateAsync(Arg.Any<FunctionModel>()).Returns(mockedFunctionModel);

            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);

            // Act
            Exception caughEx = null;
            try
            {
                var functionResource = await functionService.CreateAsync(mockedFunctionSubmitModel, Guid.NewGuid());
            }
            catch (Exception ex)
            {
                caughEx = ex;
            }

            // Assert
            Assert.True(caughEx is ItemNotProcessableException, "Attempted create with an already used name must throw an ItemNotProcessableException.");
        }

        [Fact]
        public async Task GetListAsync_Executed_ReturnsList()
        {
            // Arrange

            mockedFunctionRepository.GetListAsync().Returns(
                new List<FunctionModel>()
                {
                    mockedFunctionModel,
                    mockedFunctionModel
                });

            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);

            // Act
            var functionList = await functionService.GetListAsync();

            // Assert
            Assert.True(functionList.Count == 2, "Expected list count is 2");
            Assert.True(functionList[0].Name == mockedFunctionModel.Name, $"Expected applicationFunction name: '{functionList[0].Name}' does not equal expected value: '{mockedFunctionModel.Name}'");
            Assert.True(functionList[0].Uuid == mockedFunctionModel.Id, $"Expected applicationFunction UUID: '{functionList[0].Uuid}' does not equal expected value: '{mockedFunctionModel.Id}'");
        }

        [Fact]
        public async Task UpdateAsync_GivenFullProcessableModel_ReturnsUpdatedModel()
        {
            // Arrange
            mockedApplicationRepository.GetByIdAsync(mockedFunctionModel.Application.Id)
                .Returns(mockedFunctionModel.Application);
            mockedPermissionRepository.GetByIdWithApplicationAsync(mockedFunctionModel.FunctionPermissions[0].PermissionId)
                .Returns(mockedFunctionModel.FunctionPermissions[0].Permission);
            mockedFunctionRepository.GetByIdAsync(mockedFunctionModel.Id).Returns(mockedFunctionModel);
            mockedFunctionRepository.UpdateAsync(Arg.Any<FunctionModel>()).Returns(mockedFunctionModel);

            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);

            // Act
            var functionResource = await functionService.UpdateAsync(mockedFunctionSubmitModel, Guid.NewGuid());

            // Assert
            Assert.NotNull(functionResource);
            Assert.True(functionResource.Name == mockedFunctionSubmitModel.Name, $"Function Resource name: '{functionResource.Name}' not the expected value: '{mockedFunctionSubmitModel.Name}'");
            Assert.True(functionResource.Application.Uuid == mockedFunctionSubmitModel.ApplicationId, $"Function Resource name: '{functionResource.Application.Uuid}' not the expected value: '{mockedFunctionSubmitModel.ApplicationId}'");
            Assert.True(functionResource.Permissions.Count == mockedFunctionSubmitModel.Permissions.Count, $"Function Resource Permission Count: '{functionResource.Permissions.Count}' not the expected value: '{mockedFunctionSubmitModel.Permissions.Count}'");
        }

        [Fact]
        public async Task UpdateAsync_GivenUnfindableFunction_ThrowsItemNotFoundException()
        {
            // Arrange
            mockedApplicationRepository.GetByIdAsync(mockedFunctionModel.Application.Id)
                .Returns(mockedFunctionModel.Application);
            mockedPermissionRepository.GetByIdWithApplicationAsync(mockedFunctionModel.FunctionPermissions[0].PermissionId)
                .Returns(mockedFunctionModel.FunctionPermissions[0].Permission);
            mockedFunctionRepository.UpdateAsync(Arg.Any<FunctionModel>()).Returns(mockedFunctionModel);

            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);

            // Act
            Exception caughEx = null;
            try
            {
                var functionResource = await functionService.UpdateAsync(mockedFunctionSubmitModel, Guid.NewGuid());
            }
            catch (Exception ex)
            {
                caughEx = ex;
            }

            // Assert
            Assert.True(caughEx is ItemNotFoundException, "Unfindable functions must throw an ItemNotFoundException");
        }

        [Fact]
        public async Task UpdateAsync_GivenNewTakenName_ThrowsItemNotProcessableException()
        {
            // Arrange
            mockedFunctionSubmitModel.Name += "_changed_name";

            mockedApplicationRepository.GetByIdAsync(mockedFunctionModel.Application.Id)
                .Returns(mockedFunctionModel.Application);
            mockedPermissionRepository.GetByIdWithApplicationAsync(mockedFunctionModel.FunctionPermissions[0].PermissionId)
                .Returns(mockedFunctionModel.FunctionPermissions[0].Permission);
            mockedFunctionRepository.GetByIdAsync(mockedFunctionModel.Id).Returns(mockedFunctionModel);
            mockedFunctionRepository.GetByNameAsync(mockedFunctionSubmitModel.Name).Returns(mockedFunctionModel);
            mockedFunctionRepository.UpdateAsync(Arg.Any<FunctionModel>()).Returns(mockedFunctionModel);

            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);

            // Act
            Exception caughEx = null;
            try
            {
                var functionResource = await functionService.UpdateAsync(mockedFunctionSubmitModel, Guid.NewGuid());
            }
            catch (Exception ex)
            {
                caughEx = ex;
            }

            // Assert
            Assert.True(caughEx is ItemNotProcessableException, "New taken name must throw an ItemNotProcessableException");
        }

        [Fact]
        public async Task UpdateAsync_GivenNewUntakenName_ReturnsUpdatedFunction()
        {
            // Arrange
            mockedFunctionSubmitModel.Name += "_changed_name";

            mockedApplicationRepository.GetByIdAsync(mockedFunctionModel.Application.Id)
                .Returns(mockedFunctionModel.Application);
            mockedPermissionRepository.GetByIdWithApplicationAsync(mockedFunctionModel.FunctionPermissions[0].PermissionId)
                .Returns(mockedFunctionModel.FunctionPermissions[0].Permission);
            mockedFunctionRepository.GetByIdAsync(mockedFunctionModel.Id).Returns(mockedFunctionModel);
            mockedFunctionRepository.UpdateAsync(Arg.Any<FunctionModel>()).Returns(mockedFunctionModel);

            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);

            // Act
            var functionResource = await functionService.UpdateAsync(mockedFunctionSubmitModel, Guid.NewGuid());

            // Assert
            Assert.NotNull(functionResource);
            Assert.True(functionResource.Name == mockedFunctionSubmitModel.Name, $"Function Resource name: '{functionResource.Name}' not the expected value: '{mockedFunctionSubmitModel.Name}'");
            Assert.True(functionResource.Application.Uuid == mockedFunctionSubmitModel.ApplicationId, $"Function Resource name: '{functionResource.Application.Uuid}' not the expected value: '{mockedFunctionSubmitModel.ApplicationId}'");
            Assert.True(functionResource.Permissions.Count == mockedFunctionSubmitModel.Permissions.Count, $"Function Resource Permission Count: '{functionResource.Permissions.Count}' not the expected value: '{mockedFunctionSubmitModel.Permissions.Count}'");
        }

        [Fact]
        public async Task DeleteAsync_GivenFindableGuid_ExecutesSuccessfully()
        {
            // Arrange
            mockedFunctionRepository.GetByIdAsync(mockedFunctionModel.Id).Returns(mockedFunctionModel);

            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);

            // Act
            Exception caughEx = null;
            try
            {
                await functionService.DeleteAsync(mockedFunctionModel.Id);
            }
            catch (Exception ex)
            {
                caughEx = ex;
            }

            // Assert
            Assert.True(caughEx is null, "Delete on a findable GUID must execute successfully.");
        }

        [Fact]
        public async Task DeleteAsync_GivenUnfindableGuid_ThrowsItemNotFoundException()
        {
            // Arrange
            var functionService = new FunctionService(mockedFunctionRepository, mockedPermissionRepository, mockedApplicationRepository, mockedFunctionTransientRepository, mockedSubRealmRepository, mapper);

            // Act
            Exception caughEx = null;
            try
            {
                await functionService.DeleteAsync(mockedFunctionModel.Id);
            }
            catch (Exception ex)
            {
                caughEx = ex;
            }

            // Assert
            Assert.True(caughEx is ItemNotFoundException, "Delete on a findable GUID must throw an ItemNotFoundException.");
        }
    }
}

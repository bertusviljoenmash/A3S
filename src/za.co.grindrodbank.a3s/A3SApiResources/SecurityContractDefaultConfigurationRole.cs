/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
/*
 * A3S
 *
 * API Definition for A3S. This service allows authentication, authorisation and accounting.
 *
 * The version of the OpenAPI document: 1.1.4
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using za.co.grindrodbank.a3s.Converters;

namespace za.co.grindrodbank.a3s.A3SApiResources
{ 
    /// <summary>
    /// Models a role definition within the roles section of the default configuration. 
    /// </summary>
    [DataContract]
    public partial class SecurityContractDefaultConfigurationRole : IEquatable<SecurityContractDefaultConfigurationRole>
    { 
        /// <summary>
        /// The name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        [Required]
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// A description of the role.
        /// </summary>
        /// <value>A description of the role.</value>
        [Required]
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// An array of all the function names that are to be added to the role. The functions must already exist or be defined in other sections of the security contract.
        /// </summary>
        /// <value>An array of all the function names that are to be added to the role. The functions must already exist or be defined in other sections of the security contract.</value>
        [DataMember(Name="functions", EmitDefaultValue=false)]
        public List<string> Functions { get; set; }

        /// <summary>
        /// An array of all the child roles that are to be added to the role. The roles must already exist or be defined in other sections of the security contract.
        /// </summary>
        /// <value>An array of all the child roles that are to be added to the role. The roles must already exist or be defined in other sections of the security contract.</value>
        [DataMember(Name="roles", EmitDefaultValue=false)]
        public List<string> Roles { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SecurityContractDefaultConfigurationRole {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Functions: ").Append(Functions).Append("\n");
            sb.Append("  Roles: ").Append(Roles).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SecurityContractDefaultConfigurationRole)obj);
        }

        /// <summary>
        /// Returns true if SecurityContractDefaultConfigurationRole instances are equal
        /// </summary>
        /// <param name="other">Instance of SecurityContractDefaultConfigurationRole to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SecurityContractDefaultConfigurationRole other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) && 
                (
                    Description == other.Description ||
                    Description != null &&
                    Description.Equals(other.Description)
                ) && 
                (
                    Functions == other.Functions ||
                    Functions != null &&
                    other.Functions != null &&
                    Functions.SequenceEqual(other.Functions)
                ) && 
                (
                    Roles == other.Roles ||
                    Roles != null &&
                    other.Roles != null &&
                    Roles.SequenceEqual(other.Roles)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Name != null)
                    hashCode = hashCode * 59 + Name.GetHashCode();
                    if (Description != null)
                    hashCode = hashCode * 59 + Description.GetHashCode();
                    if (Functions != null)
                    hashCode = hashCode * 59 + Functions.GetHashCode();
                    if (Roles != null)
                    hashCode = hashCode * 59 + Roles.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(SecurityContractDefaultConfigurationRole left, SecurityContractDefaultConfigurationRole right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SecurityContractDefaultConfigurationRole left, SecurityContractDefaultConfigurationRole right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}

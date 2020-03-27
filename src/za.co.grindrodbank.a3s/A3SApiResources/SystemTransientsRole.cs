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
    /// Represents a collection of all the latest transients for roles with transie. 
    /// </summary>
    [DataContract]
    public partial class SystemTransientsRole : IEquatable<SystemTransientsRole>
    { 
        /// <summary>
        /// Gets or Sets Uuid
        /// </summary>
        [DataMember(Name="uuid", EmitDefaultValue=false)]
        public Guid Uuid { get; set; }

        /// <summary>
        /// Gets or Sets RoleName
        /// </summary>
        [DataMember(Name="roleName", EmitDefaultValue=false)]
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or Sets CapturerUuid
        /// </summary>
        [DataMember(Name="capturerUuid", EmitDefaultValue=false)]
        public Guid CapturerUuid { get; set; }

        /// <summary>
        /// Gets or Sets CapturerName
        /// </summary>
        [DataMember(Name="capturerName", EmitDefaultValue=false)]
        public string CapturerName { get; set; }

        /// <summary>
        /// Gets or Sets LatestActiveRoleTransient
        /// </summary>
        [DataMember(Name="latestActiveRoleTransient", EmitDefaultValue=false)]
        public RoleTransientsItem LatestActiveRoleTransient { get; set; }

        /// <summary>
        /// A list of the latest transient role function assignments for this transient role.
        /// </summary>
        /// <value>A list of the latest transient role function assignments for this transient role.</value>
        [DataMember(Name="latestTransientRoleFunctions", EmitDefaultValue=false)]
        public List<RoleFunctionTransient> LatestTransientRoleFunctions { get; set; }

        /// <summary>
        /// A list of the latest transient role - child role assignments for this transient role.
        /// </summary>
        /// <value>A list of the latest transient role - child role assignments for this transient role.</value>
        [DataMember(Name="latestTransientRoleChildRoles", EmitDefaultValue=false)]
        public List<RoleChildRoleTransient> LatestTransientRoleChildRoles { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SystemTransientsRole {\n");
            sb.Append("  Uuid: ").Append(Uuid).Append("\n");
            sb.Append("  RoleName: ").Append(RoleName).Append("\n");
            sb.Append("  CapturerUuid: ").Append(CapturerUuid).Append("\n");
            sb.Append("  CapturerName: ").Append(CapturerName).Append("\n");
            sb.Append("  LatestActiveRoleTransient: ").Append(LatestActiveRoleTransient).Append("\n");
            sb.Append("  LatestTransientRoleFunctions: ").Append(LatestTransientRoleFunctions).Append("\n");
            sb.Append("  LatestTransientRoleChildRoles: ").Append(LatestTransientRoleChildRoles).Append("\n");
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
            return obj.GetType() == GetType() && Equals((SystemTransientsRole)obj);
        }

        /// <summary>
        /// Returns true if SystemTransientsRole instances are equal
        /// </summary>
        /// <param name="other">Instance of SystemTransientsRole to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SystemTransientsRole other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Uuid == other.Uuid ||
                    Uuid != null &&
                    Uuid.Equals(other.Uuid)
                ) && 
                (
                    RoleName == other.RoleName ||
                    RoleName != null &&
                    RoleName.Equals(other.RoleName)
                ) && 
                (
                    CapturerUuid == other.CapturerUuid ||
                    CapturerUuid != null &&
                    CapturerUuid.Equals(other.CapturerUuid)
                ) && 
                (
                    CapturerName == other.CapturerName ||
                    CapturerName != null &&
                    CapturerName.Equals(other.CapturerName)
                ) && 
                (
                    LatestActiveRoleTransient == other.LatestActiveRoleTransient ||
                    LatestActiveRoleTransient != null &&
                    LatestActiveRoleTransient.Equals(other.LatestActiveRoleTransient)
                ) && 
                (
                    LatestTransientRoleFunctions == other.LatestTransientRoleFunctions ||
                    LatestTransientRoleFunctions != null &&
                    other.LatestTransientRoleFunctions != null &&
                    LatestTransientRoleFunctions.SequenceEqual(other.LatestTransientRoleFunctions)
                ) && 
                (
                    LatestTransientRoleChildRoles == other.LatestTransientRoleChildRoles ||
                    LatestTransientRoleChildRoles != null &&
                    other.LatestTransientRoleChildRoles != null &&
                    LatestTransientRoleChildRoles.SequenceEqual(other.LatestTransientRoleChildRoles)
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
                    if (Uuid != null)
                    hashCode = hashCode * 59 + Uuid.GetHashCode();
                    if (RoleName != null)
                    hashCode = hashCode * 59 + RoleName.GetHashCode();
                    if (CapturerUuid != null)
                    hashCode = hashCode * 59 + CapturerUuid.GetHashCode();
                    if (CapturerName != null)
                    hashCode = hashCode * 59 + CapturerName.GetHashCode();
                    if (LatestActiveRoleTransient != null)
                    hashCode = hashCode * 59 + LatestActiveRoleTransient.GetHashCode();
                    if (LatestTransientRoleFunctions != null)
                    hashCode = hashCode * 59 + LatestTransientRoleFunctions.GetHashCode();
                    if (LatestTransientRoleChildRoles != null)
                    hashCode = hashCode * 59 + LatestTransientRoleChildRoles.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(SystemTransientsRole left, SystemTransientsRole right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SystemTransientsRole left, SystemTransientsRole right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}

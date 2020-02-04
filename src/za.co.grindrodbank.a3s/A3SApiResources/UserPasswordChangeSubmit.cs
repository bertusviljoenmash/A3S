/**
 * *************************************************
 * Copyright (c) 2019, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
/*
 * A3S
 *
 * API Definition for A3S. This service allows authentication, authorisation and accounting.
 *
 * The version of the OpenAPI document: 1.1.0
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
    /// Used to change a user&#39;s password. 
    /// </summary>
    [DataContract]
    public partial class UserPasswordChangeSubmit : IEquatable<UserPasswordChangeSubmit>
    { 
        /// <summary>
        /// Gets or Sets Uuid
        /// </summary>
        [DataMember(Name="uuid", EmitDefaultValue=false)]
        public Guid Uuid { get; set; }

        /// <summary>
        /// Gets or Sets NewPassword
        /// </summary>
        [DataMember(Name="newPassword", EmitDefaultValue=false)]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or Sets NewPasswordConfirmed
        /// </summary>
        [DataMember(Name="newPasswordConfirmed", EmitDefaultValue=false)]
        public string NewPasswordConfirmed { get; set; }

        /// <summary>
        /// Gets or Sets OldPassword
        /// </summary>
        [DataMember(Name="oldPassword", EmitDefaultValue=false)]
        public string OldPassword { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UserPasswordChangeSubmit {\n");
            sb.Append("  Uuid: ").Append(Uuid).Append("\n");
            sb.Append("  NewPassword: ").Append(NewPassword).Append("\n");
            sb.Append("  NewPasswordConfirmed: ").Append(NewPasswordConfirmed).Append("\n");
            sb.Append("  OldPassword: ").Append(OldPassword).Append("\n");
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
            return obj.GetType() == GetType() && Equals((UserPasswordChangeSubmit)obj);
        }

        /// <summary>
        /// Returns true if UserPasswordChangeSubmit instances are equal
        /// </summary>
        /// <param name="other">Instance of UserPasswordChangeSubmit to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserPasswordChangeSubmit other)
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
                    NewPassword == other.NewPassword ||
                    NewPassword != null &&
                    NewPassword.Equals(other.NewPassword)
                ) && 
                (
                    NewPasswordConfirmed == other.NewPasswordConfirmed ||
                    NewPasswordConfirmed != null &&
                    NewPasswordConfirmed.Equals(other.NewPasswordConfirmed)
                ) && 
                (
                    OldPassword == other.OldPassword ||
                    OldPassword != null &&
                    OldPassword.Equals(other.OldPassword)
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
                    if (NewPassword != null)
                    hashCode = hashCode * 59 + NewPassword.GetHashCode();
                    if (NewPasswordConfirmed != null)
                    hashCode = hashCode * 59 + NewPasswordConfirmed.GetHashCode();
                    if (OldPassword != null)
                    hashCode = hashCode * 59 + OldPassword.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(UserPasswordChangeSubmit left, UserPasswordChangeSubmit right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserPasswordChangeSubmit left, UserPasswordChangeSubmit right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}

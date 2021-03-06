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
 * The version of the OpenAPI document: 1.1.6
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
    /// Models the result of a security contract dry run validation.
    /// </summary>
    [DataContract]
    public partial class SecurityContractValidationResult : IEquatable<SecurityContractValidationResult>
    { 
        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [Required]
        [DataMember(Name="message", EmitDefaultValue=false)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or Sets ValidationErrors
        /// </summary>
        [DataMember(Name="validationErrors", EmitDefaultValue=false)]
        public List<SecurityContractValidationError> ValidationErrors { get; set; }

        /// <summary>
        /// Gets or Sets ValidationWarnings
        /// </summary>
        [DataMember(Name="validationWarnings", EmitDefaultValue=false)]
        public List<SecurityContractValidationWarning> ValidationWarnings { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SecurityContractValidationResult {\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
            sb.Append("  ValidationErrors: ").Append(ValidationErrors).Append("\n");
            sb.Append("  ValidationWarnings: ").Append(ValidationWarnings).Append("\n");
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
            return obj.GetType() == GetType() && Equals((SecurityContractValidationResult)obj);
        }

        /// <summary>
        /// Returns true if SecurityContractValidationResult instances are equal
        /// </summary>
        /// <param name="other">Instance of SecurityContractValidationResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SecurityContractValidationResult other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Message == other.Message ||
                    Message != null &&
                    Message.Equals(other.Message)
                ) && 
                (
                    ValidationErrors == other.ValidationErrors ||
                    ValidationErrors != null &&
                    other.ValidationErrors != null &&
                    ValidationErrors.SequenceEqual(other.ValidationErrors)
                ) && 
                (
                    ValidationWarnings == other.ValidationWarnings ||
                    ValidationWarnings != null &&
                    other.ValidationWarnings != null &&
                    ValidationWarnings.SequenceEqual(other.ValidationWarnings)
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
                    if (Message != null)
                    hashCode = hashCode * 59 + Message.GetHashCode();
                    if (ValidationErrors != null)
                    hashCode = hashCode * 59 + ValidationErrors.GetHashCode();
                    if (ValidationWarnings != null)
                    hashCode = hashCode * 59 + ValidationWarnings.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(SecurityContractValidationResult left, SecurityContractValidationResult right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SecurityContractValidationResult left, SecurityContractValidationResult right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}

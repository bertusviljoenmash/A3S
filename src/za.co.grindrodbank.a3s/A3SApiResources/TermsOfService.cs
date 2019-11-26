/*
 * A3S
 *
 * API Definition for the A3S. This service allows authentication, authorisation and accounting.
 *
 * The version of the OpenAPI document: 1.0.2
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
    /// An agreement the needs to be agreed to by a user 
    /// </summary>
    [DataContract]
    public partial class TermsOfService : IEquatable<TermsOfService>
    { 
        /// <summary>
        /// Gets or Sets Uuid
        /// </summary>
        [DataMember(Name="uuid", EmitDefaultValue=false)]
        public Guid Uuid { get; set; }

        /// <summary>
        /// Gets or Sets AgreementName
        /// </summary>
        [DataMember(Name="agreementName", EmitDefaultValue=false)]
        public string AgreementName { get; set; }

        /// <summary>
        /// Gets or Sets Version
        /// </summary>
        [DataMember(Name="version", EmitDefaultValue=false)]
        public string Version { get; set; }

        /// <summary>
        /// Gets or Sets AgreementFileData
        /// </summary>
        [DataMember(Name="agreementFileData", EmitDefaultValue=false)]
        public string AgreementFileData { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TermsOfService {\n");
            sb.Append("  Uuid: ").Append(Uuid).Append("\n");
            sb.Append("  AgreementName: ").Append(AgreementName).Append("\n");
            sb.Append("  Version: ").Append(Version).Append("\n");
            sb.Append("  AgreementFileData: ").Append(AgreementFileData).Append("\n");
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
            return obj.GetType() == GetType() && Equals((TermsOfService)obj);
        }

        /// <summary>
        /// Returns true if TermsOfService instances are equal
        /// </summary>
        /// <param name="other">Instance of TermsOfService to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TermsOfService other)
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
                    AgreementName == other.AgreementName ||
                    AgreementName != null &&
                    AgreementName.Equals(other.AgreementName)
                ) && 
                (
                    Version == other.Version ||
                    Version != null &&
                    Version.Equals(other.Version)
                ) && 
                (
                    AgreementFileData == other.AgreementFileData ||
                    AgreementFileData != null &&
                    AgreementFileData.Equals(other.AgreementFileData)
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
                    if (AgreementName != null)
                    hashCode = hashCode * 59 + AgreementName.GetHashCode();
                    if (Version != null)
                    hashCode = hashCode * 59 + Version.GetHashCode();
                    if (AgreementFileData != null)
                    hashCode = hashCode * 59 + AgreementFileData.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(TermsOfService left, TermsOfService right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TermsOfService left, TermsOfService right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}

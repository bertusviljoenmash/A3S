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
 * The version of the OpenAPI document: 2.0.0-alpha-1.0.0
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
    /// Represents the latest transients (only the most recent transient record) for a terms of service agreement with any active transients. 
    /// </summary>
    [DataContract]
    public partial class SystemTransientsTermsOfService : IEquatable<SystemTransientsTermsOfService>
    { 
        /// <summary>
        /// Gets or Sets Uuid
        /// </summary>
        [DataMember(Name="uuid", EmitDefaultValue=false)]
        public Guid Uuid { get; set; }

        /// <summary>
        /// Gets or Sets TermsOfServiceName
        /// </summary>
        [DataMember(Name="TermsOfServiceName", EmitDefaultValue=false)]
        public string TermsOfServiceName { get; set; }

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
        /// Gets or Sets CapturedDate
        /// </summary>
        [DataMember(Name="capturedDate", EmitDefaultValue=false)]
        public DateTime CapturedDate { get; set; }

        /// <summary>
        /// Gets or Sets LatestActiveTermsOfServiceTransient
        /// </summary>
        [DataMember(Name="latestActiveTermsOfServiceTransient", EmitDefaultValue=false)]
        public TermsOfServiceTransient LatestActiveTermsOfServiceTransient { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SystemTransientsTermsOfService {\n");
            sb.Append("  Uuid: ").Append(Uuid).Append("\n");
            sb.Append("  TermsOfServiceName: ").Append(TermsOfServiceName).Append("\n");
            sb.Append("  CapturerUuid: ").Append(CapturerUuid).Append("\n");
            sb.Append("  CapturerName: ").Append(CapturerName).Append("\n");
            sb.Append("  CapturedDate: ").Append(CapturedDate).Append("\n");
            sb.Append("  LatestActiveTermsOfServiceTransient: ").Append(LatestActiveTermsOfServiceTransient).Append("\n");
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
            return obj.GetType() == GetType() && Equals((SystemTransientsTermsOfService)obj);
        }

        /// <summary>
        /// Returns true if SystemTransientsTermsOfService instances are equal
        /// </summary>
        /// <param name="other">Instance of SystemTransientsTermsOfService to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SystemTransientsTermsOfService other)
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
                    TermsOfServiceName == other.TermsOfServiceName ||
                    TermsOfServiceName != null &&
                    TermsOfServiceName.Equals(other.TermsOfServiceName)
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
                    CapturedDate == other.CapturedDate ||
                    CapturedDate != null &&
                    CapturedDate.Equals(other.CapturedDate)
                ) && 
                (
                    LatestActiveTermsOfServiceTransient == other.LatestActiveTermsOfServiceTransient ||
                    LatestActiveTermsOfServiceTransient != null &&
                    LatestActiveTermsOfServiceTransient.Equals(other.LatestActiveTermsOfServiceTransient)
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
                    if (TermsOfServiceName != null)
                    hashCode = hashCode * 59 + TermsOfServiceName.GetHashCode();
                    if (CapturerUuid != null)
                    hashCode = hashCode * 59 + CapturerUuid.GetHashCode();
                    if (CapturerName != null)
                    hashCode = hashCode * 59 + CapturerName.GetHashCode();
                    if (CapturedDate != null)
                    hashCode = hashCode * 59 + CapturedDate.GetHashCode();
                    if (LatestActiveTermsOfServiceTransient != null)
                    hashCode = hashCode * 59 + LatestActiveTermsOfServiceTransient.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(SystemTransientsTermsOfService left, SystemTransientsTermsOfService right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SystemTransientsTermsOfService left, SystemTransientsTermsOfService right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
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
 * The version of the OpenAPI document: 1.1.5
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
    /// Models an application&#39;s functions and the corresponding permissions. 
    /// </summary>
    [DataContract]
    public partial class SecurityContractApplication : IEquatable<SecurityContractApplication>
    { 
        /// <summary>
        /// The name of the application function.
        /// </summary>
        /// <value>The name of the application function.</value>
        [Required]
        [DataMember(Name="fullname", EmitDefaultValue=false)]
        public string Fullname { get; set; }

        /// <summary>
        /// Gets or Sets ApplicationFunctions
        /// </summary>
        [Required]
        [DataMember(Name="applicationFunctions", EmitDefaultValue=false)]
        public List<SecurityContractFunction> ApplicationFunctions { get; set; }

        /// <summary>
        /// Gets or Sets DataPolicies
        /// </summary>
        [DataMember(Name="dataPolicies", EmitDefaultValue=false)]
        public List<SecurityContractApplicationDataPolicy> DataPolicies { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SecurityContractApplication {\n");
            sb.Append("  Fullname: ").Append(Fullname).Append("\n");
            sb.Append("  ApplicationFunctions: ").Append(ApplicationFunctions).Append("\n");
            sb.Append("  DataPolicies: ").Append(DataPolicies).Append("\n");
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
            return obj.GetType() == GetType() && Equals((SecurityContractApplication)obj);
        }

        /// <summary>
        /// Returns true if SecurityContractApplication instances are equal
        /// </summary>
        /// <param name="other">Instance of SecurityContractApplication to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SecurityContractApplication other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Fullname == other.Fullname ||
                    Fullname != null &&
                    Fullname.Equals(other.Fullname)
                ) && 
                (
                    ApplicationFunctions == other.ApplicationFunctions ||
                    ApplicationFunctions != null &&
                    other.ApplicationFunctions != null &&
                    ApplicationFunctions.SequenceEqual(other.ApplicationFunctions)
                ) && 
                (
                    DataPolicies == other.DataPolicies ||
                    DataPolicies != null &&
                    other.DataPolicies != null &&
                    DataPolicies.SequenceEqual(other.DataPolicies)
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
                    if (Fullname != null)
                    hashCode = hashCode * 59 + Fullname.GetHashCode();
                    if (ApplicationFunctions != null)
                    hashCode = hashCode * 59 + ApplicationFunctions.GetHashCode();
                    if (DataPolicies != null)
                    hashCode = hashCode * 59 + DataPolicies.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(SecurityContractApplication left, SecurityContractApplication right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SecurityContractApplication left, SecurityContractApplication right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}

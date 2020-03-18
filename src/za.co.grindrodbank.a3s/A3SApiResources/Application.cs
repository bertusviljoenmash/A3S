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
 * The version of the OpenAPI document: 1.1.3
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
    /// An application 
    /// </summary>
    [DataContract]
    public partial class Application : IEquatable<Application>
    { 
        /// <summary>
        /// The UUID of an application.
        /// </summary>
        /// <value>The UUID of an application.</value>
        [DataMember(Name="uuid", EmitDefaultValue=false)]
        public Guid Uuid { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets Functions
        /// </summary>
        [DataMember(Name="functions", EmitDefaultValue=false)]
        public List<Function> Functions { get; set; }

        /// <summary>
        /// Gets or Sets DataPolicies
        /// </summary>
        [DataMember(Name="dataPolicies", EmitDefaultValue=false)]
        public List<ApplicationDataPolicy> DataPolicies { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Application {\n");
            sb.Append("  Uuid: ").Append(Uuid).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Functions: ").Append(Functions).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Application)obj);
        }

        /// <summary>
        /// Returns true if Application instances are equal
        /// </summary>
        /// <param name="other">Instance of Application to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Application other)
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
                    if (Uuid != null)
                    hashCode = hashCode * 59 + Uuid.GetHashCode();
                    if (Name != null)
                    hashCode = hashCode * 59 + Name.GetHashCode();
                    if (Description != null)
                    hashCode = hashCode * 59 + Description.GetHashCode();
                    if (Functions != null)
                    hashCode = hashCode * 59 + Functions.GetHashCode();
                    if (DataPolicies != null)
                    hashCode = hashCode * 59 + DataPolicies.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Application left, Application right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Application left, Application right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}

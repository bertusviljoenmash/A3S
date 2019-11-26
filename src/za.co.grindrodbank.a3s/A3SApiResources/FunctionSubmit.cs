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
    /// Model used to create an new function or update an existing Function. 
    /// </summary>
    [DataContract]
    public partial class FunctionSubmit : IEquatable<FunctionSubmit>
    { 
        /// <summary>
        /// Gets or Sets Uuid
        /// </summary>
        [Required]
        [DataMember(Name="uuid", EmitDefaultValue=false)]
        public Guid Uuid { get; set; }

        /// <summary>
        /// The name of the function.
        /// </summary>
        /// <value>The name of the function.</value>
        [Required]
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// A brief description of the function.
        /// </summary>
        /// <value>A brief description of the function.</value>
        [Required]
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// The UUID of an application.
        /// </summary>
        /// <value>The UUID of an application.</value>
        [Required]
        [DataMember(Name="applicationId", EmitDefaultValue=false)]
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// A list of permission UUIDs that are to be added to the function.
        /// </summary>
        /// <value>A list of permission UUIDs that are to be added to the function.</value>
        [Required]
        [DataMember(Name="permissions", EmitDefaultValue=false)]
        public List<Guid> Permissions { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class FunctionSubmit {\n");
            sb.Append("  Uuid: ").Append(Uuid).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  ApplicationId: ").Append(ApplicationId).Append("\n");
            sb.Append("  Permissions: ").Append(Permissions).Append("\n");
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
            return obj.GetType() == GetType() && Equals((FunctionSubmit)obj);
        }

        /// <summary>
        /// Returns true if FunctionSubmit instances are equal
        /// </summary>
        /// <param name="other">Instance of FunctionSubmit to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(FunctionSubmit other)
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
                    ApplicationId == other.ApplicationId ||
                    ApplicationId != null &&
                    ApplicationId.Equals(other.ApplicationId)
                ) && 
                (
                    Permissions == other.Permissions ||
                    Permissions != null &&
                    other.Permissions != null &&
                    Permissions.SequenceEqual(other.Permissions)
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
                    if (ApplicationId != null)
                    hashCode = hashCode * 59 + ApplicationId.GetHashCode();
                    if (Permissions != null)
                    hashCode = hashCode * 59 + Permissions.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(FunctionSubmit left, FunctionSubmit right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FunctionSubmit left, FunctionSubmit right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}

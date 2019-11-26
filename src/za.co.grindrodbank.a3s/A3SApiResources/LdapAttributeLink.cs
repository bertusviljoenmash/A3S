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
    /// An LdapAttributeLink - A LDAP attribute link definition 
    /// </summary>
    [DataContract]
    public partial class LdapAttributeLink : IEquatable<LdapAttributeLink>
    { 
        /// <summary>
        /// Gets or Sets UserField
        /// </summary>
        [TypeConverter(typeof(CustomEnumConverter<UserFieldType>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum UserFieldType
        {
            
            /// <summary>
            /// Enum UserNameEnum for userName
            /// </summary>
            [EnumMember(Value = "userName")]
            UserNameEnum = 1,
            
            /// <summary>
            /// Enum FirstNameEnum for firstName
            /// </summary>
            [EnumMember(Value = "firstName")]
            FirstNameEnum = 2,
            
            /// <summary>
            /// Enum SurnameEnum for surname
            /// </summary>
            [EnumMember(Value = "surname")]
            SurnameEnum = 3,
            
            /// <summary>
            /// Enum EmailEnum for email
            /// </summary>
            [EnumMember(Value = "email")]
            EmailEnum = 4,
            
            /// <summary>
            /// Enum AvatarEnum for avatar
            /// </summary>
            [EnumMember(Value = "avatar")]
            AvatarEnum = 5
        }

        /// <summary>
        /// Gets or Sets UserField
        /// </summary>
        [DataMember(Name="userField", EmitDefaultValue=false)]
        public UserFieldType UserField { get; set; }

        /// <summary>
        /// Gets or Sets LdapField
        /// </summary>
        [DataMember(Name="ldapField", EmitDefaultValue=false)]
        public string LdapField { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class LdapAttributeLink {\n");
            sb.Append("  UserField: ").Append(UserField).Append("\n");
            sb.Append("  LdapField: ").Append(LdapField).Append("\n");
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
            return obj.GetType() == GetType() && Equals((LdapAttributeLink)obj);
        }

        /// <summary>
        /// Returns true if LdapAttributeLink instances are equal
        /// </summary>
        /// <param name="other">Instance of LdapAttributeLink to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(LdapAttributeLink other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    UserField == other.UserField ||
                    
                    UserField.Equals(other.UserField)
                ) && 
                (
                    LdapField == other.LdapField ||
                    LdapField != null &&
                    LdapField.Equals(other.LdapField)
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
                    
                    hashCode = hashCode * 59 + UserField.GetHashCode();
                    if (LdapField != null)
                    hashCode = hashCode * 59 + LdapField.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(LdapAttributeLink left, LdapAttributeLink right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LdapAttributeLink left, LdapAttributeLink right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}

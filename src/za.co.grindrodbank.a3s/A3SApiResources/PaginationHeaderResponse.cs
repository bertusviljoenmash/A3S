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
    /// This is the standard response for requests that have paginated collections
    /// </summary>
    [DataContract]
    public partial class PaginationHeaderResponse : IEquatable<PaginationHeaderResponse>
    { 
        /// <summary>
        /// The total number of pages in the result set
        /// </summary>
        /// <value>The total number of pages in the result set</value>
        [DataMember(Name="total", EmitDefaultValue=false)]
        public int Total { get; set; }

        /// <summary>
        /// The total number of results in the result set
        /// </summary>
        /// <value>The total number of results in the result set</value>
        [DataMember(Name="count", EmitDefaultValue=false)]
        public int Count { get; set; }

        /// <summary>
        /// The number items to include in a page of results. The page size for the result set
        /// </summary>
        /// <value>The number items to include in a page of results. The page size for the result set</value>
        [Range(1, 20)]
        [DataMember(Name="size", EmitDefaultValue=false)]
        public int Size { get; set; } = 10;

        /// <summary>
        /// The position of the page in the paged result set that is being returned
        /// </summary>
        /// <value>The position of the page in the paged result set that is being returned</value>
        [DataMember(Name="current", EmitDefaultValue=false)]
        public int Current { get; set; }

        /// <summary>
        /// The link to the first page of results containing [size] results. This link includes the page, size, filter, orderBy and fields query parameters.
        /// </summary>
        /// <value>The link to the first page of results containing [size] results. This link includes the page, size, filter, orderBy and fields query parameters.</value>
        [DataMember(Name="first", EmitDefaultValue=false)]
        public string First { get; set; }

        /// <summary>
        /// The link to the last page of results containing [size] results. This link includes the page, size, filter, orderBy and fields query parameters.
        /// </summary>
        /// <value>The link to the last page of results containing [size] results. This link includes the page, size, filter, orderBy and fields query parameters.</value>
        [DataMember(Name="last", EmitDefaultValue=false)]
        public string Last { get; set; }

        /// <summary>
        /// The link to the previous page of results containing [size] results. This link includes the page, size, filter, orderBy and fields query parameters. If this is the first page in the result set then this will be [null].
        /// </summary>
        /// <value>The link to the previous page of results containing [size] results. This link includes the page, size, filter, orderBy and fields query parameters. If this is the first page in the result set then this will be [null].</value>
        [DataMember(Name="prev", EmitDefaultValue=false)]
        public string Prev { get; set; }

        /// <summary>
        /// The link to the next page of results containing [size] results. This link includes the page, size, filter, orderBy and fields query parameters. If this is the last page in the result set then this will be [null].
        /// </summary>
        /// <value>The link to the next page of results containing [size] results. This link includes the page, size, filter, orderBy and fields query parameters. If this is the last page in the result set then this will be [null].</value>
        [DataMember(Name="next", EmitDefaultValue=false)]
        public string Next { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PaginationHeaderResponse {\n");
            sb.Append("  Total: ").Append(Total).Append("\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
            sb.Append("  Size: ").Append(Size).Append("\n");
            sb.Append("  Current: ").Append(Current).Append("\n");
            sb.Append("  First: ").Append(First).Append("\n");
            sb.Append("  Last: ").Append(Last).Append("\n");
            sb.Append("  Prev: ").Append(Prev).Append("\n");
            sb.Append("  Next: ").Append(Next).Append("\n");
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
            return obj.GetType() == GetType() && Equals((PaginationHeaderResponse)obj);
        }

        /// <summary>
        /// Returns true if PaginationHeaderResponse instances are equal
        /// </summary>
        /// <param name="other">Instance of PaginationHeaderResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PaginationHeaderResponse other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Total == other.Total ||
                    
                    Total.Equals(other.Total)
                ) && 
                (
                    Count == other.Count ||
                    
                    Count.Equals(other.Count)
                ) && 
                (
                    Size == other.Size ||
                    
                    Size.Equals(other.Size)
                ) && 
                (
                    Current == other.Current ||
                    
                    Current.Equals(other.Current)
                ) && 
                (
                    First == other.First ||
                    First != null &&
                    First.Equals(other.First)
                ) && 
                (
                    Last == other.Last ||
                    Last != null &&
                    Last.Equals(other.Last)
                ) && 
                (
                    Prev == other.Prev ||
                    Prev != null &&
                    Prev.Equals(other.Prev)
                ) && 
                (
                    Next == other.Next ||
                    Next != null &&
                    Next.Equals(other.Next)
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
                    
                    hashCode = hashCode * 59 + Total.GetHashCode();
                    
                    hashCode = hashCode * 59 + Count.GetHashCode();
                    
                    hashCode = hashCode * 59 + Size.GetHashCode();
                    
                    hashCode = hashCode * 59 + Current.GetHashCode();
                    if (First != null)
                    hashCode = hashCode * 59 + First.GetHashCode();
                    if (Last != null)
                    hashCode = hashCode * 59 + Last.GetHashCode();
                    if (Prev != null)
                    hashCode = hashCode * 59 + Prev.GetHashCode();
                    if (Next != null)
                    hashCode = hashCode * 59 + Next.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(PaginationHeaderResponse left, PaginationHeaderResponse right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PaginationHeaderResponse left, PaginationHeaderResponse right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}

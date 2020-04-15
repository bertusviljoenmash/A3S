/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using System;
using System.Runtime.Serialization;

namespace za.co.grindrodbank.a3s.Exceptions
{
    [Serializable]
    public sealed class EntityStateConflictException : Exception
    {
        private const string defaultMessage = "Entity state conflicts with another entity.";

        public EntityStateConflictException() : base(defaultMessage)
        {
        }

        public EntityStateConflictException(string message) : base(!string.IsNullOrEmpty(message) ? message : defaultMessage)
        {
        }

        public EntityStateConflictException(string message, Exception innerException) : base(!string.IsNullOrEmpty(message) ? message : defaultMessage, innerException)
        {
        }

        private EntityStateConflictException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }
    }
}

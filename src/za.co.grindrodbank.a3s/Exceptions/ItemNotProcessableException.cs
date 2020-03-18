/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.Runtime.Serialization;

namespace za.co.grindrodbank.a3s.Exceptions
{
    [Serializable]
    public sealed class ItemNotProcessableException : Exception
    {
        private const string defaultMessage = "Item not processable.";

        public ItemNotProcessableException() : base(defaultMessage)
        {
        }

        public ItemNotProcessableException(string message) : base(!string.IsNullOrEmpty(message) ? message : defaultMessage)
        {
        }

        public ItemNotProcessableException(string message, Exception innerException) : base(!string.IsNullOrEmpty(message) ? message : defaultMessage, innerException)
        {
        }

        private ItemNotProcessableException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }
    }
}

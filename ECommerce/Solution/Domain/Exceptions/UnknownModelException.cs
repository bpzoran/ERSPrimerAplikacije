﻿using System;

namespace Domain.Exceptions
{
    public class UnknownModelException : Exception
    {
        private static readonly string ERROR_MESSAGE = "Unknown model type";
        public UnknownModelException() : base(ERROR_MESSAGE)
        {
        }
    }
}

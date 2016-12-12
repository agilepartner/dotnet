using System;

namespace Structurizr.Client
{
    public class StructurizrClientException : Exception
    {

        public StructurizrClientException(String message) : base(message) { }

        public StructurizrClientException(String message, Exception innerException) : base(message, innerException) { }

    }
}

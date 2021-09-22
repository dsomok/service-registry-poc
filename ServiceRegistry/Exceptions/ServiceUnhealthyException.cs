using System;

namespace ServiceRegistry.Exceptions
{
    public class ServiceUnhealthyException : Exception
    {
        public ServiceUnhealthyException(string name) : base($"Service {name} is unhealthy")
        {
        }
    }
}

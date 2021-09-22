using System;

namespace ServiceRegistry.Exceptions
{
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(string name) : base($"Service {name} not found")
        {
        }
    }
}

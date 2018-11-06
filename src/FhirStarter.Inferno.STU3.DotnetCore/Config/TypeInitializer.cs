using System;

namespace FhirStarter.Inferno.STU3.DotnetCore.Config
{
    public class TypeInitializer
    {
        public bool MustBeInitialized { get; }
        public Type ServiceType { get; }
        public string Name { get; }

        public TypeInitializer(bool mustBeInitialized, Type serviceType, string name)
        {
            MustBeInitialized = mustBeInitialized;
            ServiceType = serviceType;
            Name = name;
        }
    }
}

using System;

namespace Structurizr.Analysis
{
    public class InterfaceImplementationTypeMatcher : ITypeMatcher
    {

        public Type InterfaceType { get; private set; }
        public string Description { get; private set; }
        public string Technology { get; private set; }

        public InterfaceImplementationTypeMatcher(Type interfaceType, string description, string technology)
        {
            this.InterfaceType = interfaceType;
            this.Description = description;
            this.Technology = technology;

            if (!InterfaceType.IsInterface)
            {
                throw new ArgumentException(InterfaceType.FullName + " is not an interface type");
            }
        }

        public bool Matches(Type type)
        {
            return InterfaceType.IsAssignableFrom(type);
        }

        public string GetDescription()
        {
            return this.Description;
        }

        public string GetTechnology()
        {
            return this.Technology;
        }

    }
}
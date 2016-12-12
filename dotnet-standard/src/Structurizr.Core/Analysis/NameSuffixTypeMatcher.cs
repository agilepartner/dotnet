using System;

namespace Structurizr.Analysis
{
    public class NameSuffixTypeMatcher : ITypeMatcher
    {

        public string Suffix { get; private set; }
        public string Description { get; private set; }
        public string Technology { get; private set; }

        public NameSuffixTypeMatcher(string suffix, string description, string technology)
        {
            this.Suffix = suffix;
            this.Description = description;
            this.Technology = technology;

            if (Suffix == null || Suffix.Trim().Length == 0)
            {
                throw new ArgumentException("A suffix must be supplied");
            }
        }

        public bool Matches(Type type)
        {
            return type.Name.EndsWith(Suffix);
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
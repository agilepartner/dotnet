using System;

namespace Structurizr.Analysis
{
    public interface ITypeMatcher
    {

        bool Matches(Type type);

        string GetDescription();

        string GetTechnology();

    }
}

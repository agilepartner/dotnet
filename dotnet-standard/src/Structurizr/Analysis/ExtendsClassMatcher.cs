﻿using System;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace Structurizr.Analysis
{
    public class ExtendsClassTypeMatcher : ITypeMatcher
    {

        public Type ClassType { get; private set; }
        public string Description { get; private set; }
        public string Technology { get; private set; }

        public ExtendsClassTypeMatcher(Type classType, string description, string technology)
        {
            this.ClassType = classType;
            this.Description = description;
            this.Technology = technology;

            if (!ClassType.GetTypeInfo().IsClass)
            {
                throw new ArgumentException(ClassType.FullName + " is not a class type");
            }
        }

        public bool Matches(Type type)
        {
            return type.GetTypeInfo().IsSubclassOf(ClassType);
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
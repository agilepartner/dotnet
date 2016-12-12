﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Structurizr.Analysis
{

    /// <summary>
    /// This is a component finder strategy that looks for components that are based upon a specific type.
    /// It uses the standard .NET reflection facility to find components.
    /// </summary>
    public class TypeBasedComponentFinderStrategy : ComponentFinderStrategy
    {
        private List<ITypeMatcher> typeMatchers = new List<ITypeMatcher>();

        public TypeBasedComponentFinderStrategy(params ITypeMatcher[] typeMatchers)
        {
            this.typeMatchers.AddRange(typeMatchers);
        }

        public override ICollection<Component> FindComponents()
        {
            List<Component> components = new List<Component>();

            //Review this part
            var callingAssemblyReferences = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            var assemblies = callingAssemblyReferences
                                        .Select(a => Assembly.Load(a))
                                        .Where(a => a.GetTypes().Any(t => InNamespace(t)))
                                        .Select(a => a.GetReferencedAssemblies())
                                        .SelectMany(a => a.Select(b => Assembly.Load(b)));

            var types = assemblies.SelectMany(a => a.GetTypes());

            if(!types.Any())
            {
                types = from a in Referencing.GetReferencingAssemblies(GetType().GetTypeInfo().Assembly.GetName().Name)
                        from t in a.GetTypes()
                        where InNamespace(t)
                        select t;
            }

            foreach (Type type in types)
            {
                foreach (ITypeMatcher typeMatcher in this.typeMatchers)
                {
                    if (typeMatcher.Matches(type))
                    {
                        Component component = ComponentFinder.Container.AddComponent(
                            type.Name,
                            type,
                            typeMatcher.GetDescription(),
                            typeMatcher.GetTechnology());

                        components.Add(component);
                    }
                }
            }

            return components;
        }

        public override void FindDependencies()
        {
            foreach (Component component in ComponentFinder.Container.Components)
            {
                if (component.Type != null)
                {
                    Type type = Type.GetType(component.Type);
                    foreach (PropertyInfo propertyInfo in type.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        string propertyType = propertyInfo.PropertyType.AssemblyQualifiedName;
                        Component c = ComponentFinder.Container.GetComponentOfType(propertyType);
                        if (c != null)
                        {
                            component.Uses(c, null);
                        }
                    }
                    foreach (FieldInfo fieldInfo in type.GetTypeInfo().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        string fieldType = fieldInfo.FieldType.AssemblyQualifiedName;
                        Component c = ComponentFinder.Container.GetComponentOfType(fieldType);
                        if (c != null)
                        {
                            component.Uses(c, "");
                        }
                    }
                }
            }
        }

        private bool InNamespace(Type type)
        {
            return type.Namespace != null && type.Namespace.StartsWith(ComponentFinder.Namespace);
        }

    }
}
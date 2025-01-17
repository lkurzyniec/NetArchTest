﻿namespace NetArchTest.Rules.Dependencies
{
    using System.Collections.Generic;
    using Mono.Cecil;
    using NetArchTest.Rules.Dependencies.DataStructures;

    /// <summary>
    /// Finds dependencies within a given set of types.
    /// </summary>
    internal class DependencySearch
    {
        /// <summary>
        /// Finds types that have a dependency on any item in the given list of dependencies.
        /// </summary>
        /// <param name="input">The set of type definitions to search.</param>
        /// <param name="dependencies">The set of dependencies to look for.</param>
        /// <returns>A list of found types.</returns>
        public IReadOnlyList<TypeDefinition> FindTypesThatHaveDependencyOnAny(IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies)
        {  
            return FindTypes(input, TypeDefinitionCheckingResult.SearchType.HaveDependencyOnAny, dependencies, true);           
        }

        /// <summary>
        /// Finds types that have a dependency on every item in the given list of dependencies.
        /// </summary>
        /// <param name="input">The set of type definitions to search.</param>
        /// <param name="dependencies">The set of dependencies to look for.</param>
        /// <returns>A list of found types.</returns>
        public IReadOnlyList<TypeDefinition> FindTypesThatHaveDependencyOnAll(IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies)
        {  
            return FindTypes(input, TypeDefinitionCheckingResult.SearchType.HaveDependencyOnAll, dependencies, true);         
        }

        /// <summary>
        /// Finds types that may have a dependency on any item in the given list of dependencies, but cannot have a dependency that is not in the list.
        /// </summary>
        /// <param name="input">The set of type definitions to search.</param>
        /// <param name="dependencies">The set of dependencies to look for.</param>
        /// <returns>A list of found types.</returns>
        public IReadOnlyList<TypeDefinition> FindTypesThatOnlyHaveDependenciesOnAnyOrNone(IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies)
        {           
            return FindTypes(input, TypeDefinitionCheckingResult.SearchType.OnlyHaveDependenciesOnAnyOrNone, dependencies, false);
        }

        /// <summary>
        /// Finds types that have a dependency on any item in the given list of dependencies, but cannot have a dependency that is not in the list.
        /// </summary>
        /// <param name="input">The set of type definitions to search.</param>
        /// <param name="dependencies">The set of dependencies to look for.</param>
        /// <returns>A list of found types.</returns>
        public IReadOnlyList<TypeDefinition> FindTypesThatOnlyHaveDependenciesOnAny(IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies)
        {
            return FindTypes(input, TypeDefinitionCheckingResult.SearchType.OnlyHaveDependenciesOnAny, dependencies, false);
        }

        /// <summary>
        /// Finds types that have a dependency on every item in the given list of dependencies, but cannot have a dependency that is not in the list.
        /// </summary>
        /// <param name="input">The set of type definitions to search.</param>
        /// <param name="dependencies">The set of dependencies to look for.</param>
        /// <returns>A list of found types.</returns>
        public IReadOnlyList<TypeDefinition> FindTypesThatOnlyOnlyHaveDependenciesOnAll(IEnumerable<TypeDefinition> input, IEnumerable<string> dependencies)
        {
            return FindTypes(input, TypeDefinitionCheckingResult.SearchType.OnlyHaveDependenciesOnAll, dependencies, false);
        }

        private List<TypeDefinition> FindTypes(IEnumerable<TypeDefinition> input, TypeDefinitionCheckingResult.SearchType searchType, IEnumerable<string> dependencies, bool serachForDependencyInFieldConstant)
        {
            var output = new List<TypeDefinition>();
            var searchTree = new CachedNamespaceTree(dependencies);

            foreach (var type in input)
            {
                var context = new TypeDefinitionCheckingContext(type, searchType, searchTree, serachForDependencyInFieldConstant);
                
                if (context.IsTypeFound())
                {
                    output.Add(type);
                }
            }

            return output;
        }       
    }
}
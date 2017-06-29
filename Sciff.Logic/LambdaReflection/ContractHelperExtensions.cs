using System;

namespace Sciff.Logic.LambdaReflection
{
    /// <summary>
    ///     Fixes CodeContracts for selected reflection methods
    /// </summary>
    public static class ContractHelperExtensions
    {
        /// <summary>
        ///     Makes a generic type
        /// </summary>
        public static Type MakeType(this Type genericTypeDefinition, params Type[] typeArguments)
        {
            if (!genericTypeDefinition.IsGenericTypeDefinition)
                throw new ArgumentException("must be a generic type definition");

            if (typeArguments.Length != genericTypeDefinition.GetGenericArguments().Length)
                throw new ArgumentException("typeArguments must be supplied for every generic parameter");

            return genericTypeDefinition.MakeGenericType(typeArguments);
        }
    }
}
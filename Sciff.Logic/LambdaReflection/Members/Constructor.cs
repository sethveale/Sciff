using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sciff.Logic.LambdaReflection.Members
{
    /// <summary>
    ///     Finds and wraps constructors that match the type parameters of <see cref="Func{TResult}" />s
    /// </summary>
    public static class Constructor
    {
        #region AsFunc

        /// <summary>
        ///     Creates a func that wraps a constructor for <typeparamref name="TResult" /> that all the given parameter types
        ///     (in order).
        /// </summary>
        public static Func<T1, TResult> AsFunc<T1, TResult>()
        {
            return FindAndWrapConstructor(
                (body, args) => Expression.Lambda<Func<T1, TResult>>(body, args).Compile()
            );
        }

        /// <summary>
        ///     Creates a func that wraps a constructor for <typeparamref name="TResult" /> that all the given parameter types
        ///     (in order).
        /// </summary>
        public static Func<T1, T2, TResult> AsFunc<T1, T2, TResult>()
        {
            return FindAndWrapConstructor(
                (body, args) => Expression.Lambda<Func<T1, T2, TResult>>(body, args).Compile()
            );
        }

        /// <summary>
        ///     Creates a func that wraps a constructor for <typeparamref name="TResult" /> that all the given parameter types
        ///     (in order).
        /// </summary>
        public static Func<T1, T2, T3, TResult> AsFunc<T1, T2, T3, TResult>()
        {
            return FindAndWrapConstructor(
                (body, args) => Expression.Lambda<Func<T1, T2, T3, TResult>>(body, args).Compile()
            );
        }

        /// <summary>
        ///     Creates a func that wraps a constructor for <typeparamref name="TResult" /> that all the given parameter types
        ///     (in order).
        /// </summary>
        public static Func<T1, T2, T3, T4, TResult> AsFunc<T1, T2, T3, T4, TResult>()
        {
            return FindAndWrapConstructor(
                (body, args) => Expression.Lambda<Func<T1, T2, T3, T4, TResult>>(body, args).Compile()
            );
        }

        #endregion

        #region AsLambda

        /// <summary>
        ///     Creates a lambda that wraps a constructor for <typeparamref name="TResult" /> that all the given parameter types
        ///     (in order).
        /// </summary>
        public static Expression<Func<T1, TResult>> AsLambda<T1, TResult>()
        {
            return FindAndWrapConstructor(Expression.Lambda<Func<T1, TResult>>);
        }

        /// <summary>
        ///     Creates a lambda that wraps a constructor for <typeparamref name="TResult" /> that all the given parameter types
        ///     (in order).
        /// </summary>
        public static Expression<Func<T1, T2, TResult>> AsLambda<T1, T2, TResult>()
        {
            return FindAndWrapConstructor(Expression.Lambda<Func<T1, T2, TResult>>);
        }

        /// <summary>
        ///     Creates a lambda that wraps a constructor for <typeparamref name="TResult" /> that all the given parameter types
        ///     (in order).
        /// </summary>
        public static Expression<Func<T1, T2, T3, TResult>> AsLambda<T1, T2, T3, TResult>()
        {
            return FindAndWrapConstructor(Expression.Lambda<Func<T1, T2, T3, TResult>>);
        }

        /// <summary>
        ///     Creates a lambda that wraps a constructor for <typeparamref name="TResult" /> that all the given parameter types
        ///     (in order).
        /// </summary>
        public static Expression<Func<T1, T2, T3, T4, TResult>> AsLambda<T1, T2, T3, T4, TResult>()
        {
            return FindAndWrapConstructor(Expression.Lambda<Func<T1, T2, T3, T4, TResult>>);
        }

        #endregion

        private static readonly ConcurrentDictionary<Type, object> Cache = new ConcurrentDictionary<Type, object>();

        private static TFuncOrLambda FindAndWrapConstructor<TFuncOrLambda>(
            Func<Expression, ParameterExpression[],
                TFuncOrLambda> compile)
            where TFuncOrLambda : class
        {
            var funcOrLambdaType = typeof(TFuncOrLambda);

            var funcType = funcOrLambdaType.IsGenericType &&
                           funcOrLambdaType.GetGenericTypeDefinition() == typeof(Expression<>)
                ? funcOrLambdaType.GenericTypeArguments[0]
                : funcOrLambdaType;

            return (TFuncOrLambda) Cache.GetOrAdd(funcOrLambdaType, _ => FindAndWrapConstructor(funcType, compile));
        }

        private static TFunc FindAndWrapConstructor<TFunc>(Type funcType,
            Func<Expression, ParameterExpression[], TFunc> compile)
        {
            var typeArgs = funcType.GenericTypeArguments;

            var parameters = typeArgs.Take(typeArgs.Length - 1).ToArray();
            var constructedType = typeArgs[typeArgs.Length - 1];

            var constructor = constructedType.GetConstructors().FirstOrDefault(
                c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameters)
            );

            if (constructor == null)
                // ReSharper disable once CoVariantArrayConversion
                throw new KeyNotFoundException(
                    $"Constructor for {constructedType} with parameters ({string.Join(", ", (object[]) parameters)}) not found"
                );

            var parameterExpressions = constructor.GetParameters().Select(
                p => Expression.Parameter(p.ParameterType, p.Name)
            ).ToArray();

            return compile(
                // ReSharper disable once CoVariantArrayConversion
                Expression.New(constructor, parameterExpressions),
                parameterExpressions
            );
        }
    }
}
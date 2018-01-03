using System;
using System.Linq.Expressions;

namespace Tiveria.Common.Extensions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static string GetPropertyName<T1, T2>(this Expression<Func<T1, T2>> action)
        {
            var expression = (MemberExpression)action.Body;
            var propertyName = expression.Member.Name;
            return propertyName;
        }
    }
}

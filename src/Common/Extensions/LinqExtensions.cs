using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Tiveria.Common.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// An extension to Linq allwoing to write where clauses that also cover Contains, startwith and endswith functionality by introducing a wildcard(*)
        /// <example>var q = db.Customers.WhereLike(c => c.CompanyName, "*GmbH").ToList();</example> 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="valueSelector"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereLike<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, string>> valueSelector,
            string value)
        {
            return source.Where(BuildLikeExpression(valueSelector, value, '*'));
        }

        public static Expression<Func<TElement, bool>> BuildLikeExpression<TElement>(
            Expression<Func<TElement, string>> valueSelector,
            string value,
            char wildcard)
        {
            if (valueSelector == null)
                throw new ArgumentNullException("valueSelector");

            var method = GetLikeMethod(value, wildcard);

            value = value.Trim(wildcard);
            var body = Expression.Call(valueSelector.Body, method, Expression.Constant(value));

            var parameter = valueSelector.Parameters.Single();
            return Expression.Lambda<Func<TElement, bool>>(body, parameter);
        }

        private static MethodInfo GetLikeMethod(string value, char wildcard)
        {
            var methodName = "Contains";

            var textLength = value.Length;
            value = value.TrimEnd(wildcard);
            if (textLength > value.Length)
            {
                methodName = "StartsWith";
                textLength = value.Length;
            }

            value = value.TrimStart(wildcard);
            if (textLength > value.Length)
            {
                methodName = (methodName == "StartsWith") ? "Contains" : "EndsWith";
                textLength = value.Length;
            }

            var stringType = typeof(string);
            return stringType.GetMethod(methodName, new Type[] { stringType });
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                    Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }

    }
    /*
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
     * */
}

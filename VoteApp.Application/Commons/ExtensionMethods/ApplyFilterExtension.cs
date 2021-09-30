using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace VoteApp.Application.Commons.ExtensionMethods
{
    public static class ApplyFilterExtension
    {
        public static IQueryable<TEntity> ApplyFilter<TEntity, TKey>(this IQueryable<TEntity> source,
             Expression<Func<TEntity, TKey>> selector, FilterOperator<TKey> filterOperator)
        {
            var filterExpressions = CreateFilter(selector, filterOperator);
            foreach (var filterExpression in filterExpressions)
            {
                source = source.Where(filterExpression);
            }
            return source;
        }

        public static List<Expression<Func<TEntity, bool>>> CreateFilter<TEntity, TKey>(Expression<Func<TEntity, TKey>> selector, FilterOperator<TKey> filterOperator)
        {
            var expressions = new List<Expression<Func<TEntity, bool>>>();
            if (filterOperator.EQ != null) expressions.Add(CreateBasicExpression(Expression.Equal, selector, filterOperator.EQ));
            if (filterOperator.NE != null) expressions.Add(CreateBasicExpression(Expression.NotEqual, selector, filterOperator.NE));
            if (filterOperator.GT != null) expressions.Add(CreateBasicExpression(Expression.GreaterThan, selector, filterOperator.GT));
            if (filterOperator.GTE != null) expressions.Add(CreateBasicExpression(Expression.GreaterThanOrEqual, selector, filterOperator.GTE));
            if (filterOperator.LT != null) expressions.Add(CreateBasicExpression(Expression.LessThan, selector, filterOperator.LT));
            if (filterOperator.LTE != null) expressions.Add(CreateBasicExpression(Expression.LessThanOrEqual, selector, filterOperator.LTE));
            if (filterOperator.IN.Count > 0) expressions.Add(CreateInOrNotInExpression(selector, filterOperator.IN));
            if (filterOperator.NIN.Count > 0) expressions.Add(CreateInOrNotInExpression(selector, filterOperator.NIN, true));

            return expressions;
        }

        private static Expression<Func<TEntity, bool>> CreateContainsExpression<TEntity, TKey>(TKey value)
        {
            var total = new List<Expression>();
            var fields = GetEntityFieldsToCompareTo<TEntity, TKey>().ToArray();
            var parameterExpression = Expression.Parameter(typeof(TEntity), "x");
            for (var i = 0; i < fields.Length; i++)
            {
                var containMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var property = Expression.Property(parameterExpression, fields[i]);
                var containsExpression = Expression.Call(
                        property,
                        containMethod,
                        Expression.Constant(value)
                    );
                total.Add(containsExpression);
            }
            var orElseExpression = total.Aggregate((x, y) => Expression.OrElse(x, y));
            var lamda = Expression.Lambda<Func<TEntity, bool>>(orElseExpression, parameterExpression);
            return lamda;
        }

        private static Expression<Func<TEntity, bool>> CreateInOrNotInExpression<TEntity, TKey>(
           Expression<Func<TEntity, TKey>> selector,
           List<TKey> values,
           bool invert = false)
        {
            (var left, var param) = CreateLeftExpression(selector);
            left = Expression.Convert(left, typeof(TKey));
            Expression right = Expression.Constant(values);

            var containsMethodRef = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                   .Single(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2);
            MethodInfo containsMethod = containsMethodRef.MakeGenericMethod(typeof(TKey));

            Expression containsExpression = Expression.Call(containsMethod, right, left);
            if (invert == true) containsExpression = Expression.Not(containsExpression);

            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(containsExpression, param);
            return lambdaExpression;
        }

        public static Expression<Func<TEntity, bool>> CreateBasicExpression<TEntity, TKey>(
            Func<Expression, Expression, Expression> expresionOperator, Expression<Func<TEntity, TKey>> selector,
            TKey value
        )
        {
            var (leftExpression, param) = CreateLeftExpression(selector);
            var nullableType = NullableType(typeof(TKey));
            var rightExpression = Expression.Constant(value, nullableType);
            var body = expresionOperator.Invoke(leftExpression, rightExpression);
            var lamda = Expression.Lambda<Func<TEntity, bool>>(body, param);
            return lamda;
        }

        public static (Expression Left, ParameterExpression Param) CreateLeftExpression<TEntity, TKey>(Expression<Func<TEntity, TKey>> selector)
        {
            var parameterExpression = Expression.Parameter(typeof(TEntity), "x");
            var propertyName = GetPropertyName(selector);
            var nullableType = NullableType(typeof(TKey));
            return (Expression.Convert(Expression.PropertyOrField(parameterExpression, propertyName), nullableType), parameterExpression);
        }

        public static Type NullableType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) return type;
            if (type == typeof(string)) return type;
            return typeof(Nullable<>).MakeGenericType(type);
        }
        public static string GetPropertyName<TEntity, TKey>(Expression<Func<TEntity, TKey>> selector)
        {
            if (selector.Body is not MemberExpression me)
            {
                me = (selector.Body as UnaryExpression).Operand as MemberExpression;
            }
            return me.ToString().Split('.').Skip(1).First();
        }
        private static IEnumerable<string> GetEntityFieldsToCompareTo<TEntity, TValue>()
        {
            var entityType = typeof(TEntity);
            var valueType = typeof(TValue);

            var fields = entityType.GetFields()
                .Where(x => x.FieldType == valueType)
                .Select(x => x.Name);

            var properties = entityType.GetProperties()
                .Where(x => x.PropertyType == valueType)
                .Select(x => x.Name);

            return fields.Concat(properties);
        }
    }
}

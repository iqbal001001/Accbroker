using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Web;

namespace AccBroker.WebAPI.Helper
{
    public static class Class1
    {
        public static IQueryable<TEntity> ApplySort<TEntity>(this IQueryable<TEntity> source, string sort) 
            where TEntity : class
        {
            IQueryable<TEntity> returnValue = null;

            string orderPair = sort.Trim().Split(',')[0];
            string command = orderPair.StartsWith("-") ? "OrderByDescending" : "OrderBy";

            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type, "p");

            string propertyName = orderPair.StartsWith("-") ? (orderPair.Remove(0,1).Split(' ')[0]).Trim() : (orderPair.Split(' ')[0]).Trim();

            System.Reflection.PropertyInfo property;
            MemberExpression propertyAccess;

            if (propertyName.Contains('.'))
            {
                // support to be sorted on child fields. 
                String[] childProperties = propertyName.Split('.');
                property = typeof(TEntity).GetProperty(childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);

                for (int i = 1; i < childProperties.Length; i++)
                {
                    Type t = property.PropertyType;
                    if (!t.IsGenericType)
                    {
                        property = t.GetProperty(childProperties[i]);
                    }
                    else
                    {
                        property = t.GetGenericArguments().First().GetProperty(childProperties[i]);
                    }

                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = type.GetProperty(propertyName);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }

            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },

            source.Expression, Expression.Quote(orderByExpression));

            returnValue = source.Provider.CreateQuery<TEntity>(resultExpression);

            if (sort.Trim().Split(',').Count() > 1)
            {
                // remove first item
                string newSearchForWords = sort.ToString().Remove(0, sort.ToString().IndexOf(',') + 1);
                return source.OrderBy(newSearchForWords);
            }

            return returnValue;
        }
    }
}
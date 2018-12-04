using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DRS.Model;
using DRS.Model.Models;
using Newtonsoft.Json.Linq;

namespace DRS.Data
{
    public static class EfUtilities
    {
        public static bool IsNavigationEnabled { get; set; }
        public static string[] NavigationTypes { get; set; }
        /// <summary>
        /// get subarray from existing array starting from index to length-1 of original array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        /// <summary>
        /// check if a type has a particular property
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this Type obj, string propertyName)
        {
            return obj.GetProperty(propertyName) != null;
        }
        /// <summary>
        /// get the type of property on a type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Type GetPropType(this Type obj, string propertyName)
        {
            return obj.GetProperty(propertyName).GetType();
        }
        /// <summary>
        /// cast a value to a destination type
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static dynamic ConvertToType(dynamic source, Type dest, bool nullable = false)
        {
            if (!nullable)
            {
                return Convert.ChangeType(source, dest);
            }
            return Convert.ChangeType(source, Nullable.GetUnderlyingType(dest));
        }
        /// <summary>
        /// used by LINQ to SQL
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
        /// <summary>
        /// used by LINQ
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
        /// <summary>
        /// search properties with specific attribute on instance of Types
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="attribute"></param>
        /// <param name="attrParams"></param>
        /// <returns></returns>
        public static List<string> GetPropsWithAttributeOnInstance<TEntity>(this TEntity source, Type attribute, List<AttributeParams> attrParams)
            where TEntity : class
        {
            List<string> props = new List<string>();
            if (attrParams != null && attrParams.Count > 0)
            {
                var valid = true;
                foreach (var item in attrParams)
                {
                    if (!attribute.HasProperty(item.PropName) || attribute.GetProperties().Where(x => x.Name == item.PropName).Count() == 0)
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    foreach (var prop in source.GetType().GetProperties())
                    {
                        object attrInstance = prop.GetCustomAttributes(attribute, true).ToList().FirstOrDefault();
                        if (attrInstance != null)
                        {
                            var isValidAttribute = true;
                            foreach (var item in attrParams)
                            {
                                if (attribute.HasProperty(item.PropName))
                                {
                                    if (attrInstance.GetType().GetProperty(item.PropName).GetValue(attrInstance) != item.PropValue)
                                    {
                                        isValidAttribute = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    isValidAttribute = false;
                                    break;
                                }
                            }
                            if (isValidAttribute)
                            {
                                props.Add(prop.Name);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var prop in source.GetType().GetProperties())
                {
                    object attrInstance = prop.GetCustomAttributes(attribute, true).ToList().FirstOrDefault();
                    if (attrInstance != null)
                    {
                        props.Add(prop.Name);
                    }
                }
            }
            return props;
        }
        /// <summary>
        /// search properties with specific attribute on Types
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <param name="attrParams"></param>
        /// <returns></returns>
        public static List<string> GetPropsWithAttributeOnType(this Type type, Type attribute, List<AttributeParams> attrParams)
        {
            List<string> props = new List<string>();
            if (attrParams != null && attrParams.Count > 0)
            {
                var valid = true;
                foreach (var item in attrParams)
                {
                    if (!attribute.HasProperty(item.PropName) || attribute.GetProperties().Where(x => x.Name == item.PropName).Count() == 0)
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    foreach (var prop in type.GetProperties())
                    {
                        object attrInstance = prop.GetCustomAttributes(attribute, true).ToList().FirstOrDefault();
                        if (attrInstance != null)
                        {
                            var isValidAttribute = true;
                            foreach (var item in attrParams)
                            {
                                if (attribute.HasProperty(item.PropName))
                                {
                                    if (attrInstance.GetType().GetProperty(item.PropName).GetValue(attrInstance) != item.PropValue)
                                    {
                                        isValidAttribute = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    isValidAttribute = false;
                                    break;
                                }
                            }
                            if (isValidAttribute)
                            {
                                props.Add(prop.Name);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var prop in type.GetProperties())
                {
                    object attrInstance = prop.GetCustomAttributes(attribute, true).ToList().FirstOrDefault();
                    if (attrInstance != null)
                    {
                        props.Add(prop.Name);
                    }
                }
            }
            return props;
        }
        /// <summary>
        /// get property for any object instance
        /// </summary>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object target, string name)
        {
            var site = System.Runtime.CompilerServices.CallSite<Func<System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, name, target.GetType(), new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) }));
            return site.Target(site, target);
        }
        /// <summary>
        /// cast to any type from any type
        /// var type = typeof(List<>).MakeGenericType(baseType);
        /// MethodInfo castMethod = typeof(EfUtilities).GetMethod("Cast").MakeGenericMethod(type);
        /// object castedObject = castMethod.Invoke(null, new object[] { obj });
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T Cast<T>(this object input) where T : class
        {
            return (T)input;
        }
        /// <summary>
        /// get expression
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name=""></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, TResult>> GetExpression<TEntity, TResult>(string prop, ParameterExpression parameter)
        {
            var parts = prop.Split('.');
            Expression parent = parts.Aggregate<string, Expression>(parameter, Expression.Property);
            Expression conversion = Expression.Convert(parent, typeof(object));

            //var tryExpression = Expression.TryCatch(Expression.Block(typeof(object), conversion),
            //                                        Expression.Catch(typeof(object), Expression.Constant(null)));

            return Expression.Lambda<Func<TEntity, TResult>>(conversion, parameter);
        }
        /// <summary>
        /// prepare master parts for preparing null check expressions
        /// </summary>
        /// <param name="basetype"></param>
        /// <param name="propname"></param>
        /// <param name="isvalid"></param>
        /// <param name="currentindex"></param>
        /// <param name="masterparts"></param>
        /// <returns></returns>
        public static (bool, int, List<string>) PrepareMasterPartsForNullCheck(Type basetype, string propname, bool isvalid, int currentindex, List<string> masterparts, bool includelast = true)
        {
            if (includelast)
            {
                if (propname.Contains("."))
                {
                    var parts = propname.Split('.');
                    if (parts != null && parts.Length >= 2)
                    {
                        if (currentindex <= parts.Length - 1)
                        {
                            var part = parts[currentindex].ToString();
                            if (!String.IsNullOrEmpty(part))
                            {
                                if (basetype.HasProperty(part))
                                {
                                    if (basetype.GetProperties().Where(x => x.Name == part).Count() > 0)
                                    {
                                        var partType = basetype.GetProperties().Where(x => x.Name == part).First().PropertyType;
                                        currentindex += 1;
                                        // check property for reference type
                                        if (!partType.GetTypeInfo().IsValueType)
                                        {
                                            var pushpropname = String.Join(".", parts.Take(currentindex));
                                            if (masterparts.Where(x => x == pushpropname).Count() == 0)
                                            {
                                                masterparts.Add(pushpropname);
                                            }
                                        }
                                        else
                                        {
                                            if (Nullable.GetUnderlyingType(partType) != null)
                                            {
                                                var pushpropname = String.Join(".", parts.Take(currentindex));
                                                if (masterparts.Where(x => x == pushpropname).Count() == 0)
                                                {
                                                    masterparts.Add(pushpropname);
                                                }
                                            }
                                        }
                                        (isvalid, currentindex, masterparts) = EfUtilities.PrepareMasterPartsForNullCheck(partType, propname, isvalid, currentindex, masterparts, includelast);
                                    }
                                    else
                                        isvalid = false;
                                }
                                else
                                    isvalid = false;
                            }
                        }
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(propname))
                    {
                        if (basetype.HasProperty(propname))
                        {
                            if (basetype.GetProperties().Where(x => x.Name == propname).Count() > 0)
                            {
                                var partType = basetype.GetProperties().Where(x => x.Name == propname).First().PropertyType;
                                // check property for reference type
                                if (!partType.GetTypeInfo().IsValueType)
                                {
                                    if (masterparts.Where(x => x == propname).Count() == 0)
                                    {
                                        masterparts.Add(propname);
                                    }
                                }
                                else
                                {
                                    if (Nullable.GetUnderlyingType(partType) != null)
                                    {
                                        if (masterparts.Where(x => x == propname).Count() == 0)
                                        {
                                            masterparts.Add(propname);
                                        }
                                    }
                                }
                            }
                            else
                                isvalid = false;
                        }
                        else
                            isvalid = false;
                    }
                }
            }
            else
            {
                if (propname.Contains("."))
                {
                    var parts = propname.Split('.');
                    if (parts != null && parts.Length >= 2)
                    {
                        if (currentindex < parts.Length - 1)
                        {
                            var part = parts[currentindex].ToString();
                            if (!String.IsNullOrEmpty(part))
                            {
                                if (basetype.HasProperty(part))
                                {
                                    if (basetype.GetProperties().Where(x => x.Name == part).Count() > 0)
                                    {
                                        var partType = basetype.GetProperties().Where(x => x.Name == part).First().PropertyType;
                                        currentindex += 1;
                                        // check property for reference type
                                        if (!partType.GetTypeInfo().IsValueType)
                                        {
                                            var pushpropname = String.Join(".", parts.Take(currentindex));
                                            if (masterparts.Where(x => x == pushpropname).Count() == 0)
                                            {
                                                masterparts.Add(pushpropname);
                                            }
                                        }
                                        else
                                        {
                                            if (Nullable.GetUnderlyingType(partType) != null)
                                            {
                                                var pushpropname = String.Join(".", parts.Take(currentindex));
                                                if (masterparts.Where(x => x == pushpropname).Count() == 0)
                                                {
                                                    masterparts.Add(pushpropname);
                                                }
                                            }
                                        }
                                        (isvalid, currentindex, masterparts) = EfUtilities.PrepareMasterPartsForNullCheck(partType, propname, isvalid, currentindex, masterparts, includelast);
                                    }
                                    else
                                        isvalid = false;
                                }
                                else
                                    isvalid = false;
                            }
                        }
                    }
                }
            }
            return (isvalid, currentindex, masterparts);
        }
        /// <summary>
        /// traverse base type for existence of property
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="joinedParts"></param>
        /// <param name="isvalid"></param>
        /// <param name="propType"></param>
        /// <returns></returns>
        public static (bool, Type, string[]) TraverseType(Type baseType, string joinedParts, bool isvalid, Type propType, string[] allParts)
        {
            if (joinedParts.Contains("."))
            {
                var parts = joinedParts.Split('.');
                if (allParts.Length == 0)
                {
                    allParts = parts;
                }
                if (parts != null && parts.Length >= 2)
                {
                    var part = parts[0].ToString();
                    if (!String.IsNullOrEmpty(part))
                    {
                        if (baseType.HasProperty(part))
                        {
                            if (baseType.GetProperties().Where(x => x.Name == part).Count() > 0)
                            {
                                var partType = baseType.GetProperties().Where(x => x.Name == part).First().PropertyType;
                                var subarray = parts.SubArray<string>(1, parts.Length - 1);
                                var remainingParts = string.Join(".", subarray);
                                (isvalid, propType, allParts) = EfUtilities.TraverseType(partType, remainingParts, isvalid, propType, allParts);
                            }
                            else
                                isvalid = false;
                        }
                        else
                            isvalid = false;
                    }
                }
            }
            else
            {
                if (baseType.HasProperty(joinedParts))
                {
                    if (baseType.GetProperties().Where(x => x.Name == joinedParts).Count() == 0)
                    {
                        isvalid = false;
                    }
                    else
                    {
                        isvalid = true;
                        propType = baseType.GetProperties().Where(x => x.Name == joinedParts).First().PropertyType;
                    }
                }
                else
                    isvalid = false;
            }
            return (isvalid, propType, allParts);
        }
        /// <summary>
        /// prepare member expression for expression tree
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="member"></param>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static Expression PrepareMemberExpression(ParameterExpression parameter, Expression member, string[] parts, Type PropType)
        {
            if (parts.Length > 0)
            {
                var first = parts[0].ToString();
                var last = parts.SubArray<string>(1, parts.Length - 1);
                if (parameter != null)
                {
                    return EfUtilities.PrepareMemberExpression(null, Expression.Property(parameter, first), last, PropType);
                }
                else
                {
                    if (last.Length > 0)
                    {
                        return EfUtilities.PrepareMemberExpression(null, Expression.Property(member, first), last, PropType);
                    }
                    else
                    {
                        if (PropType.GetTypeInfo().IsValueType)
                        {
                            if (Nullable.GetUnderlyingType(PropType) != null)
                            {
                                return EfUtilities.PrepareMemberExpression(null, Expression.Convert(Expression.Property(member, first), Nullable.GetUnderlyingType(PropType)), last, PropType);
                            }
                            else
                            {
                                return EfUtilities.PrepareMemberExpression(null, Expression.Property(member, first), last, PropType);
                            }
                        }
                        else
                        {
                            return EfUtilities.PrepareMemberExpression(null, Expression.Property(member, first), last, PropType);
                        }
                    }
                }
            }
            return member;
        }
        /// <summary>
        /// combine expression for null check
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="props"></param>
        /// <returns></returns>
        public static Expression CombineExpressionForNullCheck<TEntity>(ParameterExpression parameter, INavigatioPropsStructure[] props)
        {

            var type = typeof(TEntity);
            var masterparts = new List<string>();
            if (props != null && props.Count() > 0)
            {
                foreach (var prop in props)
                {
                    var (isvalid, _, tempmasterparts) = EfUtilities.PrepareMasterPartsForNullCheck(type, prop.PropName, true, 0, new List<string>());
                    if (isvalid && tempmasterparts.Count > 0)
                    {
                        foreach (var t in tempmasterparts)
                        {
                            if (masterparts.Where(x => x == t).Count() == 0)
                            {
                                masterparts.Add(t);
                            }
                        }
                    }
                }
                if (masterparts.Count > 0)
                {
                    var inputnullcheck = masterparts.Select(m => new FilterNavigationProps { PropName = m, IsNestedProp = m.Contains(".") ? true : false });
                    return EfUtilities.CombineExpression<TEntity>(parameter, inputnullcheck.ToArray(), "NULLCHECK");
                }
            }
            return null;
        }
        /// <summary>
        /// combine expressions with and
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="props"></param>
        /// <returns></returns>
        public static Expression CombineExpression<TEntity>(ParameterExpression parameter, FilterNavigationProps[] props, string joiningClause)
        {
            var prop = props.FirstOrDefault();
            var last = props.ToArray().SubArray(1, props.Length - 1);
            var _type = typeof(TEntity);
            if (!String.IsNullOrEmpty(prop.PropName))
            {
                if (!prop.IsNestedProp)
                {
                    if (_type.HasProperty(prop.PropName))
                    {
                        if (_type.GetProperties().Where(x => x.Name == prop.PropName).Count() > 0)
                        {
                            var propType = _type.GetProperties().Where(x => x.Name == prop.PropName).First().PropertyType;
                            if (last.Length > 0)
                            {
                                if (joiningClause == "AND")
                                {
                                    if (propType == typeof(System.String))
                                    {
                                        var property = Expression.Property(parameter, prop.PropName);
                                        var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                        var left = Expression.Call(property, toLower);
                                        var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), propType));
                                        var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                        var call = (Expression)Expression.Call(left, containsmethod, value);
                                        return (Expression)Expression.And(call, EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause));
                                    }
                                    else
                                    {
                                        if (propType.GetTypeInfo().IsValueType)
                                        {
                                            if (Nullable.GetUnderlyingType(propType) != null)
                                            {
                                                //
                                                return (Expression)Expression.And(
                                    Expression.Equal(
                                                        Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                    EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause)
                                    );
                                            }
                                            return (Expression)Expression.And(
                                    Expression.Equal(
                                                        Expression.Property(parameter, prop.PropName),
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                    EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause)
                                    );
                                        }
                                    }
                                }
                                else if (joiningClause == "OR")
                                {
                                    if (propType == typeof(System.String))
                                    {
                                        var property = Expression.Property(parameter, prop.PropName);
                                        var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                        var left = Expression.Call(property, toLower);
                                        var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), propType));
                                        var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                        var call = (Expression)Expression.Call(left, containsmethod, value);
                                        return (Expression)Expression.Or(call, EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause));
                                    }
                                    else
                                    {
                                        if (propType.GetTypeInfo().IsValueType)
                                        {
                                            if (Nullable.GetUnderlyingType(propType) != null)
                                            {
                                                //
                                                return (Expression)Expression.Or(
                                    Expression.Equal(
                                                        Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                    EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause)
                                    );
                                            }
                                            return (Expression)Expression.Or(
                                    Expression.Equal(
                                                        Expression.Property(parameter, prop.PropName),
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                    EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause)
                                    );
                                        }
                                    }
                                }
                                else if (joiningClause == "NULLCHECK")
                                {
                                    return (Expression)Expression.And(
                                                    Expression.NotEqual(
                                                        Expression.Convert(Expression.Property(parameter, prop.PropName), typeof(object)),
                                                        Expression.Constant(null, typeof(object))
                                                        ),
                                                    EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause)
                                                    );
                                }
                            }
                            else
                            {
                                if (joiningClause == "NULLCHECK")
                                {
                                    return (Expression)Expression.NotEqual(
                                         Expression.Convert(Expression.Property(parameter, prop.PropName), typeof(object)),
                                          Expression.Constant(null, typeof(object))
                                          );
                                }
                                else
                                {
                                    if (propType == typeof(System.String))
                                    {
                                        var property = Expression.Property(parameter, prop.PropName);
                                        var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                        var left = Expression.Call(property, toLower);
                                        var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), propType));
                                        var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                        var call = Expression.Call(left, containsmethod, value);
                                        return (Expression)call;
                                    }
                                    else
                                    {
                                        if (propType.GetTypeInfo().IsValueType)
                                        {
                                            //
                                            if (Nullable.GetUnderlyingType(propType) != null)
                                            {
                                                return (Expression)Expression.Equal(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true)));
                                            }
                                            return (Expression)Expression.Equal(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType)));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var (IsValid, PropType, AllParts) = EfUtilities.TraverseType(_type, prop.PropName, true, null, new string[] { });
                    if (IsValid)
                    {
                        var member = EfUtilities.PrepareMemberExpression(parameter, null, AllParts, PropType);
                        if (last.Length > 0)
                        {
                            if (joiningClause == "AND")
                            {
                                if (PropType == typeof(System.String))
                                {
                                    var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                    var left = Expression.Call(member, toLower);
                                    var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), PropType));
                                    var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                    var call = (Expression)Expression.Call(left, containsmethod, value);
                                    return (Expression)Expression.And(call, EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause));
                                }
                                else
                                {
                                    if (PropType.GetTypeInfo().IsValueType)
                                    {
                                        if (Nullable.GetUnderlyingType(PropType) != null)
                                        {
                                            //
                                            return (Expression)Expression.And(
                                Expression.Equal(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause)
                                );
                                        }
                                        else
                                        {
                                            return (Expression)Expression.And(
                               Expression.Equal(
                                                       member,
                                                       Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                               EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause)
                               );
                                        }
                                    }
                                    else
                                    {
                                        return (Expression)Expression.And(
                               Expression.Equal(
                                                       member,
                                                       Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                               EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause)
                               );
                                    }
                                }
                            }
                            else if (joiningClause == "OR")
                            {
                                if (PropType == typeof(System.String))
                                {
                                    var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                    var left = Expression.Call(member, toLower);
                                    var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), PropType));
                                    var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                    var call = (Expression)Expression.Call(left, containsmethod, value);
                                    return (Expression)Expression.Or(call, EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause));
                                }
                                else
                                {
                                    if (PropType.GetTypeInfo().IsValueType)
                                    {
                                        if (Nullable.GetUnderlyingType(PropType) != null)
                                        {
                                            //
                                            return (Expression)Expression.Or(
                                Expression.Equal(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause)
                                );
                                        }
                                        else
                                        {
                                            return (Expression)Expression.Or(
                                Expression.Equal(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause)
                                );
                                        }
                                    }
                                    else
                                    {
                                        return (Expression)Expression.Or(
                                Expression.Equal(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause)
                                );
                                    }
                                }
                            }
                            else if (joiningClause == "NULLCHECK")
                            {
                                return (Expression)Expression.And(
                                 Expression.NotEqual(
                                     Expression.Convert(member, typeof(object)),
                                     Expression.Constant(null, typeof(object))
                                     ),
                                 EfUtilities.CombineExpression<TEntity>(parameter, last, joiningClause)
                                 );
                            }
                        }
                        else
                        {
                            if (joiningClause == "NULLCHECK")
                            {
                                return (Expression)Expression.NotEqual(
                                   Expression.Convert(member, typeof(object)),
                                   Expression.Constant(null, typeof(object))
                                   );
                            }
                            else
                            {
                                if (PropType == typeof(System.String))
                                {
                                    var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                    var left = Expression.Call(member, toLower);
                                    var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), PropType));
                                    var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                    var call = Expression.Call(left, containsmethod, value);
                                    return (Expression)call;
                                }
                                else
                                {
                                    if (PropType.GetTypeInfo().IsValueType)
                                    {
                                        if (Nullable.GetUnderlyingType(PropType) != null)
                                        {
                                            //
                                            return (Expression)Expression.Equal(
                                                            member,
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true)));
                                        }
                                        else
                                        {
                                            return (Expression)Expression.Equal(
                                                          member,
                                                          Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType)));
                                        }
                                    }
                                    else
                                    {
                                        return (Expression)Expression.Equal(
                                                             member,
                                                             Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType)));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// combine expression for custom filter arguments with shape: PropName, PropValue, IsNestedProp, Clause, Operator
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="paramenter"></param>
        /// <param name="props"></param>
        /// <returns></returns>
        public static Expression CombineExpressionForCustomFilter<TEntity>(ParameterExpression parameter, CustomFilterNavigationProps[] props)
        {
            Expression expression = null;
            var prop = props.FirstOrDefault();
            var joiningClause = prop.JoiningClause;
            var last = props.ToArray().SubArray(1, props.Length - 1);
            var _type = typeof(TEntity);
            if (!String.IsNullOrEmpty(prop.PropName))
            {
                if (!prop.IsNestedProp)
                {
                    if (_type.HasProperty(prop.PropName))
                    {
                        if (_type.GetProperties().Where(x => x.Name == prop.PropName).Count() > 0)
                        {
                            var propType = _type.GetProperties().Where(x => x.Name == prop.PropName).First().PropertyType;
                            if (last.Length > 0)
                            {
                                if (!String.IsNullOrEmpty(joiningClause))
                                {
                                    if (joiningClause == "and")
                                    {
                                        switch (prop.Operator)
                                        {
                                            case "eq":
                                                if (Nullable.GetUnderlyingType(propType) != null)
                                                {
                                                    expression = (Expression)Expression.And(
                                        Expression.Equal(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                }
                                                expression = (Expression)Expression.And(
                                        Expression.Equal(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                break;
                                            case "not-eq":
                                                if (Nullable.GetUnderlyingType(propType) != null)
                                                {
                                                    expression = (Expression)Expression.And(
                                        Expression.NotEqual(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                }
                                                expression = (Expression)Expression.And(
                                        Expression.NotEqual(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                break;
                                            case "gt-eq":
                                                if (Nullable.GetUnderlyingType(propType) != null)
                                                {
                                                    expression = (Expression)Expression.And(
                                        Expression.GreaterThanOrEqual(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                }
                                                expression = (Expression)Expression.And(
                                        Expression.GreaterThanOrEqual(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                break;
                                            case "gt":
                                                if (Nullable.GetUnderlyingType(propType) != null)
                                                {
                                                    expression = (Expression)Expression.And(
                                        Expression.GreaterThan(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                }
                                                expression = (Expression)Expression.And(
                                        Expression.GreaterThan(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                break;
                                            case "lt":
                                                if (Nullable.GetUnderlyingType(propType) != null)
                                                {
                                                    expression = (Expression)Expression.And(
                                        Expression.LessThan(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                }
                                                expression = (Expression)Expression.And(
                                        Expression.LessThan(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                break;
                                            case "lt-eq":
                                                if (Nullable.GetUnderlyingType(propType) != null)
                                                {
                                                    expression = (Expression)Expression.And(
                                        Expression.LessThanOrEqual(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                }
                                                expression = (Expression)Expression.And(
                                        Expression.LessThanOrEqual(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                break;
                                            case "contains":
                                                if (propType == typeof(System.String))
                                                {
                                                    var property = Expression.Property(parameter, prop.PropName);
                                                    var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                                    var left = Expression.Call(property, toLower);
                                                    var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), propType));
                                                    var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                                    var call = (Expression)Expression.Call(left, containsmethod, value);
                                                    expression = (Expression)Expression.And(call, EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last));
                                                }
                                                break;
                                            case "not-contains":
                                                if (propType == typeof(System.String))
                                                {
                                                    var property = Expression.Property(parameter, prop.PropName);
                                                    var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                                    var left = Expression.Call(property, toLower);
                                                    var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), propType));
                                                    var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                                    var call = (Expression)Expression.Call(left, containsmethod, value);
                                                    expression = (Expression)Expression.And(Expression.Not(call), EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last));
                                                }
                                                break;
                                            case "starts-with":
                                                if (propType == typeof(System.String))
                                                {
                                                    var property = Expression.Property(parameter, prop.PropName);
                                                    var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                                    var left = Expression.Call(property, toLower);
                                                    var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), propType));
                                                    var containsmethod = typeof(String).GetMethod("StartsWith", new[] { typeof(string) });
                                                    var call = (Expression)Expression.Call(left, containsmethod, value);
                                                    expression = (Expression)Expression.And(call, EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last));
                                                }
                                                break;
                                        }
                                    }
                                    else if (joiningClause == "or")
                                    {
                                        switch (prop.Operator)
                                        {
                                            case "eq":
                                                if (Nullable.GetUnderlyingType(propType) != null)
                                                {
                                                    expression = (Expression)Expression.Or(
                                        Expression.Equal(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                }
                                                expression = (Expression)Expression.Or(
                                        Expression.Equal(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                break;
                                            case "not-eq":
                                                if (Nullable.GetUnderlyingType(propType) != null)
                                                {
                                                    expression = (Expression)Expression.Or(
                                        Expression.NotEqual(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                }
                                                expression = (Expression)Expression.Or(
                                        Expression.NotEqual(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                break;
                                            case "gt-eq":
                                                if (Nullable.GetUnderlyingType(propType) != null)
                                                {
                                                    expression = (Expression)Expression.Or(
                                        Expression.GreaterThanOrEqual(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                }
                                                expression = (Expression)Expression.Or(
                                        Expression.GreaterThanOrEqual(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                break;
                                            case "gt":
                                                if (Nullable.GetUnderlyingType(propType) != null)
                                                {
                                                    expression = (Expression)Expression.Or(
                                        Expression.GreaterThan(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                }
                                                expression = (Expression)Expression.Or(
                                        Expression.GreaterThan(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                break;
                                            case "lt":
                                                if (Nullable.GetUnderlyingType(propType) != null)
                                                {
                                                    expression = (Expression)Expression.Or(
                                        Expression.LessThan(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                }
                                                expression = (Expression)Expression.Or(
                                        Expression.LessThan(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                break;
                                            case "lt-eq":
                                                if (Nullable.GetUnderlyingType(propType) != null)
                                                {
                                                    expression = (Expression)Expression.Or(
                                        Expression.LessThanOrEqual(
                                                            Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                }
                                                expression = (Expression)Expression.Or(
                                        Expression.LessThanOrEqual(
                                                            Expression.Property(parameter, prop.PropName),
                                                            Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType))),
                                        EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                        );
                                                break;
                                            case "contains":
                                                if (propType == typeof(System.String))
                                                {
                                                    var property = Expression.Property(parameter, prop.PropName);
                                                    var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                                    var left = Expression.Call(property, toLower);
                                                    var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), propType));
                                                    var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                                    var call = (Expression)Expression.Call(left, containsmethod, value);
                                                    expression = (Expression)Expression.Or(call, EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last));
                                                }
                                                break;
                                            case "not-contains":
                                                if (propType == typeof(System.String))
                                                {
                                                    var property = Expression.Property(parameter, prop.PropName);
                                                    var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                                    var left = Expression.Call(property, toLower);
                                                    var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), propType));
                                                    var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                                    var call = (Expression)Expression.Call(left, containsmethod, value);
                                                    expression = (Expression)Expression.Or(Expression.Not(call), EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last));
                                                }
                                                break;
                                            case "starts-with":
                                                if (propType == typeof(System.String))
                                                {
                                                    var property = Expression.Property(parameter, prop.PropName);
                                                    var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                                    var left = Expression.Call(property, toLower);
                                                    var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), propType));
                                                    var containsmethod = typeof(String).GetMethod("StartsWith", new[] { typeof(string) });
                                                    var call = (Expression)Expression.Call(left, containsmethod, value);
                                                    expression = (Expression)Expression.Or(call, EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last));
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                switch (prop.Operator)
                                {
                                    case "eq":
                                        if (Nullable.GetUnderlyingType(propType) != null)
                                        {
                                            expression = (Expression)Expression.Equal(
                                                    Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                    Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true)));
                                        }
                                        expression = (Expression)Expression.Equal(
                                                    Expression.Property(parameter, prop.PropName),
                                                    Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType)));
                                        break;
                                    case "not-eq":
                                        if (Nullable.GetUnderlyingType(propType) != null)
                                        {
                                            expression = (Expression)Expression.NotEqual(
                                                    Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                    Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true)));
                                        }
                                        expression = (Expression)Expression.NotEqual(
                                                    Expression.Property(parameter, prop.PropName),
                                                    Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType)));
                                        break;
                                    case "gt-eq":
                                        if (Nullable.GetUnderlyingType(propType) != null)
                                        {
                                            expression = (Expression)Expression.GreaterThanOrEqual(
                                                    Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                    Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true)));
                                        }
                                        expression = (Expression)Expression.GreaterThanOrEqual(
                                                    Expression.Property(parameter, prop.PropName),
                                                    Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType)));
                                        break;
                                    case "gt":
                                        if (Nullable.GetUnderlyingType(propType) != null)
                                        {
                                            expression = (Expression)Expression.GreaterThan(
                                                    Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                    Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true)));
                                        }
                                        expression = (Expression)Expression.GreaterThan(
                                                    Expression.Property(parameter, prop.PropName),
                                                    Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType)));
                                        break;
                                    case "lt":
                                        if (Nullable.GetUnderlyingType(propType) != null)
                                        {
                                            expression = (Expression)Expression.LessThan(
                                                    Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                    Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true)));
                                        }
                                        expression = (Expression)Expression.LessThan(
                                                    Expression.Property(parameter, prop.PropName),
                                                    Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType)));
                                        break;
                                    case "lt-eq":
                                        if (Nullable.GetUnderlyingType(propType) != null)
                                        {
                                            expression = (Expression)Expression.LessThanOrEqual(
                                                    Expression.Convert(Expression.Property(parameter, prop.PropName), Nullable.GetUnderlyingType(propType)),
                                                    Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType, nullable: true)));
                                        }
                                        expression = (Expression)Expression.LessThanOrEqual(
                                                    Expression.Property(parameter, prop.PropName),
                                                    Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, propType)));
                                        break;
                                    case "contains":
                                        if (propType == typeof(System.String))
                                        {
                                            var property = Expression.Property(parameter, prop.PropName);
                                            var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                            var left = Expression.Call(property, toLower);
                                            var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), propType));
                                            var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                            var call = (Expression)Expression.Call(left, containsmethod, value);
                                            expression = (Expression)call;
                                        }
                                        break;
                                    case "not-contains":
                                        if (propType == typeof(System.String))
                                        {
                                            var property = Expression.Property(parameter, prop.PropName);
                                            var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                            var left = Expression.Call(property, toLower);
                                            var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), propType));
                                            var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                            var call = (Expression)Expression.Call(left, containsmethod, value);
                                            expression = (Expression)Expression.Not(call);
                                        }
                                        break;
                                    case "starts-with":
                                        if (propType == typeof(System.String))
                                        {
                                            var property = Expression.Property(parameter, prop.PropName);
                                            var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                            var left = Expression.Call(property, toLower);
                                            var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), propType));
                                            var containsmethod = typeof(String).GetMethod("StartsWith", new[] { typeof(string) });
                                            var call = (Expression)Expression.Call(left, containsmethod, value);
                                            expression = (Expression)call;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    var (IsValid, PropType, AllParts) = EfUtilities.TraverseType(_type, prop.PropName, true, null, new string[] { });
                    if (IsValid)
                    {
                        var member = EfUtilities.PrepareMemberExpression(parameter, null, AllParts, PropType);
                        if (last.Length > 0)
                        {
                            if (!String.IsNullOrEmpty(joiningClause))
                            {
                                if (joiningClause == "and")
                                {
                                    switch (prop.Operator)
                                    {
                                        case "eq":
                                            if (Nullable.GetUnderlyingType(PropType) != null)
                                            {
                                                expression = (Expression)Expression.And(
                                    Expression.Equal(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            }
                                            expression = (Expression)Expression.And(
                                    Expression.Equal(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            break;
                                        case "not-eq":
                                            if (Nullable.GetUnderlyingType(PropType) != null)
                                            {
                                                expression = (Expression)Expression.And(
                                    Expression.NotEqual(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            }
                                            expression = (Expression)Expression.And(
                                    Expression.NotEqual(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            break;
                                        case "gt-eq":
                                            if (Nullable.GetUnderlyingType(PropType) != null)
                                            {
                                                expression = (Expression)Expression.And(
                                    Expression.GreaterThanOrEqual(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            }
                                            expression = (Expression)Expression.And(
                                    Expression.GreaterThanOrEqual(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            break;
                                        case "gt":
                                            if (Nullable.GetUnderlyingType(PropType) != null)
                                            {
                                                expression = (Expression)Expression.And(
                                    Expression.GreaterThan(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            }
                                            expression = (Expression)Expression.And(
                                    Expression.GreaterThan(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            break;
                                        case "lt":
                                            if (Nullable.GetUnderlyingType(PropType) != null)
                                            {
                                                expression = (Expression)Expression.And(
                                    Expression.LessThan(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            }
                                            expression = (Expression)Expression.And(
                                    Expression.LessThan(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            break;
                                        case "lt-eq":
                                            if (Nullable.GetUnderlyingType(PropType) != null)
                                            {
                                                expression = (Expression)Expression.And(
                                    Expression.LessThanOrEqual(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            }
                                            expression = (Expression)Expression.And(
                                    Expression.LessThanOrEqual(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            break;
                                        case "contains":
                                            if (PropType == typeof(System.String))
                                            {
                                                var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                                var left = Expression.Call(member, toLower);
                                                var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), PropType));
                                                var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                                var call = (Expression)Expression.Call(left, containsmethod, value);
                                                expression = (Expression)Expression.And(call, EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last));
                                            }
                                            break;
                                        case "not-contains":
                                            if (PropType == typeof(System.String))
                                            {
                                                var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                                var left = Expression.Call(member, toLower);
                                                var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), PropType));
                                                var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                                var call = (Expression)Expression.Call(left, containsmethod, value);
                                                expression = (Expression)Expression.And(Expression.Not(call), EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last));
                                            }
                                            break;
                                        case "starts-with":
                                            if (PropType == typeof(System.String))
                                            {
                                                var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                                var left = Expression.Call(member, toLower);
                                                var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), PropType));
                                                var containsmethod = typeof(String).GetMethod("StartsWith", new[] { typeof(string) });
                                                var call = (Expression)Expression.Call(left, containsmethod, value);
                                                expression = (Expression)Expression.And(call, EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last));
                                            }
                                            break;
                                    }
                                }
                                else if (joiningClause == "or")
                                {
                                    switch (prop.Operator)
                                    {
                                        case "eq":
                                            if (Nullable.GetUnderlyingType(PropType) != null)
                                            {
                                                expression = (Expression)Expression.Or(
                                    Expression.Equal(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            }
                                            expression = (Expression)Expression.Or(
                                    Expression.Equal(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            break;
                                        case "not-eq":
                                            if (Nullable.GetUnderlyingType(PropType) != null)
                                            {
                                                expression = (Expression)Expression.Or(
                                    Expression.NotEqual(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            }
                                            expression = (Expression)Expression.Or(
                                    Expression.NotEqual(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            break;
                                        case "gt-eq":
                                            if (Nullable.GetUnderlyingType(PropType) != null)
                                            {
                                                expression = (Expression)Expression.Or(
                                    Expression.GreaterThanOrEqual(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            }
                                            expression = (Expression)Expression.Or(
                                    Expression.GreaterThanOrEqual(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            break;
                                        case "gt":
                                            if (Nullable.GetUnderlyingType(PropType) != null)
                                            {
                                                expression = (Expression)Expression.Or(
                                    Expression.GreaterThan(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            }
                                            expression = (Expression)Expression.Or(
                                    Expression.GreaterThan(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            break;
                                        case "lt":
                                            if (Nullable.GetUnderlyingType(PropType) != null)
                                            {
                                                expression = (Expression)Expression.Or(
                                    Expression.LessThan(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            }
                                            expression = (Expression)Expression.Or(
                                    Expression.LessThan(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            break;
                                        case "lt-eq":
                                            if (Nullable.GetUnderlyingType(PropType) != null)
                                            {
                                                expression = (Expression)Expression.Or(
                                    Expression.LessThanOrEqual(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            }
                                            expression = (Expression)Expression.Or(
                                    Expression.LessThanOrEqual(
                                                        member,
                                                        Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType))),
                                    EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last)
                                    );
                                            break;
                                        case "contains":
                                            if (PropType == typeof(System.String))
                                            {
                                                var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                                var left = Expression.Call(member, toLower);
                                                var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), PropType));
                                                var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                                var call = (Expression)Expression.Call(left, containsmethod, value);
                                                expression = (Expression)Expression.Or(call, EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last));
                                            }
                                            break;
                                        case "not-contains":
                                            if (PropType == typeof(System.String))
                                            {
                                                var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                                var left = Expression.Call(member, toLower);
                                                var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), PropType));
                                                var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                                var call = (Expression)Expression.Call(left, containsmethod, value);
                                                expression = (Expression)Expression.Or(Expression.Not(call), EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last));
                                            }
                                            break;
                                        case "starts-with":
                                            if (PropType == typeof(System.String))
                                            {
                                                var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                                var left = Expression.Call(member, toLower);
                                                var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), PropType));
                                                var containsmethod = typeof(String).GetMethod("StartsWith", new[] { typeof(string) });
                                                var call = (Expression)Expression.Call(left, containsmethod, value);
                                                expression = (Expression)Expression.Or(call, EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, last));
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            switch (prop.Operator)
                            {
                                case "eq":
                                    if (Nullable.GetUnderlyingType(PropType) != null)
                                    {
                                        expression = (Expression)Expression.Equal(
                                                member,
                                                Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true)));
                                    }
                                    expression = (Expression)Expression.Equal(
                                                member,
                                                Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType)));
                                    break;
                                case "not-eq":
                                    if (Nullable.GetUnderlyingType(PropType) != null)
                                    {
                                        expression = (Expression)Expression.NotEqual(
                                                member,
                                                Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true)));
                                    }
                                    expression = (Expression)Expression.NotEqual(
                                                member,
                                                Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType)));
                                    break;
                                case "gt-eq":
                                    if (Nullable.GetUnderlyingType(PropType) != null)
                                    {
                                        expression = (Expression)Expression.GreaterThanOrEqual(
                                                member,
                                                Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true)));
                                    }
                                    expression = (Expression)Expression.GreaterThanOrEqual(
                                                member,
                                                Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType)));
                                    break;
                                case "gt":
                                    if (Nullable.GetUnderlyingType(PropType) != null)
                                    {
                                        expression = (Expression)Expression.GreaterThan(
                                                member,
                                                Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true)));
                                    }
                                    expression = (Expression)Expression.GreaterThan(
                                                member,
                                                Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType)));
                                    break;
                                case "lt":
                                    if (Nullable.GetUnderlyingType(PropType) != null)
                                    {
                                        expression = (Expression)Expression.LessThan(
                                                member,
                                                Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true)));
                                    }
                                    expression = (Expression)Expression.LessThan(
                                                member,
                                                Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType)));
                                    break;
                                case "lt-eq":
                                    if (Nullable.GetUnderlyingType(PropType) != null)
                                    {
                                        expression = (Expression)Expression.LessThanOrEqual(
                                                member,
                                                Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType, nullable: true)));
                                    }
                                    expression = (Expression)Expression.LessThanOrEqual(
                                                member,
                                                Expression.Constant(EfUtilities.ConvertToType(prop.PropValue, PropType)));
                                    break;
                                case "contains":
                                    if (PropType == typeof(System.String))
                                    {
                                        var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                        var left = Expression.Call(member, toLower);
                                        var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), PropType));
                                        var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                        var call = (Expression)Expression.Call(left, containsmethod, value);
                                        expression = (Expression)call;
                                    }
                                    break;
                                case "not-contains":
                                    if (PropType == typeof(System.String))
                                    {
                                        var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                        var left = Expression.Call(member, toLower);
                                        var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), PropType));
                                        var containsmethod = typeof(String).GetMethod("Contains", new[] { typeof(string) });
                                        var call = (Expression)Expression.Call(left, containsmethod, value);
                                        expression = (Expression)Expression.Not(call);
                                    }
                                    break;
                                case "starts-with":
                                    if (PropType == typeof(System.String))
                                    {
                                        var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                                        var left = Expression.Call(member, toLower);
                                        var value = Expression.Constant(EfUtilities.ConvertToType(prop.PropValue.ToLower(), PropType));
                                        var containsmethod = typeof(String).GetMethod("StartsWith", new[] { typeof(string) });
                                        var call = (Expression)Expression.Call(left, containsmethod, value);
                                        expression = (Expression)call;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            return expression;
        }
        /// <summary>
        /// perform filteration on navigation props
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_type"></param>
        /// <param name="NavigationProperty"></param>
        /// <param name="NavigationPropertyValue"></param>
        /// <param name="IsNavigationNested"></param>
        /// <param name="_unitOfWork"></param>
        /// <returns></returns>
        public static List<TEntity> PerformFilterOnNavigationProp<TEntity>(PaginationArgs args, UnitOfWork _unitOfWork)
            where TEntity : class
        {
            var _type = typeof(TEntity);
            List<TEntity> entities = null;
            try
            {
                if (args.NavigationProperty != null && args.NavigationProperty.Props.Count > 0)
                {
                    var attrParams = new List<AttributeParams> { new AttributeParams { PropName = "TargetType", PropValue = _type } };
                    var propnames = _unitOfWork.GetPropsWithAttributeOnInstance(typeof(RegisterRepositoryAttribute), attrParams);
                    if (propnames != null && propnames.Count > 0)
                    {
                        var propname = propnames[0];
                        if (!String.IsNullOrEmpty(propname))
                        {
                            dynamic _repo = _unitOfWork.GetPropertyValue(propname);
                            var parameter = Expression.Parameter(typeof(TEntity), "x");
                            Expression member;

                            var nullCheckMemberExpression = EfUtilities.CombineExpressionForNullCheck<TEntity>(parameter, args.NavigationProperty.Props.ToArray());
                            if (args.NavigationProperty.IsMultipleValue)
                                member = EfUtilities.CombineExpression<TEntity>(parameter, args.NavigationProperty.Props.ToArray(), "AND");
                            else
                                member = EfUtilities.CombineExpression<TEntity>(parameter, args.NavigationProperty.Props.ToArray(), "OR");
                            if (nullCheckMemberExpression != null)
                            {
                                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                                   nullCheckMemberExpression, parameter
                                   );
                                List<TEntity> local = null;

                                local = ((IQueryable<TEntity>)_repo.GetManyQueryable(predicate, EfUtilities.NavigationTypes, EfUtilities.IsNavigationEnabled)).ToList();
                                predicate = Expression.Lambda<Func<TEntity, bool>>(
                               member, parameter);
                                entities = local.Where(predicate.Compile()).ToList();
                            }
                            else
                            {
                                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                               member, parameter);
                                entities = ((IQueryable<TEntity>)_repo.GetManyQueryable(predicate, EfUtilities.NavigationTypes, EfUtilities.IsNavigationEnabled)).ToList();
                            }
                            entities = Sort<TEntity>(entities, args.OrderByProp.PropName, args.OrderByProp.Order);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return entities;
        }
        /// <summary>
        /// peform filteration on base filter props for the base query
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="args"></param>
        /// <param name="entities"></param>
        /// <param name="_unitOfWork"></param>
        /// <returns></returns>
        public static List<TEntity> PerformFilterOnBaseFilterProps<TEntity>(PaginationArgs args, List<TEntity> entities, UnitOfWork _unitOfWork)
            where TEntity : class
        {
            List<TEntity> result = entities;
            var _type = typeof(TEntity);
            try
            {
                if (args.BaseFilterProps != null && args.BaseFilterProps.Props.Count > 0)
                {
                    var attrParams = new List<AttributeParams> { new AttributeParams { PropName = "TargetType", PropValue = _type } };
                    var propnames = _unitOfWork.GetPropsWithAttributeOnInstance(typeof(RegisterRepositoryAttribute), attrParams);
                    if (propnames != null && propnames.Count > 0)
                    {
                        var propname = propnames[0];
                        if (!String.IsNullOrEmpty(propname))
                        {
                            dynamic _repo = _unitOfWork.GetPropertyValue(propname);
                            var parameter = Expression.Parameter(typeof(TEntity), "x");
                            Expression member;

                            var nullCheckMemberExpression = EfUtilities.CombineExpressionForNullCheck<TEntity>(parameter, args.BaseFilterProps.Props.ToArray());
                            if (args.BaseFilterProps.IsMultipleValue)
                                member = EfUtilities.CombineExpression<TEntity>(parameter, args.BaseFilterProps.Props.ToArray(), "AND");
                            else
                                member = EfUtilities.CombineExpression<TEntity>(parameter, args.BaseFilterProps.Props.ToArray(), "OR");
                            if (nullCheckMemberExpression != null)
                            {
                                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                                    nullCheckMemberExpression, parameter
                                    );
                                List<TEntity> local = null;
                                if (entities != null)
                                    local = entities.Where(predicate.Compile()).ToList();
                                else
                                {
                                    local = ((IQueryable<TEntity>)_repo.GetManyQueryable(predicate, EfUtilities.NavigationTypes, EfUtilities.IsNavigationEnabled)).ToList();
                                }
                                predicate = Expression.Lambda<Func<TEntity, bool>>(
                               member, parameter);
                                result = local.Where(predicate.Compile()).ToList();
                            }
                            else
                            {
                                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                               member, parameter);
                                if (entities != null)
                                    result = entities.Where(predicate.Compile()).ToList();
                                else
                                {
                                    result = ((IQueryable<TEntity>)_repo.GetManyQueryable(predicate, EfUtilities.NavigationTypes, EfUtilities.IsNavigationEnabled)).ToList();
                                }
                            }
                            result = Sort<TEntity>(result, args.OrderByProp.PropName, args.OrderByProp.Order);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }
        public static List<TEntity> PerformFilterOnCustomFilterProps<TEntity>(PaginationArgs args, List<TEntity> entities, UnitOfWork _unitOfWork)
            where TEntity : class
        {
            List<TEntity> result = entities;
            var _type = typeof(TEntity);
            try
            {
                if (args.CustomFilterProps != null && args.CustomFilterProps.Props.Count > 0)
                {
                    var attrParams = new List<AttributeParams> { new AttributeParams { PropName = "TargetType", PropValue = _type } };
                    var propnames = _unitOfWork.GetPropsWithAttributeOnInstance(typeof(RegisterRepositoryAttribute), attrParams);
                    if (propnames != null && propnames.Count > 0)
                    {
                        var propname = propnames[0];
                        if (!String.IsNullOrEmpty(propname))
                        {
                            dynamic _repo = _unitOfWork.GetPropertyValue(propname);
                            var parameter = Expression.Parameter(typeof(TEntity), "x");
                            Expression member;

                            var nullCheckMemberExpression = EfUtilities.CombineExpressionForNullCheck<TEntity>(parameter, args.CustomFilterProps.Props.ToArray());
                            member = EfUtilities.CombineExpressionForCustomFilter<TEntity>(parameter, args.CustomFilterProps.Props.ToArray());
                            if (nullCheckMemberExpression != null)
                            {
                                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                                    nullCheckMemberExpression, parameter
                                    );
                                List<TEntity> local = null;
                                if (entities != null)
                                    local = entities.Where(predicate.Compile()).ToList();
                                else
                                {
                                    local = ((IQueryable<TEntity>)_repo.GetManyQueryable(predicate, EfUtilities.NavigationTypes, EfUtilities.IsNavigationEnabled)).ToList();
                                }
                                predicate = Expression.Lambda<Func<TEntity, bool>>(
                               member, parameter);
                                switch (args.CustomFilterProps.FilterClause)
                                {
                                    case "where":
                                        result = local.Where(predicate.Compile()).ToList();
                                        break;
                                    case "where-not":
                                        result = local.Except(local.Where(predicate.Compile()).ToList()).ToList();
                                        break;
                                }
                            }
                            else
                            {
                                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                               member, parameter);
                                if (entities != null)
                                {
                                    switch (args.CustomFilterProps.FilterClause)
                                    {
                                        case "where":
                                            result = entities.Where(predicate.Compile()).ToList();
                                            break;
                                        case "where-not":
                                            result = entities.Except(entities.Where(predicate.Compile()).ToList()).ToList();
                                            break;
                                    }
                                }
                                else
                                {
                                    result = ((IQueryable<TEntity>)_repo.GetManyQueryable(predicate, EfUtilities.NavigationTypes, EfUtilities.IsNavigationEnabled, args.CustomFilterProps.FilterClause)).ToList();
                                }
                            }
                            result = Sort<TEntity>(result, args.OrderByProp.PropName, args.OrderByProp.Order);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }
        /// <summary>
        /// peform fileration on filter props
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="_type"></param>
        /// <param name="FilterProps"></param>
        /// <param name="entities"></param>
        /// <param name="_unitOfWork"></param>
        /// <returns></returns>
        public static List<TEntity> PerformFilterOnProps<TEntity>(PaginationArgs args, List<TEntity> entities, UnitOfWork _unitOfWork)
            where TEntity : class
        {
            List<TEntity> result = entities;
            var _type = typeof(TEntity);
            try
            {
                if (args.FilterProps != null && args.FilterProps.Props.Count > 0)
                {
                    var attrParams = new List<AttributeParams> { new AttributeParams { PropName = "TargetType", PropValue = _type } };
                    var propnames = _unitOfWork.GetPropsWithAttributeOnInstance(typeof(RegisterRepositoryAttribute), attrParams);
                    if (propnames != null && propnames.Count > 0)
                    {
                        var propname = propnames[0];
                        if (!String.IsNullOrEmpty(propname))
                        {
                            dynamic _repo = _unitOfWork.GetPropertyValue(propname);
                            var parameter = Expression.Parameter(typeof(TEntity), "x");
                            Expression member;

                            var nullCheckMemberExpression = EfUtilities.CombineExpressionForNullCheck<TEntity>(parameter, args.FilterProps.Props.ToArray());
                            if (args.FilterProps.IsMultipleValue)
                                member = EfUtilities.CombineExpression<TEntity>(parameter, args.FilterProps.Props.ToArray(), "AND");
                            else
                                member = EfUtilities.CombineExpression<TEntity>(parameter, args.FilterProps.Props.ToArray(), "OR");
                            if (nullCheckMemberExpression != null)
                            {
                                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                                    nullCheckMemberExpression, parameter
                                    );
                                List<TEntity> local = null;
                                if (entities != null)
                                    local = entities.Where(predicate.Compile()).ToList();
                                else
                                {
                                    local = ((IQueryable<TEntity>)_repo.GetManyQueryable(predicate, EfUtilities.NavigationTypes, EfUtilities.IsNavigationEnabled)).ToList();
                                }
                                predicate = Expression.Lambda<Func<TEntity, bool>>(
                               member, parameter);
                                result = local.Where(predicate.Compile()).ToList();
                            }
                            else
                            {
                                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                               member, parameter);
                                if (entities != null)
                                    result = entities.Where(predicate.Compile()).ToList();
                                else
                                {
                                    result = ((IQueryable<TEntity>)_repo.GetManyQueryable(predicate, EfUtilities.NavigationTypes, EfUtilities.IsNavigationEnabled)).ToList();
                                }
                            }
                            result = Sort<TEntity>(result, args.OrderByProp.PropName, args.OrderByProp.Order);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }
        /// <summary>
        /// performs paging according to page and pagesize
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <param name="_unitOfWork"></param>
        /// <returns></returns>
        public static Int64 PerformPaging<TEntity>(ref List<TEntity> entities, PaginationArgs args, UnitOfWork _unitOfWork)
            where TEntity : class
        {
            Int64 Total = 0;
            var _type = typeof(TEntity);
            try
            {
                if (entities != null)
                {
                    Total = entities.Count;
                    entities = entities.Page(args.Page, args.PageSize).ToList<TEntity>();
                }
                else
                {
                    var attrParams = new List<AttributeParams> { new AttributeParams { PropName = "TargetType", PropValue = _type } };
                    var propnames = _unitOfWork.GetPropsWithAttributeOnInstance(typeof(RegisterRepositoryAttribute), attrParams);
                    if (propnames != null && propnames.Count > 0)
                    {
                        var propname = propnames[0];
                        if (!String.IsNullOrEmpty(propname))
                        {
                            dynamic _repo = _unitOfWork.GetPropertyValue(propname);
                            var parameter = Expression.Parameter(typeof(TEntity), "x");
                            List<TEntity> local = null;
                            //IQueryable<TEntity> temp = null;
                            var nullcheckexp = Expression.NotEqual(Expression.Convert(parameter, typeof(object)), Expression.Constant(null, typeof(object)));
                            var predicate = Expression.Lambda<Func<TEntity, bool>>(nullcheckexp, parameter);
                            local = ((IQueryable<TEntity>)_repo.GetManyQueryable(predicate, EfUtilities.NavigationTypes, EfUtilities.IsNavigationEnabled)).ToList();

                            Total = local.Count;
                            entities = local.Page(args.Page, args.PageSize).ToList<TEntity>();

                            entities = Sort<TEntity>(entities, args.OrderByProp.PropName, args.OrderByProp.Order);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Total;
        }

        public static List<TEntity> Sort<TEntity>(List<TEntity> input, string property, string order)
        {
            var type = typeof(TEntity);
            var sortProperty = type.GetProperty(property);
            if (order == "asc")
            {
                return input.OrderBy(p => sortProperty.GetValue(p, null)).ToList();
            }
            else if (order == "desc")
            {
                return input.OrderByDescending(p => sortProperty.GetValue(p, null)).ToList();
            }
            return null;
        }
        /// <summary>
        /// get navigation types from an entity
        /// </summary>
        /// <returns></returns>
        public static string[] GetNavigationTypes<TEntity>(PaginationArgs args)
        {
            var t = typeof(TEntity);
            List<string> includeList = new List<string>();
            // navigation props
            foreach (var prop in args.NavigationProperty.Props)
            {
                var (isvalid, _, tempmasterparts) = EfUtilities.PrepareMasterPartsForNullCheck(t, prop.PropName, true, 0, new List<string>(), includelast: false);
                if (isvalid && tempmasterparts.Count > 0)
                {
                    foreach (var temp in tempmasterparts)
                    {
                        if (includeList.Where(x => x == temp).Count() == 0)
                        {
                            includeList.Add(temp);
                        }
                    }
                }
            }
            // base filter props
            foreach (var prop in args.BaseFilterProps.Props)
            {
                var (isvalid, _, tempmasterparts) = EfUtilities.PrepareMasterPartsForNullCheck(t, prop.PropName, true, 0, new List<string>(), includelast: false);
                if (isvalid && tempmasterparts.Count > 0)
                {
                    foreach (var temp in tempmasterparts)
                    {
                        if (includeList.Where(x => x == temp).Count() == 0)
                        {
                            includeList.Add(temp);
                        }
                    }
                }
            }
            // custome filter props
            foreach (var prop in args.CustomFilterProps.Props)
            {
                var (isvalid, _, tempmasterparts) = EfUtilities.PrepareMasterPartsForNullCheck(t, prop.PropName, true, 0, new List<string>(), includelast: false);
                if (isvalid && tempmasterparts.Count > 0)
                {
                    foreach (var temp in tempmasterparts)
                    {
                        if (includeList.Where(x => x == temp).Count() == 0)
                        {
                            includeList.Add(temp);
                        }
                    }
                }
            }
            //filter props
            foreach (var prop in args.FilterProps.Props)
            {
                var (isvalid, _, tempmasterparts) = EfUtilities.PrepareMasterPartsForNullCheck(t, prop.PropName, true, 0, new List<string>(), includelast: false);
                if (isvalid && tempmasterparts.Count > 0)
                {
                    foreach (var temp in tempmasterparts)
                    {
                        if (includeList.Where(x => x == temp).Count() == 0)
                        {
                            includeList.Add(temp);
                        }
                    }
                }
            }
            // orderby props
            if (!String.IsNullOrEmpty(args.OrderByProp.PropName))
            {
                var (isvalid, _, tempmasterparts) = EfUtilities.PrepareMasterPartsForNullCheck(t, args.OrderByProp.PropName, true, 0, new List<string>(), includelast: false);
                if (isvalid && tempmasterparts.Count > 0)
                {
                    foreach (var temp in tempmasterparts)
                    {
                        if (includeList.Where(x => x == temp).Count() == 0)
                        {
                            includeList.Add(temp);
                        }
                    }
                }
            }
            var props = t.GetProperties().Where(x => x.GetGetMethod().IsVirtual);
            var filterprops = props.Where(x => !EfUtilities.DoesTypeSupportInterface(x.PropertyType, "IEnumerable"));
            foreach (var item in filterprops)
            {
                if (includeList.Where(x=>x==item.Name).Count()==0)
                {
                    includeList.Add(item.Name);
                }
            }
            return includeList.ToArray();
        }
        public static string[] GetNavigationTypesForSingle<TEntity>(GetSingleArgs args)
        {
            var t = typeof(TEntity);
            List<string> includeList = new List<string>();
            foreach (var prop in args.FilterProps.Props)
            {
                var (isvalid, _, tempmasterparts) = EfUtilities.PrepareMasterPartsForNullCheck(t, prop.PropName, true, 0, new List<string>(), includelast: false);
                if (isvalid && tempmasterparts.Count > 0)
                {
                    foreach (var temp in tempmasterparts)
                    {
                        if (includeList.Where(x => x == temp).Count() == 0)
                        {
                            includeList.Add(temp);
                        }
                    }
                }
            }
            var props = t.GetProperties().Where(x => x.GetGetMethod().IsVirtual);
            var filterprops = props.Where(x => !EfUtilities.DoesTypeSupportInterface(x.PropertyType, "IEnumerable"));
            foreach (var item in filterprops)
            {
                if (includeList.Where(x => x == item.Name).Count() == 0)
                {
                    includeList.Add(item.Name);
                }
            }
            return includeList.ToArray();
        }
        /// <summary>
        /// Function triggering all the filteration activities:
        /// 1: PerformFilterOnNavigationProp,
        /// 2: PerformFilterOnBaseFilterProps,
        /// 3: PerformFilterOnCustomFilterProps,
        /// 4: PerformFilterOnProps,
        /// 5: PerformPaging
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="args"></param>
        /// <param name="_unitOfWork"></param>
        /// <returns></returns>
        public static (List<TEntity>, Int64) PerformFilterationAndPaging<TEntity>(PaginationArgs args, UnitOfWork _unitOfWork)
            where TEntity : class
        {
            EfUtilities.IsNavigationEnabled = args.IsNavigationEnabled;
            EfUtilities.NavigationTypes = EfUtilities.GetNavigationTypes<TEntity>(args);

            var entities = EfUtilities.PerformFilterOnNavigationProp<TEntity>(args, _unitOfWork);
            entities = EfUtilities.PerformFilterOnBaseFilterProps<TEntity>(args, entities, _unitOfWork);
            entities = EfUtilities.PerformFilterOnCustomFilterProps<TEntity>(args, entities, _unitOfWork);
            entities = EfUtilities.PerformFilterOnProps<TEntity>(args, entities, _unitOfWork);
            var Total = EfUtilities.PerformPaging<TEntity>(ref entities, args, _unitOfWork);
            return (entities, Total);
        }
        /// <summary>
        /// Check if a type implements a particular interface
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inter"></param>
        /// <returns></returns>
        public static bool DoesTypeSupportInterface(Type type, string inter)
        {
            if (type.GetInterfaces().Where(x => x.Name == inter).Count() > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Logic for getting object graph of properties of a type including navigation types
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static JArray GetEntityPropertyHierarchy<TEntity>(string baseIndexForType)
            where TEntity : class
        {
            var _type = typeof(TEntity);
            int nonLeafIndexbase = 0;
            JArray objectGraph = new JArray();
            foreach (var prop in _type.GetProperties())
            {
                JObject singleObject = new JObject();
                if (_type.HasProperty(prop.Name))
                {
                    var propType = prop.PropertyType;
                    if (propType.GetTypeInfo().IsValueType)
                    {
                        // assign property value to JObject in case of value type
                        singleObject["label"] = prop.Name;
                        singleObject["hasChildren"] = false;
                        singleObject["children"] = null;
                        singleObject["nonLeafnodeIndex"] = baseIndexForType == "0" ? $"{nonLeafIndexbase}" : $"{baseIndexForType}.{nonLeafIndexbase}";
                        objectGraph.Add(singleObject);
                    }
                    else
                    {
                        if (propType == typeof(System.String))
                        {
                            // assign property value to JObject in case of string
                            singleObject["label"] = prop.Name;
                            singleObject["hasChildren"] = false;
                            singleObject["children"] = null;
                            singleObject["nonLeafnodeIndex"] = baseIndexForType == "0" ? $"{nonLeafIndexbase}" : $"{baseIndexForType}.{nonLeafIndexbase}";
                            objectGraph.Add(singleObject);
                        }
                        else
                        {
                            if (propType != typeof(System.Byte[]))
                            {
                                // traverse nested type to generated nested type hierarchy
                                // Note: exclude properties of ICollection type

                                if (!EfUtilities.DoesTypeSupportInterface(propType, "IEnumerable"))
                                {
                                    nonLeafIndexbase += 1;
                                    singleObject["label"] = prop.Name;
                                    singleObject["hasChildren"] = true;
                                    singleObject["children"] = new JArray();
                                    singleObject["nonLeafnodeIndex"] = baseIndexForType == "0" ? $"{nonLeafIndexbase}" : $"{baseIndexForType}.{nonLeafIndexbase}";
                                    // call GetEntityPropertyHierarchy recursively with type parameter to get children properties
                                    var mi = typeof(EfUtilities).GetMethod("GetEntityPropertyHierarchy");
                                    var miRef = mi.MakeGenericMethod(new[] { propType });
                                    singleObject["children"] = (JArray)miRef.Invoke(null, new object[] { baseIndexForType == "0" ? $"{nonLeafIndexbase}" : $"{baseIndexForType}.{nonLeafIndexbase}" });
                                    objectGraph.Add(singleObject);
                                }
                            }
                        }
                    }
                }
            }
            return objectGraph;
        }
        /// <summary>
        /// Filteration logicwith GetSingleArgs
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="args"></param>
        /// <param name="_unitOfWork"></param>
        /// <returns></returns>
        public static TEntity GetSingleWithProps<TEntity>(GetSingleArgs args, UnitOfWork _unitOfWork)
            where TEntity : class
        {
            EfUtilities.IsNavigationEnabled = args.IsNavigationEnabled;
            EfUtilities.NavigationTypes = EfUtilities.GetNavigationTypesForSingle<TEntity>(args);
            var _type = typeof(TEntity);
            TEntity entity = null;
            try
            {
                if (args.FilterProps != null && args.FilterProps.Props.Count > 0)
                {
                    var attrParams = new List<AttributeParams> { new AttributeParams { PropName = "TargetType", PropValue = _type } };
                    var propnames = _unitOfWork.GetPropsWithAttributeOnInstance(typeof(RegisterRepositoryAttribute), attrParams);
                    if (propnames != null && propnames.Count > 0)
                    {
                        var propname = propnames[0];
                        if (!String.IsNullOrEmpty(propname))
                        {
                            dynamic _repo = _unitOfWork.GetPropertyValue(propname);
                            var parameter = Expression.Parameter(typeof(TEntity), "x");
                            Expression member;

                            var nullCheckMemberExpression = EfUtilities.CombineExpressionForNullCheck<TEntity>(parameter, args.FilterProps.Props.ToArray());
                            if (args.FilterProps.IsMultipleValue)
                                member = EfUtilities.CombineExpression<TEntity>(parameter, args.FilterProps.Props.ToArray(), "AND");
                            else
                                member = EfUtilities.CombineExpression<TEntity>(parameter, args.FilterProps.Props.ToArray(), "OR");
                            if (nullCheckMemberExpression != null)
                            {
                                var expression = Expression.Lambda<Func<TEntity, bool>>(
                                 nullCheckMemberExpression, parameter
                                 );
                                List<TEntity> local = _repo.GetMany(expression, EfUtilities.IsNavigationEnabled, EfUtilities.NavigationTypes);
                                expression = Expression.Lambda<Func<TEntity, bool>>(
                              member, parameter);
                                entity = local.Where(expression.Compile()).ToList().FirstOrDefault();
                            }
                            else
                            {
                                var expression = Expression.Lambda<Func<TEntity, bool>>(
                              member, parameter);
                                entity = _repo.GetSingle(expression, EfUtilities.IsNavigationEnabled, EfUtilities.NavigationTypes);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return entity;
        }
        //        public static TEntity Find<TEntity>(this DbSet<TEntity> set, params object[] keyValues) where TEntity : class
        //        {
        //            var context = ((IInfrastructure<IServiceProvider>)set).GetService<DbContext>();
        //
        //            var entityType = context.Model.FindEntityType(typeof(TEntity));
        //            var key = entityType.FindPrimaryKey();
        //
        //            var entries = context.ChangeTracker.Entries<TEntity>();
        //
        //            var i = 0;
        //
        //            foreach (var property in key.Properties)
        //            {
        //                entries = entries.Where(e => e.Property(property.Name).CurrentValue == keyValues[i]);
        //                i++;
        //            }
        //
        //            var entry = entries.FirstOrDefault();
        //            if (entry != null)
        //            {
        //                // Return the local object if it exists.
        //                return entry.Entity;
        //            }
        //
        //            // TODO: Build the real LINQ Expression
        //            // set.Where(x => x.Id == keyValues[0]);
        //            var parameter = Expression.Parameter(typeof(TEntity), "x");
        //            var query = set.Where((Expression<Func<TEntity, bool>>)
        //                Expression.Lambda(
        //                    Expression.Equal(
        //                        Expression.Property(parameter, "Id"),
        //                        Expression.Constant(keyValues[0])),
        //                    parameter));
        //
        //            // Look in the database
        //            return query.FirstOrDefault();
        //        }
    }
}

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Automation.HOS.TimelineNavigator.Extensions;

[ExcludeFromCodeCoverage]
public static class QuickMapExtensionMethods
{
    private static readonly ConcurrentDictionary<(Type, Type), Delegate> _cache = new();

    public static T MapTo<T>(this object fromObject) where T : class
    {
        var fromType = fromObject.GetType();
        var toType = typeof(T);
        var key = (fromType, toType);
        Delegate del;
        if (!HasDefaultConstructor(toType))
        {
            del = _cache.GetOrAdd(key, CreateConstructorDelegate(fromType, toType));
            var args = GetConstructorArgs(fromObject, toType);
            return (T)del.DynamicInvoke(args)!;
        }
        else
        {
            del = _cache.GetOrAdd(key, CreateMemberDelegate(fromType, toType));
            return (T)del.DynamicInvoke(fromObject)!;
        }
    }

    private static bool HasDefaultConstructor(Type type)
    {
        return type.GetConstructor(Type.EmptyTypes) != null;
    }

    private static Delegate CreateConstructorDelegate(Type inType, Type outType)
    {
        var types = outType.GetProperties()
            .Select(p => p.PropertyType)
            .ToArray();

        var constructor = outType.GetConstructor(types);

        var constructorParams = types.Select(t => Expression.Parameter(t)).ToArray();

        var newExpression = Expression.New(constructor!, constructorParams);

        return Expression.Lambda(newExpression, constructorParams).Compile();
    }

    private static Delegate CreateMemberDelegate(Type inType, Type outType)
    {
        var param = Expression.Parameter(inType);
        var newExpression = Expression.New(outType.GetConstructor(Type.EmptyTypes)!);

        List<MemberBinding> bindings = new();
        foreach (var prop in inType.GetProperties())
        {
            var tbm = outType.GetProperty(prop.Name);
            if (tbm == null)
            {
                continue;
            }

            var pma = Expression.MakeMemberAccess(param, prop);

            var binding = Expression.Bind(tbm, pma);
            bindings.Add(binding);
        }        

        var body = Expression.MemberInit(newExpression, bindings);

        return Expression.Lambda(body, false, param).Compile();
    }

    private static object[] GetConstructorArgs(object fromObject, Type toType)
    {
        var fromType = fromObject.GetType();
        var result = new List<object>();

        var constructorParams = toType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .First()
            .GetParameters();

        foreach (var param in constructorParams)
        {
            var prop = fromType.GetProperty(param.Name!);
            if (prop == null)
            {
                result.Add(default!);
                continue;
            }
            result.Add(prop!.GetValue(fromObject)!);
        }

        return result.ToArray();
    }
}

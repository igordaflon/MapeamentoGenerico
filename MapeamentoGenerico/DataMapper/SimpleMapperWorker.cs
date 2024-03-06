using System.Reflection;

namespace MapeamentoGenerico.DataMapper;

public class SimpleMapperWorker : ISimpleMapper
{
    private readonly ISimpleMapperCache cache;

    public SimpleMapperWorker(ISimpleMapperCache cache)
    {
        this.cache = cache;
    }

    public IEnumerable<Mapping> GetMapping(Type source, Type target)
    {
        var mappings = cache.GetMappings(source, target);
        if (mappings != null)
            return mappings;

        lock (cache)
        {
            mappings = cache.GetMappings(source, target);
            if (mappings != null)
                return mappings;

            var items = new List<Mapping>();

            var sourceProps = source.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var targetProps = target.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var sourceProp in sourceProps)
            {
                if (sourceProp.CanRead)
                {
                    foreach (var targetProp in targetProps)
                    {
                        if (targetProp.CanWrite)
                        {
                            if (sourceProp.Name == targetProp.Name)
                            {
                                items.Add(new Mapping(sourceProp, targetProp));
                                break;
                            }
                        }
                    }
                }
            }

            cache.Add(source, target, items);
            return items;
        }
    }

    public IEnumerable<string> Map<T, U>(T source, U target)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        var type1 = typeof(T);

        if (type1 == typeof(object))
            type1 = source.GetType();

        var type2 = typeof(U);

        if (type2 == typeof(object))
            type2 = target.GetType();

        var mapping = GetMapping(type1, type2);

        SimpleMapper.Map(source, target, mapping);

        return mapping.Select(i => i.Target.Name);
    }

    public U Map<U>(object source) where U : class, new()
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        var target = new U();

        var sourceType = source.GetType();
        var targetType = typeof(U);

        SimpleMapper.Map(source, target, GetMapping(sourceType, targetType));

        return target;
    }
}

namespace MapeamentoGenerico.DataMapper;

public class SimpleMapperCache : SimpleMapperCache<IEnumerable<Mapping>>, ISimpleMapperCache
{
}

public class SimpleMapperCache<T> : ISimpleMapperCache<T> where T : class
{
    private readonly Dictionary<Type, Dictionary<Type, T>> cache = new();

    public T? GetMappings(Type source, Type target)
    {
        if (cache.TryGetValue(source, out var targetMappings))
            if (targetMappings.TryGetValue(target, out var result))
                return result;
        return null;
    }

    public void Add(Type source, Type target, T value)
    {
        lock (cache)
        {
            if (cache.TryGetValue(source, out var targetMappings))
            {
                if (targetMappings.TryGetValue(target, out _))
                    throw new ArgumentException("Já existe um mapeamento para os tipos informados.");
            }
            else
            {
                targetMappings = new();
                cache.Add(source, targetMappings);
            }
            targetMappings.Add(target, value);
        }
    }
}

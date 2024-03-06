namespace MapeamentoGenerico.DataMapper;

public interface ISimpleMapperCache : ISimpleMapperCache<IEnumerable<Mapping>>
{
}

public interface ISimpleMapperCache<T> where T : class
{
    void Add(Type source, Type target, T value);
    T? GetMappings(Type source, Type target);
}

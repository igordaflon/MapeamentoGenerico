namespace MapeamentoGenerico.DataMapper;

public static class SimpleMapper
{
    private readonly static SimpleMapperWorker mapper = new(new SimpleMapperCache());

    public static IEnumerable<string> Map<T, U>(T source, U target)
        => mapper.Map(source, target);

    public static U Map<U>(object source) where U : class, new()
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        var target = new U();

        var sourceType = source.GetType();
        var targetType = typeof(U);

        Map(source, target, mapper.GetMapping(sourceType, targetType));

        return target;
    }

    public static IEnumerable<U> Map<U>(IEnumerable<object> sourceList) where U : class, new()
    {
        if (sourceList == null)
            throw new ArgumentNullException(nameof(sourceList));

        var targetType = typeof(U);

        return sourceList.Select(source =>
        {
            var target = new U();
            Map(source, target, mapper.GetMapping(source.GetType(), targetType));
            return target;
        });
    }

    public static void Map(object source, object target, IEnumerable<Mapping> mappings)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (target == null)
            throw new ArgumentNullException(nameof(target));
        if (mappings == null)
            throw new ArgumentNullException(nameof(mappings));

        foreach (var item in mappings)
            item.Target.SetValue(target, item.Source.GetValue(source));
    }
}

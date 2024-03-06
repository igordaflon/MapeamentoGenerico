namespace MapeamentoGenerico.DataMapper;

public interface ISimpleMapper
{
    IEnumerable<string> Map<T, U>(T source, U target);
}

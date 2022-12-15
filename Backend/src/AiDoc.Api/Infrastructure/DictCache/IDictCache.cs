namespace AiDoc.Api.Infrastructure.DictCache;

public interface IDictCache<T,TK> where T : notnull where TK : struct
{
    public void Add(T key, TK value);
    public TK? Get(T key);
}
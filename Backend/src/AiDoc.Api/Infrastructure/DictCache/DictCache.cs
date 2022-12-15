using System.Collections.Concurrent;

namespace AiDoc.Api.Infrastructure.DictCache;

public sealed class DictCache<T, TK> : IDictCache<T, TK> where T : notnull where TK : struct
{
    private readonly ConcurrentDictionary<T, TK> _dictionary;

    public DictCache()
        => _dictionary = new ConcurrentDictionary<T, TK>();

    public void Add(T key, TK value)
    {
        if (!_dictionary.TryAdd(key, value))
        {
            _dictionary[key] = value;
        }
    }

    public TK? Get(T key)
        => _dictionary.TryGetValue(key, out var value) ? value : null;
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AiDoc.Api.Infrastructure.ScopeDisposer;

public sealed class ScopeDisposer : IScopeDisposer, IDisposable, IAsyncDisposable
{
    private readonly HashSet<IDisposable> _disposables = new();
    private readonly HashSet<IAsyncDisposable> _asyncDisposables = new();

    public void Add(IDisposable disposable)
        => _disposables.Add(disposable);

    public void AddAsync(IAsyncDisposable disposable)
        => _asyncDisposables.Add(disposable);

    public void Dispose()
    {
        foreach (var disposable in _disposables) 
            disposable.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var disposable in _asyncDisposables)
            await disposable.DisposeAsync();
    }
}
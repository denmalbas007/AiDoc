using System;

namespace AiDoc.Api.Infrastructure.ScopeDisposer;

public interface IScopeDisposer
{
    public void Add(IDisposable disposable);
    
    public void AddAsync(IAsyncDisposable disposable);
}
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Dmnk.Toolkit.TestUtil;

public class InMemoryLogMessage(LogLevel level, string message)
{
    public LogLevel Level { get; } = level;
    public string Message { get; } = message;
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}

public class InMemoryLogger<T> : ILogger<T>
{
    public IReadOnlyList<InMemoryLogMessage> Messages => _messagesInternal;
    private readonly List<InMemoryLogMessage> _messagesInternal = [];
    
    public void ClearMessages() => _messagesInternal.Clear();
    
    public void Log<TState>(
        LogLevel logLevel, EventId eventId, TState state, Exception? exception, 
        Func<TState, Exception?, string> formatter
    )
    {
        var message = formatter(state, exception);
        _messagesInternal.Add(new InMemoryLogMessage(logLevel, message));
    }

    public bool IsEnabled(LogLevel logLevel) => true;
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
}
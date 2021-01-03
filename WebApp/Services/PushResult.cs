#nullable enable
using System;

namespace WebApp.Services {
    public record PushResult<T>() {
        public T Pulled() =>
            this switch {
                PushSuccess<T> p => p.Pull,
                ConcurrencyConflict<T> => throw new InvalidOperationException("No value was pulled; concurrency conflict."),
                _ => throw new NotImplementedException("Unexpected type which is neither a push or concurrency conflict.")
            };
    }

    public record PushSuccess<T>(T Pushed, T Pull) : PushResult<T>;

    public record ConcurrencyConflict<T>(T FailedPush) : PushResult<T>;
}
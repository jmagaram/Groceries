#nullable enable

using System;

namespace WebApp.Services {
    public record PushResult<T>() {
        public U Map<U>(Func<PushSuccess<T>, U> ifSuccess, Func<ConcurrencyConflict<T>, U> ifConflict) =>
            this switch {
                PushSuccess<T> p => ifSuccess(p),
                ConcurrencyConflict<T> c => ifConflict(c),
                _ => throw new NotImplementedException("Unexpected type which is neither a push or concurrency conflict.")
            };
        public T UpdatedDocument() => Map(ifSuccess: p => p.UpdatedServerDocument, ifConflict: _ => throw new InvalidOperationException("No document was successfully pushed."));
        public T ServerVersion() => Map(ifSuccess: p => p.UpdatedServerDocument, ifConflict: p => p.UnchangedServerDocument);
    }

    public record PushSuccess<T>(T Pushed, T UpdatedServerDocument) : PushResult<T>;

    public record ConcurrencyConflict<T>(T PushFailure, T UnchangedServerDocument) : PushResult<T>;
}
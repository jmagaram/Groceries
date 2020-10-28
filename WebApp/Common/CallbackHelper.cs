using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace WebApp.Common {
    public class CallbackHelper<T> {
        private readonly Func<T,Task> _action;

        public CallbackHelper(Func<T, Task> action) =>
            _action = action;

        [JSInvokable("WebApp")]
        public async Task Invoke(string _, T result) => 
            await _action.Invoke(result);
    }
}

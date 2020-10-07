using Microsoft.FSharp.Core;
using Models;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using static Models.StateTypes;

namespace WebApp.Data {
    public class ApplicationStateService {
        readonly BehaviorSubject<State> _state;
        readonly IObservable<State> _stateObs;

        public ApplicationStateService() {
            _state = new BehaviorSubject<State>(StateModule.createSampleData());
            _stateObs = _state;
            ShoppingList = _stateObs.Select(x => ShoppingListModule.create(x));
            Converter<Unit, DateTimeOffset> clock = (x => DateTimeOffset.Now);
            Clock = FuncConvert.ToFSharpFunc(clock);
        }

        public State Current => _state.Value;

        public void Update(StateMessage msg) {
            var state = _state.Value;
            state = StateModule.update(msg, state);
            _state.OnNext(state);
        }

        public FSharpFunc<Unit, DateTimeOffset> Clock { get; private set; }

        public IObservable<ShoppingListModule.ShoppingList> ShoppingList { get; }

    }
}

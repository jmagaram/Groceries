using Microsoft.FSharp.Core;
using Models;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WebApp.Common;
using static Models.StateTypes;

namespace WebApp.Data {
    public class ApplicationStateService {
        readonly BehaviorSubject<State> _state;
        readonly IObservable<State> _stateObs;

        public ApplicationStateService() {
            //_state = new BehaviorSubject<State>(StateModule.createSampleData());
            _state = new BehaviorSubject<State>(StateUpdateModule.createDefault);
            _stateObs = _state;
            ShoppingList = _stateObs.Select(x => ShoppingListModule.create(DateTimeOffset.Now, x));
            CategoryEditPage = _stateObs.Select(x => x.CategoryEditPage).Where(x => x.IsSome()).Select(i => i.Value).DistinctUntilChanged();
            StoreEditPage = _stateObs.Select(x => x.StoreEditPage).Where(x => x.IsSome()).Select(i => i.Value).DistinctUntilChanged();
            ItemEditPage = _stateObs.Select(x => x.ItemEditPage).Where(x => x.IsSome()).Select(i => i.Value).DistinctUntilChanged();
            Converter<Unit, DateTimeOffset> clock = (x => DateTimeOffset.Now);
            Clock = FuncConvert.ToFSharpFunc(clock);
        }

        public State Current => _state.Value;

        public void Update(StateUpdateModule.StateMessage msg) {
            var state = _state.Value;
            state = StateUpdateModule.update(msg, state);
            _state.OnNext(state);
        }

        public FSharpFunc<Unit, DateTimeOffset> Clock { get; private set; }

        public IObservable<ShoppingListModule.ShoppingList> ShoppingList { get; }

        public IObservable<StateTypes.CategoryEditForm> CategoryEditPage { get; }

        public IObservable<StateTypes.StoreEditForm> StoreEditPage { get; }

        // Put all this inside F#
        // Rename "ItemForm" to "ItemEditForm" or rename all the others to just "StoreForm"
        public IObservable<StateTypes.ItemForm> ItemEditPage { get; }
    }
}

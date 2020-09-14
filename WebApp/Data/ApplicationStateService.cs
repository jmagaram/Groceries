using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using Models;
using static Models.StateTypes;

namespace WebApp.Data {
    public class ApplicationStateService {
        readonly BehaviorSubject<State> _state;
        readonly IObservable<State> _stateObs;

        public ApplicationStateService() {
            _state = new BehaviorSubject<State>(StateModule.createWithSampleData);
            _stateObs = _state;
            ShoppingListView = ShoppingListModule.fromObservable(_stateObs);
        }

        public void Update(StateMessage msg) {
            var state = _state.Value;
            state = StateModule.update(msg, state);
            _state.OnNext(state);
        }

        public IObservable<ViewTypes.ShoppingList> ShoppingListView { get;  }
    }
}

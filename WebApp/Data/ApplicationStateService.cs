using Models;
using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace WebApp.Data {
    public class ApplicationStateService {
        readonly BehaviorSubject<StateTypes.State> _state;
        readonly IObservable<StateTypes.State> _stateObs;

        public ApplicationStateService() {
            _state = new BehaviorSubject<StateTypes.State>(State.createWithSampleData);
            _stateObs = _state;
            ShoppingListView = ShoppingList.fromObservable(_stateObs);
        }

        public void Update(StateTypes.StateMessage msg) {
            var state = _state.Value;
            state = State.update(msg, state);
            _state.OnNext(state);
        }

        public IObservable<ViewTypes.ShoppingList> ShoppingListView { get;  }
    }
}

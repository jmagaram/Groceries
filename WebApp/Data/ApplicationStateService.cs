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
            _state = new BehaviorSubject<State>(StateModule.createWithSampleData);
            _stateObs = _state;
            ShoppingListQry = _stateObs.Select(Query.shoppingListQry);
        }

        public void Update(StateMessage msg) {
            var state = _state.Value;
            state = StateModule.update(msg, state);
            _state.OnNext(state);
        }

        public IObservable<QueryTypes.ShoppingListQry> ShoppingListQry { get; }
    }
}

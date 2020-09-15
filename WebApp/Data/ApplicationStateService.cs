using Microsoft.FSharp.Collections;
using Models;
using System;
using System.Reactive.Subjects;
using static Models.StateTypes;

namespace WebApp.Data {
    public class ApplicationStateService {
        readonly BehaviorSubject<State> _state;
        readonly IObservable<State> _stateObs;

        public ApplicationStateService() {
            _state = new BehaviorSubject<State>(StateModule.createWithSampleData);
            _stateObs = _state;
            Items = Query.items(_stateObs);
            Stores = Query.stores(_stateObs);
            Categories = Query.categories(_stateObs);
        }

        public void Update(StateMessage msg) {
            var state = _state.Value;
            state = StateModule.update(msg, state);
            _state.OnNext(state);
        }

        public IObservable<FSharpList<QueryTypes.StoreQry>> Stores { get; }

        public IObservable<FSharpList<QueryTypes.ItemQry>> Items { get; }
        public IObservable<FSharpList<QueryTypes.CategoryQry>> Categories { get; }
    }
}

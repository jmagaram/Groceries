using System;
using System.Reactive.Subjects;
using static Models.StateTypes;

namespace WebApp.Data {
    public class ApplicationStateService {
        readonly BehaviorSubject<State> _state;
        readonly IObservable<State> _stateObs;

        public ApplicationStateService() {
            _state = new BehaviorSubject<State>(Models.State.createWithSampleData);
            _stateObs = _state;
        }

        //public DomainTypes.State State { get; set; }

        //public IObservable<DomainTypes.State> StateObservable => _stateObs;

        public void Update(StateMessage msg) {
            var state = _state.Value;
            state = Models.State.update(msg, state);
            _state.OnNext(state);
        }
    }
}

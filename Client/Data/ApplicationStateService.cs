using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace Client.Data
{
    public class ApplicationStateService
    {
        BehaviorSubject<DomainTypes.State> _state;
        IObservable<DomainTypes.State> _stateObs;

        public ApplicationStateService()
        {
            _state = new BehaviorSubject<DomainTypes.State>(global::State.stateWithSampleItems);
            _stateObs = _state;
        }

        public DomainTypes.State State { get; set; }

        public IObservable<DomainTypes.State> StateObservable => _stateObs;

        public void Update(DomainTypes.StateMessage msg)
        {
            var state = _state.Value;
            var stateNext = global::State.update(msg, state);
            _state.OnNext(stateNext);
        }
    }
}

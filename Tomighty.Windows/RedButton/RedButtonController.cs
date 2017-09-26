using System;
using Tomighty.Events;
using Tomighty.Windows.Events;

namespace Tomighty.Windows {
    public class RedButtonController {
        readonly SynchronousEventHub eventHub;
        public RedButtonController(SynchronousEventHub eventHub) {
            this.eventHub = eventHub;
            eventHub.Subscribe<TimeElapsed>(OnTimeElasped);

        }

        void OnTimeElasped(TimeElapsed obj) {
            
        }

        public void Connect() {
            
        }
    }
}

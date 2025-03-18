using System;
using UnityEngine;

namespace Default {
    [Serializable]
    public struct InputQueue<T> {

        [Min(0f)] public float queuedInputDuration;
        private T _queuedInput;
        private float _queueTime;

        public void Queue(T isUp) {
            _queuedInput = isUp;
            _queueTime = Time.time;
        }

        public void TryDequeue(Action<T> dequeueAction) {
            if ((Time.time - _queueTime) < queuedInputDuration) dequeueAction.Invoke(_queuedInput);
        }

    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Malkuth.Player {
    public class Movement : MonoBehaviour {

        [Header("Parameters")]

        [SerializeField] private float _heightTop;
        private float _heightMid;
        [SerializeField] private float _heightBottom;
        [SerializeField, Min(0f)] private float _baseDuration;
        [SerializeField] private AnimationCurve _motionCurve;

        [Header("Input")]

        [SerializeField] private InputActionReference _actionUp;
        [SerializeField] private InputActionReference _actionDown;
        [SerializeField, Min(0f)] private float _queuedInputDuration;
        private bool _queuedInput;
        private float _queueTime;

        [Header("Cache")]

        private Vector3 _positionCache;
        private float _heightTarget;
        private float _motionProgress;

        private void Awake() {
            _positionCache = transform.position;
            _heightMid = transform.position.y;
        }

        private void Start() {
            _actionUp.action.performed += (context) => TryMove(true);
            _actionDown.action.performed += (context) => TryMove(false);
        }

        private void TryMove(bool isUp) {
            if(_motionProgress == 0) Move(isUp);
            else QueueMove(isUp);
        }

        private void Move(bool isUp) {
            StartCoroutine(MoveRoutine(isUp));
        }

        private IEnumerator MoveRoutine(bool isUp) {
            _heightTarget = isUp ? _heightTop : _heightBottom;
            while (_motionProgress < 1f) {
                _motionProgress += Time.deltaTime / _baseDuration;
                _positionCache[1] = Mathf.LerpUnclamped(_heightMid, _heightTarget, _motionCurve.Evaluate(_motionProgress));
                transform.position = _positionCache;

                yield return null;
            }
            _positionCache[1] = _heightMid;
            transform.position = _positionCache;
            _motionProgress = 0;
            TryDequeue();
        }

        private void QueueMove(bool isUp) {
            _queuedInput = isUp;
            _queueTime = Time.time;
        }

        private void TryDequeue() {
            if((Time.time - _queueTime) < _queuedInputDuration) Move(_queuedInput);
        }

#if UNITY_EDITOR
        private void OnValidate() {
            if(_queuedInputDuration > _baseDuration) _queuedInputDuration = _baseDuration;
        }
#endif

    }
}

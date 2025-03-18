using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Default;

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
        [SerializeField] private InputQueue<bool> _moveQueue;

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
            if (_motionProgress == 0) Move(isUp);
            else _moveQueue.Queue(isUp);
        }

        private void Move(bool isUp) {
            StartCoroutine(MoveRoutine(isUp));
        }

        private IEnumerator MoveRoutine(bool isUp) {
            _heightTarget = isUp ? _heightTop : _heightBottom;
            while (_motionProgress < 1f) {
                _motionProgress += Time.deltaTime / _baseDuration; // Change to check game speed
                _positionCache[1] = Mathf.LerpUnclamped(_heightMid, _heightTarget, _motionCurve.Evaluate(_motionProgress));
                transform.position = _positionCache;

                yield return null;
            }
            _positionCache[1] = _heightMid;
            transform.position = _positionCache;
            _motionProgress = 0;
            _moveQueue.TryDequeue(Move);
        }

#if UNITY_EDITOR
        private void OnValidate() {
            if(_moveQueue.queuedInputDuration > _baseDuration) _moveQueue.queuedInputDuration = _baseDuration;
        }
#endif

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Default;

namespace Malkuth.Player {
    public class Throw : MonoBehaviour {

        [Header("Parameters")]

        [SerializeField] private float _throwSpeed;
        [SerializeField] private float _throwDespawnLimit;
        [SerializeField] private Transform _grabHoldPoint;
        [SerializeField] private Collider2D _grabHitbox;
        [SerializeField] private ContactFilter2D _grabFilter;
        private Transform _objectGrabbed;

        [Header("Input")]

        [SerializeField] private InputActionReference _actionThrow;
        [SerializeField] private InputActionReference _actionGrab;
        [SerializeField] private InputQueue<bool> _throwQueue;

        private void Awake() {
            if (_grabHitbox == null) Debug.LogError("No grab hitbox assigned to 'Throw'!");
        }

        private void Start() {
            _actionThrow.action.performed += (context) => TryThrow();
            _actionGrab.action.performed += (context) => TryGrab();
        }

        private void TryThrow() {
            if (!_objectGrabbed) return;
            StartCoroutine(ThrowRoutine(_objectGrabbed));
            _objectGrabbed.parent = null;
            _objectGrabbed = null;
        }

        private IEnumerator ThrowRoutine(Transform objectThrown) {
            while (objectThrown.position.x < _throwDespawnLimit) {
                objectThrown.Translate(Vector3.right * _throwSpeed * Time.deltaTime); // To Optimize

                yield return null;
            }
            Destroy(objectThrown);
        }

        private void TryGrab() {
            if (_objectGrabbed) return;
            Transform closestGrabbable = null;
            List<Collider2D> results = new List<Collider2D>();
            for (int i = 0; i < _grabHitbox.Overlap(results); i++) {
                if (i == 0) closestGrabbable = results[0].transform;
                else if (results[i].transform.position.x < closestGrabbable.position.x) closestGrabbable = results[i].transform;
            }
            _objectGrabbed = closestGrabbable;
            if (_objectGrabbed != null) {
                _objectGrabbed.parent = _grabHoldPoint;
                _objectGrabbed.localPosition = Vector3.zero;
            }
        }

    }
}

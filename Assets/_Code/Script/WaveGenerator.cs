using System.Collections;
using UnityEngine;

namespace Malkuth.Wave {
    public class WaveGenerator : MonoBehaviour {

        [Header("Parameters")]

        [SerializeField, Min(0f)] private float _baseSpeed;
        [SerializeField, Min(0f)] private float _baseDelayMin;
        [SerializeField, Min(0f)] private float _baseDelayMax;
        [SerializeField] private GameObject[] _waveTemplates;
        [SerializeField] private float _waveDespawnLimit;

        private void Awake() {
            if (_waveTemplates.Length == 0) Debug.LogError("No wave template is set in 'WaveGenerator'!");
        }

        private void Update() {
            int i = 0;
            while (i < transform.childCount) {
                transform.GetChild(i).Translate(Vector3.left * _baseSpeed * Time.deltaTime); // Debug / To Optimize
                if (transform.GetChild(i).position.x < _waveDespawnLimit) Destroy(transform.GetChild(i).gameObject);
                i++;
            }
        }

        [ContextMenu("Start")]
        public void GenerateStart() {
            StartCoroutine(GenerateRoutine());
        }

        private IEnumerator GenerateRoutine() {
            while (true) { // Change to check for game status
                GenerateWave();

                yield return new WaitForSeconds(Random.Range(_baseDelayMin, _baseDelayMax)); // Change to check for game speed
            }
        }

        private void GenerateWave() {
            Instantiate(_waveTemplates[Random.Range(0, _waveTemplates.Length)], transform);
        }

    }
}

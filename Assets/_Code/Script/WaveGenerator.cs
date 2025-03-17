using System.Collections;
using UnityEngine;

namespace Malkuth.Wave {
    public class WaveGenerator : MonoBehaviour {

        [Header("Parameters")]

        [SerializeField, Min(0f)] private float _baseSpeed;
        [SerializeField, Min(0f)] private float _baseDelayMin;
        [SerializeField, Min(0f)] private float _baseDelayMax;
        [SerializeField] private GameObject[] _waveTemplates;

        private void Update() {
            for(int i = 0; i < transform.childCount; i++) transform.GetChild(i).Translate(Vector3.left * _baseSpeed * Time.deltaTime); // Debug
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

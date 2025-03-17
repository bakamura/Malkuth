using UnityEngine;

namespace Default {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

        [Header("Header")]
        [field: SerializeField] public T Instance { get; private set; }

        protected virtual void Awake() {
            if (Instance == null) Instance = this as T;
            else if (Instance != this) {
                Debug.LogWarning($"Duplicate instance of '{typeof(T).Name}', destroying it!");
                Destroy(gameObject);
            }
        }

    }
}

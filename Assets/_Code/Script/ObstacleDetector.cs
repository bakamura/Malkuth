using UnityEngine;

namespace Malkuth.Player {
    public class ObstacleDetector : MonoBehaviour {

        [Header("Parameters")]

        [SerializeField] private int _obstacleLayer;

        private void OnCollisionEnter2D(Collision2D collision) {
            if(collision.gameObject.layer == _obstacleLayer) TakeHit();
        }
        
        private void TakeHit() {
            Debug.Log("Took hit");
        }

    }
}

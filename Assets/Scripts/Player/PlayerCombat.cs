using System;
using UnityEngine;

namespace Player {
    public class PlayerCombat : MonoBehaviour {
        [SerializeField] private int _health;
        [SerializeField] private Collider _weaponCollider;

        public event Action PlayerDied;

        public void TakeDamage(int damage) {
            _health -= damage;
            if (_health <= 0) {
                HandleDeath(); 
            }
        }

        private void HandleDeath() {
            PlayerDied?.Invoke();
        }
    }
}

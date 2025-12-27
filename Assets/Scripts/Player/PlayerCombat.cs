using System;
using Combat;
using UnityEngine;

namespace Player {
    public class PlayerCombat : MonoBehaviour {
        [SerializeField] private int _health;
        [SerializeField] private WeaponBase[] _weapons;

        private int _activeWeaponSlot = 0;
        
        public event Action PlayerDied;

        public void TriggerWeaponImpact()
        {
            WeaponBase activeWeapon = _weapons[_activeWeaponSlot];
            activeWeapon.OnAttackImpact(); 
            Debug.Log("Triggered weapon impact");
        }

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

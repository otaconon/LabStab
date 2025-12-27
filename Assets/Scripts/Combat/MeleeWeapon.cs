using System;
using Player;
using UnityEngine;

namespace Combat {
    [RequireComponent(typeof(Collider))]
    public class MeleeWeapon : MonoBehaviour {
        [SerializeField] protected int _damage = 1;

        private Collider _hitbox;

        private void Start() {
            _hitbox = GetComponent<Collider>();
            _hitbox.enabled = false;
        }

        public void EnableHitbox() {
            _hitbox.enabled = true;
        }

        public void DisableHitbox() {
            _hitbox.enabled = false;
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) {
                return;
            }

            var playerCombat = other.GetComponentInParent<PlayerCombat>();
            if (playerCombat == null) {
                return;
            }

            playerCombat.TakeDamage(_damage);
        }
    }
}
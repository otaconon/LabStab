using Player;
using UnityEngine;

namespace Combat {
    [System.Serializable]
    public class Gun : WeaponBase {
        [SerializeField] private Transform _originTransform;
        [SerializeField] private float _range = 50f;
        [SerializeField] private int _damage = 0;

        public override void OnAttackStart() {
        }

        public override void OnAttackImpact() {
            Shoot();
        }

        public override void OnAttackEnd() {
        }

        public void Shoot() {
            RaycastHit hit;
            if (!Physics.Raycast(_originTransform.position, _originTransform.forward, out hit, _range)) {
                return;
            }

            Debug.Log("Hit: " + hit.transform.name);

            var target = hit.transform.GetComponent<PlayerCombat>();
            if (target != null) {
                target.TakeDamage(_damage);
            }
        }
    }
}
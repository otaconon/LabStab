using UnityEngine;

namespace Combat {
    public abstract class WeaponBase : MonoBehaviour {
        public abstract void OnAttackStart();
        public abstract void OnAttackImpact();
        public abstract void OnAttackEnd();
    }
}
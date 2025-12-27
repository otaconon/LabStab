using UnityEngine;

namespace Combat {
    public abstract class WeaponBase : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] public string AnimationTriggerName;

        public abstract void OnAttackStart();
        public abstract void OnAttackImpact();
        public abstract void OnAttackEnd();
    }
}
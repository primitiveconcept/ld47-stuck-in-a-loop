namespace StuckInALoop
{
    using System;
    using PrimitiveEngine.Unity;
    using Spineless;
    using Spineless.Extensions.Components;
    using UnityEngine;


    public class Pickup : MonoBehaviour
    {
        public bool IsWeapon;
        public bool DestroyOnPickup = true;
        public AudioClip PickupSound;
        public Collider2DEvent OnPickup;


        public void PickUp(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            if (this.OnPickup != null)
                this.OnPickup.Invoke(other);

            if (this.IsWeapon)
            {
                AttackSpawner pickupAttackSpawner = GetComponent<AttackSpawner>();
                if (pickupAttackSpawner != null)
                    ReplaceAttackSpawner(other.gameObject, pickupAttackSpawner);    
            }

            if (this.PickupSound != null)
                AudioPlayer.Play(this.PickupSound, false);

            if (this.DestroyOnPickup)
                Destroy(this.gameObject);
        }


        public void ReplaceAttackSpawner(GameObject target, AttackSpawner attackSpawner)
        {
            AttackSpawner existingAttackSpawner = target.GetComponent<AttackSpawner>();
            
            bool shouldEnable = true;
            if (existingAttackSpawner != null)
            {
                shouldEnable = existingAttackSpawner.enabled;
                //Destroy(existingAttackSpawner);
            }

            AttackSpawner createdAttackSpawner = attackSpawner.CopyComponentTo(target);
            createdAttackSpawner.enabled = shouldEnable;
        }
    }
}
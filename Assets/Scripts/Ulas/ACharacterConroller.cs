using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Character
{
    public abstract class ACharacterConroller : MonoBehaviour
{
        [SerializeField] protected float Health;
        [SerializeField] protected float RunSpeed;
        [SerializeField] protected float BackWalkSpeed;
        [SerializeField] protected float JumpingPower;

        internal abstract void Move();
        internal abstract void Damage();
        internal abstract void TakeDamage();
        internal abstract void Dead();
        internal abstract bool IsGrounded();
    }   
}




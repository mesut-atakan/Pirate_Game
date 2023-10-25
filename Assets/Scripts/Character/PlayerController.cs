using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;



namespace Character
{
    // siniflarin alabildigi erisim belirtecleri sadece => private, internal

    [RequireComponent(typeof(Rigidbody2D))]
    class PlayerController : MonoBehaviour
    {
        // Erisim Belirtecleri
        // public, private, internal, protected




#region ||~~~~~~~~~~|| XX ||~~~~~~~~~~|| SERIALIZE FIELDS ||~~~~~~~~~~|| XX ||~~~~~~~~~~||
        // properties
        // speed, Gravity, RigidBody2D, isArmor, respawnPoint => bool,
        [Header("Character Properties")]

        
        [Tooltip("The variable that determines the character's speed.")]
        [SerializeField] private float regularSpeed = 5.0f;
        // forward speed    = 10
        // backward speed = -3

        
        [Tooltip("Does the character have armor or not?")]
        [SerializeField] private bool isArmor = false;







        [Header("Components")]

        [Tooltip("We will move the character with the rigidbody component!")]
        [SerializeField] private Rigidbody2D rb;

        



#endregion ||~~~~~~~~~~|| XX ||~~~~~~~~~~|| XXXX ||~~~~~~~~~~|| XX ||~~~~~~~~~~||






#region ||~~~~~~~~~~|| XX ||~~~~~~~~~~|| PRIVATE FIELDS ||~~~~~~~~~~|| XX ||~~~~~~~~~~||

        private bool _respawnPoint = false;

#endregion ||~~~~~~~~~~|| XX ||~~~~~~~~~~|| XXXX ||~~~~~~~~~~|| XX ||~~~~~~~~~~||










        private void FixedUpdate() {
            RegularMove();
        }







        private void RegularMove()
        {
            // oyuncu a ya basiyorsa
                // backwardSpeed
            // karketer d ye basyisa
                // forwardSpeed
            rb.velocity = new Vector2(regularSpeed * Time.deltaTime, rb.velocity.y);
        }
    }
}
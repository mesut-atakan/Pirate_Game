using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



namespace Character
{
    internal class PlayerController : MonoBehaviour
    {
#region ||~~~~~|| X ||~~~~~|| SERIALIZE FIELDS ||~~~~~|| X ||~~~~~||

        [Header("Classes")]

        [SerializeField] private GameManager gameManager;



        [Header("CharcterProperties")]

        [Tooltip("Enter the character's forward speed")]
        [SerializeField] [Range(0, 10)] private float forwardSpeed = 5.0f;


        [Tooltip("Enter the character's backward speed!")]
        [SerializeField] [Range(0, 10)] private float backwardSpeed = 2.0f;











        [Space(15f), Header("Jump Properties")]

        [Tooltip("Enter the character's jumping power here!")]
        [SerializeField] private float jumpForce = 8;


        [Tooltip("To check whether the character touches the ground, select the layer adapted for the ground!")]
        [SerializeField] private LayerMask groundLayerMask;


        [Tooltip("To understand whether the character is on a platform, assign the layer adapted for the platform to this variable!")]
        [SerializeField] private LayerMask platformLayerMask;


        [Tooltip("Enter the transform component of the point object that will control the ground into this variable!")]
        [SerializeField] private Transform groundCheckTransform;

        
        [SerializeField] private Vector2 groundCheckTransformSize;















        [Space(15.0f), Header("Slip Properties")]

        [Tooltip("`DEMO` Enter the size of the character as the character scrolls!")]
        [SerializeField] private Vector2 characterSize;

        
        [Tooltip("Enter how long the character will be dragged on the ground!")]
        [SerializeField, Range(0.0f, 3.0f)] private float slipTime = 1.0f;


        [Tooltip("Enter how long the character can drift!")]
        [SerializeField, Range(0.0f, 5.0f)] private float reSlipTime = 3.0f; 


        [Tooltip("Specify how much faster the character's speed will be when the character is drifting!")]
        [SerializeField, Range(0.0f, +5.0f)] private float characterSlipSpeed = 0.4f;




















        [Space(15.0f), Header("Components")]

        [Tooltip("Since we will be moving our character with Physics, add the 'Rigidbody2D' component here!")]
        [SerializeField] private Rigidbody2D rb;


        [Tooltip("Assign the 'Capsule Collider' component, which controls the character's collision with objects, to this variable!")]
        [SerializeField] private new Collider2D collider;

#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||








#region ||~~~~~|| X ||~~~~~|| PRIVATE FIELDS ||~~~~~|| X ||~~~~~||

        // To prevent your liver developer friends from entering high values ​​for character speed, multiply this variable where the character's speed is multiplied!
        private const int characterSpeedMultiply = 100;



        private bool _isGround = false;     // With this variable, we will understand whether the character will contact the ground or not!

        private bool _isPlatform = false;   // With this variable, we will understand whether the character is on a platform or not.

#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||









#region ||~~~~~|| X ||~~~~~|| PROPERTIES ||~~~~~|| X ||~~~~~||

        internal Vector2 _groundCheckTransformSize { get => this.groundCheckTransformSize; }

        internal Transform _groundCheckTransform { get => this.groundCheckTransform; }

#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||















        /// <summary>
        /// With this method you can make the character move!
        /// </summary>
        internal void Move()
        {
            // ~~ Variables ~~
            // The data of whether the player presses a key on the horizontal axis or not will be stored in this variable!
            float _horizontal;
            float _characterSpeed;
            
            _horizontal = Input.GetAxisRaw("Horizontal");

            // If the character does not scroll
            
            
            if (_horizontal > 0 && this.transform.position.x <= 8)
            {
                _characterSpeed = this.forwardSpeed;
            }
            else if (_horizontal < 0 && this.transform.position.x >= -8)
            {
                _characterSpeed = this.backwardSpeed;
            }
            else
            {
                _characterSpeed = 0;
            }
            



            this.rb.velocity = new Vector2(_characterSpeed * _horizontal * characterSpeedMultiply * Time.deltaTime, this.rb.velocity.y);
        }










        /// <summary>
        /// With this method, the character's jumping activity will be realized!
        /// </summary>
        internal void Jump()
        {
            // ~~ Variables ~~
            
            this._isGround = Physics2D.OverlapCapsule(this.groundCheckTransform.position, this.groundCheckTransformSize, CapsuleDirection2D.Horizontal, 0, this.groundLayerMask);

            if (this._isGround)
            {
                this.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }










        /// <summary>
        /// With this method, the character will be able to go down from the platform he is on!
        /// </summary>
        internal IEnumerator FallDown()
        {
            this._isPlatform = Physics2D.OverlapCapsule(this.groundCheckTransform.position, this.groundCheckTransformSize, CapsuleDirection2D.Horizontal, 0, this.platformLayerMask);

            if (_isPlatform)
            {
                this.collider.excludeLayers = this.platformLayerMask;
                yield return new WaitForSeconds(0.47f);
                this.collider.excludeLayers = 0;
            }
        }
    }
}
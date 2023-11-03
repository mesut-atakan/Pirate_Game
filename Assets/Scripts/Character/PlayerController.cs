using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
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





        [Header("Jump Up Platform Control")]

        [Tooltip("Enter the transform component of the Object that will check whether there is a platform on the character or not!")]
        [SerializeField] private Transform upPlatformControlTransform;















        [Space(15.0f), Header("Slip Properties")]

        [Tooltip("`DEMO` Enter the size of the character as the character scrolls!")]
        [SerializeField] private Vector2 characterSize;

        
        [Tooltip("Enter how long the character will be dragged on the ground!")]
        [SerializeField, Range(0.0f, 3.0f)] private float slipTime = 1.0f;


        [Tooltip("Enter how long the character can drift!")]
        [SerializeField, Range(0.0f, 5.0f)] private float reSlipTime = 3.0f; 


        [Tooltip("Specify how much faster the character's speed will be when the character is drifting!")]
        [SerializeField, Range(0.0f, +5.0f)] private float characterSlipSpeed = 0.4f;











        [Space(15f), Header("Fly Fields")]

        [Tooltip("Enter the gravity value required for the character to fly!")]
        [SerializeField, Range(0.05f, 2.5f)] private float characterFlyGravityScale = 0.15f;


        [Tooltip("Enter how fast the character will go in the air!")]
        [SerializeField, Range(0, 10)] private float flySpeed = 3.0f;


        [Tooltip("karakter suzuklmeye baslarken ne kadar guc ile yukariya dogru ziplayacagini giriniz!")]
        [SerializeField, Range(0, 3f)] private float flyJumpForce = 1.0f;

























        [Space(15f), Header("Damage Fields")]


        [Tooltip("How many melee hits will the opponent Maksimum take to die?")]
        [SerializeField, Range(1, 10)] private ushort maxAttackDie = 1;

        [Tooltip("Enter the size of the character's melee area")]
        [SerializeField] private Vector2 infightingAreaSize;

        [Tooltip("Add the Transform component of the Melee field to this variable!")]
        [SerializeField] private Transform infightingAreaTransform;

        
















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

        // ~~ SLIP ~~
        private bool _characterIsSlip = false;  // The variable that will control whether the character momentarily slides or not!

        private bool _canSlip = true;       // Variable that controls whether the character can be dragged on the ground or not!

    #region Skills
        // double jump
        private bool _doubleJump = false;

        // Fly
        private bool _fly = false;
    #endregion




        // Timer
        private float currentTime = 0.0f;
        private float _maxUpPlatformControlTimer = 0.62f;
#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||









#region ||~~~~~|| X ||~~~~~|| PROPERTIES ||~~~~~|| X ||~~~~~||

        internal Vector2 _groundCheckTransformSize { get => this.groundCheckTransformSize; }

        internal Transform _groundCheckTransform { get => this.groundCheckTransform; }

        // ~~ Damage ~~
        internal Vector2 _infightingAreaSize { get => this.infightingAreaSize; }
        internal Transform _infightingAreaTransform { get => this.infightingAreaTransform; }


        internal Transform _upPlatformControlTransform { get => this.upPlatformControlTransform; }

        



        internal bool _flyPrp { get => this._fly; }
        internal bool _canFly { get; set; } = false;
        internal GameObject _ramPlatform { get; set; }

#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||






        private void OnTriggerEnter2D(Collider2D other) {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.layer == LayerMask.NameToLayer("Skill"))
            {
                Debug.Log("A");
                if (other.gameObject.tag == "DoubleJump")
                {
                    this._doubleJump = true;
                    other.gameObject.SetActive(false);
                }
                else if (other.gameObject.CompareTag("Fly"))
                {
                    this._canFly = true;
                    other.gameObject.SetActive(false);
                }
            }
        }






















        /// <summary>
        /// With this method you can make the character move!
        /// </summary>
        internal void Move()
        {
            // ~~ Variables ~~
            // The data of whether the player presses a key on the horizontal axis or not will be stored in this variable!
            float _horizontal = 0.0f;
            float _characterSpeed = 0.0f;
            

            // If the character does not scroll

            if (this._characterIsSlip)
            {
                _characterSpeed = this.forwardSpeed + this.characterSlipSpeed;
                this.rb.velocity = new Vector2(_characterSpeed * characterSpeedMultiply * Time.deltaTime, this.rb.velocity.y);
            }
            else
            {
                _horizontal = Input.GetAxisRaw("Horizontal");
                if (_horizontal > 0 && this.transform.position.x <= this.gameManager._cameraXMax)
                {
                    _characterSpeed = this.forwardSpeed;
                }
                else if (_horizontal < 0 && this.transform.position.x >= this.gameManager._cameraXMin)
                {
                    _characterSpeed = this.backwardSpeed;
                }
                else
                {
                    _characterSpeed = 0;
                }

                if (!this._fly)
                    this.rb.velocity = new Vector2(_characterSpeed * _horizontal * characterSpeedMultiply * Time.deltaTime, this.rb.velocity.y);
            }
        }










        /// <summary>
        /// With this method, the character's jumping activity will be realized!
        /// </summary>
        internal void Jump()
        {
            // ~~ Variables ~~
            
            this._isGround = CharacterIsGround();

            if (this._isGround)
            {
                this.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                return;
            }
            else if (this._doubleJump)
            {
                this.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                this._doubleJump = false;

                return;
            }
        }



        /// <summary>
        /// Function that controls whether the character comes into contact with the ground or not
        /// </summary>
        /// <returns>If the character touches the ground, it will be true, otherwise it will be false!</returns>
        private bool CharacterIsGround()
        {
            return Physics2D.OverlapCapsule(this.groundCheckTransform.position, this.groundCheckTransformSize, CapsuleDirection2D.Horizontal, 0, this.groundLayerMask);
        }



        /// <summary>
        /// Returns the ground the character momentarily touches!
        /// </summary>
        /// <returns>If the character is not touching a ground, this method may return null!</returns>
        private GameObject CharacterIsGroundObject()
        {
            return Physics2D.OverlapCapsule(this.groundCheckTransform.position, this.groundCheckTransformSize, CapsuleDirection2D.Horizontal, 0, this.groundLayerMask)?.gameObject;
        }





        private IEnumerator JumpPlatform()
        {
            // ~~ Variables ~~
            GameObject _platform;
            Collider2D _collider;

            while (this.currentTime <= this._maxUpPlatformControlTimer)
            {
                _platform = Physics2D.OverlapBox(this.upPlatformControlTransform.position, this.groundCheckTransformSize, 0, this.platformLayerMask)?.gameObject;
                if (_platform != null)
                {
                    Debug.Log("Ust tarfata platform var!!!!");
                    _collider = _platform.GetComponent<Collider2D>();
                    _collider.excludeLayers = this.gameObject.layer;
                    StartCoroutine(JumpPlatformColliderClear(_collider));
                    yield return null;
                }
                yield return new WaitForSeconds(0.1f);
                this.currentTime += 0.1f;
            }
            this.currentTime = 0.0f;
        }




        private IEnumerator JumpPlatformColliderClear(Collider2D collider)
        {
            yield return new WaitForSeconds(0.5f);
            collider.excludeLayers = 0;
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







        /// <summary>
        /// The method that must be used to make the character drift on the ground!
        /// </summary>
        internal IEnumerator Slip()
        {
            // We check whether the character is momentarily dragged on the ground and whether he can stay at the moment!
            if (!this._characterIsSlip && this._canSlip && CharacterIsGround()) 
            {
                this._characterIsSlip = true;       // We mark the `_characterIsSlip` variable as true to indicate that the character is dragging on the ground!
                this.transform.localScale = characterSize;  // We change the value of the character to see if the character is drifting momentarily!

                yield return new WaitForSeconds(this.slipTime); // We are entering the waiting period!

                this.transform.localScale = new Vector2(1, 1);      // We are correcting the character's scale values!
                this._characterIsSlip = false;                  // We state that the character is now dragging on the ground!
                this._canSlip = false;
                StartCoroutine(SlipWaitter());
            }
        }



        /// <summary>
        /// With this method, you determine how quickly the character can slide!
        /// </summary>
        private IEnumerator SlipWaitter()
        {
            yield return new WaitForSeconds(this.reSlipTime);
            this._canSlip = true;
        }




        /// <summary>
        /// This method will be used for the character to attack!
        /// </summary>
        internal void Attack()
        {
            // ~~ Variables ~~
            GameObject _interactionObject;

            _interactionObject = Physics2D.OverlapBox(this.infightingAreaTransform.position, this.infightingAreaSize, 0.0f)?.gameObject;

            if (_interactionObject == null) return;


            
            if (_interactionObject.tag == "Enemy")
            {
                _interactionObject.SetActive(false);
                return;
            }
        }



        /// <summary>
        /// With this method, you can start the character floating in the air!
        /// </summary>
        internal void FlyStart()
        {
            this._ramPlatform = CharacterIsGroundObject();
            this.rb.velocity = new Vector2(this.rb.velocity.x + this.forwardSpeed * this.flySpeed * characterSpeedMultiply * Time.deltaTime, this.rb.velocity.y);
            this.rb.AddForce(Vector2.up * flyJumpForce, ForceMode2D.Impulse);
            this._fly = true;
            this.rb.gravityScale = this.characterFlyGravityScale;
        }






        internal void FlyControl()
        {
            if (CharacterIsGround() && this._ramPlatform != CharacterIsGroundObject())
            {
                this.rb.gravityScale = 2.5f;
                this._fly = false;
                this._canFly = false;
            }
            else
            {
                if (this.transform.position.x > this.gameManager._cameraXMax)
                {
                    this.transform.position = new Vector2(this.gameManager._cameraXMax, this.transform.position.y);
                }

            }
        }
    }
}
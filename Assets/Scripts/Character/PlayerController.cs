using UnityEngine;



namespace Character
{
    internal class PlayerController : MonoBehaviour
    {
#region ||~~~~~|| X ||~~~~~|| SERIALIZE FIELDS ||~~~~~|| X ||~~~~~||

        [Header("CharcterProperties")]

        [Tooltip("Enter the character's forward speed")]
        [SerializeField] [Range(0, 10)] private float forwardSpeed = 5.0f;


        [Tooltip("Enter the character's backward speed!")]
        [SerializeField] [Range(0, 10)] private float backwardSpeed = 2.0f;







        [Space(15.0f), Header("Components")]

        [Tooltip("Since we will be moving our character with Physics, add the 'Rigidbody2D' component here!")]
        [SerializeField] private Rigidbody2D rb;

#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||








#region ||~~~~~|| X ||~~~~~|| PRIVATE FIELDS ||~~~~~|| X ||~~~~~||

        // To prevent your liver developer friends from entering high values ​​for character speed, multiply this variable where the character's speed is multiplied!
        private const int characterSpeedMultiply = 100;

#endregion






        private void FixedUpdate() 
        {
            Move();
        }





        /// <summary>
        /// With this method you can make the character move!
        /// </summary>
        private void Move()
        {
            // ~~ Variables ~~
            // The data of whether the player presses a key on the horizontal axis or not will be stored in this variable!
            float _horizontal;

            float _characterSpeed;
            
            _horizontal = Input.GetAxisRaw("Horizontal");

            if (_horizontal > 0 && this.transform.position.x <= 8)
                _characterSpeed = this.forwardSpeed;
            else if (_horizontal < 0 && this.transform.position.x >= -8)
                _characterSpeed = this.backwardSpeed;
            else
            _characterSpeed = 0;

            this.rb.velocity = new Vector2(_characterSpeed * _horizontal * characterSpeedMultiply * Time.deltaTime, this.rb.velocity.y);
            
        }
    }
}
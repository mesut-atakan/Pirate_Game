using Character;
using UnityEngine;



internal class GameManager : MonoBehaviour
{
#region ||~~~~~|| X ||~~~~~|| SERIALIZE FIELDS ||~~~~~|| X ||~~~~~||
    [Header("Classes")]


    [SerializeField] private PlayerController playerController;


    [SerializeField] private MapIllusion mapIllusion;

    [Space(15f), SerializeField] private InputManager inputManager;











    [Header("Components")]

    [Tooltip("Insert the camera in the scene here!")]
    [SerializeField] private Camera mainCamera;



#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||









#region ||~~~~~|| X ||~~~~~|| PRIVATE FIELDS ||~~~~~|| X ||~~~~~||

    internal bool _slipTimerStart { get; set; } = true;




    // ~~ Camera ~~
    private float _cameraHeight;
    private float _cameraWidth;

#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||












#region ||~~~~~|| X ||~~~~~|| PROPERTIES ||~~~~~|| X ||~~~~~||

    internal float _cameraXMin { get; set; }
    internal float _cameraXMax { get; set; }
    

#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||















    private void Awake() {
        this.mainCamera = Camera.main;
    }









    private void Start() {
        // We used it to calculate the boundaries of objects within the camera angle
        this._cameraHeight = 2f * this.mainCamera.orthographicSize;
        this._cameraWidth = _cameraHeight * mainCamera.aspect;

        // The center position of the camera will be found!
        Vector2 _cameraPosition = this.mainCamera.transform.position;

        // Minimum and maximum values ​​on the x-axis entering the camera angle!
        this._cameraXMin = _cameraPosition.x - this._cameraWidth / 2;
        this._cameraXMax = _cameraPosition.x + _cameraWidth / 2;
    }















    private void FixedUpdate() {

#region ||~~~~~|| X ||~~~~~|| PLAYER ||~~~~~|| X ||~~~~~||
        this.playerController.Move();


        if (Input.GetKey(this.inputManager._characterPlatformDownKey))
        {
            StartCoroutine(this.playerController.FallDown());
        }


#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||
    }




    private void Update() 
    {
#region ||~~~~~|| X ||~~~~~|| PLAYER ||~~~~~|| X ||~~~~~||

        if (Input.GetKeyDown(inputManager._characterJumpKey))
        {
            this.playerController.Jump();
        }
        
        if (Input.GetKeyDown(this.inputManager._characterSlipKey))
        {
            StartCoroutine(this.playerController.Slip());
        }

#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||
    }







    private void LateUpdate()
    {
        mapIllusion.MapMove();
    }







    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(this.playerController._groundCheckTransform.position, this.playerController._groundCheckTransformSize);
    }
}
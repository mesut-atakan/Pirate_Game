using Character;
using UnityEngine;



internal class GameManager : MonoBehaviour
{
#region ||~~~~~|| X ||~~~~~|| SERIALIZE FIELDS ||~~~~~|| X ||~~~~~||
    [Header("Classes")]


    [SerializeField] private PlayerController playerController;


    [SerializeField] private MapIllusion mapIllusion;

    [Space(15f), SerializeField] private InputManager inputManager;



#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||









#region Private Fields

    internal bool _slipTimerStart { get; set; } = true;

#endregion





























    private void FixedUpdate() {

#region ||~~~~~|| X ||~~~~~|| PLAYER ||~~~~~|| X ||~~~~~||
        this.playerController.Move();

        if (Input.GetKey(inputManager._characterJumpKey))
        {
            this.playerController.Jump();
        }

        if (Input.GetKey(this.inputManager._characterPlatformDownKey))
        {
            StartCoroutine(this.playerController.FallDown());
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
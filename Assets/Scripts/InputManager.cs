using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;



[System.Serializable]
internal struct InputManager
{
#region ||~~~~~|| X ||~~~~~|| SERIALIZE FIELDS ||~~~~~|| X ||~~~~~||
    [Header("Controller Keys")]

    [Tooltip("Enter the key that the player must press to make the character drift on the ground!")]
    [SerializeField] private KeyCode characterSlipKey;



    [Tooltip("Enter the key that the player will press to have the character go down from a platform!")]
    [SerializeField] private KeyCode characterPlatformDownKey;




    [Tooltip("Enter the key the player must press for the character to jump!")]
    [SerializeField] private KeyCode characterJumpKey;



#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||












#region ||~~~~~|| X ||~~~~~|| PROPERTIES ||~~~~~|| X ||~~~~~||

    internal KeyCode _characterSlipKey { get => this.characterSlipKey; } 

    internal KeyCode _characterJumpKey { get => this.characterJumpKey; }

    internal KeyCode _characterPlatformDownKey { get => this.characterPlatformDownKey; }

#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||


}
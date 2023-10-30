using UnityEngine;



internal class MapIllusion : MonoBehaviour
{
#region ||~~~~~|| X ||~~~~~|| SERIALIZE FIELDS ||~~~~~|| X ||~~~~~||

    [Header("Illusion Properties")]

    [Tooltip("Enter how fast you will advance on the map!")]
    [SerializeField, Range(0, 10)] private float mapSpeed;

#endregion ||~~~~~|| X ||~~~~~|| X  X   X   X ||~~~~~|| X ||~~~~~||


















    internal void MapMove()
    {
        this.transform.position = new Vector2(this.transform.position.x - this.mapSpeed * Time.deltaTime, this.transform.position.y);
    }
}

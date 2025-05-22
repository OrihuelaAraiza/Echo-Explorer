using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [Tooltip("Waypoints de esta ruta (en orden)")]
    public Transform[] points;

    public Transform[] GetPoints() => points;
}

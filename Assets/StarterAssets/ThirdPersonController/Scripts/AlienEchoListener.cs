using UnityEngine;

/// <summary>
/// Se suscribe al ping y avisa al AlienController si está lo bastante cerca.
/// </summary>
[RequireComponent(typeof(AlienController))]
public class AlienEchoListener : MonoBehaviour
{
    public float hearingRadius = 40f;

    AlienController ctrl;

    void Awake() => ctrl = GetComponent<AlienController>();
    void OnEnable() => EchoEventManager.OnEcho += Heard;
    void OnDisable() => EchoEventManager.OnEcho -= Heard;

    void Heard(Vector3 origin, float radius)
    {
        float d = Vector3.Distance(transform.position, origin);
        if (d <= Mathf.Min(radius, hearingRadius))
            ctrl.Chase(origin);
    }
}

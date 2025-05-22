using UnityEngine;
using System;

/// <summary>Evento global del ping sonar.</summary>
public static class EchoEventManager
{
    /// <param name="origin">Posición del ping</param>
    /// <param name="radius">Alcance del sonido en metros</param>
    public static event System.Action<Vector3, float> OnEcho;

    public static void Broadcast(Vector3 origin, float radius)
    {
        Debug.Log($"[Ping] Broadcast en {origin}  (radio {radius})");
        OnEcho?.Invoke(origin, radius);
    }
}

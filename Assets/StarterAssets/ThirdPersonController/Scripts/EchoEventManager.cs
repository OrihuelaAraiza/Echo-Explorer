using UnityEngine;
using System;

/// <summary>
/// Dispatcher global: cualquier objeto puede disparar un ping y
/// cualquier listener puede suscribirse sin referencias explícitas.
/// </summary>
public static class EchoEventManager
{
    /// <param name="origin">Posición exacta del ping.</param>
    /// <param name="radius">Alcance máximo del sonido (metros).</param>
    public static event Action<Vector3, float> OnEcho;

    /// <summary>Invoca el evento. Llama solo desde el jugador.</summary>
    public static void Broadcast(Vector3 origin, float radius)
    {
        OnEcho?.Invoke(origin, radius);
    }
}

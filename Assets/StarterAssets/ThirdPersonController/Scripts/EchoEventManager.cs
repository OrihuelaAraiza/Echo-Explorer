using UnityEngine;
using System;

/// <summary>
/// Dispatcher global: cualquier objeto puede disparar un ping y
/// cualquier listener puede suscribirse sin referencias expl�citas.
/// </summary>
public static class EchoEventManager
{
    /// <param name="origin">Posici�n exacta del ping.</param>
    /// <param name="radius">Alcance m�ximo del sonido (metros).</param>
    public static event Action<Vector3, float> OnEcho;

    /// <summary>Invoca el evento. Llama solo desde el jugador.</summary>
    public static void Broadcast(Vector3 origin, float radius)
    {
        OnEcho?.Invoke(origin, radius);
    }
}

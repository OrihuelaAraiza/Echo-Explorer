using UnityEngine;

/// <summary>
/// Clona el material con EchoShader y copia texturas.
/// Mantiene rojo/transparente durante el tiempo indicado.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class EchoReceiver : MonoBehaviour
{
    [Tooltip("Material plantilla con EchoShader")]
    public Material echoTemplate;

    [Tooltip("Tiempo que permanece rojo desde que oye el ping (segundos)")]
    public float flashDuration = 2f;          // ← pon el mismo que fadeduration del IncreaseScript

    Renderer rend;
    static readonly int idTransp = Shader.PropertyToID("_Transparency");

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (!echoTemplate) { Debug.LogError("EchoReceiver: falta template", this); return; }

        Material inst = new Material(echoTemplate);
        CopyTextures(rend.sharedMaterial, inst);
        rend.material = inst;
    }

    void OnEnable() => EchoEventManager.OnEcho += Flash;
    void OnDisable() => EchoEventManager.OnEcho -= Flash;

    void Flash(Vector3 origin, float radius)
    {
        if (Vector3.Distance(transform.position, origin) > radius) return;
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    System.Collections.IEnumerator FlashRoutine()
    {
        float t = 0;
        // valor 1 al recibir el ping
        rend.material.SetFloat(idTransp, 1f);

        while (t < flashDuration)
        {
            float v = Mathf.Lerp(1f, 0f, t / flashDuration);  // 1 → 0
            rend.material.SetFloat(idTransp, v);
            t += Time.deltaTime;
            yield return null;
        }
        rend.material.SetFloat(idTransp, 0f);
    }

    // ----------------------------------------------------------
    void CopyTextures(Material src, Material dst)
    {
        if (!src || !dst) return;
        foreach (var name in src.GetTexturePropertyNames())
            if (dst.HasProperty(name))
                dst.SetTexture(name, src.GetTexture(name));
    }
}

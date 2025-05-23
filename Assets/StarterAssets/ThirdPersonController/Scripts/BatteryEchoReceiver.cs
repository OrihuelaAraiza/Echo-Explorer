using UnityEngine;
using System.Collections;

/// <summary>
/// Clona un material EchoShader (amarillo) y lo hace visible solo
/// durante el eco. De resto, alpha = 0 así se ve negro como los aliens.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class BatteryEchoReceiver : MonoBehaviour
{
    [Tooltip("Material plantilla con EchoShader (color amarillo)")]
    public Material echoTemplate;

    [Tooltip("Tiempo que permanece amarillo (segundos)")]
    public float flashDuration = 2f;

    Renderer rend;
    Material mat;                        // instancia local
    static readonly int idCol = Shader.PropertyToID("_BaseColor");
    static readonly int idTran = Shader.PropertyToID("_Transparency");

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (!echoTemplate)
        {
            Debug.LogError($"{name}: asigna Echo Template amarillo");
            enabled = false;
            return;
        }

        // ── instanciar plantillla y copiar texturas del material original
        mat = new Material(echoTemplate);
        CopyTextures(rend.sharedMaterial, mat);
        rend.material = mat;

        // alpha = 0  → barril negro al iniciar
        mat.SetFloat(idTran, 0f);
    }

    void OnEnable() => EchoEventManager.OnEcho += Flash;
    void OnDisable() => EchoEventManager.OnEcho -= Flash;

    void Flash(Vector3 origin, float radius)
    {
        if (Vector3.Distance(transform.position, origin) > radius) return;
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        float t = 0f;
        mat.SetFloat(idTran, 1f);                // se ve amarillo

        while (t < flashDuration)
        {
            t += Time.deltaTime;
            mat.SetFloat(idTran, Mathf.Lerp(1f, 0f, t / flashDuration));
            yield return null;
        }
        mat.SetFloat(idTran, 0f);                // vuelve a negro
    }

    /*────────────────── util ──────────────────*/
    void CopyTextures(Material src, Material dst)
    {
        if (!src || !dst) return;
        foreach (var name in src.GetTexturePropertyNames())
            if (dst.HasProperty(name))
                dst.SetTexture(name, src.GetTexture(name));
    }
}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class IncreaseScript : MonoBehaviour
{
    [Header("Escala visual")]
    public Vector3 targetScale = new(2f, 2f, 2f);
    public Vector3 initialScale = new(1f, 1f, 1f);
    public float scaleSpeed = 1f;

    [Header("Material del eco")]
    public Material Echo;
    public float holdDuration = 0.5f;
    public float fadeDuration = 2f;

    [Header("Consumo y cooldown")]
    public int costPerPing = 1;   // 1 uso
    public float cooldownTime = 2f;

    [Header("Ping Audio + Radio")]
    public AudioClip pingClip;
    public float sonarRadius = 30f;

    // -------------------------------------------------------
    AudioSource src;
    EnergySystem energy;
    Coroutine fadeCo;
    float nextAllowed;
    static readonly int idTransp = Shader.PropertyToID("_Transparency");

    void Start()
    {
        src = GetComponent<AudioSource>();
        src.playOnAwake = false;
        src.spatialBlend = 1f;
        if (pingClip) src.clip = pingClip;

        energy = GetComponentInParent<EnergySystem>(); // ← raíz jugador
        Echo.SetFloat(idTransp, 0f);
        transform.localScale = initialScale;
    }

    void Update()
    {
        if (Time.time < nextAllowed) return;
        if (!Input.GetKeyDown(KeyCode.Q)) return;

        // ¿hay energía suficiente?
        if (!energy || !energy.TryConsume(costPerPing)) return;

        // activa eco
        Echo.SetFloat(idTransp, 1f);
        if (fadeCo != null) StopCoroutine(fadeCo);
        fadeCo = StartCoroutine(TransparencyRoutine());

        transform.localScale = initialScale;
        StartCoroutine(ScaleRing());

        if (pingClip) src.PlayOneShot(pingClip);
        EchoEventManager.Broadcast(transform.position, sonarRadius);

        nextAllowed = Time.time + cooldownTime;
    }

    IEnumerator ScaleRing()
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.05f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale,
                                                targetScale,
                                                scaleSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator TransparencyRoutine()
    {
        yield return new WaitForSeconds(holdDuration);

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            Echo.SetFloat(idTransp, Mathf.Lerp(1f, 0f, t / fadeDuration));
            yield return null;
        }
        Echo.SetFloat(idTransp, 0f);
    }
}

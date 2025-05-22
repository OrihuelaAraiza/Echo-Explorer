using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class IncreaseScript : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(2f, 2f, 2f);
    public Vector3 initialScale = new Vector3(1f, 1f, 1f);
    public float scaleSpeed = 1f;

    private bool isScaling = false;
    private bool hasFaded = false;

    public Material Echo;

    [Header("Duraciones")]
    public float holdDuration = 0.5f;   // ⟵ NUEVO: tiempo a α = 1
    public float fadeDuration = 2f;     // antes llamado fadeduration

    public float cooldownTime = 2f;
    private float nextAllowedUseTime = 0f;

    [Header("Ping Audio + Radio")]
    public AudioClip pingClip;
    public float sonarRadius = 30f;

    AudioSource src;

    static readonly int idTransp = Shader.PropertyToID("_Transparency");

    void Start()
    {
        transform.localScale = initialScale;

        src = GetComponent<AudioSource>();
        src.playOnAwake = false;
        src.spatialBlend = 1f;
        if (pingClip) src.clip = pingClip;

        Echo.SetFloat(idTransp, 0f);            // siempre inicia 0
    }

    void Update()
    {
        if (Time.time >= nextAllowedUseTime && Input.GetKeyDown(KeyCode.Q))
        {
            StopAllCoroutines();                // ⟵ NUEVO: resetea corrutinas
            Echo.SetFloat(idTransp, 1f);        // sube a 1

            transform.localScale = initialScale;
            isScaling = true;
            hasFaded = false;

            if (pingClip) src.PlayOneShot(pingClip); else src.Play();
            EchoEventManager.Broadcast(transform.position, sonarRadius);

            nextAllowedUseTime = Time.time + cooldownTime;
        }

        if (isScaling)
        {
            transform.localScale = Vector3.Lerp(
                                      transform.localScale,
                                      targetScale,
                                      scaleSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localScale, targetScale) < 0.05f
                && !hasFaded)
            {
                transform.localScale = targetScale;
                isScaling = false;
                hasFaded = true;
                StartCoroutine(FadeTransparency());     // inicia fade UNA vez
            }
        }
    }

    IEnumerator FadeTransparency()
    {
        /* mantiene α = 1 durante holdDuration */
        yield return new WaitForSeconds(holdDuration);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            Echo.SetFloat(idTransp, Mathf.Lerp(1f, 0f, t));
            elapsed += Time.deltaTime;
            yield return null;
        }

        Echo.SetFloat(idTransp, 0f);             // vuelve a 0 garantizado
        transform.localScale = initialScale;
    }
}

using System.Collections;
using UnityEngine;

public class EchoScript : MonoBehaviour
{
    public float fadeDuration = 1f;      // tiempo 1→0
    public float holdDuration = 0.5f;    // NUEVO: tiempo a 1 antes de desvanecer

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EchoTarget"))
        {
            Renderer r = other.GetComponent<Renderer>();
            if (!r || !r.material.HasProperty("_Transparency")) return;

            Material m = r.material;
            m.SetFloat("_Transparency", 1f);
            StartCoroutine(FadeAfterHold(m));          // ← ACTIVA fade
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EchoTarget"))
        {
            Renderer r = other.GetComponent<Renderer>();
            if (r && r.material.HasProperty("_Transparency"))
                StartCoroutine(FadeToZero(r.material));
        }
    }

    /* -------- corrutinas -------- */

    IEnumerator FadeAfterHold(Material mat)            // NUEVA
    {
        yield return new WaitForSeconds(holdDuration);
        yield return FadeToZero(mat);
    }

    IEnumerator FadeToZero(Material mat)
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            mat.SetFloat("_Transparency", Mathf.Lerp(1f, 0f, t / fadeDuration));
            yield return null;
        }
        mat.SetFloat("_Transparency", 0f);
    }
}

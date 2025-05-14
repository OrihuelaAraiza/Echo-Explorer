using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoScript : MonoBehaviour
{
    public float fadeDuration = 1f; // Time to fade from 1 to 0

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("EchoTarget"))
        {
            Renderer rend = other.gameObject.GetComponent<Renderer>();
            if (rend != null)
            {
                Material mat = rend.material; // Get instance material
                if (mat.HasProperty("_Transparency"))
                {
                    // Set to full visibility immediately
                    mat.SetFloat("_Transparency", 1f);
                    // Start fading it back to 0
                    //StartCoroutine(FadeToZero(mat));
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("EchoTarget"))
        {
            Renderer rend = other.gameObject.GetComponent<Renderer>();
            if (rend != null)
            {
                Material mat = rend.material;
                if (mat.HasProperty("_Transparency"))
                {
                    // Start fading only after leaving the trigger
                    StartCoroutine(FadeToZero(mat));
                }
            }
        }
    }

    private System.Collections.IEnumerator FadeToZero(Material mat)
    {
        float elapsed = 0f;
        float startValue = 1f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, 0f, elapsed / fadeDuration);
            mat.SetFloat("_Transparency", newValue);
            yield return null;
        }

        mat.SetFloat("_Transparency", 0f); // Ensure it hits zero cleanly
    }
}

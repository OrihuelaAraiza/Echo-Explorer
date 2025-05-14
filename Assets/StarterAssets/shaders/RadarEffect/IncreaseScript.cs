using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseScript : MonoBehaviour
{
    public Vector3 targetScale = new Vector3 (2f, 2f, 2f);
    public Vector3 initialScale = new Vector3(1f, 1f, 1f);

    public float scaleSpeed = 1f;

    private bool isScaling = false;
    private bool hasFaded = false;

    public Material Echo;
    public float fadeduration = 2f;

    public float cooldownTime = 2f; // Editable in Inspector
    private float nextAllowedUseTime = 0f;



    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = initialScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Time.time >= nextAllowedUseTime)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Echo.SetFloat("_Transparency", 1f);
                transform.localScale = initialScale;
                isScaling = true;
                hasFaded = false;

                AudioSource audio = GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.enabled = true;
                    audio.Play();
                    StartCoroutine(DisableAudioSourceAfterPlay(audio));
                }

                nextAllowedUseTime = Time.time + cooldownTime;
            }

        }

        if (isScaling)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);

            float distance = Vector3.Distance(transform.localScale, targetScale);

            if ( distance< 1000f && !hasFaded)
            {
                transform.localScale = targetScale;
                isScaling = false;
                hasFaded = true;
                StartCoroutine(FadeTransparency());
                
            }
            
        }
        
    }

    private IEnumerator DisableAudioSourceAfterPlay(AudioSource audio)
    {
        yield return new WaitForSeconds(audio.clip.length);
        audio.Stop();        
        audio.enabled = false;
    }

    private IEnumerator FadeTransparency()
    {
        float startValue = 1f;
        float endValue = 0f;
        float elapsed = 0f;

        while (elapsed < fadeduration)
        {
            float t = elapsed / fadeduration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            Echo.SetFloat("_Transparency", currentValue);
            elapsed += Time.deltaTime;
            yield return null;
        }

        
        Echo.SetFloat("_Transparency", 0f);

        transform.localScale = initialScale;
    }
}

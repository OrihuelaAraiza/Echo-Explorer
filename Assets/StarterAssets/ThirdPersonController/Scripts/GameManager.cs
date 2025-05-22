using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Controla Game Over sencillo (fade + pausa + reinicio).
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("UI")]
    public CanvasGroup gameOverCanvas;   // arrastra un panel con texto "GAME OVER"
    public float fadeTime = 0.6f;
    public float waitBeforeRestart = 2f;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        if (gameOverCanvas)
        {
            gameOverCanvas.alpha = 0;
            gameOverCanvas.blocksRaycasts = false;
        }
    }

    public void GameOver()
    {
        StartCoroutine(CoGameOver());
    }

    IEnumerator CoGameOver()
    {
        // Fade in panel
        if (gameOverCanvas)
        {
            for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
            {
                gameOverCanvas.alpha = t / fadeTime;
                yield return null;
            }
            gameOverCanvas.alpha = 1;
            gameOverCanvas.blocksRaycasts = true;
        }

        // Pausa el juego
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(waitBeforeRestart);

        // Reinicia la escena actual
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

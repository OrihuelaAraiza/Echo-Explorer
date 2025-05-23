using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;            // para los Buttons
using TMPro;                     // si usas TextMeshPro

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuUI;       // asigna PauseMenuCanvas
    public Button btnResume;          // asigna BtnResume
    public Button btnMainMenu;        // asigna BtnMainMenu
    public Button btnQuit;            // opcional

    bool isPaused = false;

    void Start()
    {
        // arranca sin menú
        pauseMenuUI.SetActive(false);

        // conexión botones
        btnResume.onClick.AddListener(Resume);
        btnMainMenu.onClick.AddListener(GoToMainMenu);
        if (btnQuit != null)
            btnQuit.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        // tecla para pausar/reanudar
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        // opcional: silencia audio, desactiva otros controles...
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void GoToMainMenu()
    {
        Time.timeScale = 1f;  // asegúrate de reanudar el tiempo
        SceneManager.LoadScene("MainMenu");  // o índice de tu menú
    }

    void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

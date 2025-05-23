using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject tutorialPanel;       // el panel completo
    public TextMeshProUGUI tutorialText;   // componente donde va el texto

    [Header("Páginas del tutorial")]
    [TextArea(3, 6)]
    public string[] pages = new string[]
    {
        "Controles:\nW/A/S/D – Mover\nMouse – Cámara\n\n(Pulsa Espacio)",
        "Q – Emite el radar para iluminar objetos.\n\n(Pulsa Espacio)",
        "Acércate a las baterías para recargarte.\n\n(Pulsa Espacio)",
        "¡Listo! Suerte explorando.\n\n(Pulsa Espacio para comenzar)"
    };

    int pageIndex = 0;

    void Start()
    {
        // Pausamos el juego y mostramos el panel
        Time.timeScale = 0f;
        tutorialPanel.SetActive(true);
        ShowPage(0);
    }

    void Update()
    {
        // Sólo atendemos Espacio mientras el tutorial está activo
        if (tutorialPanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
            NextPage();
    }

    void ShowPage(int i)
    {
        pageIndex = i;
        tutorialText.text = pages[i];
    }

    void NextPage()
    {
        int next = pageIndex + 1;
        if (next < pages.Length)
        {
            ShowPage(next);
        }
        else
        {
            // fin del tutorial: oculta UI y reanuda el juego
            tutorialPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}

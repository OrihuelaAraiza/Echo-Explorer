using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class LevelExit : MonoBehaviour
{
    public string nextScene = "Level 2";
    public float enableDelay = 0.5f;   // medio segundo

    Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = false;            // empieza desactivado
    }

    void Start()                       // se llama después de Awake
    {
        Invoke(nameof(EnableTrigger), enableDelay);
    }

    void EnableTrigger() => col.enabled = true;

    void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<CharacterController>()) return;
        col.enabled = false;            // evita rebotes dobles
        SceneManager.LoadScene(nextScene);
    }
}

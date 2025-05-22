using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AlienTouchKill : MonoBehaviour
{
    [Tooltip("Tag del objeto jugador que matará al tocar")]
    public string playerTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("[Alien] He tocado al jugador: ¡Game Over!");
            GameManager.I.GameOver();
        }
    }
}

// PlayerSpawner.cs  (ponlo en un objeto vac�o de cada escena)
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    void Start()
    {
        if (FindObjectOfType<CharacterController>()) return;   // ya existe

        var prefab = Resources.Load<GameObject>("Player");      // usa Resources o arrastra v�a Inspector
        Instantiate(prefab, transform.position, transform.rotation);
    }
}

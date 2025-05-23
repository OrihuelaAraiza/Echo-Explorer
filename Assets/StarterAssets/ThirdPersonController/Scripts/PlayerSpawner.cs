// PlayerSpawner.cs  (ponlo en un objeto vacío de cada escena)
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    void Start()
    {
        if (FindObjectOfType<CharacterController>()) return;   // ya existe

        var prefab = Resources.Load<GameObject>("Player");      // usa Resources o arrastra vía Inspector
        Instantiate(prefab, transform.position, transform.rotation);
    }
}

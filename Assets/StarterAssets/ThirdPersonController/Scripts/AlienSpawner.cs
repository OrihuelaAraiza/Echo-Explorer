using UnityEngine;
using UnityEngine.AI;

public class AlienSpawner : MonoBehaviour
{
    [Header("Prefab a instanciar")]
    public GameObject alienPrefab;

    [Header("Spawn points")]
    public Transform[] points;

    [Header("Rondas")]
    public int initialCount = 3;     // cuántos al iniciar
    public float spawnDelay = 10f;   // segundos entre rondas
    public int perWave = 2;     // cuántos por ronda
    public int maxAlive = 10;    // límite simultáneo

    [Header("Waypoints compartidos")]
    public Transform[] patrolPoints;   // los mismos WP_A, WP_B… de antes

    float nextSpawn;
    int alive;

    void Start()
    {
        // primera oleada
        for (int i = 0; i < initialCount; i++) SpawnOne();
        nextSpawn = Time.time + spawnDelay;
    }

    void Update()
    {
        // controla oleadas
        if (Time.time >= nextSpawn && alive < maxAlive)
        {
            for (int i = 0; i < perWave && alive < maxAlive; i++) SpawnOne();
            nextSpawn = Time.time + spawnDelay;
        }
    }

    void SpawnOne()
    {
        if (alienPrefab == null || points.Length == 0) return;

        Transform p = points[Random.Range(0, points.Length)];
        GameObject go = Instantiate(alienPrefab, p.position, p.rotation);

        // pasa waypoints al AlienController
        AlienController ctrl = go.GetComponent<AlienController>();
        if (ctrl) ctrl.patrolPoints = patrolPoints;

        // escucha cuando muere (si implementas vida) ↓
        // AlienHealth hp = go.GetComponent<AlienHealth>();
        // if (hp) hp.onDeath += () => alive--;

        alive++;
    }
}

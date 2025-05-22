using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// IA básica: patrulla → persigue (eco) → ataca. Terminada la persecución,
/// vuelve a patrullar aunque no haya llegado al último destino.
/// Requiere un Animator con:
///   Float   Speed
///   Trigger DoAttack
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AlienController : MonoBehaviour
{
    /*──────────────────────────────────────────────────────────────
     *  Ajustes públicos
     *────────────────────────────────────────────────────────────*/
    [Header("Waypoints")]
    public Transform[] patrolPoints;
    public float waypointTolerance = 0.6f;

    [Header("Velocidades")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 4f;

    [Header("Persecución")]
    [Tooltip("Cuántos segundos persigue tras oír el eco")]
    public float chaseDuration = 6f;

    [Header("Ataque")]
    public float attackDistance = 1.6f;
    public float attackCooldown = 1.2f;
    public string playerTag = "Player";

    /*──────────────────────────────────────────────────────────────
     *  Internos
     *────────────────────────────────────────────────────────────*/
    NavMeshAgent agent;
    Animator anim;
    Transform player;

    int wpIndex;
    bool chasing;
    float chaseEndTime;
    float nextAttackTime;

    /*──────────────────────────────────────────────────────────────
     *  Inicialización
     *────────────────────────────────────────────────────────────*/
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;

        agent.speed = patrolSpeed;
    }

    void Start() => GoNextWaypoint();

    /*──────────────────────────────────────────────────────────────
     *  Bucle principal
     *────────────────────────────────────────────────────────────*/
    void Update()
    {
        HandleAttack();

        if (chasing)
            HandleChase();
        else
            HandlePatrol();

        UpdateAnimator();
    }

    /*──────────────────────────────────────────────────────────────
     *  Persecución (llamado por AlienEchoListener → Chase)
     *────────────────────────────────────────────────────────────*/
    public void Chase(Vector3 _)
    {
        if (!player) return;

        chasing = true;
        chaseEndTime = Time.time + chaseDuration;

        agent.speed = chaseSpeed;
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void HandleChase()
    {
        if (player) agent.SetDestination(player.position);

        if (Time.time >= chaseEndTime)
        {
            // fin de persecución: vuelve a patrulla
            chasing = false;
            agent.speed = patrolSpeed;
            GoNextWaypoint();
        }
    }

    /*──────────────────────────────────────────────────────────────
     *  Patrulla
     *────────────────────────────────────────────────────────────*/
    void HandlePatrol()
    {
        if (agent.remainingDistance < waypointTolerance)
            GoNextWaypoint();
    }

    void GoNextWaypoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[wpIndex].position;
        wpIndex = (wpIndex + 1) % patrolPoints.Length;
    }

    /*──────────────────────────────────────────────────────────────
     *  Ataque
     *────────────────────────────────────────────────────────────*/
    void HandleAttack()
    {
        if (!player) return;

        float d = Vector3.Distance(transform.position, player.position);
        if (d <= attackDistance && Time.time >= nextAttackTime)
        {
            anim.ResetTrigger("DoAttack");
            anim.SetTrigger("DoAttack");
            nextAttackTime = Time.time + attackCooldown;
        }

        // opcional: mirar al jugador mientras está muy cerca
        if (d <= attackDistance + 0.5f)
        {
            Vector3 dir = player.position - transform.position;
            dir.y = 0;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(dir),
                                                      10 * Time.deltaTime);
        }
    }

    /*──────────────────────────────────────────────────────────────
     *  Animaciones
     *────────────────────────────────────────────────────────────*/
    void UpdateAnimator()
    {
        float speed = agent.desiredVelocity.magnitude;   // valor estable
        anim.SetFloat("Speed", speed);
    }

    /*──────────────────────────────────────────────────────────────
     *  GETTERS para otros scripts (opcional)
     *────────────────────────────────────────────────────────────*/
    public float CurrentSpeed => agent ? agent.desiredVelocity.magnitude : 0f;
    public bool IsChasing => chasing;
}

using UnityEngine;
using UnityEngine.AI;

public class EnemyCore : MonoBehaviour
{
    enum State
    {
        Patrol,
        Chase
    }

    [Header("参照")]
    public Transform waypointParent;
    public Transform player;

    [Header("距離設定")]
    public float detectDistance = 8f;
    public float loseDistance = 12f;
    public float arriveDistance = 0.5f;

    [Header("速度")]
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;

    NavMeshAgent agent;
    Transform[] waypoints;
    int currentIndex;
    State state = State.Patrol;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        waypoints = waypointParent.GetComponentsInChildren<Transform>();
        waypoints = System.Array.FindAll(waypoints, t => t != waypointParent);

        agent.speed = patrolSpeed;
        agent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Patrol:
                UpdatePatrol(distanceToPlayer);
                break;

            case State.Chase:
                UpdateChase(distanceToPlayer);
                break;
        }
    }

    // ---------- Patrol ----------
    void UpdatePatrol(float distanceToPlayer)
    {
        // プレイヤー発見 → 追跡
        if (distanceToPlayer <= detectDistance)
        {
            EnterChase();
            return;
        }

        // Waypoint到着
        if (!agent.pathPending &&
            agent.remainingDistance <= arriveDistance)
        {
            GoNextWaypoint();
        }
    }

    void GoNextWaypoint()
    {
        currentIndex = (currentIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentIndex].position);
    }

    // ---------- Chase ----------
    void UpdateChase(float distanceToPlayer)
    {
        agent.SetDestination(player.position);

        // プレイヤーを見失う → 巡回へ
        if (distanceToPlayer >= loseDistance)
        {
            EnterPatrol();
        }
    }

    void EnterPatrol()
    {
        state = State.Patrol;
        agent.speed = patrolSpeed;
        agent.SetDestination(waypoints[currentIndex].position);
    }

    void EnterChase()
    {
        state = State.Chase;
        agent.speed = chaseSpeed;
    }
}



// {
//     [Header("Waypoint設定")]
//     public Transform waypointParent;
//     public float arriveDistance = 0.5f;

//     [Header("Player検知")]
//     public Transform player;
//     public float detectDistance = 8f;

//     NavMeshAgent agent;
//     Transform[] waypoints;
//     int currentIndex = 0;

//     bool playerDetected = false; // ログ多重防止

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();

//         waypoints = waypointParent.GetComponentsInChildren<Transform>();
//         waypoints = System.Array.FindAll(waypoints, t => t != waypointParent);

//         agent.SetDestination(waypoints[currentIndex].position);
//     }

//     void Update()
//     {
//         CheckPlayer();

//         if (!agent.pathPending &&
//             agent.remainingDistance <= arriveDistance)
//         {
//             GoNext();
//         }
//     }

//     void GoNext()
//     {
//         currentIndex = (currentIndex + 1) % waypoints.Length;
//         agent.SetDestination(waypoints[currentIndex].position);
//     }

//     // --------------------
//     // プレイヤー検知（距離のみ）
//     // --------------------
//     void CheckPlayer()
//     {
//         if (player == null) return;

//         float distance = Vector3.Distance(transform.position, player.position);

//         if (distance <= detectDistance && !playerDetected)
//         {
//             playerDetected = true;
//             Debug.LogWarning("プレイヤーを発見！");
//         }

//         // 離れたらリセット（再検知できるように）
//         if (distance > detectDistance)
//         {
//             playerDetected = false;
//         }
//     }
// }


//接近のみ
// {
//     [SerializeField]
//     private NavMeshAgent agent;
//     [SerializeField]
//     private Transform target;

//     private void Update()
//     {
//         agent.SetDestination(target.position);
//     }
// }

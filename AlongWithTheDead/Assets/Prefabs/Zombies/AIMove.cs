using UnityEngine;
using UnityEngine.AI;

public class AIMove : MonoBehaviour
{
    #region Variables
    [Header("For AI Navigation")]
    public float walkPointRange;
    public Transform centrePoint;
    public NavMeshAgent agent;
    private bool walkPointSet = false;
    Vector3 RandomDestination;

    [Header("Stats")]
    public float health;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }


    private void Update()
    {
        #region STAGE 1.1 RANDOM MOVEMENT
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, walkPointRange, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
            }
        }
        #endregion
    }

    #region Patrolling
    bool RandomPoint(Vector3 center, float Range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * Range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }
    #endregion   

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, walkPointRange);
    }
}

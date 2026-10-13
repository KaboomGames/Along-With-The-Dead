using System.Collections;
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

    [Header("References")]
    private Animator animator;
    private bool isWaiting;

    [Header("SFX")]
    public AudioClip[] clips;
    public float[] volumes;
    private AudioSource source;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }


    private void Update()
    {
        #region STAGE 1.1 RANDOM MOVEMENT
        if (!isWaiting && agent.remainingDistance <= agent.stoppingDistance)
            StartCoroutine(WaitBeforeWalk());
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
                animator.SetFloat("Speed", 1);
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }
    #endregion

    #region Delay Between Talk Points
    IEnumerator WaitBeforeWalk()
    {
        isWaiting = true;
        animator.SetFloat("Speed", 0);
        yield return new WaitForSeconds(5f);
        Vector3 point;
        if (RandomPoint(centrePoint.position, walkPointRange, out point))
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
            agent.SetDestination(point);
        }
        isWaiting = false;
    }
    #endregion

    #region Take Damage
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            animator.Play("DieFront");
            agent.enabled = false;
        }
    }
    #endregion

    #region Play Audio
    public void PlayAudio(int id)
    {
        source.Stop();
        source.clip = clips[id];
        source.volume = volumes[id];
        source.Play();
    }
    #endregion

    #region Draw Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, walkPointRange);
    }
    #endregion
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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
    public float originalSpeed;
    public float chaseSpeed;
    [HideInInspector] public bool playerDetected;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public bool dealDamage = false;

    [Header("Events")]
    public UnityEvent deathEvent;

    [Header("References")]
    [HideInInspector] public Animator animator;
    private bool isWaiting;
    private AIDetect aiDetect;

    [Header("SFX")]
    public AudioSource ambientSource;
    public AudioSource SFXSource;
    public AudioClip[] clips;
    public float[] volumes;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        aiDetect = GetComponentInChildren<AIDetect>();
    }

    #region OnTriggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerDetected = true;
            playerTransform = other.transform;
        }
    }
    #endregion

    private void Update()
    {
        if (!playerDetected)
        {
            #region STAGE 1.1 RANDOM MOVEMENT
            if (!isWaiting && agent.remainingDistance <= agent.stoppingDistance)
                StartCoroutine(WaitBeforeWalk());
            #endregion
        }
        else
        {
            animator.SetFloat("Speed", 2);
            agent.SetDestination(playerTransform.position);
            agent.speed = chaseSpeed;
            PlayChaseAmbient();
        }
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

    #region AI Control
    public void Freeze()
    {
        agent.speed = 0;
        agent.enabled = false;
        canAttack = false;
    }

    public void Unfreeze()
    {
        agent.enabled = true;
        canAttack = true;

        if (playerDetected)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            agent.speed = originalSpeed;
            StartCoroutine(WaitBeforeWalk());
        }
    }

    public void PlayerCheck()
    {
        if (!aiDetect.isClose)
        {
            animator.SetTrigger("Move");
            Unfreeze();
        }
    }
    #endregion

    #region Toggle Deal Damage
    public void Toggle_DealDamage(int number)
    {
        if (number == 0) dealDamage = false;
        else dealDamage = true;
    }
    #endregion

    #region Take Damage
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            deathEvent.Invoke();
            animator.Play("DieFront");
            agent.enabled = false;
            ambientSource.Stop();
        }
    }
    #endregion

    #region Play Chase Ambient
    private bool playedChaseAmbien;
    private void PlayChaseAmbient()
    {
        if (!playedChaseAmbien)
        {
            playedChaseAmbien = true;
            ambientSource.Stop();
            ambientSource.clip = clips[1];
            ambientSource.Play();
        }
    }
    #endregion

    #region Play SFX
    public void PlayAudio(int id)
    {
        SFXSource.Stop();
        SFXSource.clip = clips[id];
        SFXSource.volume = volumes[id];
        SFXSource.Play();
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

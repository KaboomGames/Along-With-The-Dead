using UnityEngine;

public class AIDetect : MonoBehaviour
{
    #region Variables
    public bool isClose;
    [Header("References")]
    private AIMove aiMove;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aiMove = GetComponentInParent<AIMove>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
            isClose = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player" && aiMove.playerDetected && aiMove.canAttack && aiMove.health > 0 && isClose)
        {
            aiMove.animator.Play("Attack");
            aiMove.canAttack = false;
            aiMove.agent.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
            isClose = false;
    }
}

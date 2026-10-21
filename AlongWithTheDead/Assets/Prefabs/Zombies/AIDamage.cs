using UnityEngine;

public class AIDamage : MonoBehaviour
{
    #region Variables
    public int hitSFX;
    [Header("References")]
    private AIMove aiMove;
    private BoxCollider bc;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aiMove = GetComponentInParent<AIMove>();
        bc = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            aiMove.dealDamage = false;
            bc.enabled = false;
            aiMove.PlayAudio(hitSFX);
            Debug.Log("I hit the player");
        }
    }

    private void Update()
    {
        if (aiMove.dealDamage) bc.enabled = true;
        else bc.enabled = false;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Shoving_Hitbox : MonoBehaviour
{
    public LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        Collider[] cols = Physics.OverlapBox(this.transform.position, this.transform.localScale,
            this.transform.rotation, layerMask);

        bool[] alreadyShoved = new bool[cols.Length];

        for (int i = 0; i < cols.Length; i++)
        {
            AIMove aiMove = cols[i].GetComponent<AIMove>();
            if (aiMove != null && !alreadyShoved[i])
            {
                alreadyShoved[i] = true;
                aiMove.StartCoroutine(aiMove.Shoved());
                Debug.Log(aiMove.name);
            }
        }
    }
}

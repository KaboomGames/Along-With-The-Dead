using System.Collections;
using UnityEngine;

public class DestroyAfterTimer : MonoBehaviour
{
    public float timer;
    void Start()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }
}

using UnityEngine;

public class WeaponData : MonoBehaviour
{
    private WeaponManager wm;
    public bool useAction;
    public Animator animator;
    public AudioSource thisSource;
    public AudioClip[] clip;
    public float[] clipVolume;

    private void OnEnable()
    {
        if (wm == null)
            wm = GetComponentInParent<WeaponManager>();
    }
    public void Toggle_UseAction()
    {
        useAction = true;
    }

    public void PlayAudio(int id)
    {
        thisSource.clip = clip[id];
        thisSource.volume = clipVolume[id];
        thisSource.Play();
    }

    public void CheckBullets()
    {
        if (wm.usePrimary)
        {
            if (wm.primaryClip == wm.primaryMag)
                animator.SetTrigger("Finished");
            else
                wm.Reloading_Primary();
        }
        else
        {
            if (wm.secondaryClip == wm.secondaryMag)
                animator.SetTrigger("Finished");
            else
                wm.Reloading_Secondary();
        }
    }
}

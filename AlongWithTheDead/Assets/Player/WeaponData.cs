using UnityEngine;

public class WeaponData : MonoBehaviour
{
    public bool useAction;
    public Animator animator;
    public AudioSource thisSource;
    public AudioClip[] clip;
    public float[] clipVolume;

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
}

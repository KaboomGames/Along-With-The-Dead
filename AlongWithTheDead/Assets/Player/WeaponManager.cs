using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    #region Variables
    public GameObject[] weapons;
    public Animator[] weaponAnimator;

    [Header("Stats")]
    public int currentWeapon;
    public int ammoClip;
    public int maxAmmo;
    #endregion

    public void AssignWeapon(int id, int assignClip, int assignMaxAmmo)
    {
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].SetActive(false);

        weapons[id].SetActive(true);
        currentWeapon = id;
        ammoClip = assignClip;
        maxAmmo = assignMaxAmmo;
    }
}

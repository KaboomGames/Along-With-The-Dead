using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    #region Variables
    public GameObject[] weapons;
    public WeaponData[] wd;

    [Header("Stats")]
    public int weaponId;
    public int ammoClip;
    public int magazine;
    public int maxAmmo;

    private InputSystem_Actions inputAction;
    #endregion

    #region Enable/Disable Input Actions
    private void Awake()
    {
        inputAction = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
    }

    private void OnDisable()
    {
        inputAction.Player.Disable();
    }
    #endregion

    private void Update()
    {
        if (inputAction.Player.Shoot.IsPressed() && ammoClip > 0 && wd[weaponId].useAction)
        {
            wd[weaponId].useAction = false;
            Shooting();
        }

        if (inputAction.Player.Reload.IsPressed() && ammoClip < magazine && wd[weaponId].useAction)
        {
            wd[weaponId].useAction = false;
            Reloading();
        }
    }

    public void Shooting()
    {
        wd[weaponId].animator.Play("Shoot");
        ammoClip--;
    }

    public void Reloading()
    {
        wd[weaponId].animator.Play("Reload");
        maxAmmo += ammoClip;
        ammoClip = 0;
        if ((maxAmmo - magazine) > 0)
        {
            ammoClip = magazine;
            maxAmmo -= magazine;
        }
        else
        {
            ammoClip = maxAmmo;
            maxAmmo = 0;
        }
    }

    #region Assign new weapon
    public void AssignWeapon(int id, int assignClip, int assignMagazine, int assignMaxAmmo)
    {
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].SetActive(false);

        weaponId = id;
        weapons[id].SetActive(true);
        ammoClip = assignClip;
        magazine = assignMagazine;
        maxAmmo = assignMaxAmmo;
    }
    #endregion
}

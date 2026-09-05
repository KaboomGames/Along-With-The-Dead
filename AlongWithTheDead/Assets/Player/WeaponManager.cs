using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    #region Variables
    public GameObject[] weapons;
    public WeaponData[] wd;
    public bool usePrimary;

    [Header("Primary")]
    public int primaryID;
    public int primaryClip;
    public int primaryMag;
    public int primaryMax;

    [Header("Secondary")]
    public int secondaryID;
    public int secondaryClip;
    public int secondaryMag;
    public int secondaryMax;


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
        #region Switch Weapons
        if (inputAction.Player.Switch_Weapon.WasPressedThisFrame())
            Switch_Weapon(!usePrimary);
        if (inputAction.Player.Switch_Primary.WasPressedThisFrame())
            Switch_Weapon(true);
        if (inputAction.Player.Switch_Secondary.WasPerformedThisFrame())
            Switch_Weapon(false);
        #endregion

        #region Use Weapon Functions
        if (usePrimary)
            Primary_Buttons();
        else
            Secondary_Buttons();
        #endregion
    }

    #region Switch Weapon
    void Switch_Weapon(bool toPrimary)
    {
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].SetActive(false);

        if (toPrimary)
        {
            weapons[primaryID].SetActive(true);
            usePrimary = true;
        }
        else
        {
            weapons[secondaryID].SetActive(true);
            usePrimary = false;
        }
    }
    #endregion

    #region Primary Weapon Functions
    void Primary_Buttons()
    {
        if (inputAction.Player.Shoot.IsPressed() && primaryClip > 0 && wd[primaryID].useAction)
        {
            wd[primaryID].useAction = false;
            Shooting_Primary();
        }

        if (inputAction.Player.Reload.IsPressed() && primaryClip < primaryMag && wd[primaryID].useAction)
        {
            wd[primaryID].useAction = false;
            Reloading_Primary();
        }
    }

    public void Shooting_Primary()
    {
        wd[primaryID].animator.Play("Shoot");
        primaryClip--;
    }

    public void Reloading_Primary()
    {
        wd[primaryID].animator.Play("Reload");
        primaryMax += primaryClip;
        primaryClip = 0;
        if ((primaryMax - primaryMag) > 0)
        {
            primaryClip = primaryMag;
            primaryMax -= primaryMag;
        }
        else
        {
            primaryClip = primaryMax;
            primaryMax = 0;
        }
    }
    #endregion

    #region Secondary Weapon Functions
    void Secondary_Buttons()
    {
        if (inputAction.Player.Shoot.IsPressed() && secondaryClip > 0 && wd[secondaryID].useAction)
        {
            wd[secondaryID].useAction = false;
            Shooting_Secondary();
        }

        if (inputAction.Player.Reload.IsPressed() && secondaryClip < secondaryMag && wd[secondaryID].useAction)
        {
            wd[secondaryID].useAction = false;
            Reloading_Secondary();
        }
    }

    public void Shooting_Secondary()
    {
        wd[secondaryID].animator.Play("Shoot");
        secondaryClip--;
    }

    public void Reloading_Secondary()
    {
        wd[secondaryID].animator.Play("Reload");
        secondaryMax += secondaryClip;
        secondaryClip = 0;
        if ((secondaryMax - secondaryMag) > 0)
        {
            secondaryClip = secondaryMag;
            secondaryMax -= secondaryMag;
        }
        else
        {
            secondaryClip = secondaryMax;
            secondaryMax = 0;
        }
    }
    #endregion

    #region Assign new weapon
    public void AssignWeapon(bool isPrimary, int id, int assignClip, int assignMagazine, int assignMaxAmmo)
    {
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].SetActive(false);

        if (isPrimary)
        {
            usePrimary = true;
            primaryID = id;
            weapons[id].SetActive(true);
            primaryClip = assignClip;
            primaryMag = assignMagazine;
            primaryMax = assignMaxAmmo;
        }
        else
        {
            usePrimary = false;
            secondaryID = id;
            weapons[id].SetActive(true);
            secondaryClip = assignClip;
            secondaryMag = assignMagazine;
            secondaryMax = assignMaxAmmo;
        }
    }
    #endregion
}

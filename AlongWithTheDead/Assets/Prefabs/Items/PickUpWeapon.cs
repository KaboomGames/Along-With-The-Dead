using UnityEngine;
using UnityEngine.InputSystem;
public class PickUpWeapon : MonoBehaviour
{
    #region Variables
    private bool isClose;
    public bool tableWeapon;

    [Header("Stats")]
    public bool isPrimary;
    public int weaponID;
    public int ammoClip;
    public int magazine;
    public int amount;
    public int maxAmmo;

    private WeaponManager wm;
    private InputSystem_Actions inputActions;
    #endregion

    #region Awake/Enable/Disable
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    #endregion

    #region On Trigger Stay/Exit
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            isClose = true;
            wm = other.GetComponent<WeaponManager>();
            model[weaponID].SetActive(false);
            outlineModel[weaponID].SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            isClose = false;
            wm = null;
            model[weaponID].SetActive(true);
            outlineModel[weaponID].SetActive(false);
        }
    }
    #endregion

    #region Interaction Raycast
    [Header("Interaction")]
    public GameObject[] model;
    public GameObject[] outlineModel;
    public void Interacting(WeaponManager weaponManager)
    {
        isClose = true;
        model[weaponID].SetActive(false);
        outlineModel[weaponID].SetActive(true);
        wm = weaponManager;
    }

    public void StopInteracting()
    {
        isClose = false;
        model[weaponID].SetActive(true);
        outlineModel[weaponID].SetActive(false);
        wm = null;
    }
    #endregion

    private void Start()
    {
        model[weaponID].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (inputActions.Player.Interact.WasPressedThisFrame() && isClose)
        {
            if (tableWeapon)
            {
                wm.AssignWeapon(isPrimary, weaponID, ammoClip, magazine, amount, maxAmmo);
            }
            else
            {
                if (wm.usePrimary)
                    DropAndPickNewWeapon(wm.usePrimary, wm.primaryID, wm.primaryClip, wm.primaryMag, wm.primaryAmount, wm.primaryMax);
                else
                    DropAndPickNewWeapon(wm.usePrimary, wm.secondaryID, wm.secondaryClip, wm.secondaryMag, wm.secondaryAmount, wm.secondaryMax);
            }
        }
    }

    #region Drop And Pick New Weapon
    void DropAndPickNewWeapon(bool primary, int id, int clip, int mag, int Amount,int max)
    {
        wm.AssignWeapon(isPrimary, weaponID, ammoClip, magazine, amount, maxAmmo);
        isPrimary = primary;
        weaponID = id;
        ammoClip = clip;
        magazine = mag;
        amount = Amount;
        maxAmmo = max;
        for (int i = 0; i < model.Length; i++)
        {
            model[i].SetActive(false);
            outlineModel[i].SetActive(false);
        }
        outlineModel[weaponID].SetActive(true);
    }
    #endregion
}

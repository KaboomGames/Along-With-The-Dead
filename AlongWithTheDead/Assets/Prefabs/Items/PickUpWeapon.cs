using UnityEngine;
using UnityEngine.InputSystem;
public class PickUpWeapon : MonoBehaviour
{
    #region Variables
    private bool isClose;

    [Header("Stats")]
    public bool isPrimary;
    public int weaponID;
    public int ammoClip;
    public int magazine;
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
            weaponModel[0].SetActive(false);
            weaponModel[1].SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            isClose = false;
            wm = null;
            weaponModel[0].SetActive(true);
            weaponModel[1].SetActive(false);
        }
    }
    #endregion

    #region Interaction Raycast
    [Header("Interaction")]
    public GameObject[] weaponModel; //0 actual model, 1 outline model
    public void Interacting()
    {
        isClose = true;
        weaponModel[0].SetActive(false);
        weaponModel[1].SetActive(true);
    }

    public void StopInteracting()
    {
        isClose = false;
        weaponModel[0].SetActive(true);
        weaponModel[1].SetActive(false);
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (inputActions.Player.Interact.IsPressed() && isClose)
        {
            wm.AssignWeapon(isPrimary, weaponID, ammoClip, magazine, maxAmmo);
            this.gameObject.SetActive(false);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
public class PickUpWeapon : MonoBehaviour
{
    #region Variables
    private bool isClose;

    [Header("Stats")]
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

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            isClose = true;
            wm = other.GetComponent<WeaponManager>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            isClose = false;
            wm = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputActions.Player.Interact.IsPressed() && isClose)
        {
            wm.AssignWeapon(weaponID, ammoClip, magazine, maxAmmo);
            this.gameObject.SetActive(false);
        }
    }
}

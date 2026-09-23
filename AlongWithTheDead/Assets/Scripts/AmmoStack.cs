using Unity.VisualScripting;
using UnityEngine;

public class AmmoStack : MonoBehaviour
{
    #region Variables
    private bool isClose;
    public int[] ammoAmount;

    public GameObject model;
    public GameObject outlineModel;

    private InputSystem_Actions inputActions;
    private WeaponManager wm;
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

    #region OnTriggers + Interacting/StopInteracting
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            model.SetActive(false);
            outlineModel.SetActive(true);
            isClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            model.SetActive(true);
            outlineModel.SetActive(false);
            isClose = false;
        }
    }

    public void Interacting(WeaponManager wm_ref)
    {
        model.SetActive(false);
        outlineModel.SetActive(true);
        isClose = true;
        wm = wm_ref;
    }

    public void StopInteracting()
    {
        model.SetActive(true);
        outlineModel.SetActive(false);
        isClose = false;
        wm = null;
    }
    #endregion

    void Update()
    {
        if (inputActions.Player.Interact.WasPressedThisFrame() && isClose)
        {
            if (wm.usePrimary)
                wm.primaryMax = ammoAmount[wm.primaryID];
            else
                wm.secondaryMax = ammoAmount[wm.secondaryID];
        }
    }
}

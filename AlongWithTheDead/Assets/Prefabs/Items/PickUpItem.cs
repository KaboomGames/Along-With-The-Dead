using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpItem : MonoBehaviour
{
    #region Variables
    public enum ItemType {FirstAid,Syringe,Vaccine}
    public ItemType type;

    public GameObject nonOutline;
    public GameObject outline;

    [Header("For Interaction Raycast")]
    public int inventoryID;
    public int itemID;
    public Sprite sprite;

    private bool isClose;

    private InputSystem_Actions inputActions;
    private Interaction_Raycast ir;
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
            nonOutline.SetActive(false);
            outline.SetActive(true);
            isClose = true;
            ir = other.GetComponent<Interaction_Raycast>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            nonOutline.SetActive(true);
            outline.SetActive(false);
            isClose = false;
            ir = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputActions.Player.Interact.IsPressed() && isClose)
        {
            ir.AssignUIImage(inventoryID, itemID, sprite);
            this.gameObject.SetActive(false);
        }
    }

    #region Start/Stop Interacting
    public void Interacting(Interaction_Raycast irRef)
    {
        nonOutline.SetActive(false);
        outline.SetActive(true);
        isClose = true;
        ir = irRef;
    }

    public void StopInteracting()
    {
        nonOutline.SetActive(true);
        outline.SetActive(false);
        isClose = false;
        ir = null;
    }
    #endregion
}

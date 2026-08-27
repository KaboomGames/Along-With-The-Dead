using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpItem : MonoBehaviour
{
    #region Variables
    private bool isClose;

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
            isClose = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
            isClose = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputActions.Player.Interact.IsPressed() && isClose)
            Debug.Log("Interacted");
    }
}

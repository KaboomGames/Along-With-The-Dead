using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Controls")]
    public bool useAction;
    public bool progress;
    private InputSystem_Actions inputActions;

    [Header("Item Values")]
    public string itemName;
    public int itemSlotID;
    public float useDuration;

    [Header("References")]
    public UIManager uim;
    private PlayerMovement pm;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (inputActions == null)
            inputActions = new InputSystem_Actions();

        inputActions.Enable();
        animator = GetComponent<Animator>();
        pm = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Shoving
        if (useAction && inputActions.Player.Shoving.WasPressedThisFrame())
        {
            useAction = false;
            animator.Play("Shoving");
        }

        if (inputActions.Player.Shoot.IsPressed() && !progress)
        {
            progress = true;
            uim.StartProgress("Healing", useDuration, itemName, itemSlotID);
        }
        else if (!inputActions.Player.Shoot.IsPressed() && progress)
        {
            progress = false;
            uim.StopProgress();
        }
    }

    #region Toggle Use Action
    public void Toggle_UseAction(int id)
    {
        if (id == 1) useAction = true;
        else useAction = false;
    }
    #endregion
}

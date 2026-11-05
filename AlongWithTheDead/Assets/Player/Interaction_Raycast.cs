using UnityEngine;
using UnityEngine.UI;

public class Interaction_Raycast : MonoBehaviour
{
    #region Variables
    [Header("Raycast")]
    public Transform raycastT;
    public float range;
    public LayerMask layer;

    [Header("References")]
    private PickUpWeapon puw_ref;
    private AmmoStack as_ref;
    private PickUpItem pui_ref;
    private WeaponManager wm;

    [Header("Inventory")]
    public Image[] inventoryImage;
    public bool[] slotFull;
    public int[] itemID;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wm = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(raycastT.position, raycastT.forward * range, Color.red, 0.3f);
        RaycastHit hit;
        if (Physics.Raycast(raycastT.position, raycastT.forward, out hit, range, layer))
        {
            PickUpWeapon puw = hit.transform.GetComponent<PickUpWeapon>();
            AmmoStack ammoStack = hit.transform.GetComponent<AmmoStack>();
            PickUpItem pui = hit.transform.GetComponent<PickUpItem>();
            if (puw != null)
            {
                puw_ref = puw;
                puw.Interacting(wm);
            }
            else if (ammoStack != null)
            {
                as_ref = ammoStack;
                ammoStack.Interacting(wm);
            }
            else if (pui != null)
            {
                pui_ref = pui;
                pui.Interacting(this);
            }

        }
        else
            StopInteracting();
    }

    #region Stop Interacting
    void StopInteracting()
    {
        if (puw_ref != null)
        {
            puw_ref.StopInteracting();
            puw_ref = null;
        }
        else if (as_ref != null)
        {
            as_ref.StopInteracting();
            as_ref = null;
        }
        else if (pui_ref != null)
        {
            pui_ref.StopInteracting();
            pui_ref = null;
        }
    }
    #endregion

    #region Assign UI Image
    public void AssignUIImage(int id, int item,Sprite sprite)
    {
        inventoryImage[id].sprite = sprite;
        itemID[id] = item;
        slotFull[id] = true;
    }
    #endregion
}

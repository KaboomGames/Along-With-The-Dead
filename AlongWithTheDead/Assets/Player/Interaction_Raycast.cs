using UnityEngine;

public class Interaction_Raycast : MonoBehaviour
{
    #region Variables
    [Header("Raycast")]
    public Transform raycastT;
    public float range;
    public LayerMask layer;

    [Header("References")]
    private PickUpWeapon puw_ref;
    private WeaponManager wm;
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
            if (puw != null)
            {
                puw_ref = puw;
                puw.Interacting(wm);
            }
        }
        else
            StopInteracting();
    }

    void StopInteracting()
    {
        if (puw_ref != null)
        {
            puw_ref.StopInteracting();
            puw_ref = null;
        }
    }
}

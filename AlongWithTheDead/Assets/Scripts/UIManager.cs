using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables
    public Texture[] weaponImage;
    [Header("Primary")]
    public GameObject primaryHolder;
    public RawImage primaryImage;
    public TextMeshProUGUI primaryAmmo;
    public TextMeshProUGUI primaryMax;

    [Header("Secondary")]
    public GameObject secondaryHolder;
    public RawImage secondaryImage;
    public TextMeshProUGUI secondaryAmmo;
    public TextMeshProUGUI secondaryMax;

    [Header("Stats")]
    public Slider healthBar;

    private WeaponManager wm;
    private PlayerMovement pm;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        wm = FindObjectOfType<WeaponManager>();
        healthBar.maxValue = pm.maxHealth;
        healthBar.value = pm.health;
    }

    // Update is called once per frame
    void Update()
    {
        #region Assign UI Variables
        //Assign primary data
        if (wm.primaryID > -1)
        {
            primaryHolder.SetActive(true);
            primaryImage.texture = weaponImage[wm.primaryID];
            primaryAmmo.text = wm.primaryClip.ToString();
            primaryMax.text = wm.primaryMax.ToString();
        }
        else
            primaryHolder.SetActive(false);
        //Assign secondary data
        secondaryImage.texture = weaponImage[wm.secondaryID];
        secondaryAmmo.text = wm.secondaryClip.ToString();
        secondaryMax.text = wm.secondaryMax.ToString();
        //Assign Player's Heatlh
        healthBar.value = pm.health;
        #endregion
    }
}

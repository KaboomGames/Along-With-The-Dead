using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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
    private Interaction_Raycast ir;

    [Header("Progress Bar")]
    public GameObject progressBar;
    public TextMeshProUGUI progressText;
    public Slider progressSlider;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        wm = FindObjectOfType<WeaponManager>();
        ir = FindObjectOfType<Interaction_Raycast>();
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

    #region Progress
    public void StartProgress(string name, float duration, string item, int slotID)
    {
        StartCoroutine(ProgressRoutine(name, duration, item, slotID));
    }
    public void StopProgress()
    {
        StopCoroutine(ProgressRoutine("None", 0, "None", 0));
        progressText.text = "None";
        progressSlider.value = 0;
        progressBar.SetActive(false);
    }

    IEnumerator ProgressRoutine(string name, float duration, string item, int slotID)
    {
        bool complete = true;
        progressBar.SetActive(true);
        progressText.text = name;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float step = timer / duration;
            progressSlider.value = Mathf.Lerp(0, 1, step);

            if (!progressBar.activeSelf)
            {
                timer = duration;
                complete = false;
            }
            else yield return null;
        }

        if (!progressBar.activeSelf)
        {
            complete = false;
            progressSlider.value = 0;
        }

        if (complete)
        {
            progressBar.SetActive(false);
            ir.UseItem(item, slotID);
        }
    }
    #endregion
}

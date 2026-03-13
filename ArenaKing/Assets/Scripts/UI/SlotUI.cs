using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using NUnit.Framework.Interfaces;
using static UnityEditor.Progress;

public class SlotUI : MonoBehaviour
{
    public Image iconImage;
    public Image newImage;
    public TextMeshProUGUI newText;
    public Image qualityImage;
    public Image lockImage;
    public Image statImage; 
    public TextMeshProUGUI statText;    
    public Image abilityImage;
    public Image slotBackground;
    public Image statBackground;
    public RectTransform rectTransform;

    private string itemID;
    private string item_stat;
    private string item_value;

    public Item itemData;
    private MainMenuManager menuManager;

    private bool isAnimating = false;
    private Vector2 originalPosition;
    private float animationAmplitude = 1.5f; // amplitude du tremblement
    private float animationSpeed = 10f;      // vitesse de l'animation
    private float animationTimer = 0f;

    private void Awake()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        originalPosition = rectTransform.anchoredPosition;
    }

    public void Initialize(Item item, MainMenuManager manager)
    {
        itemData = item;
        menuManager = manager;
    }

    private void Update()
    {
        if (isAnimating)
        {
            animationTimer += Time.deltaTime * animationSpeed;
            float offsetX = Mathf.Sin(animationTimer) * animationAmplitude;
            float offsetY = Mathf.Cos(animationTimer * 0.8f) * animationAmplitude;

            rectTransform.anchoredPosition = originalPosition + new Vector2(offsetX, offsetY);
        }
    }

    public void DestroySlot()
    {
        Destroy(this.gameObject);
    }

    public void SetRecyclingMode(bool active)
    {
        isAnimating = active;

        if (isAnimating)
        {
            statText.text = item_value;
        } else
        {
            rectTransform.anchoredPosition = originalPosition;
            statText.text = item_stat;
        }
    }

    public void OnSlotClicked()
    {
        Debug.Log("Slot cliqué");
        if (itemData == null || menuManager == null)
        {
            Debug.LogWarning("SlotUI - itemData ou menuManager est null.");
            return;
        }

        menuManager.OnInventoryItemClicked(itemData);
    }


    public IEnumerator SetSlot(Sprite icon, Sprite statimage, int level, string stat, string value, bool isLock, string rarity_id, string item_id)
    {
        iconImage.sprite = icon;
        statImage.sprite = statimage;
        statText.text = stat;

        item_stat = stat;
        item_value = value;
        itemID = item_id;

        switch(rarity_id)
        {
            case "316FF0CF-BAD9-43CD-B353-CFD1A12BA5E3":
                slotBackground.color = new Color32(104, 98, 86, 255);
                statBackground.color = new Color32(73, 68, 61, 255);
                break;
            case "E255A0A1-095C-46A9-9FCC-7212C7F37CA6":
                slotBackground.color = new Color32(3, 112, 143, 255);
                statBackground.color = new Color32(2, 78, 99, 255);
                break;
            case "52D9040B-55BA-4536-9CFA-10BCD451B4BD":
                slotBackground.color = new Color32(84, 122, 27, 255);
                statBackground.color = new Color32(55, 79, 18, 255);
                break;
            case "9BA00FC5-9327-406D-9316-4D2813DB0CBF":
                slotBackground.color = new Color32(200, 93, 22, 255);
                statBackground.color = new Color32(130, 60, 14, 255);
                break;
            case "B198F4B4-0BEF-44C7-98F2-A324995188BB":
                slotBackground.color = new Color32(119, 45, 160, 255);
                statBackground.color = new Color32(77, 30, 104, 255);
                break;
            default:
                break;

        }

        yield return 0;
    }

}
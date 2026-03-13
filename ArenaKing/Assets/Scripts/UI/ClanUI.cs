using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using NUnit.Framework.Interfaces;
using static UnityEditor.Progress;

public class ClanUI : MonoBehaviour
{
    public Image clanIcon;
    public Image clanLvlImage;
    public TextMeshProUGUI clanLvlText;
    public Image clanRegionIcon;
    public TextMeshProUGUI clanMembersText;
    public TextMeshProUGUI clanNameText;
    public TextMeshProUGUI clanDescriptionText;


    public Clan clanData;
    private MainMenuManager menuManager;

    public void Initialize(Clan clan, MainMenuManager manager)
    {
        clanData = clan;
        menuManager = manager;
    }

    public void DestroySlot()
    {
        Destroy(this.gameObject);
    }

    public void OnSlotClicked()
    {
        Debug.Log("Slot cliqué");
        if (clanData == null || menuManager == null)
        {
            Debug.LogWarning("SlotUI - clanData ou menuManager est null.");
            return;
        }
        // TODO Changer l'appel ci-dessous pour appeler le bon event
        menuManager.OnClanSlotClicked(clanData);
    }


    public IEnumerator SetSlot()
    {
        clanLvlText.GetComponent<TextMeshProUGUI>().text = clanData.clan_lvl.ToString();
        clanMembersText.GetComponent<TextMeshProUGUI>().text = clanData.clan_total_members + "/" + clanData.clan_max_members.ToString();
        clanNameText.GetComponent<TextMeshProUGUI>().text = clanData.clan_name;
        clanDescriptionText.GetComponent<TextMeshProUGUI>().text = clanData.clan_description;


        yield return 0;
    }

}
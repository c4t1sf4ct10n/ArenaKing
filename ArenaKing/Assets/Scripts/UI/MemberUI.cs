using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using NUnit.Framework.Interfaces;
using static UnityEditor.Progress;

public class MemberUI : MonoBehaviour
{
    public Image memberIcon;
    public Image memberChampionshipIcon;
    public TextMeshProUGUI memberLevelText;
    public TextMeshProUGUI memberNameText;
    public TextMeshProUGUI memberRankText;


    public Member memberData;
    private MainMenuManager menuManager;

    public void Initialize(Member member, MainMenuManager manager)
    {
        memberData = member;
        menuManager = manager;
    }

    public void DestroySlot()
    {
        Destroy(this.gameObject);
    }

    public void OnSlotClicked()
    {
        Debug.Log("Slot cliqué");
        if (memberData == null || menuManager == null)
        {
            Debug.LogWarning("SlotUI - memberData ou menuManager est null.");
            return;
        }
        StartCoroutine(menuManager.OnPlayerSlotClicked(memberData.player_id));
    }


    public IEnumerator SetSlot()
    {
        //memberIcon;
        //memberChampionshipIcon;
        memberLevelText.text = memberData.level.ToString();
        memberNameText.text = memberData.name;
        memberRankText.GetComponent<TextMeshProUGUI>().text = memberData.role_name;

        yield return 0;
    }

}
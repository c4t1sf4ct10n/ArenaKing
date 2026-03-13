using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using NUnit.Framework.Interfaces;
using static UnityEditor.Progress;

public class ChampUI : MonoBehaviour
{
    public Image playerIcon;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerRankText;


    public Player playerData;
    private MainMenuManager menuManager;

    public void Initialize(Player player, MainMenuManager manager)
    {
        playerData = player;
        menuManager = manager;
    }

    public void DestroySlot()
    {
        Destroy(this.gameObject);
    }

    public void OnSlotClicked()
    {
        Debug.Log("Slot cliqué");
        if (playerData == null || menuManager == null)
        {
            Debug.LogWarning("ChampUI - playerData ou menuManager est null.");
            return;
        }
        StartCoroutine(menuManager.OnPlayerSlotClicked(playerData.player_id));
    }


    public IEnumerator SetSlot()
    {
        //memberIcon;
        playerLevelText.text = playerData.level.ToString();
        playerNameText.text = playerData.name;
        playerRankText.text = playerData.current_points.ToString();

        yield return 0;
    }

}
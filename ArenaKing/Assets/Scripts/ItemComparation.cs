using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemComparation : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject itemComparePanel;

    public GameObject equippedLvlText;
    public GameObject equippedStatIcon;
    public GameObject equippedStatText;

    public GameObject selectedLvlText;
    public GameObject selectedStatIcon;
    public GameObject selectedStatText;

    public void ShowItemCompare(Item equippedItem, Item selectedItem)
    {
        //mainMenuPanel.SetActive(false);

        equippedLvlText.GetComponent<TextMeshProUGUI>().text = equippedItem.item_level.ToString();
        //equippedStatIcon.GetComponent<TextMeshProUGUI>().text = equippedItem.item_level.ToString();
        equippedStatText.GetComponent<TextMeshProUGUI>().text = equippedItem.stat_value.ToString();

        selectedLvlText.GetComponent<TextMeshProUGUI>().text = selectedItem.item_level.ToString();
        //selectedStatIcon.GetComponent<TextMeshProUGUI>().text = selectedItem.item_level.ToString();
        selectedStatText.GetComponent<TextMeshProUGUI>().text = selectedItem.stat_value.ToString();

        itemComparePanel.SetActive(true);
        
    }

    public void ExchangeItem()
    {
        // Charger la scène du combat
        GameManager.instance.LoadPlayer();
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitItemCompare()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using NUnit.Framework.Interfaces;
using static UnityEditor.Progress;

public class EquipmentUI : MonoBehaviour
{

    public Item itemData;
    private MainMenuManager menuManager;


    private void Awake()
    {
    }

    public void Initialize(Item item, MainMenuManager manager)
    {
        itemData = item;
        menuManager = manager;
    }

    private void Update()
    {
    }

    public void OnSlotClicked()
    {
        Debug.Log("Slot cliqué");
        if (itemData == null || menuManager == null)
        {
            Debug.LogWarning("EquipmentUI - itemData ou menuManager est null.");
            return;
        }

        menuManager.OnPlayerResumeItemClicked(itemData);
    }

}
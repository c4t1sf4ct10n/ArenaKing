using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using NUnit.Framework.Interfaces;
using static UnityEditor.Progress;
using TMPro.Examples;

public class LogUI : MonoBehaviour
{
    public TextMeshProUGUI message;
    public Image background;


    public Log logData;
    private MainMenuManager menuManager;

    private void Awake()
    {

    }

    public void Initialize(Log log, MainMenuManager manager)
    {
        logData = log;
        menuManager = manager;
    }

    private void Update()
    {

    }

    public void DestroySlot()
    {
        Destroy(this.gameObject);
    }


    //public void OnSlotClicked()
    //{
    //    Debug.Log("Slot cliqué");
    //    if (logData == null || menuManager == null)
    //    {
    //        Debug.LogWarning("SlotUI - logData ou menuManager est null.");
    //        return;
    //    }

    //    //menuManager.OnInventoryItemClicked(logData);
    //}


    public IEnumerator SetSlot()
    {
        // Mise en forme du message selon le type d'action
        string content = "";
        Color bgColor = Color.gray; // gris par défaut
        Color textColor = Color.black;

        switch (logData.action_type)
        {
            case "message":
                content = $"{logData.actor} : {logData.action_details}";
                bgColor = new Color(0.9f, 0.9f, 0.9f); // gris clair
                break;

            case "join":
                content = $"{logData.actor} a rejoint le clan.";
                bgColor = new Color(0.6f, 1f, 0.6f); // vert clair
                break;

            case "leave":
                content = $"{logData.actor} a quitté le clan.";
                bgColor = new Color(1f, 0.6f, 0.4f); // orange/rouge
                break;

            case "promote":
                content = $"{logData.actor} a promu {logData.target}."; // Personnalise si besoin
                bgColor = new Color(0.6f, 0.8f, 1f); // bleu clair
                break;

            case "demote":
                content = $"{logData.actor} a rétrogradé {logData.target}.";
                bgColor = new Color(1f, 0.85f, 0.6f); // orange clair
                break;

            case "kick":
                content = $"{logData.actor} a expulsé {logData.target}.";
                bgColor = new Color(1f, 0.6f, 0.6f); // rouge clair
                break;

            default:
                content = logData.action_details;
                bgColor = Color.gray;
                break;
        }

        message.text = content;
        message.color = textColor;
        background.color = bgColor;

        

        yield return null;
    }



}
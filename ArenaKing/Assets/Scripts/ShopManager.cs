using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class ShopManager : MonoBehaviour
{
    const string openChestUrl = "http://localhost:3000/api/openchest";

    void Start()
    {
    }    
       

    public void BuyChest(int chestType)
    {
        StartCoroutine(OpenChest(chestType));

    }

    IEnumerator OpenChest(int chestType)
    {
        Debug.Log("Achat d'un coffre de type : " + chestType.ToString()); // Message de débogage
        Debug.Log("Achat d'un coffre par le joueur : " + PlayerPrefs.GetString("playerId"));

        string loot_type = "";
        if (chestType == 1)
        {
            loot_type = "wooden_chest";
        } else if (chestType == 2)
        {
            loot_type = "silver_chest";
        } else if (chestType == 3)
        {
            loot_type = "golden_chest";
        }
        Debug.Log("Achat d'un coffre de type : " + loot_type);


        WWWForm form = new WWWForm();
        form.AddField("playerId", PlayerPrefs.GetString("playerId"));
        form.AddField("chestType", loot_type);

        UnityWebRequest request = UnityWebRequest.Post(openChestUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            List<Item> result = JsonConvert.DeserializeObject<List<Item>>(jsonResponse);
        }
        else
        {
            Debug.LogError("Combat API error: " + request.error);
        }

    }

    
}

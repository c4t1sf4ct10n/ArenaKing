using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // ROOT
    // -- PROD
    //const string rootApiUrl = "https://arenakingserver-g9b2chh4enbbe0hz.northeurope-01.azurewebsites.net/api/";
    // -- LOCAL
    const string rootApiUrl = "http://localhost:3000/api/";
    // GET
    const string checkDeviceUrl = rootApiUrl + "checkDevice/";
    const string playerUrl = rootApiUrl + "player/";
    // POST
    const string createUserUrl = rootApiUrl + "createUser";
    const string openChestUrl = "http://localhost:3000/api/openchest";

    // Stocker les informations de l'utilisateur et du personnage
    public string deviceId;
    public User user;
    public Player player;

    private string userId;
    private string playerId;
    private string inventoryId;
    private int playerFreemiumCurrency;
    private int playerPremiumCurrency;

      

    const string getInventoryUrl = "http://localhost:3000/api/inventory/";
    const string getEquipmentsUrl = "http://localhost:3000/api/equipments/";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartApp()
    {
        deviceId = SystemInfo.deviceUniqueIdentifier;
        StartCoroutine(UpdateUser());
    }

    private IEnumerator CheckPlayer()
    {
        UnityWebRequest request = UnityWebRequest.Get(checkDeviceUrl + deviceId);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            if (jsonResponse.Contains("exists"))
            {
                StartCoroutine(CreateUser());
            }
            else
            {
                user = JsonConvert.DeserializeObject<User>(jsonResponse);
                StartCoroutine(LoadPlayerDetails(user.player_id));
            }
        }
    }

    IEnumerator LoadPlayerDetails(string playerId)
    {
        UnityWebRequest request = UnityWebRequest.Get(playerUrl + playerId);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            player = JsonConvert.DeserializeObject<Player>(jsonResponse);

            // Puis transition vers le MainMenu
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator CreateUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("device_id", deviceId);

        UnityWebRequest request = UnityWebRequest.Post(createUserUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            user = JsonConvert.DeserializeObject<User>(jsonResponse); // Désérialiser les données utilisateur
            Debug.Log("Utilisateur créé avec succès !");
            StartCoroutine(UpdatePlayer()); // Charger les détails du joueur
        }
        else
        {
            Debug.LogError("Erreur lors de la création du compte : " + request.error);
        }
    }

    IEnumerator UpdateUser()
    {
        UnityWebRequest request = UnityWebRequest.Get(checkDeviceUrl + deviceId);
        Debug.Log(deviceId);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            if (jsonResponse.Contains("exists"))
            {
                StartCoroutine(CreateUser());
            }
            else
            {
                Debug.Log(jsonResponse);
                // Stocker les données de l'utilisateur existant
                user = JsonConvert.DeserializeObject<User>(jsonResponse); // Désérialiser les données utilisateur
                Debug.Log("Utilisateur existant : " + user.user_id);
                StartCoroutine(UpdatePlayer()); // Charger les détails du joueur
            }
        }
        else
        {
            Debug.LogError("Erreur lors de la vérification du compte : " + request.error);
        }
    }

    IEnumerator UpdatePlayer()
    {
        Debug.Log("GameManager - UpdatePlayerDetails - Debut");
        UnityWebRequest request = UnityWebRequest.Get(playerUrl + user.player_id);
        Debug.Log("playerUrl : " + playerUrl);
        Debug.Log("playerId : " + user.player_id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            player = JsonConvert.DeserializeObject<Player>(jsonResponse); // Désérialiser les données du joueur
            Debug.Log("ID du joueur : " + player.player_id);
            Debug.Log("Niveau du joueur : " + player.level);
            StartCoroutine(UpdateEquipments());
            StartCoroutine(UpdateInventory());
            // Transition vers la scène principale après chargement
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogError("Erreur lors de la récupération des données du joueur : " + request.error);
        }
    }

    IEnumerator UpdateInventory()
    {
        UnityWebRequest request = UnityWebRequest.Get(getInventoryUrl + player.inventory_id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            // Désérialiser les objets depuis la réponse de l'API
            player.unequippedItems = JsonConvert.DeserializeObject<List<Item>>(jsonResponse);
        }
        else
        {
            Debug.LogError("Erreur lors du chargement de l'inventaire: " + request.error);
        }
    }

    IEnumerator UpdateEquipments()
    {
        UnityWebRequest request = UnityWebRequest.Get(getEquipmentsUrl + player.player_id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            // Désérialiser les objets depuis la réponse de l'API
            player.equippedItems = JsonConvert.DeserializeObject<List<Item>>(request.downloadHandler.text);
            //DisplayEquipments();
        }
        else
        {
            Debug.LogError("Erreur lors du chargement de l'inventaire: " + request.error);
        }
    }

    public void LoadPlayer()
    {
        StartCoroutine(UpdateUser());
    }

    public void LoadPlayerDetails()
    {
        // Loading logic goes here
    }
    public void SetDeviceId(string id)
    {
        deviceId = id;
    }
    public void SetPlayerId(string id)
    {
        playerId = id;
    }
    public string GetPlayerId()
    {
        return playerId;
    }
    public void SetInventoryId(string id)
    {
        inventoryId = id;
    }
    public string GetInventoryId()
    {
        return inventoryId;
    }
    public void SetUserId(string id)
    {
        userId = id;
    }
    public string GetUserId()
    {
        return userId;
    }
    public int GetPlayerCurrencyFreemium()
    {
        return player.freemium_currency;
    }
    public int GetPlayerCurrencyPremium()
    {
        return player.premium_currency;
    }
    public int GetPlayerAttack()
    {
        return player.attack;
    }
    public int GetPlayerHealth()
    {
        return player.health;
    }

    public void BuyChest(int chestType)
    {
        StartCoroutine(OpenChest(chestType));

    }

    IEnumerator OpenChest(int chestType)
    {
        Debug.Log("Achat d'un coffre de type : " + chestType.ToString()); // Message de débogage
        Debug.Log("Achat d'un coffre par le joueur : " + player.player_id);

        string loot_type = "";
        if (chestType == 1)
        {
            loot_type = "wooden_chest";
        }
        else if (chestType == 2)
        {
            loot_type = "silver_chest";
        }
        else if (chestType == 3)
        {
            loot_type = "golden_chest";
        }
        Debug.Log("Achat d'un coffre de type : " + loot_type);


        WWWForm form = new WWWForm();
        form.AddField("playerId", player.player_id);
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
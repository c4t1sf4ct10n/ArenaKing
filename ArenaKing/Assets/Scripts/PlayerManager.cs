using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

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

    private string deviceId;
    private string userId;
    private string playerId;
    private string inventoryId;
    private int playerFreemiumCurrency;
    private int playerPremiumCurrency;
    private Player player;
    private User user;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Keep the object alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadPlayer()
    {
        StartCoroutine(GetPlayer());
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
            PlayerPrefs.SetString("userId", user.user_id);
            PlayerPrefs.SetString("playerId", user.player_id);
            StartCoroutine(GetPlayerDetails(user.player_id)); // Charger les détails du joueur
        }
        else
        {
            Debug.LogError("Erreur lors de la création du compte : " + request.error);
        }
    }
    IEnumerator GetPlayer()
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
                // Stocker les données de l'utilisateur existant
                user = JsonConvert.DeserializeObject<User>(jsonResponse); // Désérialiser les données utilisateur
                PlayerPrefs.SetString("userId", user.user_id);
                PlayerPrefs.SetString("playerId", user.player_id);
                StartCoroutine(GetPlayerDetails(user.player_id)); // Charger les détails du joueur
            }
        }
        else
        {
            Debug.LogError("Erreur lors de la vérification du compte : " + request.error);
        }
    }
    IEnumerator GetPlayerDetails(string playerId)
    {
        UnityWebRequest request = UnityWebRequest.Get(playerUrl + playerId);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            player = JsonConvert.DeserializeObject<Player>(jsonResponse); // Désérialiser les données du joueur
            PlayerPrefs.SetString("playerId", player.player_id);
            PlayerManager.instance.SetPlayerId(player.player_id);

            PlayerPrefs.SetString("inventoryId", player.inventory_id);
            PlayerManager.instance.SetInventoryId(player.inventory_id);

            Debug.Log("Niveau du joueur : " + player.level);
            // Transition vers la scène principale après chargement
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogError("Erreur lors de la récupération des données du joueur : " + request.error);
        }
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
        return player.currency_freemium;
    }
    public int GetPlayerCurrencyPremium()
    {
        return player.currency_premium;
    }
    public int GetPlayerAttack()
    {
        return player.attack;
    }
    public int GetPlayerHealth()
    {
        return player.health;
    }
}
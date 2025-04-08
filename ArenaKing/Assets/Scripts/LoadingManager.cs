using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class LoadingManager : MonoBehaviour
{
    //const string rootApiUrl = "https://arenakingserver-g9b2chh4enbbe0hz.northeurope-01.azurewebsites.net/api/";
    const string rootApiUrl = "http://localhost:3000/api/";
    private string deviceId;
    //GET
    private string checkDeviceUrl = rootApiUrl + "checkDevice/";
    private string playerUrl = rootApiUrl + "player/";
    //POST
    private string createUserUrl = rootApiUrl + "createUser";

    // Stocker les informations de l'utilisateur et du personnage
    public User currentUser;
    public Player currentPlayer;

    void Start()
    {
        if (Application.isPlaying)
        {
            deviceId = SystemInfo.deviceUniqueIdentifier;
            PlayerManager.instance.SetDeviceId(deviceId);
            PlayerManager.instance.LoadPlayer();
        }
    }

    IEnumerator GetPlayer()
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
                currentUser = JsonConvert.DeserializeObject<User>(jsonResponse); // Désérialiser les données utilisateur
                Debug.Log("Utilisateur existant : " + currentUser.user_id);
                PlayerPrefs.SetString("userId", currentUser.user_id);
                StartCoroutine(GetPlayerDetails(currentUser.user_id)); // Charger les détails du joueur
            }
        }
        else
        {
            Debug.LogError("Erreur lors de la vérification du compte : " + request.error);
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
            currentUser = JsonConvert.DeserializeObject<User>(jsonResponse); // Désérialiser les données utilisateur
            Debug.Log("Utilisateur créé avec succès !");
            PlayerPrefs.SetString("userId", currentUser.user_id);
            StartCoroutine(GetPlayerDetails(currentUser.user_id)); // Charger les détails du joueur
        }
        else
        {
            Debug.LogError("Erreur lors de la création du compte : " + request.error);
        }
    }

    IEnumerator GetPlayerDetails(string userId)
    {
        UnityWebRequest request = UnityWebRequest.Get(playerUrl + userId);
        Debug.Log(userId);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            currentPlayer = JsonConvert.DeserializeObject<Player>(jsonResponse); // Désérialiser les données du joueur
            PlayerPrefs.SetString("playerId", currentPlayer.player_id);
            PlayerPrefs.SetString("inventoryId", currentPlayer.inventory_id);
            PlayerManager.instance.SetPlayerId(currentPlayer.player_id);

            Debug.Log("Niveau du joueur : " + currentPlayer.level);
            // Transition vers la scène principale après chargement
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogError("Erreur lors de la récupération des données du joueur : " + request.error);
        }
    }
}

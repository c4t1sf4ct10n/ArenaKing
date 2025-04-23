using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string deviceId;
    public User user;
    public Player player;

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
        StartCoroutine(CheckPlayer());
    }

    IEnumerator CheckPlayer()
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
                PlayerPrefs.SetString("userId", user.user_id);
                PlayerPrefs.SetString("playerId", user.player_id);
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
            PlayerPrefs.SetString("playerId", player.player_id);
            PlayerPrefs.SetString("inventoryId", player.inventory_id);

            // Puis transition vers le MainMenu
            SceneManager.LoadScene("MainMenu");
        }
    }
}
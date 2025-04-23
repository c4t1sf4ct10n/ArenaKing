using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

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

    private string deviceId;
    private string userId;
    private string playerId;
    private string inventoryId;
    private int playerFreemiumCurrency;
    private int playerPremiumCurrency;
    private Player player;
    private User user;

    public GameObject CurrencyFreemiumButton;
    public GameObject CurrencyPremiumButton;
    public GameObject CurrencyFreemiumText;
    public GameObject CurrencyPremiumText;

    public GameObject inventoryPanel; // Panel de l'inventaire où afficher les objets non équipés
    public GameObject shieldVisual;
    public GameObject pauldronsVisual;
    public GameObject chestVisual;
    public GameObject bootsVisual;
    public GameObject weaponVisual;
    public GameObject helmetVisual;
    public GameObject glovesVisual;
    public GameObject greavesVisual;
    public GameObject shieldSlot;
    public GameObject pauldronsSlot;
    public GameObject chestSlot;
    public GameObject bootsSlot;
    public GameObject weaponSlot;
    public GameObject helmetSlot;
    public GameObject glovesSlot;
    public GameObject greavesSlot;
    public GameObject health;
    public GameObject attack;
    public Sprite healthIcon;
    public Sprite attackIcon;
    public Sprite shieldIcon;
    public Sprite pauldronsIcon;
    public Sprite chestIcon;
    public Sprite bootsIcon;
    public Sprite weaponIcon;
    public Sprite helmetIcon;
    public Sprite glovesIcon;
    public Sprite greavesIcon;
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;
    public GameObject slot5;
    public GameObject slot6;
    public GameObject slot7;
    public GameObject slot8;
    public GameObject slot9;
    public GameObject slot10;
    public GameObject slot11;
    public GameObject slot12;
    public GameObject slot13;
    public GameObject slot14;
    public GameObject slot15;
    public GameObject slot16;
    public GameObject slot17;
    public GameObject slot18;
    public GameObject slot19;
    public GameObject slot20;
    public GameObject slot21;
    public GameObject slot22;
    public GameObject slot23;
    public GameObject slot24;
    public GameObject slot25;
    public GameObject slot26;
    public GameObject slot27;
    public GameObject slot28;
    public GameObject slot29;
    public GameObject slot30;
    private GameObject slot;
    public GameObject itemButtonPrefab; // Préfabriqué pour les boutons d'item
    private List<Item> unequippedItems = new List<Item>();
    private List<Item> equippedItems = new List<Item>();

    const string getInventoryUrl = "http://localhost:3000/api/inventory/";
    const string getEquipmentsUrl = "http://localhost:3000/api/equipments/";

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

    void Start()
    {
        RefreshCurrency();
        inventoryPanel = GameObject.Find("InventoryPanel");
        shieldVisual = GameObject.Find("ShieldVisual");
        pauldronsVisual = GameObject.Find("PauldronsVisual");
        chestVisual = GameObject.Find("ChestVisual");
        bootsVisual = GameObject.Find("BootsVisual");
        weaponVisual = GameObject.Find("WeaponVisual");
        helmetVisual = GameObject.Find("HelmetVisual");
        glovesVisual = GameObject.Find("GlovesVisual");
        greavesVisual = GameObject.Find("GreavesVisual");
        shieldSlot = GameObject.Find("ShieldSlot");
        pauldronsSlot = GameObject.Find("PauldronsSlot");
        chestSlot = GameObject.Find("ChestSlot");
        bootsSlot = GameObject.Find("BootsSlot");
        weaponSlot = GameObject.Find("WeaponSlot");
        helmetSlot = GameObject.Find("HelmetSlot");
        glovesSlot = GameObject.Find("GlovesSlot");
        greavesSlot = GameObject.Find("GreavesSlot");
        health = GameObject.Find("Health");
        attack = GameObject.Find("Attack");
        //healthIcon = GameObject.Find("ShieldVisual");
        //attackIcon = GameObject.Find("ShieldVisual");
        //shieldIcon = GameObject.Find("ShieldVisual");
        //pauldronsIcon = GameObject.Find("ShieldVisual");
        //chestIcon = GameObject.Find("ShieldVisual");
        //bootsIcon = GameObject.Find("ShieldVisual");
        //weaponIcon = GameObject.Find("ShieldVisual");
        //helmetIcon = GameObject.Find("ShieldVisual");
        //glovesIcon = GameObject.Find("ShieldVisual");
        //greavesIcon = GameObject.Find("ShieldVisual");
        StartCoroutine(LoadInventory());
        StartCoroutine(LoadEquipments());
    }

    public void RefreshCurrency()
    {
        CurrencyFreemiumText = GameObject.Find("CurrencyFreemiumText");
        CurrencyFreemiumText.GetComponent<Text>().text = MainMenuManager.instance.GetPlayerCurrencyFreemium().ToString();
        Debug.Log(MainMenuManager.instance.GetPlayerCurrencyFreemium().ToString());

    }

    public void RefreshInventory()
    {
        inventoryPanel = GameObject.Find("InventoryPanel");
        shieldVisual = GameObject.Find("ShieldVisual");
        pauldronsVisual = GameObject.Find("PauldronsVisual");
        chestVisual = GameObject.Find("ChestVisual");
        bootsVisual = GameObject.Find("BootsVisual");
        weaponVisual = GameObject.Find("WeaponVisual");
        helmetVisual = GameObject.Find("HelmetVisual");
        glovesVisual = GameObject.Find("GlovesVisual");
        greavesVisual = GameObject.Find("GreavesVisual");
        shieldSlot = GameObject.Find("ShieldSlot");
        pauldronsSlot = GameObject.Find("PauldronsSlot");
        chestSlot = GameObject.Find("ChestSlot");
        bootsSlot = GameObject.Find("BootsSlot");
        weaponSlot = GameObject.Find("WeaponSlot");
        helmetSlot = GameObject.Find("HelmetSlot");
        glovesSlot = GameObject.Find("GlovesSlot");
        greavesSlot = GameObject.Find("GreavesSlot");
        health = GameObject.Find("Health");
        attack = GameObject.Find("Attack");
        StartCoroutine(LoadInventory());
        StartCoroutine(LoadEquipments());
    }

    IEnumerator LoadInventory()
    {
        Debug.Log(PlayerPrefs.GetString("inventoryId"));
        UnityWebRequest request = UnityWebRequest.Get(getInventoryUrl + PlayerPrefs.GetString("inventoryId"));
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            // Désérialiser les objets depuis la réponse de l'API
            unequippedItems = JsonConvert.DeserializeObject<List<Item>>(jsonResponse);
            StartCoroutine(DisplayInventory());
        }
        else
        {
            Debug.LogError("Erreur lors du chargement de l'inventaire: " + request.error);
        }
    }

    IEnumerator DisplayInventory()
    {
        int i = 0;
        foreach (var item in unequippedItems)
        {
            Debug.Log(item.item_name);
            // Créer un bouton pour chaque item et l'afficher dans le panel
            //GameObject itemButton = Instantiate(itemButtonPrefab, inventoryPanel.transform);
            //itemButton.GetComponentInChildren<Text>().text = item.name; // Affiche le nom de l'item
            //itemButton.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
            //C'est moche mais c'est pour les tests
            //itemVisual = itemVisual;
            switch (i)
            {
                case 0: slot = slot1; break;
                case 1: slot = slot2; break;
                case 2: slot = slot3; break;
                case 3: slot = slot4; break;
                case 4: slot = slot5; break;
                case 5: slot = slot6; break;
                case 6: slot = slot7; break;
                case 7: slot = slot8; break;
                case 8: slot = slot9; break;
                case 9: slot = slot10; break;
                case 10: slot = slot11; break;
                case 11: slot = slot12; break;
                case 12: slot = slot13; break;
                case 13: slot = slot14; break;
                case 14: slot = slot15; break;
                case 15: slot = slot16; break;
                case 16: slot = slot17; break;
                case 17: slot = slot18; break;
                case 18: slot = slot19; break;
                case 19: slot = slot20; break;
                case 20: slot = slot21; break;
                case 21: slot = slot22; break;
                case 22: slot = slot23; break;
                case 23: slot = slot24; break;
                case 24: slot = slot25; break;
                case 25: slot = slot26; break;
                case 26: slot = slot27; break;
                case 27: slot = slot28; break;
                case 28: slot = slot29; break;
                case 29: slot = slot30; break;
            }
            switch (item.item_type_id)
            {
                case "2DA73382-6DD7-44F6-8B24-A6B9896B0694": //offhand
                    slot.GetComponent<Image>().sprite = shieldIcon;
                    //shieldVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    //shieldSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "37A560D9-F3D1-4664-A8B0-7D363DDD4B9F": //shoulders
                    slot.GetComponent<Image>().sprite = pauldronsIcon;
                    //pauldronsVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    //pauldronsSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "AC6AA9DC-B7EC-4214-A11D-A875C78D0293": //body
                    slot.GetComponent<Image>().sprite = chestIcon;
                    //chestVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    //chestSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "1DA42203-BBD4-400A-9FBB-716DA54486BB": //foot
                    slot.GetComponent<Image>().sprite = bootsIcon;
                    //bootsVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    //bootsSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "CC23CCB5-3159-437C-B1D3-A6481DA4BA19": //weapon
                    slot.GetComponent<Image>().sprite = weaponIcon;
                    //weaponVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    //weaponSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "D2E1771B-9B6B-4DFB-8C54-48F6CCF55397": //head
                    slot.GetComponent<Image>().sprite = helmetIcon;
                    //helmetVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    //helmetSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "8F9B6906-4227-4112-AF5D-04535148089D": //hands
                    slot.GetComponent<Image>().sprite = glovesIcon;
                    //glovesVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    //glovesSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "CFE789F3-6A7E-42D5-BD55-4324D31A7D30": //thigh
                    slot.GetComponent<Image>().sprite = greavesIcon;
                    //greavesVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    //greavesSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                default:
                    break;
            }
            i++;
        }
        yield return 0;
    }

    IEnumerator LoadEquipments()
    {
        UnityWebRequest request = UnityWebRequest.Get(getEquipmentsUrl + PlayerPrefs.GetString("playerId"));
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            // Désérialiser les objets depuis la réponse de l'API
            equippedItems = JsonConvert.DeserializeObject<List<Item>>(request.downloadHandler.text);
            DisplayEquipments();
        }
        else
        {
            Debug.LogError("Erreur lors du chargement de l'inventaire: " + request.error);
        }
    }

    void DisplayEquipments()
    {
        // On met à jour l'affichage des stats globales
        attack.GetComponent<Text>().text = MainMenuManager.instance.GetPlayerAttack().ToString();
        health.GetComponent<Text>().text = MainMenuManager.instance.GetPlayerHealth().ToString();



        foreach (var item in equippedItems)
        {
            Debug.Log(item.item_name);
            // Mettre à jour le visuel de l'équipement
            switch (item.item_type_id)
            {
                case "2DA73382-6DD7-44F6-8B24-A6B9896B0694": //offhand
                    //shieldVisual.GetComponent<Image>().sprite = //sprite de l'objet équipé
                    shieldVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    shieldSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "37A560D9-F3D1-4664-A8B0-7D363DDD4B9F": //shoulders
                    pauldronsVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    pauldronsSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "AC6AA9DC-B7EC-4214-A11D-A875C78D0293": //body
                    chestVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    chestSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "1DA42203-BBD4-400A-9FBB-716DA54486BB": //foot
                    bootsVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    bootsSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "CC23CCB5-3159-437C-B1D3-A6481DA4BA19": //weapon
                    weaponVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    weaponSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "D2E1771B-9B6B-4DFB-8C54-48F6CCF55397": //head
                    helmetVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    helmetSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "8F9B6906-4227-4112-AF5D-04535148089D": //hands
                    glovesVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    glovesSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                case "CFE789F3-6A7E-42D5-BD55-4324D31A7D30": //thigh
                    greavesVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    greavesSlot.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
                    break;
                default:
                    break;
            }

            //GameObject itemButton = Instantiate(itemButtonPrefab, inventoryPanel.transform);
            //itemButton.GetComponentInChildren<Text>().text = item.name; // Affiche le nom de l'item
            //itemButton.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
        }
    }

    void OnItemClicked(Item item)
    {
        Debug.Log("Item cliqué: " + item.item_name);
        // Ouvrir le panneau de comparaison ici
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
            MainMenuManager.instance.SetPlayerId(player.player_id);

            PlayerPrefs.SetString("inventoryId", player.inventory_id);
            MainMenuManager.instance.SetInventoryId(player.inventory_id);

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

[System.Serializable]
public class ItemList
{
    public List<Item> items;
}

public class Item
{
    public string inventory_item_id;
    public string inventory_id;
    public string item_name;
    public string item_type_id;
    public string item_model_id;
    public string rarity_id;
    public int item_quality;
    public int item_level;
    public string stat_id;
    public int stat_value;
    public string ability_id;
    public string affixe1_id;
    public int? affixe1_value;
    public string affixe2_id;
    public int? affixe2_value;
    public string affixe3_id;
    public int? affixe3_value;
    public string affixe4_id;
    public int? affixe4_value;
}

public class User
{
    public string user_id;    // UUID de l'utilisateur
    public string player_id;
    public string device_id;
    public string created_at;
    public string last_login;
}

public class Player
{
    public string player_id;
    public string inventory_id;
    public int status;
    public string user_id; //?
    public string level;
    public string experience;
    public int currency_freemium;
    public int currency_premium;
    public int health;
    public int attack;
    public int loot_series;
    public int loot_series_nb;
    public string weapon_id;
    public string offhand_id;
    public string head_id;
    public string shoulders_id;
    public string hands_id;
    public string body_id;
    public string thigh_id;
    public string foot_id;
}
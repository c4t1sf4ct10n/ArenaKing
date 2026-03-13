using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using TMPro.Examples;
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
    const string addCurrencyUrl = "http://localhost:3000/api/addcurrency";

    // Stocker les informations de l'utilisateur et du personnage
    public string deviceId;
    public User user;
    public Player player;
    public Clan clan;
    public Player selectedPlayer;

    private string userId;
    private string playerId;
    private string inventoryId;
    private int playerFreemiumCurrency;
    private int playerPremiumCurrency;

      

    const string getInventoryUrl = "http://localhost:3000/api/inventory/";
    const string getEquipmentsUrl = "http://localhost:3000/api/equipments/";
    const string getSocialUrl = "http://localhost:3000/api/equipments/";
    const string getClanUrl = "http://localhost:3000/api/clans/infos/";
    const string getClanLogsUrl = "http://localhost:3000/api/clan/logs/";
    const string searchClanUrl = "http://localhost:3000/api/clans/search/";
    const string sendClanLogUrl = "http://localhost:3000/api/clan/message/";    
    //const string getClanMembersUrl = "http://localhost:3000/api/clan/members/";
    const string getClanMembersUrl = rootApiUrl + "clan/members/";
    const string getChampMembersUrl = rootApiUrl + "clan/championship/";
    const string createClanUrl = "http://localhost:3000/api/clans/create/";
    const string leaveClanUrl = "http://localhost:3000/api/clans/leave/";
    const string joinClanUrl = "http://localhost:3000/api/clans/join/";
    const string opponentUrl = "http://localhost:3000/api/opponent/";
    const string promoteClanMemberUrl = "http://localhost:3000/api/clan/promote/";
    const string demoteClanMemberUrl = "http://localhost:3000/api/clan/demote/";
    const string kickClanMemberUrl = "http://localhost:3000/api/clan/kick/";




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
        StartCoroutine(LoadGame());
    }
    private IEnumerator LoadGame()
    {
        yield return StartCoroutine(UpdateUser());
        SceneManager.LoadScene("MainMenu");
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
                yield return StartCoroutine(CreateUser());
            }
            else
            {
                user = JsonConvert.DeserializeObject<User>(jsonResponse);
                yield return StartCoroutine(LoadPlayerDetails(user.player_id));
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
            yield return StartCoroutine(UpdatePlayer()); // Charger les détails du joueur
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
                yield return StartCoroutine(CreateUser());
            }
            else
            {
                Debug.Log(jsonResponse);
                // Stocker les données de l'utilisateur existant
                user = JsonConvert.DeserializeObject<User>(jsonResponse); // Désérialiser les données utilisateur
                Debug.Log("Utilisateur existant : " + user.user_id);
                yield return StartCoroutine(UpdatePlayer()); // Charger les détails du joueur
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
            Debug.Log("ID du joueur : " + player.player_id + " // Niveau du joueur : " + player.level.ToString());
            yield return StartCoroutine(UpdateEquipments());
            yield return StartCoroutine(UpdateInventory());
            if (player.clan_id != null)
            {
                yield return StartCoroutine(UpdateClan());
            }

        }
        else
        {
            Debug.LogError("Erreur lors de la récupération des données du joueur : " + request.error);
        }
    }


    public IEnumerator GetOpponent(Action<Player> onResult)
    {
        Debug.Log("GameManager - GetOpponent - Début");
        UnityWebRequest request = UnityWebRequest.Get(opponentUrl + player.player_id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Réponse JSON : " + jsonResponse);

            Player opponent = JsonConvert.DeserializeObject<Player>(jsonResponse);
            onResult?.Invoke(opponent); // Retourner l'opposant via le callback
        }
        else
        {
            Debug.LogError("Erreur lors de la récupération de l'adversaire : " + request.error);
            onResult?.Invoke(null); // Retourne null si erreur
        }
        Debug.Log("GameManager - GetOpponent - Fin");
    }

    public IEnumerator UpdateSocial()
    {
        Debug.Log("GameManager - UpdateSocial - Début");
        UnityWebRequest request = UnityWebRequest.Get(getSocialUrl + player.player_id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("UpdateSocial : " + jsonResponse);
            
        }
        else
        {
            Debug.LogError("Erreur lors du chargement du Social: " + request.error);
        }
        Debug.Log("GameManager - UpdateSocial - Fin");
    }

    public IEnumerator SearchClans(string searchTerm, Action<List<Clan>> onSuccess)
    {
        Debug.Log("GameManager - SearchClans - Début");
        if (searchTerm is null) { searchTerm = ""; }
        string url = searchClanUrl + "?term=" + UnityWebRequest.EscapeURL(searchTerm ?? "");
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("SearchClans : " + jsonResponse);

            List<Clan> clans = JsonConvert.DeserializeObject<List<Clan>>(jsonResponse);
            onSuccess?.Invoke(clans);
        }
        else
        {
            Debug.LogError("Erreur lors de la recherche de clans : " + request.error);
        }

        Debug.Log("GameManager - SearchClans - Fin");
    }
    public IEnumerator GetClanMembers(string clan_id, Action<List<Member>> onSuccess)
    {
        Debug.Log("GameManager - GetClanMembers - Début");
        if (clan_id is null) { clan_id = ""; }
        Debug.Log("clan_id : " + clan_id);
        UnityWebRequest request = UnityWebRequest.Get(getClanMembersUrl + clan_id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("GetClanMembers : " + jsonResponse);
            List<Member> members = JsonConvert.DeserializeObject<List<Member>>(jsonResponse);
            onSuccess?.Invoke(members);
        }
        else
        {
            Debug.LogError("Erreur lors de la récupération des membres du clan : " + request.error);
        }

        Debug.Log("GameManager - GetClanMembers - Fin");
    }

    public IEnumerator GetChampionshipMembers(string sublevel_id, Action<List<Player>> onSuccess)
    {
        Debug.Log("GameManager - GetChampionshipMembers - Début");
        if (sublevel_id is null) { sublevel_id = ""; }
        Debug.Log("sublevel_id : " + sublevel_id);
        UnityWebRequest request = UnityWebRequest.Get(getChampMembersUrl + sublevel_id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("GetChampionshipMembers : " + jsonResponse);
            List<Player> members = JsonConvert.DeserializeObject<List<Player>>(jsonResponse);
            onSuccess?.Invoke(members);
        }
        else
        {
            Debug.LogError("Erreur lors de la récupération des membres du clan : " + request.error);
        }

        Debug.Log("GameManager - GetChampionshipMembers - Fin");
    }

    public IEnumerator CreateClan(string clanName, string clanDescription, int clanRequiredLevel, Action<Clan> onSuccess)
    {
        Debug.Log("GameManager - CreateClan - Début");
        
        WWWForm form = new WWWForm();
        form.AddField("playerId", player.player_id);
        form.AddField("clanName", clanName);
        form.AddField("clanDescription", clanDescription);
        form.AddField("clanRequiredLevel" , clanRequiredLevel);

        UnityWebRequest request = UnityWebRequest.Post(createClanUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("CreateClan : " + jsonResponse);
            Clan tClan = JsonConvert.DeserializeObject<Clan>(jsonResponse);
            yield return StartCoroutine(UpdatePlayer());
            onSuccess?.Invoke(tClan);

        }
        else
        {
            Debug.LogError("Combat API error: " + request.error);
        }

        Debug.Log("GameManager - CreateClan - Fin");
    }

    public IEnumerator LeaveClan()
    {
        Debug.Log("GameManager - LeaveClan - Début");

        WWWForm form = new WWWForm();
        form.AddField("playerId", player.player_id);

        UnityWebRequest request = UnityWebRequest.Post(leaveClanUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("LeaveClan : " + jsonResponse);
            yield return StartCoroutine(UpdatePlayer());
        }
        else
        {
            Debug.LogError("Combat API error: " + request.error);
        }

        Debug.Log("GameManager - LeaveClan - Fin");
    }

    public IEnumerator JoinClan(Clan clan)
    {
        Debug.Log("GameManager - JoinClan - Début");

        WWWForm form = new WWWForm();
        form.AddField("playerId", player.player_id);
        form.AddField("clanId", clan.clan_id);
        Debug.Log("clan.clan_id : " + clan.clan_id);
        Debug.Log("player.player_id : " + player.player_id);
        UnityWebRequest request = UnityWebRequest.Post(joinClanUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("JoinClan : " + jsonResponse);
            yield return StartCoroutine(UpdatePlayer());
        }
        else
        {
            Debug.LogError("Combat API error: " + request.error);
        }

        Debug.Log("GameManager - JoinClan - Fin");
    }

    public IEnumerator PromoteClanMember()
    {
        Debug.Log("GameManager - PromoteClanMember - Début");
        string new_role_id = "";
        if (selectedPlayer.role_id == "null") yield return null;
        if (selectedPlayer.role_id == "E40398D6-AD90-4AF7-BE64-17F0443A3596") new_role_id = "5C360FCB-3196-4957-A815-9243890A77B4";
        if (selectedPlayer.role_id == "5C360FCB-3196-4957-A815-9243890A77B4") new_role_id = "538696AE-88A9-404C-94ED-AC0791F965D2";
        if (selectedPlayer.role_id == "538696AE-88A9-404C-94ED-AC0791F965D2") new_role_id = "FCE40B40-22DE-45CF-ABED-D5EA6EE94B27";

        WWWForm form = new WWWForm();
        Debug.Log("GameManager - PromoteClanMember - Début - selectedPlayer.clan_id : " + selectedPlayer.clan_id);
        Debug.Log("GameManager - PromoteClanMember - Début - player.player_id : " + player.player_id);
        Debug.Log("GameManager - PromoteClanMember - Début - selectedPlayer.player_id : " + selectedPlayer.player_id);
        Debug.Log("GameManager - PromoteClanMember - Début - new_role_id : " + new_role_id);
        form.AddField("clanId", selectedPlayer.clan_id);
        form.AddField("playerId", player.player_id);
        form.AddField("targetId", selectedPlayer.player_id);
        form.AddField("roleId", new_role_id);

        UnityWebRequest request = UnityWebRequest.Post(promoteClanMemberUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("PromoteClanMember : " + jsonResponse);
            yield return StartCoroutine(GetPlayerInformations(selectedPlayer.player_id));
        }
        else
        {
            Debug.LogError("Combat API error: " + request.error);
        }

        Debug.Log("GameManager - PromoteClanMember - Fin");
        yield return null;
    }

    public IEnumerator DemoteClanMember()
    {
        Debug.Log("GameManager - DemoteClanMember - Début");
        string new_role_id = "";
        if (selectedPlayer.role_id == "null") yield return null;
        if (selectedPlayer.role_id == "538696AE-88A9-404C-94ED-AC0791F965D2") new_role_id = "5C360FCB-3196-4957-A815-9243890A77B4";
        if (selectedPlayer.role_id == "5C360FCB-3196-4957-A815-9243890A77B4") new_role_id = "E40398D6-AD90-4AF7-BE64-17F0443A3596";

        WWWForm form = new WWWForm();
        form.AddField("clanId", selectedPlayer.clan_id);
        form.AddField("playerId", player.player_id);
        form.AddField("targetId", selectedPlayer.player_id);
        form.AddField("roleId", new_role_id);

        UnityWebRequest request = UnityWebRequest.Post(demoteClanMemberUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("DemoteClanMember : " + jsonResponse);
            yield return StartCoroutine(GetPlayerInformations(selectedPlayer.player_id));
        }
        else
        {
            Debug.LogError("Combat API error: " + request.error);
        }

        Debug.Log("GameManager - DemoteClanMember - Fin");
        yield return null;
    }

    public IEnumerator KickClanMember()
    {
        Debug.Log("GameManager - KickClanMember - Début");

        WWWForm form = new WWWForm();
        form.AddField("clanId", selectedPlayer.clan_id);
        form.AddField("playerId", player.player_id);
        form.AddField("targetId", selectedPlayer.player_id);

        UnityWebRequest request = UnityWebRequest.Post(kickClanMemberUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("KickClanMember : " + jsonResponse);
            yield return StartCoroutine(GetPlayerInformations(selectedPlayer.player_id));
        }
        else
        {
            Debug.LogError("Combat API error: " + request.error);
        }

        Debug.Log("GameManager - KickClanMember - Fin");
        yield return null;
    }

    public IEnumerator UpdateClan()
    {
        Debug.Log("GameManager - UpdateClan - Début");
        UnityWebRequest request = UnityWebRequest.Get(getClanUrl + player.clan_id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("UpdateClan : " + jsonResponse);
            clan = JsonConvert.DeserializeObject<Clan>(jsonResponse);
            // Chargement des logs du clan
            yield return StartCoroutine(UpdateClanLogs());
        }
        else
        {
            Debug.LogError("Erreur lors du chargement du Clan: " + request.error);
        }
        Debug.Log("GameManager - UpdateClan - Fin");
    }
    public IEnumerator SendClanLog(string message)
    {
        Debug.Log("GameManager - SendClanLog - Début");

        message ??= "";
        string clanId = clan?.clan_id ?? "";
        string playerId = player?.player_id ?? "";

        // Création d’un dictionnaire de paramètres
        WWWForm form = new WWWForm();
        form.AddField("message", message);
        form.AddField("clanId", clanId);
        form.AddField("playerId", playerId);

        UnityWebRequest request = UnityWebRequest.Post(sendClanLogUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("SendClanLog : " + jsonResponse);
        }
        else
        {
            Debug.LogError("Erreur lors de l'envoi du log de clan : " + request.error);
        }

        Debug.Log("GameManager - SendClanLog - Fin");
    }
    public IEnumerator UpdateClanLogs()
    {
        Debug.Log("GameManager - UpdateClanLogs - Début");
        UnityWebRequest request = UnityWebRequest.Get(getClanLogsUrl + player.clan_id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("UpdateClanLogs : " + jsonResponse);
            if (jsonResponse != "[]")
            {
                clan.clanLogs = JsonConvert.DeserializeObject<List<Log>>(jsonResponse);
            }
        }
        else
        {
            Debug.LogError("Erreur lors du chargement du Clan: " + request.error);
        }
        Debug.Log("GameManager - UpdateClanLogs - Fin");
    }

    IEnumerator UpdateInventory()
    {
        Debug.Log("GameManager - UpdateInventory - Début");
        UnityWebRequest request = UnityWebRequest.Get(getInventoryUrl + player.inventory_id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("UpdateInventory : " + jsonResponse);
            // Désérialiser les objets depuis la réponse de l'API
            player.unequippedItems = JsonConvert.DeserializeObject<List<Item>>(jsonResponse);
        }
        else
        {
            Debug.LogError("Erreur lors du chargement de l'inventaire: " + request.error);
        }
        Debug.Log("GameManager - UpdateInventory - Fin");
    }

    IEnumerator UpdateEquipments()
    {
        Debug.Log("GameManager - UpdateEquipments - Début");
        UnityWebRequest request = UnityWebRequest.Get(getEquipmentsUrl + player.player_id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            // Désérialiser les objets depuis la réponse de l'API
            player.equippedItems = JsonConvert.DeserializeObject<List<Item>>(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Erreur lors du chargement de l'inventaire: " + request.error);
        }
        Debug.Log("GameManager - UpdateEquipments - Fin");
    }

    IEnumerator UpdateEquipments(Player player)
    {
        Debug.Log("GameManager - UpdateEquipments - Début");
        UnityWebRequest request = UnityWebRequest.Get(getEquipmentsUrl + player.player_id);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            // Désérialiser les objets depuis la réponse de l'API
            player.equippedItems = JsonConvert.DeserializeObject<List<Item>>(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Erreur lors du chargement de l'inventaire: " + request.error);
        }
        Debug.Log("GameManager - UpdateEquipments - Fin");
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

    public IEnumerator BuyChest(int chestType)
    {
        yield return StartCoroutine(OpenChest(chestType));
    }

    public int GetHealthPotionsNumber()
    {
        return player.health_pot_nb;
    }
    public int GetAttackPotionsNumber()
    {
        return player.attack_pot_nb;
    }
    public int GetFireResistPotionsNumber()
    {
        return player.fire_res_pot_nb;
    }
    public int GetIceResistPotionsNumber()
    {
        return player.ice_res_pot_nb;
    }
    public int GetEarthResistPotionsNumber()
    {
        return player.earth_res_pot_nb;
    }
    public int GetWindResistPotionsNumber()
    {
        return player.wind_res_pot_nb;
    }

    IEnumerator OpenChest(int chestType)
    {
        Debug.Log("GameManager - OpenChest - Début");
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
            yield return StartCoroutine(UpdatePlayer());
        }
        else
        {
            Debug.LogError("Combat API error: " + request.error);
        }
        Debug.Log("GameManager - OpenChest - Fin");
        yield return 0;
    }

    public IEnumerator AddCurrency(int currencyType)
    {
        Debug.Log("GameManager - AddCurrency - Début");

        string loot_type = "";
        if (currencyType == 1) //freemium
        {
            loot_type = "freemium";
        }
        else if (currencyType == 2) // Premium
        {
            loot_type = "premium";
        }
        Debug.Log("Ajout de 50 currency " + loot_type); // Message de débogage

        WWWForm form = new WWWForm();
        form.AddField("playerId", player.player_id);
        form.AddField("currency", loot_type);

        UnityWebRequest request = UnityWebRequest.Post(addCurrencyUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            yield return StartCoroutine(UpdatePlayer());
        }
        else
        {
            Debug.LogError("Combat API error: " + request.error);
        }
        Debug.Log("GameManager - AddCurrency - Fin");
    }

    public IEnumerator RemoveItem(Item item)
    {
        Debug.Log("GameManager - RemoveItem - Début");
        // Suppression de l'inventaire logique
        //player.unequippedItems.Remove(item);
        // Suppression de la base de données.
        yield return StartCoroutine(DeleteItemFromDatabase(item));
        yield return StartCoroutine(UpdatePlayer());
        Debug.Log("GameManager - RemoveItem - Fin");
        // Callback vers MainMenuManager
    }
    public IEnumerator RemoveItemID(string item)
    {
        Debug.Log("GameManager - RemoveItemID - Début");
        // Suppression de l'inventaire logique
        //player.unequippedItems.Remove(item);
        // Suppression de la base de données.
        yield return StartCoroutine(DeleteItemIDFromDatabase(item));
        yield return StartCoroutine(UpdatePlayer());
        Debug.Log("GameManager - RemoveItemID - Fin");
    }

    IEnumerator DeleteItemFromDatabase(Item item)
    {
        string deleteItemUrl = rootApiUrl + "deleteItem"; // Adapte le nom de ton endpoint selon ton API

        WWWForm form = new WWWForm();
        form.AddField("inventory_id", player.inventory_id);
        form.AddField("item_id", item.inventory_item_id); // Assure-toi que `item.item_id` correspond à l'ID attendu par ton backend

        UnityWebRequest request = UnityWebRequest.Post(deleteItemUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Item supprimé de la base de données avec succès !");
        }
        else
        {
            Debug.LogError("Erreur lors de la suppression de l'item : " + request.error);
        }
    }
    IEnumerator DeleteItemIDFromDatabase(string item)
    {
        string deleteItemUrl = rootApiUrl + "deleteItem";
        Debug.Log("inventory_id : " + player.inventory_id);
        Debug.Log("item_id : " + item);
        WWWForm form = new WWWForm();
        form.AddField("inventory_id", player.inventory_id);
        form.AddField("item_id", item); 

        UnityWebRequest request = UnityWebRequest.Post(deleteItemUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Item supprimé de la base de données avec succès !");
        }
        else
        {
            Debug.LogError("Erreur lors de la suppression de l'item : " + request.error);
        }
    }

    public IEnumerator ChangePlayerName(string new_name)
    {
        string changeNameUrl = rootApiUrl + "changePlayerName"; // Adapte le nom de ton endpoint selon ton API

        WWWForm form = new WWWForm();
        form.AddField("player_id", player.player_id);
        form.AddField("new_name", new_name); 

        UnityWebRequest request = UnityWebRequest.Post(changeNameUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Nom de joueur changé de la base de données avec succès !");
            yield return StartCoroutine(UpdatePlayer());
        }
        else
        {
            Debug.LogError("Erreur lors de la modification du nom : " + request.error);
        }
    }
    public IEnumerator SwapItem(Item item)
    {
        Debug.Log("GameManager - SwapItem - Début");
        // Suppression de la base de données.
        yield return StartCoroutine(SwapItemFromDatabase(item));
        yield return StartCoroutine(UpdatePlayer());
        Debug.Log("GameManager - SwapItem - Fin");
        // Callback vers MainMenuManager
    }
    IEnumerator SwapItemFromDatabase(Item item)
    {
        string swapItemUrl = rootApiUrl + "swapItem"; 

        WWWForm form = new WWWForm();
        form.AddField("player_id", player.player_id);
        form.AddField("new_item_id", item.inventory_item_id); 

        UnityWebRequest request = UnityWebRequest.Post(swapItemUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Item swaopé dans la base de données avec succès !");
        }
        else
        {
            Debug.LogError("Erreur lors de l'échange de l'item : " + request.error);
        }
    }
    public IEnumerator GetPlayerInformations(string playerId)
    {
        Debug.Log("GameManager - GetPlayerInformations - Début");
        UnityWebRequest request = UnityWebRequest.Get(playerUrl + playerId);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            selectedPlayer = JsonConvert.DeserializeObject<Player>(jsonResponse);
            yield return StartCoroutine(UpdateEquipments(selectedPlayer));
        }
        yield return null;
        Debug.Log("GameManager - GetPlayerInformations - Fin");
    }
}
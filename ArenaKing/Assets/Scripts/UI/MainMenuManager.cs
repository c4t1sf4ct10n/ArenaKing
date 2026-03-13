using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;
using System;
using System.Linq;
using Unity.VisualScripting;
using static UnityEditor.Progress;


public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public ScrollRect scrollRect;  // Reference au ScrollRect
    public RectTransform[] panels; // Liste des panels à afficher
    private int currentPanelIndex = 0; // Index du panel actuellement affiché

    private bool recyclingActive = false;
    public bool isRecycling = false;

    public GameObject currencyFreemiumText;
    public GameObject currencyPremiumText;
    public GameObject playerLvlText;
    public GameObject playerNameText;
    public GameObject playerExperienceText;
    public Slider experienceSlider;
    public Slider inventoryExperienceSlider;

    public GameObject inventoryPanel; // Panel de l'inventaire où afficher les objets non équipés
    public GameObject clanPanel;
    public GameObject socialPanel;
    public GameObject socialContent;
    public GameObject createContent;
    public GameObject inventoryNBSlots;
    public Transform gridParent; // un GameObject vide qui contient la grille
    public GameObject slotPrefab; // Préfabriqué pour les boutons d'item
    private List<SlotUI> slots = new List<SlotUI>();
    private List<ClanUI> clans = new List<ClanUI>();
    private List<MemberUI> clanMembers = new List<MemberUI>();
    private List<LogUI> clanLogs = new List<LogUI>();
    private List<ChampUI> champMembers = new List<ChampUI>();
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
    public GameObject fireRes;
    public GameObject iceRes;
    public GameObject earthRes;
    public GameObject windRes;
    public GameObject blockChance;
    public GameObject dodgeChance;
    public GameObject critChance;
    public GameObject critDamage;    
    public Sprite healthIcon;
    public Sprite attackIcon;
    public Sprite slotIcon;
    public Sprite shieldIcon;
    public Sprite pauldronsIcon;
    public Sprite chestIcon;
    public Sprite bootsIcon;
    public Sprite weaponIcon;
    public Sprite helmetIcon;
    public Sprite glovesIcon;
    public Sprite greavesIcon;
    public GameObject recycleButton;
    public GameObject recycleButtonText;
    public GameObject levelLabel;
    public GameObject winStreakNumber;
    public GameObject championshipSubLevel;
    public GameObject championshipMilestone;

    public GameObject dungeonAttributeValue;
    public GameObject dungeonLevelValue;

    public GameObject health_potions_number;
    public GameObject attack_potions_number;
    public GameObject fireresist_potions_number;
    public GameObject iceresist_potions_number;
    public GameObject earthresist_potions_number;
    public GameObject windresist_potions_number;
    public GameObject search_clans_text;
    public GameObject create_clan_text;
    public GameObject create_clan_description_text;
    public TMP_InputField create_clan_required_level;
    public int minLevel = 1;
    public int maxLevel = 30;
    private int currentLevel = 1;
    public Transform clansListContent;
    public GameObject clanPrefab;
    public GameObject clanNameLabel;
    public GameObject clanInfoLabel;
    public GameObject clanMembersLabel;


    public GameObject shopPanel;
    public GameObject optionsPanel;
    public GameObject optionsPlayerNameText;
    public GameObject optionsPlayerNamePlaceHolder;
    public GameObject optionsPlayerIDText;
    public GameObject optionsUserIDText;

    private GameObject slot;
    public Item equipped_item;
    public Item selected_item;
    public Clan selected_clan;

    public Transform clanMembersListContent;
    public GameObject clanMemberSlotPrefab;

    public Transform clanLogsListContent;
    public GameObject logMessagePrefab;
    public GameObject clanLogtext;
    [SerializeField] private ScrollRect clanLogScrollRect;

    public GameObject clanResumePanel;
    public GameObject clanNameText;
    public GameObject clanDescriptionText;
    public GameObject clanLvlText;
    public GameObject clanMembersText;
    public GameObject clanRequiredLvlText;
    public GameObject joinButton;
    public GameObject quitButton;

    public Transform champMembersListContent;
    public GameObject champMemberSlotPrefab;

    public GameObject playerResumePanel;
    public GameObject playerResumeShieldVisual;
    public GameObject playerResumePauldronsVisual;
    public GameObject playerResumeChestVisual;
    public GameObject playerResumeBootsVisual;
    public GameObject playerResumeWeaponVisual;
    public GameObject playerResumeHelmetVisual;
    public GameObject playerResumeGlovesVisual;
    public GameObject playerResumeGreavesVisual;
    public GameObject playerResumeShieldSlot;
    public GameObject playerResumePauldronsSlot;
    public GameObject playerResumeChestSlot;
    public GameObject playerResumeBootsSlot;
    public GameObject playerResumeWeaponSlot;
    public GameObject playerResumeHelmetSlot;
    public GameObject playerResumeGlovesSlot;
    public GameObject playerResumeGreavesSlot;
    public GameObject playerResumeHealth;
    public GameObject playerResumeAttack;
    public GameObject playerResumefireRes;
    public GameObject playerResumeiceRes;
    public GameObject playerResumeearthRes;
    public GameObject playerResumewindRes;
    public GameObject playerResumeblockChance;
    public GameObject playerResumedodgeChance;
    public GameObject playerResumecritChance;
    public GameObject playerResumecritDamage;

    public GameObject playerResumePromoteButton;
    public GameObject playerResumeDemoteButton;
    public GameObject playerResumeKickButton;

    public GameObject itemComparePanel;    
    public GameObject equippedLvlText;
    public GameObject equippedStatIcon;
    public GameObject equippedStatText;
    public GameObject selectedLvlText;
    public GameObject selectedStatIcon;
    public GameObject selectedStatText;

    public GameObject itemShowPanel;
    public GameObject showEquippedLvlText;
    public GameObject showEquippedStatIcon;
    public GameObject showEquippedStatText;

    public GameObject championshipPanel;

    public GameObject dungeonPanel;

    public GameObject campaignPanel;

    public GameObject mailboxPanel;

    public GameObject mainMenuPanel;
    public GameObject combatPrepPanel;

    public GameObject combatPrepPlayerLvlText;
    public GameObject combatPrepPlayerAttackText;
    public GameObject combatPrepPlayerDefenseText;
    public GameObject combatPrepPlayerNameText;

    public GameObject combatPrepOpponentLvlText;
    public GameObject combatPrepOpponentAttackText;
    public GameObject combatPrepOpponentDefenseText;
    public GameObject combatPrepOpponentNameText;

    private string player1_id;
    private string player2_id;
    private int player1_Max_Health;
    private int player2_Max_Health;
    private string player1_name;
    private string player2_name;


    void Start()
    {
        optionsPlayerNamePlaceHolder.GetComponent<TextMeshProUGUI>().text = GameManager.instance.player.name;
        optionsUserIDText.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.instance.user.user_id;
        optionsPlayerIDText.GetComponent<TextMeshProUGUI>().text = GameManager.instance.player.player_id;

        StartCoroutine(SetSocialPanels());
        StartCoroutine(SetPanel(2));

        create_clan_required_level.onEndEdit.AddListener(OnInputFieldChanged);
    }

    public void SwitchSocialContent(int panelindex)
    {
        switch (panelindex) { 
            case 0: 
                socialContent.SetActive(true);
                createContent.SetActive(false);
                break;
            case 1:
                socialContent.SetActive(false);
                createContent.SetActive(true);
                currentLevel = GameManager.instance.player.level;
                UpdateInputField();
                break;
        }
    }

    public void ScrollToPanel(int panelIndex)
    {
        Debug.Log("ScrollToPanel appelé avec index : " + panelIndex); // Message de débogage
        StartCoroutine(SetPanel(panelIndex));

    }

    IEnumerator SetSocialPanels()
    {
        if ((GameManager.instance.player.clan_id == null) || (GameManager.instance.player.clan_id == ""))
        {
            clanPanel.SetActive(false);
            socialPanel.SetActive(true);
            yield return StartCoroutine(GameManager.instance.UpdateSocial());
        }
        else
        {
            clanPanel.SetActive(true);
            socialPanel.SetActive(false);
            yield return StartCoroutine(GameManager.instance.UpdateClan());
            yield return StartCoroutine(DrawClan());
            yield return StartCoroutine(DrawClanLogs());
        }
        yield return 0;
    }
    IEnumerator SetPanel(int index)
    {
        currentPanelIndex = index;

        yield return StartCoroutine(DrawMainMenuHeader());

        switch (index)
        {
            case 0: // Inventory
                yield return StartCoroutine(DrawInventoryHeader());
                yield return StartCoroutine(DrawInventory());
                yield return StartCoroutine(DrawInventoryFooter());
                break;
            case 1: // Equipments
                yield return StartCoroutine(DrawEquipments());                
                break;
            case 2: // Main
                yield return StartCoroutine(DrawWinStreak());
                break;
            case 3: // Shop (qui sera transformé en Maitrise à terme
                //yield return StartCoroutine(DrawCurrency());
                break;
            case 4: // Social
                yield return StartCoroutine(DrawClan());
                yield return StartCoroutine(DrawClanLogs());
                break;
            case 5: // Clan
                yield return StartCoroutine(DrawClan());
                yield return StartCoroutine(DrawClanLogs());
                break;
            default:
                break;
        }

        // Limite l'index au nombre de panels disponibles
        index = Mathf.Clamp(index, 0, panels.Length - 2);
        currentPanelIndex = index;

        // Calculer la position cible dans le ScrollRect
        float targetX = (float)index / (panels.Length - 2);

        // Appliquer le deplacement en modifiant l'ancre horizontale du ScrollRect
        scrollRect.horizontalNormalizedPosition = targetX;
    }

    IEnumerator DrawClan()
    {
        Debug.Log("MainMenuManager - DrawClan - Début");
        clanNameLabel.GetComponent<TextMeshProUGUI>().text = GameManager.instance.clan.clan_name;
        clanInfoLabel.GetComponent<TextMeshProUGUI>().text = "Clan Level : " + GameManager.instance.clan.clan_lvl.ToString() + " || XP : " + GameManager.instance.clan.clan_experience.ToString() + " / " + GameManager.instance.clan.clan_xp_to_level.ToString();
        clanMembersLabel.GetComponent<TextMeshProUGUI>().text = GameManager.instance.clan.clan_total_members.ToString() + " / " + GameManager.instance.clan.clan_max_members.ToString();
        yield return 0;
        Debug.Log("MainMenuManager - DrawClan - Fin");
    }

    IEnumerator DrawClanLogs()
    {
        Debug.Log("MainMenuManager - DrawClanLogs - Début");
        foreach (Transform child in clanLogsListContent)
        {
            Destroy(child.gameObject);
        }
        clanLogs.Clear();

        foreach (var log in GameManager.instance.clan.clanLogs)
        {
            Debug.Log("ID du log : " + log.log_id);
            Debug.Log("ID du log : " + log.action_details);
            GameObject newSlotObj = Instantiate(logMessagePrefab, clanLogsListContent);
            newSlotObj.SetActive(true);
            newSlotObj.transform.SetParent(clanLogsListContent, false); // << Important pour dire à Unity de respecter l'ancrage du parent

            LogUI logUI = newSlotObj.GetComponent<LogUI>();

            //Button button = newSlotObj.GetComponent<Button>();
            //if (button != null)
            //{
            //    button.onClick.AddListener(logUI.OnSlotClicked);
            //}

            if (logUI == null)
            {
                Debug.LogError("LogUI component is missing on the slot prefab!");
            }

            logUI.Initialize(log, this); // this = MainMenuManager
            StartCoroutine(logUI.SetSlot());
            Debug.Log("Ajout d'un slot");
            clanLogs.Add(logUI);

        }
        StartCoroutine(ScrollToBottomAfterDelay());
        yield return 0;
        Debug.Log("MainMenuManager - DrawClanLogs - Fin");
    }
    IEnumerator ScrollToBottomAfterDelay()
    {
        yield return null; // attendre la fin du frame
        clanLogScrollRect.verticalNormalizedPosition = 0f; // 0 = bas
    }

    IEnumerator DrawMainMenuHeader()
    {
        Debug.Log("MainMenuManager - DrawCurrency - Début");
        currencyFreemiumText.GetComponent<Text>().text = GameManager.instance.GetPlayerCurrencyFreemium().ToString();
        currencyPremiumText.GetComponent<Text>().text = GameManager.instance.GetPlayerCurrencyPremium().ToString();
        playerLvlText.GetComponent<TextMeshProUGUI>().text = GameManager.instance.player.level.ToString();
        playerNameText.GetComponent<TextMeshProUGUI>().text = GameManager.instance.player.name;
        playerExperienceText.GetComponent<TextMeshProUGUI>().text = GameManager.instance.player.experience.ToString() + "/" + GameManager.instance.player.xp_to_next_level.ToString();
        experienceSlider.maxValue = (float)GameManager.instance.player.xp_to_next_level;
        experienceSlider.value = (float)GameManager.instance.player.experience;
        yield return 0;
        Debug.Log("MainMenuManager - DrawCurrency - Fin");
    }

    IEnumerator DrawInventoryHeader()
    {
        Debug.Log("MainMenuManager - DrawInventoryHeader - Début");
        inventoryNBSlots.GetComponent<TextMeshProUGUI>().text = GameManager.instance.player.unequippedItems.Count.ToString() + " / 30";
        yield return 0;
        Debug.Log("MainMenuManager - DrawInventoryHeader - Fin");
    }
    IEnumerator DrawInventoryFooter()
    {
        Debug.Log("MainMenuManager - DrawInventoryFooter - Début");
        string labelText = GameManager.instance.player.experience.ToString() + "/" + GameManager.instance.player.xp_to_next_level.ToString();
        levelLabel.GetComponent<TextMeshProUGUI>().text = labelText;
        inventoryExperienceSlider.maxValue = GameManager.instance.player.xp_to_next_level;
        inventoryExperienceSlider.value = GameManager.instance.player.experience;
        StartCoroutine(DrawMainMenuHeader());
        yield return 0;
        Debug.Log("MainMenuManager - DrawInventoryFooter - Fin");
    }

    IEnumerator DrawWinStreak()
    {
        Debug.Log("MainMenuManager - DrawWinStreak - Début");
        // Winstreak
        string labelText = "" + GameManager.instance.player.loot_series_nb.ToString() + " / " + GameManager.instance.player.loot_series.ToString();
        winStreakNumber.GetComponent<Text>().text = labelText;
        // Championship
        championshipSubLevel.GetComponent<Text>().text = GameManager.instance.player.sublevel_name;
        championshipMilestone.GetComponent<Text>().text = GameManager.instance.player.reached_milestone_order.ToString();
        // Dungeon
        dungeonAttributeValue.GetComponent<Text>().text = GameManager.instance.player.current_dungeon;

        dungeonLevelValue.GetComponent<Text>().text = GameManager.instance.player.current_dungeon switch
        {
            "earth" => GameManager.instance.player.dungeon_floor_earth.ToString(),
            "fire" => GameManager.instance.player.dungeon_floor_fire.ToString(),
            "wind" => GameManager.instance.player.dungeon_floor_wind.ToString(),
            "ice" => GameManager.instance.player.dungeon_floor_ice.ToString(),
            _ => "1",
        };
        // Campaign
        yield return 0;
        Debug.Log("MainMenuManager - DrawWinStreak - Fin");
    }

    IEnumerator DrawInventory()
    {
        Debug.Log("MainMenuManager - DrawInventory - Début");
        Debug.Log("total inventaire : " + GameManager.instance.player.unequippedItems.Count.ToString());
        Sprite icon = shieldIcon;
        Sprite statimage = attackIcon;
        bool isLock = false;

        inventoryNBSlots.GetComponent<TextMeshProUGUI>().text = GameManager.instance.player.unequippedItems.Count.ToString() + " / 30";
        // Mise à jour des compteurs pour les potions.
        health_potions_number.GetComponent<TextMeshProUGUI>().text = GameManager.instance.GetHealthPotionsNumber().ToString();
        attack_potions_number.GetComponent<TextMeshProUGUI>().text = GameManager.instance.GetAttackPotionsNumber().ToString();
        fireresist_potions_number.GetComponent<TextMeshProUGUI>().text = GameManager.instance.GetFireResistPotionsNumber().ToString();
        iceresist_potions_number.GetComponent<TextMeshProUGUI>().text = GameManager.instance.GetIceResistPotionsNumber().ToString();
        earthresist_potions_number.GetComponent<TextMeshProUGUI>().text = GameManager.instance.GetEarthResistPotionsNumber().ToString();
        windresist_potions_number.GetComponent<TextMeshProUGUI>().text = GameManager.instance.GetWindResistPotionsNumber().ToString();

        // Mise à jour de l'inventaire

        //yield return StartCoroutine(ClearInventoryGrid());
        // D'abord, nettoyer l'inventaire existant
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
        slots.Clear();
        // Ensuite, créer un Slot pour chaque item
        Debug.Log("total inventaire : " + GameManager.instance.player.unequippedItems.Count.ToString());
        foreach (Item item in GameManager.instance.player.unequippedItems)
        {
            GameObject newSlotObj = Instantiate(slotPrefab, gridParent);
            newSlotObj.transform.SetParent(gridParent, false); // << Important pour dire à Unity de respecter l'ancrage du parent

            SlotUI slotUI = newSlotObj.GetComponent<SlotUI>();

            Button button = newSlotObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(slotUI.OnSlotClicked);
            }

            if (slotUI == null)
            {
                Debug.LogError("SlotUI component is missing on the slot prefab!");
            }

            slotUI.Initialize(item, this); // this = MainMenuManager
            slotUI.SetRecyclingMode(isRecycling);

            switch (item.item_type_id)
            {
                case "2DA73382-6DD7-44F6-8B24-A6B9896B0694": //offhand
                    icon = shieldIcon;
                    statimage = attackIcon;
                    break;
                case "37A560D9-F3D1-4664-A8B0-7D363DDD4B9F": //shoulders
                    icon = pauldronsIcon;
                    statimage = attackIcon;
                    break;
                case "AC6AA9DC-B7EC-4214-A11D-A875C78D0293": //body
                    icon = chestIcon;
                    statimage = healthIcon;
                    break;
                case "1DA42203-BBD4-400A-9FBB-716DA54486BB": //foot
                    icon = bootsIcon;
                    statimage = attackIcon;
                    break;
                case "CC23CCB5-3159-437C-B1D3-A6481DA4BA19": //weapon
                    icon = weaponIcon;
                    statimage = attackIcon;
                    break;
                case "D2E1771B-9B6B-4DFB-8C54-48F6CCF55397": //head
                    icon = helmetIcon;
                    statimage = healthIcon;
                    break;
                case "8F9B6906-4227-4112-AF5D-04535148089D": //hands
                    icon = glovesIcon;
                    statimage = attackIcon;
                    break;
                case "CFE789F3-6A7E-42D5-BD55-4324D31A7D30": //thigh
                    icon = greavesIcon;
                    statimage = healthIcon;
                    break;
                default:
                    break;
            }
            isLock = false;
            yield return slotUI.SetSlot(icon, statimage, item.item_level, item.stat_value.ToString(), item.item_xp_value.ToString(), isLock, item.rarity_id, item.inventory_item_id);
            Debug.Log("Ajout d'un slot");
            slots.Add(slotUI);
        }

        yield return 0;
        Debug.Log("MainMenuManager - DrawInventory - Fin");
    }


    IEnumerator DrawEquipments()
    {
        Debug.Log("MainMenuManager - DrawEquipments - Début");
        // On met à jour l'affichage des stats globales
        attack.GetComponent<Text>().text = GameManager.instance.player.attack.ToString();
        health.GetComponent<Text>().text = GameManager.instance.player.health.ToString();
        fireRes.GetComponent<Text>().text = GameManager.instance.player.res_fire.ToString();
        iceRes.GetComponent<Text>().text = GameManager.instance.player.res_ice.ToString();
        earthRes.GetComponent<Text>().text = GameManager.instance.player.res_earth.ToString();
        windRes.GetComponent<Text>().text = GameManager.instance.player.res_wind.ToString();
        blockChance.GetComponent<Text>().text = GameManager.instance.player.block_chance.ToString();
        dodgeChance.GetComponent<Text>().text = GameManager.instance.player.dodge_chance.ToString();
        critChance.GetComponent<Text>().text = GameManager.instance.player.critical_chance.ToString();
        critDamage.GetComponent<Text>().text = GameManager.instance.player.critical_damage.ToString();



        foreach (var item in GameManager.instance.player.equippedItems)
        {
            Debug.Log(item.item_name);
            // Mettre à jour le visuel de l'équipement
            switch (item.item_type_id)
            {
                case "2DA73382-6DD7-44F6-8B24-A6B9896B0694": //offhand
                    //shieldVisual.GetComponent<Image>().sprite = //sprite de l'objet équipé
                    shieldVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    shieldSlot.GetComponent<Button>().onClick.AddListener(() => OnEquipmentsItemClicked(item)); // Gère le clic
                    break;
                case "37A560D9-F3D1-4664-A8B0-7D363DDD4B9F": //shoulders
                    pauldronsVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    pauldronsSlot.GetComponent<Button>().onClick.AddListener(() => OnEquipmentsItemClicked(item)); // Gère le clic
                    break;
                case "AC6AA9DC-B7EC-4214-A11D-A875C78D0293": //body
                    chestVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    chestSlot.GetComponent<Button>().onClick.AddListener(() => OnEquipmentsItemClicked(item)); // Gère le clic
                    break;
                case "1DA42203-BBD4-400A-9FBB-716DA54486BB": //foot
                    bootsVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    bootsSlot.GetComponent<Button>().onClick.AddListener(() => OnEquipmentsItemClicked(item)); // Gère le clic
                    break;
                case "CC23CCB5-3159-437C-B1D3-A6481DA4BA19": //weapon
                    weaponVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    weaponSlot.GetComponent<Button>().onClick.AddListener(() => OnEquipmentsItemClicked(item)); // Gère le clic
                    break;
                case "D2E1771B-9B6B-4DFB-8C54-48F6CCF55397": //head
                    helmetVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    helmetSlot.GetComponent<Button>().onClick.AddListener(() => OnEquipmentsItemClicked(item)); // Gère le clic
                    break;
                case "8F9B6906-4227-4112-AF5D-04535148089D": //hands
                    glovesVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    glovesSlot.GetComponent<Button>().onClick.AddListener(() => OnEquipmentsItemClicked(item)); // Gère le clic
                    break;
                case "CFE789F3-6A7E-42D5-BD55-4324D31A7D30": //thigh
                    greavesVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    greavesSlot.GetComponent<Button>().onClick.AddListener(() => OnEquipmentsItemClicked(item)); // Gère le clic
                    break;
                default:
                    break;
            }

            //GameObject itemButton = Instantiate(itemButtonPrefab, inventoryPanel.transform);
            //itemButton.GetComponentInChildren<Text>().text = item.name; // Affiche le nom de l'item
            //itemButton.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(item)); // Gère le clic
        }
        yield return 0;
        Debug.Log("MainMenuManager - DrawEquipments - Fin");
    }


    void OnEquipmentsItemClicked(Item item)
    {
        Debug.Log("MainMenuManager - OnEquipmentsItemClicked - Début");
        Debug.Log("Item cliqué: " + item.inventory_item_id);
        ShowItemShow(item);
        Debug.Log("MainMenuManager - OnEquipmentsItemClicked - Fin");
    }
    public void OnPlayerResumeItemClicked(Item item)
    {
        Debug.Log("MainMenuManager - OnPlayerResumeItemClicked - Début");
        Debug.Log("Item cliqué: " + item.inventory_item_id);
        ShowItemShow(item);
        Debug.Log("MainMenuManager - OnPlayerResumeItemClicked - Fin");
    }

    public void OnInventoryItemClicked(Item item)
    {
        Debug.Log("MainMenuManager - OnInventoryItemClicked - Début");
        Debug.Log("Item cliqué: " + item.item_name);
        // Ouvrir le panneau de comparaison ici
        if (item == null) return;

        if (isRecycling)
        {
            // Suppression de l'objet
            StartCoroutine(RemoveItemAndRefreshUI(item));
        }
        else
        {
            // Comportement normal : afficher / équiper, etc.
            //SelectItem(item);
            Debug.Log("Item cliqué: " + item.inventory_item_id);
            selected_item = item;
            equipped_item = GameManager.instance.player.equippedItems.FirstOrDefault(_item => _item.item_type_id == item.item_type_id);
            ShowItemCompare(equipped_item, item);
        }
        Debug.Log("MainMenuManager - OnInventoryItemClicked - Fin");
    }

    public void OnClanSlotClicked(Clan clan)
    {
        Debug.Log("MainMenuManager - OnClanSlotClicked - Début");
        Debug.Log("Clan cliqué: " + clan.clan_name);
        // Ouvrir le panneau de comparaison ici
        if (clan == null) return;

        // Comportement normal : afficher / équiper, etc.
        selected_clan = clan;
        StartCoroutine(ShowClanInformations(clan));

        Debug.Log("MainMenuManager - OnClanSlotClicked - Fin");
    }

    public IEnumerator OnPlayerSlotClicked(string player_id)
    {
        Debug.Log("MainMenuManager - OnPlayerSlotClicked - Début");
        Debug.Log("player_id cliqué: " + player_id);        
        if (player_id == "") yield return null;

        yield return StartCoroutine(GameManager.instance.GetPlayerInformations(player_id));

        yield return StartCoroutine(ShowPlayerInformations(GameManager.instance.selectedPlayer));

        Debug.Log("MainMenuManager - OnPlayerSlotClicked - Fin");

    }

    public void SwapItem()
    {
        // Charger la scène du combat
        Debug.Log("SwapItem: ");
        if (selected_item != null)
        {
            StartCoroutine(SwapItemAndRefreshUI(selected_item));
        }
        itemComparePanel.SetActive(false);
    }

    public IEnumerator SwapItemAndRefreshUI(Item item)
    {
        yield return StartCoroutine(GameManager.instance.SwapItem(item));
        yield return StartCoroutine(DrawInventory());
    }
    public void ExitItemCompare()
    {
        Debug.Log("ExitItemCompare: ");
        itemComparePanel.SetActive(false);
    }

    public void ShowItemCompare(Item equippedItem, Item selectedItem)
    {
        //mainMenuPanel.SetActive(false);
        Sprite statimage = healthIcon;
        switch (equippedItem.item_type_id)
        {
            case "2DA73382-6DD7-44F6-8B24-A6B9896B0694": //offhand
                statimage = attackIcon;
                break;
            case "37A560D9-F3D1-4664-A8B0-7D363DDD4B9F": //shoulders
                statimage = attackIcon;
                break;
            case "AC6AA9DC-B7EC-4214-A11D-A875C78D0293": //body
                statimage = healthIcon;
                break;
            case "1DA42203-BBD4-400A-9FBB-716DA54486BB": //foot
                statimage = attackIcon;
                break;
            case "CC23CCB5-3159-437C-B1D3-A6481DA4BA19": //weapon
                statimage = attackIcon;
                break;
            case "D2E1771B-9B6B-4DFB-8C54-48F6CCF55397": //head
                statimage = healthIcon;
                break;
            case "8F9B6906-4227-4112-AF5D-04535148089D": //hands
                statimage = attackIcon;
                break;
            case "CFE789F3-6A7E-42D5-BD55-4324D31A7D30": //thigh
                statimage = healthIcon;
                break;
            default:
                break;
        }

        equippedLvlText.GetComponent<TextMeshProUGUI>().text = "LEVEL : " + equippedItem.item_level.ToString();
        equippedStatIcon.GetComponent<Image>().sprite = statimage;
        equippedStatText.GetComponent<TextMeshProUGUI>().text = equippedItem.stat_value.ToString();

        selectedLvlText.GetComponent<TextMeshProUGUI>().text = "LEVEL : " + selectedItem.item_level.ToString();
        selectedStatIcon.GetComponent<Image>().sprite = statimage;
        selectedStatText.GetComponent<TextMeshProUGUI>().text = selectedItem.stat_value.ToString();

        itemComparePanel.SetActive(true);

    }

    public void ExitItemShow()
    {
        Debug.Log("ExitItemShow: ");
        itemShowPanel.SetActive(false);
    }

    public void ShowItemShow(Item equippedItem)
    {
        //mainMenuPanel.SetActive(false);
        Sprite statimage = healthIcon;
        switch (equippedItem.item_type_id)
        {
            case "2DA73382-6DD7-44F6-8B24-A6B9896B0694": //offhand
                statimage = attackIcon;
                break;
            case "37A560D9-F3D1-4664-A8B0-7D363DDD4B9F": //shoulders
                statimage = attackIcon;
                break;
            case "AC6AA9DC-B7EC-4214-A11D-A875C78D0293": //body
                statimage = healthIcon;
                break;
            case "1DA42203-BBD4-400A-9FBB-716DA54486BB": //foot
                statimage = attackIcon;
                break;
            case "CC23CCB5-3159-437C-B1D3-A6481DA4BA19": //weapon
                statimage = attackIcon;
                break;
            case "D2E1771B-9B6B-4DFB-8C54-48F6CCF55397": //head
                statimage = healthIcon;
                break;
            case "8F9B6906-4227-4112-AF5D-04535148089D": //hands
                statimage = attackIcon;
                break;
            case "CFE789F3-6A7E-42D5-BD55-4324D31A7D30": //thigh
                statimage = healthIcon;
                break;
            default:
                break;
        }

        showEquippedLvlText.GetComponent<TextMeshProUGUI>().text = "LEVEL : " + equippedItem.item_level.ToString();
        showEquippedStatIcon.GetComponent<Image>().sprite = statimage;
        showEquippedStatText.GetComponent<TextMeshProUGUI>().text = equippedItem.stat_value.ToString();

        itemShowPanel.SetActive(true);

    }

    public void ShowShopPanel()
    {
        shopPanel.SetActive(true);
    }

    public void ExitShowShopPanel()
    {
        Debug.Log("ExitShowShopPanel: ");
        shopPanel.SetActive(false);
    }
    public void ShowOptionsPanel()
    {
        optionsPanel.SetActive(true);
    }

    public void ExitOptionsPanel()
    {
        Debug.Log("ExitOptionsPanel: ");
        optionsPanel.SetActive(false);
    }

    public void JoinClan()
    {
        // Charger la scène du combat
        Debug.Log("JoinClan: ");
        if (selected_clan != null)
        {
            StartCoroutine(JoinClanAndRefreshUI(selected_clan));
        }
        clanResumePanel.SetActive(false);
    }
    public IEnumerator JoinClanAndRefreshUI(Clan clan)
    {
        yield return StartCoroutine(GameManager.instance.JoinClan(clan));
        yield return StartCoroutine(SetSocialPanels());
    }
    public void ShowPlayerClanInformations()
    {
        StartCoroutine(ShowClanInformations(GameManager.instance.clan));
    }

    public IEnumerator ShowClanInformations(Clan clan)
    {
        quitButton.SetActive(false);
        joinButton.SetActive(false);
        yield return StartCoroutine(GetClanMembers(clan.clan_id));
        //clanIcon.GetComponent<TextMeshProUGUI>().text = "LEVEL : " + equippedItem.item_level.ToString();
        clanNameText.GetComponent<TextMeshProUGUI>().text = clan.clan_name;
        clanDescriptionText.GetComponent<TextMeshProUGUI>().text = clan.clan_description;
        clanLvlText.GetComponent<TextMeshProUGUI>().text = clan.clan_lvl.ToString();
        //clanRegionIcon.GetComponent<TextMeshProUGUI>().text = "LEVEL : " + equippedItem.item_level.ToString();
        clanMembersText.GetComponent<TextMeshProUGUI>().text = "MEMBERS: " + clan.clan_total_members.ToString() + "/"  + clan.clan_max_members.ToString();
        clanRequiredLvlText.GetComponent<TextMeshProUGUI>().text = clan.clan_required_lvl.ToString();

        if (clan.clan_is_recruiting & GameManager.instance.player.clan_id is null & clan.clan_required_lvl <= GameManager.instance.player.level)
        {
            joinButton.SetActive(true);
        }
        if (clan.clan_id == GameManager.instance.player.clan_id)
        {            
            quitButton.SetActive(true);
        }

        clanResumePanel.SetActive(true);
        yield return null;

    }
    public void ExitClanInformations()
    {
        Debug.Log("ExitClanInformations: ");
        clanResumePanel.SetActive(false);
    }
    IEnumerator ShowPlayerInformations(Player player)
    {
        Debug.Log("MainMenuManager - ShowPlayerInformations - Début");
        Debug.Log("MainMenuManager - ShowPlayerInformations - Début - Player : " + player.player_id);
        playerResumePromoteButton.SetActive(false);
        playerResumeDemoteButton.SetActive(false);
        playerResumeKickButton.SetActive(false);
        playerResumeAttack.GetComponent<Text>().text = player.attack.ToString();
        playerResumeHealth.GetComponent<Text>().text = player.health.ToString();
        playerResumefireRes.GetComponent<Text>().text = player.res_fire.ToString();
        playerResumeiceRes.GetComponent<Text>().text = player.res_ice.ToString();
        playerResumeearthRes.GetComponent<Text>().text = player.res_earth.ToString();
        playerResumewindRes.GetComponent<Text>().text = player.res_wind.ToString();
        playerResumeblockChance.GetComponent<Text>().text = player.block_chance.ToString();
        playerResumedodgeChance.GetComponent<Text>().text = player.dodge_chance.ToString();
        playerResumecritChance.GetComponent<Text>().text = player.critical_chance.ToString();
        playerResumecritDamage.GetComponent<Text>().text = player.critical_damage.ToString();


        foreach (var item in player.equippedItems)
        {
            Debug.Log(item.item_name);
            // Mettre à jour le visuel de l'équipement
            switch (item.item_type_id)
            {
                case "2DA73382-6DD7-44F6-8B24-A6B9896B0694": //offhand
                    //shieldVisual.GetComponent<Image>().sprite = //sprite de l'objet équipé
                    playerResumeShieldVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    playerResumeShieldSlot.GetComponent<Button>().onClick.AddListener(() => OnPlayerResumeItemClicked(item)); // Gère le clic
                    break;
                case "37A560D9-F3D1-4664-A8B0-7D363DDD4B9F": //shoulders
                    playerResumePauldronsVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    playerResumePauldronsSlot.GetComponent<Button>().onClick.AddListener(() => OnPlayerResumeItemClicked(item)); // Gère le clic
                    break;
                case "AC6AA9DC-B7EC-4214-A11D-A875C78D0293": //body
                    playerResumeChestVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    playerResumeChestSlot.GetComponent<Button>().onClick.AddListener(() => OnPlayerResumeItemClicked(item)); // Gère le clic
                    break;
                case "1DA42203-BBD4-400A-9FBB-716DA54486BB": //foot
                    playerResumeBootsVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    playerResumeBootsSlot.GetComponent<Button>().onClick.AddListener(() => OnPlayerResumeItemClicked(item)); // Gère le clic
                    break;
                case "CC23CCB5-3159-437C-B1D3-A6481DA4BA19": //weapon
                    playerResumeWeaponVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    playerResumeWeaponSlot.GetComponent<Button>().onClick.AddListener(() => OnPlayerResumeItemClicked(item)); // Gère le clic
                    break;
                case "D2E1771B-9B6B-4DFB-8C54-48F6CCF55397": //head
                    playerResumeHelmetVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    playerResumeHelmetSlot.GetComponent<Button>().onClick.AddListener(() => OnPlayerResumeItemClicked(item)); // Gère le clic
                    break;
                case "8F9B6906-4227-4112-AF5D-04535148089D": //hands
                    playerResumeGlovesVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    playerResumeGlovesSlot.GetComponent<Button>().onClick.AddListener(() => OnPlayerResumeItemClicked(item)); // Gère le clic
                    break;
                case "CFE789F3-6A7E-42D5-BD55-4324D31A7D30": //thigh
                    playerResumeGreavesVisual.GetComponent<Text>().text = item.stat_value.ToString();
                    playerResumeGreavesSlot.GetComponent<Button>().onClick.AddListener(() => OnPlayerResumeItemClicked(item)); // Gère le clic
                    break;
                default:
                    break;
            }
        }
        



        if (player.clan_id == GameManager.instance.player.clan_id 
            && player.player_id != GameManager.instance.player.player_id)
        {
            if ((GameManager.instance.player.can_demote != null) && ((bool)GameManager.instance.player.can_demote) && (GameManager.instance.player.rank <= player.rank))
            {
                playerResumeDemoteButton.SetActive(true);
            }
            if ((GameManager.instance.player.can_promote != null) && ((bool)GameManager.instance.player.can_promote) && (GameManager.instance.player.rank < player.rank))
            {
                playerResumePromoteButton.SetActive(true);
            }
            if ((GameManager.instance.player.can_kick != null) && ((bool)GameManager.instance.player.can_kick) && (GameManager.instance.player.rank <= player.rank))
            {
                playerResumeKickButton.SetActive(true);
            }
        }

        playerResumePanel.SetActive(true);
        Debug.Log("MainMenuManager - ShowPlayerInformations - Fin");
        yield return null;

    }
    public void ExitPlayerInformations()
    {
        Debug.Log("ExitPlayerInformations: ");
        playerResumePanel.SetActive(false);
    }
    public void PromoteClanMember()
    {
        Debug.Log("MainMenuManager - PromoteClanMember - Début");
        StartCoroutine(PromoteClanMemberAndRefreshUI());
        Debug.Log("MainMenuManager - PromoteClanMember - Fin");
    }
    IEnumerator PromoteClanMemberAndRefreshUI()
    {
        yield return StartCoroutine(GameManager.instance.PromoteClanMember());
        ExitPlayerInformations();
        yield return StartCoroutine(ShowClanInformations(GameManager.instance.clan));
    }
    public void DemoteClanMember()
    {
        Debug.Log("MainMenuManager - DemoteClanMember - Début");
        StartCoroutine(DemoteClanMemberAndRefreshUI());
        Debug.Log("MainMenuManager - DemoteClanMember - Fin");
    }
    IEnumerator DemoteClanMemberAndRefreshUI()
    {
        yield return StartCoroutine(GameManager.instance.DemoteClanMember());
        ExitPlayerInformations();
        yield return StartCoroutine(ShowClanInformations(GameManager.instance.clan));
    }
    public void KickClanMember()
    {
        Debug.Log("MainMenuManager - KickClanMember - Début");
        StartCoroutine(KickClanMemberAndRefreshUI());
        Debug.Log("MainMenuManager - KickClanMember - Fin");
    }
    IEnumerator KickClanMemberAndRefreshUI()
    {
        yield return StartCoroutine(GameManager.instance.KickClanMember());
        ExitPlayerInformations();
        yield return StartCoroutine(ShowClanInformations(GameManager.instance.clan));
    }
    public IEnumerator RemoveItemAndRefreshUI(Item item)
    {
        yield return StartCoroutine(GameManager.instance.RemoveItem(item));
        yield return StartCoroutine(DrawInventoryHeader());
        SlotUI slotToRemove = slots.FirstOrDefault(s => s.itemData.inventory_item_id == item.inventory_item_id);
        if (slotToRemove != null)
        {
            slots.Remove(slotToRemove); // Retire de la liste locale
            slotToRemove.DestroySlot(); // Détruit l'objet visuellement
        }
        //yield return StartCoroutine(DrawInventory());
        yield return StartCoroutine(DrawInventoryFooter());
    }

    public IEnumerator RemoveItemIDAndRefreshUI(string item)
    {
        yield return StartCoroutine(GameManager.instance.RemoveItemID(item));
        yield return StartCoroutine(DrawInventoryHeader());
        yield return StartCoroutine(DrawInventory());
        yield return StartCoroutine(DrawInventoryFooter());
    }

    public void ToggleRecycleMode()
    {
        recyclingActive = !recyclingActive;
        isRecycling = recyclingActive;

        // Optionnel : changer la couleur du bouton ou le texte
        if (recyclingActive) 
        { 
            recycleButtonText.GetComponent<TextMeshProUGUI>().text = "EQUIPMENT\r\nMODE";
        } 
        else
        { recycleButtonText.GetComponent<TextMeshProUGUI>().text = "DISASSEMBLE\r\nMODE"; }
        Debug.Log(slots.Count.ToString());
        foreach (SlotUI slot in slots)
        {
            slot.SetRecyclingMode(isRecycling);
        }

    }

    public void ChangePlayerName()
    {
        Debug.Log("MainMenuManager - ChangePlayerName - Début");
        string text = optionsPlayerNameText.GetComponent<TextMeshProUGUI>().text.Trim().Replace("\u00A0", "").Replace("\u200B", "").Replace("\u200C", "").Replace("\u200D", "");
        StartCoroutine(ChangePlayerNameAndRefreshUI(text));
        Debug.Log("MainMenuManager - ChangePlayerName - Fin");
    }
    IEnumerator ChangePlayerNameAndRefreshUI(string text)
    {
        yield return StartCoroutine(GameManager.instance.ChangePlayerName(text));
        yield return StartCoroutine(DrawMainMenuHeader());
    }

    public void SearchClans()
    {
        Debug.Log("MainMenuManager - SearchClans - Début");
        string text = search_clans_text.GetComponent<TextMeshProUGUI>().text.Trim().Replace("\u00A0", "").Replace("\u200B", "").Replace("\u200C", "").Replace("\u200D", "");
        StartCoroutine(SearchAndRefreshUI(text));
        Debug.Log("MainMenuManager - SearchClans - Fin");
    }
    IEnumerator SearchAndRefreshUI(string searchText)
    {
        yield return StartCoroutine(GameManager.instance.SearchClans(searchText, OnClansFound));
        //yield return StartCoroutine(DrawClansList());
    }
    public void SendClanMessage()
    {
        Debug.Log("MainMenuManager - SendClanMessage - Début");
        string text = clanLogtext.GetComponent<TextMeshProUGUI>().text.Trim().Replace("\u00A0", "").Replace("\u200B", "").Replace("\u200C", "").Replace("\u200D", "");
        StartCoroutine(SendMessageAndRefreshUI(text));
        Debug.Log("MainMenuManager - SendClanMessage - Fin");
    }
    IEnumerator SendMessageAndRefreshUI(string message)
    {
        yield return StartCoroutine(GameManager.instance.SendClanLog(message));
        yield return StartCoroutine(GameManager.instance.UpdateClanLogs());
        yield return StartCoroutine(DrawClanLogs());
    }
    IEnumerator GetClanMembers(string clan_id)
    {
        yield return StartCoroutine(GameManager.instance.GetClanMembers(clan_id, OnGetClanMembers));
        //yield return StartCoroutine(DrawClansList());
    }
    IEnumerator DrawClansList()
    {
        //foreach (Transform child in clansListContent)
        //{
        //    Destroy(child.gameObject);
        //}
        //slots.Clear();

        //foreach (var clan in results)
        //{
        //    Debug.Log("Nom du clan : " + clan.clan_name);
        //    GameObject newSlotObj = Instantiate(clanPrefab, clansListContent);
        //    newSlotObj.transform.SetParent(clansListContent, false); // << Important pour dire à Unity de respecter l'ancrage du parent

        //    ClanUI clanUI = newSlotObj.GetComponent<ClanUI>();

        //    Button button = newSlotObj.GetComponent<Button>();
        //    if (button != null)
        //    {
        //        //button.onClick.AddListener(ClanUI.OnSlotClicked);
        //    }

        //    if (clanUI == null)
        //    {
        //        Debug.LogError("SlotUI component is missing on the slot prefab!");
        //    }

        //    clanUI.Initialize(clan, this); // this = MainMenuManager
        //    yield return clanUI.SetSlot();
        //    Debug.Log("Ajout d'un slot");
        //    clans.Add(clanUI);

        //}
        yield return 0;
    }
    void OnGetClanMembers(List<Member> results)
    {
        foreach (Transform child in clanMembersListContent)
        {
            Destroy(child.gameObject);
        }
        slots.Clear();

        foreach (var member in results)
        {
            Debug.Log("Nom du player : " + member.name);
            GameObject newSlotObj = Instantiate(clanMemberSlotPrefab, clanMembersListContent);
            newSlotObj.SetActive(true);
            newSlotObj.transform.SetParent(clanMembersListContent, false); // << Important pour dire à Unity de respecter l'ancrage du parent

            MemberUI memberUI = newSlotObj.GetComponent<MemberUI>();

            Button button = newSlotObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(memberUI.OnSlotClicked);
            }

            if (memberUI == null)
            {
                Debug.LogError("memberUI component is missing on the slot prefab!");
            }

            memberUI.Initialize(member, this); // this = MainMenuManager
            StartCoroutine(memberUI.SetSlot());
            Debug.Log("Ajout d'un slot");
            clanMembers.Add(memberUI);

        }
    }
    void OnGetChampionshipPlayers(List<Player> results)
    {
        foreach (Transform child in champMembersListContent)
        {
            Destroy(child.gameObject);
        }
        slots.Clear();

        foreach (var player in results)
        {
            Debug.Log("Nom du player : " + player.name);
            GameObject newSlotObj = Instantiate(champMemberSlotPrefab, champMembersListContent);
            newSlotObj.SetActive(true);
            newSlotObj.transform.SetParent(champMembersListContent, false); // << Important pour dire à Unity de respecter l'ancrage du parent

            ChampUI champUI = newSlotObj.GetComponent<ChampUI>();

            Button button = newSlotObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(champUI.OnSlotClicked);
            }

            if (champUI == null)
            {
                Debug.LogError("champUI component is missing on the slot prefab!");
            }

            champUI.Initialize(player, this); // this = MainMenuManager
            StartCoroutine(champUI.SetSlot());
            Debug.Log("Ajout d'un slot");
            champMembers.Add(champUI);

        }
    }
    void OnClansFound(List<Clan> results)
    {
        foreach (Transform child in clansListContent)
        {
            Destroy(child.gameObject);
        }
        slots.Clear();

        foreach (var clan in results)
        {
            Debug.Log("Nom du clan : " + clan.clan_name);
            GameObject newSlotObj = Instantiate(clanPrefab, clansListContent);
            newSlotObj.SetActive(true);
            newSlotObj.transform.SetParent(clansListContent, false); // << Important pour dire à Unity de respecter l'ancrage du parent

            ClanUI clanUI = newSlotObj.GetComponent<ClanUI>();

            Button button = newSlotObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(clanUI.OnSlotClicked);
            }

            if (clanUI == null)
            {
                Debug.LogError("SlotUI component is missing on the slot prefab!");
            }

            clanUI.Initialize(clan, this); // this = MainMenuManager
            StartCoroutine(clanUI.SetSlot());
            Debug.Log("Ajout d'un slot");
            clans.Add(clanUI);

        }
    }
    public void IncreaseLevel()
    {
        currentLevel = Mathf.Min(currentLevel + 1, maxLevel);
        UpdateInputField();
    }
    public void DecreaseLevel()
    {
        currentLevel = Mathf.Max(currentLevel - 1, minLevel);
        UpdateInputField();
    }
    public void UpdateInputField()
    {
        Debug.Log("currentLevel.ToString() : " + currentLevel.ToString());
        if (create_clan_required_level != null)
        {
            create_clan_required_level.text = currentLevel.ToString();
        }
        else
        {
            Debug.LogError("TMP_InputField 'create_clan_required_level' n'est pas assigné !");
        }
    }
    void OnInputFieldChanged(string input)
    {
        if (int.TryParse(input, out int value))
        {
            currentLevel = Mathf.Clamp(value, minLevel, maxLevel);
        }
        UpdateInputField();
    }
    public int GetRequiredLevel()
    {
        return currentLevel;
    }

    public void CreateClan()
    {
        Debug.Log("MainMenuManager - CreateClan - Début");
        string clan_name = create_clan_text.GetComponent<TextMeshProUGUI>().text.Trim().Replace("\u00A0", "").Replace("\u200B", "").Replace("\u200C", "").Replace("\u200D", "");
        string clan_description = create_clan_description_text.GetComponent<TextMeshProUGUI>().text.Trim().Replace("\u00A0", "").Replace("\u200B", "").Replace("\u200C", "").Replace("\u200D", "");
        //int clan_required_level = 0;
        int clan_required_level = GetRequiredLevel();
        StartCoroutine(CreateAndRefreshUI(clan_name, clan_description, clan_required_level));
        Debug.Log("MainMenuManager - CreateClan - Fin");
    }
    IEnumerator CreateAndRefreshUI(string clanName, string clanDescription, int clanRequiredLevel)
    {
        yield return StartCoroutine(GameManager.instance.CreateClan(clanName, clanDescription, clanRequiredLevel, OnClanCreate));
        yield return StartCoroutine(SetSocialPanels());
        //yield return SetPanel(4);
        //yield return StartCoroutine(DrawClansList());
    }
    void OnClanCreate(Clan clan)
    {
        Debug.Log("ID du clan : " + clan.clan_id);
    }
    public void LeaveClan()
    {
        Debug.Log("MainMenuManager - LeaveClan - Début");
        StartCoroutine(LeaveAndRefreshUI());
        Debug.Log("MainMenuManager - LeaveClan - Fin");
    }
    IEnumerator LeaveAndRefreshUI()
    {
        yield return StartCoroutine(GameManager.instance.LeaveClan());
        yield return StartCoroutine(SetSocialPanels());
        yield return SetPanel(2);
        ExitClanInformations();
    }

    public void BuyChest(int chestType)
    {
        Debug.Log("MainMenuManager - BuyChest - Début");
        StartCoroutine(BuyChestAndRefreshUI(chestType));
        Debug.Log("MainMenuManager - BuyChest - Fin");
    }
    public void AddFreemiumCurrency()
    {
        Debug.Log("MainMenuManager - AddFreemiumCurrency - Début");
        StartCoroutine(AddCurrencyAndRefreshUI(1));
        Debug.Log("MainMenuManager - AddFreemiumCurrency - Fin");
    }
    public void AddPremiumCurrency()
    {
        Debug.Log("MainMenuManager - AddPremiumCurrency - Début");
        StartCoroutine(AddCurrencyAndRefreshUI(2));
        Debug.Log("MainMenuManager - AddPremiumCurrency - Fin");
    }
    IEnumerator BuyChestAndRefreshUI(int chestType)
    {
        yield return StartCoroutine(GameManager.instance.BuyChest(chestType));
        yield return StartCoroutine(DrawInventory());
        yield return StartCoroutine(DrawMainMenuHeader());
    }
    IEnumerator AddCurrencyAndRefreshUI(int currencyType)
    {
        yield return StartCoroutine(GameManager.instance.AddCurrency(currencyType));
        yield return StartCoroutine(DrawMainMenuHeader());
    }
    public void ShowChampionshipPanel()
    {
        StartCoroutine(ShowChampionshipPanel(GameManager.instance.player.sublevel_id));
    }

    public IEnumerator ShowChampionshipPanel(string sublevel_id)
    {
        quitButton.SetActive(false);
        joinButton.SetActive(false);
        yield return StartCoroutine(GameManager.instance.GetChampionshipMembers(sublevel_id, OnGetChampionshipPlayers));
        clanNameText.GetComponent<TextMeshProUGUI>().text = GameManager.instance.player.sublevel_name;


        championshipPanel.SetActive(true);
        yield return null;

    }
    public void ExitChampionshipPanel()
    {
        Debug.Log("ExitChampionshipPanel: ");
        championshipPanel.SetActive(false);
    }
    public void ShowDungeonPanel()
    {
        dungeonPanel.SetActive(true);
    }
    public void ExitDungeonPanel()
    {
        dungeonPanel.SetActive(false);
    }
    public void ShowCampaignPanel()
    {
        campaignPanel.SetActive(true);
    }
    public void ExitCampaignPanel()
    {
        campaignPanel.SetActive(false);
    }
    public void ShowMailboxPanel()
    {
        mailboxPanel.SetActive(true);
    }
    public void ExitMailboxPanel()
    {
        mailboxPanel.SetActive(false);
    }
    public void ShowCombatPrep()
    {
        StartCoroutine(GameManager.instance.GetOpponent((opponent) =>
        {
            StartCoroutine(ShowCombatPrep(opponent));
        }));
    }

    public void ExitCombatPrep()
    {
        combatPrepPanel.SetActive(false);
    }

    IEnumerator ShowCombatPrep(Player opponent)
    {
        if (opponent != null)
        {
            Debug.Log("Adversaire récupéré : " + opponent.name);
            combatPrepPanel.SetActive(true);
            // Informations du joueur :
            combatPrepPlayerLvlText.GetComponent<TextMeshProUGUI>().text = GameManager.instance.player.level.ToString();
            combatPrepPlayerAttackText.GetComponent<TextMeshProUGUI>().text = GameManager.instance.player.attack.ToString();
            combatPrepPlayerDefenseText.GetComponent<TextMeshProUGUI>().text = GameManager.instance.player.health.ToString();
            combatPrepPlayerNameText.GetComponent<TextMeshProUGUI>().text = GameManager.instance.player.name;
            player1_id = GameManager.instance.player.player_id;
            player1_Max_Health = GameManager.instance.player.health;
            player1_name = GameManager.instance.player.name;
            // Informations de l'adversaire
            combatPrepOpponentLvlText.GetComponent<TextMeshProUGUI>().text = opponent.level.ToString();
            combatPrepOpponentAttackText.GetComponent<TextMeshProUGUI>().text = opponent.attack.ToString();
            combatPrepOpponentDefenseText.GetComponent<TextMeshProUGUI>().text = opponent.health.ToString();
            combatPrepOpponentNameText.GetComponent<TextMeshProUGUI>().text = opponent.name.ToString();
            player2_id = opponent.player_id;
            player2_Max_Health = opponent.health;
            player2_name = opponent.name;
        }
        else
        {
            Debug.LogWarning("Aucun adversaire trouvé.");
        }
        yield return null;
    }

    public void StartCombat()
    {
        CombatData.player1Id = player1_id;
        CombatData.player2Id = player2_id;
        CombatData.player1MaxHealth = player1_Max_Health;
        CombatData.player2MaxHealth = player2_Max_Health;
        CombatData.player1Name = player1_name;
        CombatData.player2Name = player2_name;
        // Charger la scène du combat
        SceneManager.LoadScene("CombatScene");
    }

}
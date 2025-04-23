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


    void Start()
    {
        StartCoroutine(DrawInventory());
        StartCoroutine(DrawEquipments());
        StartCoroutine(DrawCurrency());
    }

    IEnumerator DrawCurrency()
    {
        CurrencyFreemiumText.GetComponent<Text>().text = GameManager.instance.GetPlayerCurrencyFreemium().ToString();
        Debug.Log(GameManager.instance.GetPlayerCurrencyFreemium().ToString());
        yield return 0;

    }



    IEnumerator DrawInventory()
    {
        int i = 0;
        foreach (var item in GameManager.instance.player.unequippedItems)
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

    IEnumerator DrawEquipments()
    {
        // On met à jour l'affichage des stats globales
        attack.GetComponent<Text>().text = GameManager.instance.GetPlayerAttack().ToString();
        health.GetComponent<Text>().text = GameManager.instance.GetPlayerHealth().ToString();



        foreach (var item in GameManager.instance.player.equippedItems)
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
        yield return 0;
    }

    void OnItemClicked(Item item)
    {
        Debug.Log("Item cliqué: " + item.item_name);
        // Ouvrir le panneau de comparaison ici
    }


}
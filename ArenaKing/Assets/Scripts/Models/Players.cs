using UnityEngine;
using System.Collections.Generic;

[System.Serializable]

public class Player
{
    public string player_id;
    public string inventory_id;
    public int status;
    public string user_id; //?
    public string level;
    public string experience;
    public int freemium_currency;
    public int premium_currency;
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
    public List<Item> unequippedItems = new List<Item>();
    public List<Item> equippedItems = new List<Item>();
}

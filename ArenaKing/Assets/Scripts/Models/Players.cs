using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]

public class Player
{   // Player
    public string player_id;
    public string inventory_id;
    public int status;
    public string user_id; //?
    public int level;
    public int experience;
    public int xp_to_next_level;
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
    public int health_pot_nb;
    public int attack_pot_nb;
    public int fire_res_pot_nb;
    public int ice_res_pot_nb;
    public int earth_res_pot_nb;
    public int wind_res_pot_nb;
    public string clan_id;
    public string name;
    public string id_next_opponent;
    public int res_fire;
    public int res_ice;
    public int res_earth;
    public int res_wind;
    public float block_chance;
    public float dodge_chance;
    public float critical_chance;
    public float critical_damage;
    public List<Item> unequippedItems = new List<Item>();
    public List<Item> equippedItems = new List<Item>();
    // Clan
    public DateTime? join_date;
    public bool? contribution_points;
    public bool? can_kick;
    public bool? can_promote;
    public bool? can_demote;
    public bool? can_edit_clan;
    public bool? can_write_message;
    public bool? can_start_war;
    public bool? can_change_leader;
    public string role_id;
    public int? rank;
    public string role_name;
    // Championship
    public string sublevel_name;
    public string sublevel_id;
    public int? current_points;
    public int? reached_milestone_order;
    public int? dungeon_floor_fire;
    public int? dungeon_floor_ice;
    public int? dungeon_floor_wind;
    public int? dungeon_floor_earth;
    public string current_dungeon;
}

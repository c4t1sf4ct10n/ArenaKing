using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]

public class Clan
{
    public string clan_id;
    public DateTime clan_creation_date;
    public string clan_name;
	public string clan_description;
	public bool clan_is_recruiting;
    public string clan_region;
	public int clan_required_lvl;
    public string clan_icon;
	public int clan_lvl;
    public int clan_experience;
	public int clan_xp_to_level;
    public int clan_max_members;
    public int clan_total_members;
    public int clan_total_contribution;
    public List<Log> clanLogs;
}

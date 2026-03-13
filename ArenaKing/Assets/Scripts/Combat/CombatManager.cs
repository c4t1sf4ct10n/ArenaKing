using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;
using Unity.Hierarchy;
using UnityEngine.SceneManagement;
using TMPro;

public class CombatManager : MonoBehaviour
{
    const string combatApiUrl = "http://localhost:3000/api/combat";

    public Animator player1Animator;
    public Animator player2Animator;
    public Animator player3Animator;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public Camera mainCamera;
    public Canvas canvas;

    public Slider player1HealthBar;
    public Slider player2HealthBar;
    public GameObject player1HealthText;
    public GameObject player2HealthText;
    public GameObject player1NameText;
    public GameObject player2NameText;

    public GameObject playerSkillPanel;
    public GameObject enemySkillPanel;

    public TextMeshProUGUI playerSkillText;
    public TextMeshProUGUI enemySkillText;
    
    public Image playerSkillBackground;
    public Image enemySkillBackground;

    private readonly float skillDisplayDuration = 1.0f;

    public GameObject damagePopupPrefab;

    private float player1MaxHealth = 100f; // Max HP du joueur 1
    private float player2MaxHealth = 100f; // Max HP du joueur 2

    private float player1CurrentHealth;
    private float player2CurrentHealth;


    public GameObject bootsPrefab;  // Assign 3D helmet prefab in the inspector
    public SkinnedMeshRenderer targetMesh;
    //Equipment[] currentEquipment;
    SkinnedMeshRenderer[] currentMeshes;
    public Animator animator;

    private float minCameraDistance = 7.0f;  // Minimum zoom level
    private float maxCameraDistance = 10.0f; // Maximum zoom level
    private float minFieldOfView = 30.0f;  // Minimum Field Of View level
    private float maxFieldOfView = 100.0f; // Maximum Field Of View level
    private float cameraZoomSpeed = 0.5f;    // Speed at which the camera zooms
    //private float attackDelay = 1.5f;
    //private float attackDuration = 1.0f;  // Time for attack animation
    private float responseDuration = 1.0f;  // Time for defender's animation
    private static string player1Id;
    private static string player2Id;
    public Dictionary<string, Skill> skills = new Dictionary<string, Skill>()
    {
        { "Stone Kick", new Skill("Stone Kick", "Kick", 1.0f, 0.4f, "earth") },
        { "Flame Kick", new Skill("Flame Kick", "Kick", 1.0f, 0.4f, "fire") },
        { "Frost Kick", new Skill("Frost Kick", "Kick", 1.0f, 0.4f, "ice") },
        { "Gale Kick", new Skill("Gale Kick", "Kick", 1.0f, 0.4f, "wind") }, 

        { "Quake Slash", new Skill("Quake Slash", "Slash", 1.0f, 0.4f, "earth") },
        { "Blazing Slash", new Skill("Blazing Slash", "Slash", 1.0f, 0.4f, "fire") },
        { "Glacial Slash", new Skill("Glacial Slash", "Slash", 1.0f, 0.4f, "ice") },
        { "Tempest Slash", new Skill("Tempest Slash", "Slash", 1.0f, 0.4f, "wind") },

        { "Terra Bash", new Skill("Terra Bash", "Slash", 1.0f, 0.4f, "earth") },
        { "Ember Bash", new Skill("Ember Bash", "Slash", 1.0f, 0.4f, "fire") },
        { "Freezing Bash", new Skill("Freezing Bash", "Slash", 1.0f, 0.4f, "ice") },
        { "Cyclone Bash", new Skill("Cyclone Bash", "Slash", 1.0f, 0.4f, "wind") },
        
        { "Earth Whirl", new Skill("Earth Whirl", "Slash", 1.0f, 0.4f, "earth") },
        { "Fire Whirl", new Skill("Fire Whirl", "Slash", 1.0f, 0.4f, "fire") },
        { "Ice Whirl", new Skill("Ice Whirl", "Slash", 1.0f, 0.4f, "ice") },
        { "Wind Whirl", new Skill("Wind Whirl", "Slash", 1.0f, 0.4f, "wind") },
        
        { "Seismic Strike", new Skill("Seismic Strike", "Slash", 1.0f, 0.4f, "earth") },
        { "Inferno Strike", new Skill("Inferno Strike", "Slash", 1.0f, 0.4f, "fire") },
        { "Arctic Strike", new Skill("Arctic Strike", "Slash", 1.0f, 0.4f, "ice") },
        { "Storm Strike", new Skill("Storm Strike", "Slash", 1.0f, 0.4f, "wind") }        

    };

    void Start()
    {
        //player1Id = "4E2F8C60-A059-43C6-81EA-CAB334FC8B86";
        //player2Id = "AA039A16-4F60-4D68-87F9-96570045DDBC";

        player1Id = CombatData.player1Id;
        player2Id = CombatData.player2Id;

        player1MaxHealth = CombatData.player1MaxHealth;
        player2MaxHealth = CombatData.player2MaxHealth;

        player1NameText.GetComponent<TextMeshProUGUI>().text = CombatData.player1Name;
        player2NameText.GetComponent<TextMeshProUGUI>().text = CombatData.player2Name;


        // Initialisation des HP
        player1CurrentHealth = player1MaxHealth;
        player2CurrentHealth = player2MaxHealth;

        // Initialisation des barres de vie
        player1HealthBar.maxValue = player1MaxHealth;
        player2HealthBar.maxValue = player2MaxHealth;

        StartCoroutine(UpdateHealthUI());

        StartCoroutine(SendCombatRequest(player1Id, player2Id, 1));
    }

    public void ShowSkillName(string skillName, string elementType, bool isPlayer)
    {
        Color color;
        switch (elementType)
        {
            case "fire":
                color = new Color(1f, 0.4f, 0.2f);
                break;
            case "ice":
                color = new Color(0.6f, 0.85f, 1f);
                break;
            case "earth":
                color = new Color(0.75f, 0.6f, 0.4f);
                break;
            case "wind":
                color = new Color(0.6f, 1f, 0.6f);
                break;
            default:
                color = Color.gray;
                break;
        }
        if (isPlayer)
        {
            playerSkillText.text = skillName;
            playerSkillBackground.color = color;
            StartCoroutine(ShowPanel(playerSkillPanel));
        }
        else
        {
            enemySkillText.text = skillName;
            enemySkillBackground.color = color;
            StartCoroutine(ShowPanel(enemySkillPanel));
        }
    }

    private IEnumerator ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
        Debug.Log(skillDisplayDuration.ToString());
        yield return new WaitForSeconds(skillDisplayDuration);
        panel.SetActive(false);
    }

    void ShowDamagePopup(Transform target, int damage)
    {
        Debug.Log("ShowDamagePopup");
        // Convertir la position monde vers l’écran (pixels)
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
        screenPos.y += 225f;
        screenPos.x += -25f;
        // Instancier dans le Canvas
        GameObject popup = Instantiate(damagePopupPrefab, canvas.transform);
        popup.SetActive(true);

        // Appliquer la position écran au RectTransform
        popup.GetComponent<RectTransform>().position = screenPos;

        // Afficher les dégâts
        popup.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
    }


    IEnumerator SendCombatRequest(string player1Id, string player2Id, int combatType)
    {
        WWWForm form = new WWWForm();
        form.AddField("player1Id", player1Id);
        form.AddField("player2Id", player2Id);
        form.AddField("combatType", combatType.ToString());

        UnityWebRequest request = UnityWebRequest.Post(combatApiUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            CombatReport result = JsonConvert.DeserializeObject<CombatReport>(jsonResponse);
            StartCoroutine(PlayCombatAnimations(result));
        }
        else
        {
            Debug.LogError("Combat API error: " + request.error);
        }
    }

    IEnumerator PlayCombatAnimations(CombatReport report)
    {
        bool isVictory = false;
        foreach (var round in report.rounds)
        {
            Debug.Log("Attaquant : " + round.attacker + " // Défenseur : " + round.defender + " // Skill : " + round.skill + " // Résultat : " + round.result);

            //Déplacement des joueurs

            // Fetch the skill to get the required distance
            Skill skill = skills[round.skill]; // Assuming skills are stored in a dictionary
            float requiredDistance = skill.Distance;

            // Calculate the current distance between player1 and player2
            float currentDistance = Vector3.Distance(player1.transform.position, player2.transform.position);

            // Move both players towards each other if they are too far apart
            if (currentDistance > requiredDistance)
            {
                // Calculate the midpoint between the two players
                Vector3 midpoint = (player1.transform.position + player2.transform.position) / 2;

                // Move both players to the correct distance
                yield return StartCoroutine(MovePlayersAndCamera(player1, player2, midpoint, requiredDistance));
            }


            // Play attack animation based on the skill used
            Animator playerAnimator;
            if (round.attacker == player1Id) playerAnimator = player1Animator;
            else playerAnimator = player2Animator;
            // Play attack animation based on the skill used
            playerAnimator.Play(skill.Animation);
            ShowSkillName(skill.Name, skill.ElementType, (round.attacker == player1Id));
            //playerAnimator.Play(round.skill);

            // Wait for attack animation to finish
            yield return new WaitForSeconds(skill.AttackDuration);

            // Play defender's response based on the result
            if (round.defender == player1Id) playerAnimator = player1Animator;
            else playerAnimator = player2Animator;
            playerAnimator.Play(round.result);

            Transform target = (round.defender == player1Id) ? player1.transform : player2.transform;
            ShowDamagePopup(target, round.damage);
            if (round.defender == player1Id) yield return UpdateHealth(round.defenderRemainingHP, player2CurrentHealth);
            else yield return UpdateHealth(player1CurrentHealth, round.defenderRemainingHP);

            // Wait for defender's animation to finish
            yield return new WaitForSeconds(responseDuration);

            if (round.result == "Death" && round.attacker == player1Id) isVictory = true;
        }
        if (isVictory) Debug.Log("Victoire");
        GameManager.instance.LoadPlayer();
        SceneManager.LoadScene("MainMenu");
    }

    // Cette méthode sera appelée pour mettre à jour les barres de vie après chaque attaque
    IEnumerator UpdateHealth(float player1Health, float player2Health)
    {
        player1CurrentHealth = player1Health;
        player2CurrentHealth = player2Health;
        if (player1CurrentHealth < 0) { player1CurrentHealth = 0; }
        if (player2CurrentHealth < 0) { player2CurrentHealth = 0; }

        // Mettre à jour les barres de vie et le texte
        player1HealthBar.value = player1CurrentHealth;
        player2HealthBar.value = player2CurrentHealth;

        player1HealthText.GetComponent<TextMeshProUGUI>().text = $"HP: {player1CurrentHealth}/{player1MaxHealth}";
        player2HealthText.GetComponent<TextMeshProUGUI>().text = $"HP: {player2CurrentHealth}/{player2MaxHealth}";
        yield return 0;
    }

    IEnumerator UpdateHealthUI()
    {
        player1HealthBar.value = player1CurrentHealth;
        player2HealthBar.value = player2CurrentHealth;

        player1HealthText.GetComponent<TextMeshProUGUI>().text = $"HP: {player1CurrentHealth}/{player1MaxHealth}";
        player2HealthText.GetComponent<TextMeshProUGUI>().text = $"HP: {player2CurrentHealth}/{player2MaxHealth}";
        yield return 0;
    }

    // Coroutine to move both players and adjust the camera distance
    IEnumerator MovePlayersAndCamera(GameObject player1, GameObject player2, Vector3 midpoint, float targetDistance)
    {
        float moveSpeed = 1.0f;  // Adjust movement speed if necessary
        bool isPlayer1MovingForward = false;
        bool isPlayer2MovingForward = false;

        // Determine if players are moving forward or backward
        if (Vector3.Distance(player1.transform.position, midpoint) > targetDistance / 2)
        {
            isPlayer1MovingForward = true;
            isPlayer2MovingForward = true;
        }

        // Play the appropriate walking animations
        if (isPlayer1MovingForward)
            player1Animator.Play("Walk Forward");
        else
            player1Animator.Play("Walk Backward");

        if (isPlayer2MovingForward)
            player2Animator.Play("Walk Forward");
        else
            player2Animator.Play("Walk Backward");

        while (Vector3.Distance(player1.transform.position, player2.transform.position) > targetDistance)
        {
            // Move both players towards the midpoint
            player1.transform.position = Vector3.MoveTowards(player1.transform.position, midpoint, moveSpeed * Time.deltaTime);
            player2.transform.position = Vector3.MoveTowards(player2.transform.position, midpoint, moveSpeed * Time.deltaTime);

            // Adjust the camera zoom based on the distance between players
            AdjustCameraZoom(player1, player2);

            yield return null; // Wait for the next frame
        }

        // Stop walking animations after moving
        player1Animator.Play("Idle");
        player2Animator.Play("Idle");
    }

    // Function to adjust the camera zoom based on the player distance
    void AdjustCameraZoom(GameObject player1, GameObject player2)
    {
        float distance = Vector3.Distance(player1.transform.position, player2.transform.position);

        // Map the player distance to a zoom level
        float targetCameraDistance = Mathf.Lerp(minCameraDistance, maxCameraDistance, distance / maxCameraDistance);
        if (targetCameraDistance < minCameraDistance) targetCameraDistance = minCameraDistance;
        else if (targetCameraDistance > maxCameraDistance) targetCameraDistance = maxCameraDistance;

        // Smoothly adjust the camera's field of view or position
        float fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetCameraDistance, cameraZoomSpeed * Time.deltaTime);
        if (fieldOfView < minFieldOfView) fieldOfView = minFieldOfView;
        else if(fieldOfView > maxFieldOfView) fieldOfView = maxFieldOfView;
        mainCamera.fieldOfView = fieldOfView;
    }

    // Coroutine to move both players to the required distance
    IEnumerator MovePlayersToDistance(GameObject player1, GameObject player2, Vector3 midpoint, float targetDistance)
    {
        float moveSpeed = 2.0f; // Adjust as necessary

        while (Vector3.Distance(player1.transform.position, player2.transform.position) > targetDistance)
        {
            // Move both players towards the midpoint
            player1.transform.position = Vector3.MoveTowards(player1.transform.position, midpoint, moveSpeed * Time.deltaTime);
            player2.transform.position = Vector3.MoveTowards(player2.transform.position, midpoint, moveSpeed * Time.deltaTime);

            yield return null; // Wait for next frame
        }
    }

}

public class CombatResult
{
    public string winner;
    public Reward reward;
}

public class Reward
{
    public string loot;
}

[System.Serializable]
public class CombatReport
{
    public Round[] rounds;
}

[System.Serializable]
public class Round
{
    public string attacker;
    public string defender;
    public string skill;
    public string result;
    public int damage;
    public int defenderRemainingHP;
}
public class Skill
{
    public string Name { get; set; }
    public string Animation { get; set; }
    public float Distance { get; set; }
    public float AttackDuration { get; set; }
    public string ElementType { get; set; }

    public Skill(string name, string animation, float distance, float attackDuration, string elementType)
    {
        Name = name;
        Animation = animation;
        Distance = distance;
        AttackDuration = attackDuration;
        ElementType = elementType;
    }
}
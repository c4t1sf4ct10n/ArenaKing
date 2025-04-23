using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;
using Unity.Hierarchy;
using UnityEngine.SceneManagement;

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
    private string player1Id;
    private string player2Id;
    public Dictionary<string, Skill> skills = new Dictionary<string, Skill>()
    {
        { "Slash", new Skill("Slash", "Slash", 1.0f, 0.4f) }, // 2.0f is the distance for the Slash skill
        { "Kick", new Skill("Kick", "Kick", 1.0f, 0.4f) } // 2.0f is the distance for the Kick skill
    };

    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        //player3 = GameObject.Find("DefaultCharacter@PartRenderer");
        player1Id = "4E2F8C60-A059-43C6-81EA-CAB334FC8B86";
        player2Id = "AA039A16-4F60-4D68-87F9-96570045DDBC";

        //player3Animator.Play("Idle");
        StartCoroutine(SendCombatRequest(player1Id, player2Id, 1));
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
            playerAnimator.Play(round.skill);
            playerAnimator.Play(round.skill);

            // Wait for attack animation to finish
            yield return new WaitForSeconds(skill.AttackDuration);

            // Play defender's response based on the result
            if (round.defender == player1Id) playerAnimator = player1Animator;
            else playerAnimator = player2Animator;
            playerAnimator.Play(round.result);

            // Wait for defender's animation to finish
            yield return new WaitForSeconds(responseDuration);

            if (round.result == "Death" && round.attacker == player1Id) isVictory = true;
        }
        if (isVictory) Debug.Log("Victoire");
        MainMenuManager.instance.LoadPlayer();
        MainMenuManager.instance.RefreshInventory();
        SceneManager.LoadScene("MainMenu");
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
}
public class Skill
{
    public string Name { get; set; }
    public string Animation { get; set; }
    public float Distance { get; set; }
    public float AttackDuration { get; set; }

    public Skill(string name, string animation, float distance, float attackDuration)
    {
        Name = name;
        Animation = animation;
        Distance = distance;
        AttackDuration = attackDuration;
    }
}
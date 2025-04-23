using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatEnd : MonoBehaviour
{
    private float startTime;
    private bool canExit = false;

    void Start()
    {
        // Enregistrer le moment où la scène a démarré
        startTime = Time.time;
        Debug.Log("Début du timer");
    }

    void Update()
    {
        // Vérifie si 5 secondes se sont écoulées
        if (!canExit && Time.time - startTime >= 1f)
        {
            canExit = true;
            Debug.Log("Fin du timer");
        }

        // Autoriser la sortie uniquement si 5 secondes sont passées
        if (canExit && Input.GetMouseButtonDown(0))
        {
            GameManager.instance.LoadPlayer();
            SceneManager.LoadScene("MainMenu");
        }
    }
}

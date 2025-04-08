using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatNavigation : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject combatPrepPanel;

    public void ShowCombatPrep()
    {
        //mainMenuPanel.SetActive(false);
        combatPrepPanel.SetActive(true);
    }

    public void StartCombat()
    {
        // Charger la scène du combat
        SceneManager.LoadScene("CombatScene");
    }
}

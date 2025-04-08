using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatEnd : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

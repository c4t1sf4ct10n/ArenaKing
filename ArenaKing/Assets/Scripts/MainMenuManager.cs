using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public GameObject CurrencyFreemiumButton;
    public GameObject CurrencyPremiumButton;
    public GameObject CurrencyFreemiumText;
    public GameObject CurrencyPremiumText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Keep the object alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        RefreshCurrency();   
    }

    public void RefreshCurrency()
    {
        CurrencyFreemiumText = GameObject.Find("CurrencyFreemiumText");
        CurrencyFreemiumText.GetComponent<Text>().text = PlayerManager.instance.GetPlayerCurrencyFreemium().ToString();
        Debug.Log(PlayerManager.instance.GetPlayerCurrencyFreemium().ToString());

    }
}

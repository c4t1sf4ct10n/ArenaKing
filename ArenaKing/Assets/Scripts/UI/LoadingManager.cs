using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class LoadingManager : MonoBehaviour
{


    void Start()
    {
        if (Application.isPlaying)
        {
            GameManager.instance.StartApp();
        }
    }


}

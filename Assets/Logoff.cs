using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logoff : MonoBehaviour
{
    public void FazerLogoff()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Login", LoadSceneMode.Single);
    }
}

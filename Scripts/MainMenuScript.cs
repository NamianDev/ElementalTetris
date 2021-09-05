using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour{

    public Sprite[] list;
    public GameObject GO;
    public void StartClick()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitMetod()
    {
        Application.Quit();
    }
}

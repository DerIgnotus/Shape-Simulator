using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CrazyGames;

public class UI : MonoBehaviour
{
    public void FirstButton()
    {
        CrazySDK.Game.GameplayStart();
        SceneManager.LoadScene("Simulation 1");
    }

    public void SecondButton()
    {
        CrazySDK.Game.GameplayStart();
        SceneManager.LoadScene("Simulation 2");
    }

    public void ThirdButton()
    {
        CrazySDK.Game.GameplayStart();
        SceneManager.LoadScene("Simulation 3");
    }

    public void BackToStart()
    {
        CrazySDK.Game.GameplayStop();
        SceneManager.LoadScene("Start");
    }
}

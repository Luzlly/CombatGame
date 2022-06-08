using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private VariableCheck varCheck;
    public Text levelText;

    public void Start()
    {
        varCheck = GameObject.Find("Variables").GetComponent<VariableCheck>();
        levelText.text = "You Lost on Level " + varCheck.sceneNum;
    }

    public void RestartGame()
    {
        varCheck.InitializeVariables();
        SceneManager.LoadScene("Battle");
        Debug.Log("Resetting Game");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}

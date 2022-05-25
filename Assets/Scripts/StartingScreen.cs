using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScreen : MonoBehaviour
{
    private VariableCheck varCheck;

    public void Start()
    {
        varCheck = GameObject.Find("Variables").GetComponent<VariableCheck>(); //Establishes Connection with Variables Script
    }

        // Start is called before the first frame update
        public void PlayGame()
    {
        varCheck.sceneNum = 1;
        SceneManager.LoadScene("FightScene");
    }

}

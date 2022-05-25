using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Upgrades : MonoBehaviour
{
    
    private VariableCheck varCheck;

    public void Start()
    {
        varCheck = GameObject.Find("Variables").GetComponent<VariableCheck>();
    }

    public void MaxHealth()
    {
        varCheck.upgMH += 5;
        SceneManager.LoadScene("FightScene");
        Debug.Log("Loaded Scene: " + varCheck.sceneNum);
    }

    public void Healing()
    {
        varCheck.upgHeal += 3;
        SceneManager.LoadScene("FightScene");
        Debug.Log("Loaded Scene: " + varCheck.sceneNum);
    }

    public void Attack()
    {
        varCheck.upgAtk += 2;
        SceneManager.LoadScene("FightScene");
        Debug.Log("Loaded Scene: " + varCheck.sceneNum);
    }
}

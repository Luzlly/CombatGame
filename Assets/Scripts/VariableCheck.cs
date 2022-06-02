using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableCheck : MonoBehaviour
{
    public int sceneNum;
    public int upgMH;
    public int upgHeal;
    public int upgAtk;
    public int enemyMaxHP;
    public int enemyAtk;

    // Start is called before the first frame update
    void Start()
    {
        upgMH = 0;
        upgHeal = 0;
        upgAtk = 0;
        sceneNum = 1;
        enemyMaxHP = 20;
        enemyAtk = 5;
    }

    private void Awake()
    {
        UnityEngine.Object.DontDestroyOnLoad(this);
    }
}

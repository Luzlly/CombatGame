using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableCheck : MonoBehaviour
{
    public int sceneNum;
    public int upgMH;
    public int upgHeal;
    public int upgAtk;

    // Start is called before the first frame update
    void Start()
    {
        upgMH = 0;
        upgHeal = 0;
        upgAtk = 0;
        sceneNum = 1;
    }

    private void Awake()
    {
        UnityEngine.Object.DontDestroyOnLoad(this);
    }
}

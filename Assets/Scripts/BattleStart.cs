using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleStart : MonoBehaviour
{
    public int playerHealth;
    int playerMaxHealth;
    public int enemyHealth;
    int enemyMaxHealth;
    int enemyPower;
    int playerPower;
    public int randVar;
    public GameObject healthIcon;
    public GameObject attackIcon;
    public HealthBar enemyHealthBar;
    public HealthBar playerHealthBar;
    public Text playerHealthText;
    public Text enemyHealthText;
    public Text levelText;
    public Text actionText;
    bool enemyAtking;
    private VariableCheck varCheck;


    public void Start()
    {
        varCheck = GameObject.Find("Variables").GetComponent<VariableCheck>(); //Establishes Connection with Variables Script
        // Initialising Variables
        playerMaxHealth = 25 + varCheck.upgMH;
        if (varCheck.sceneNum == 5)
        {
            enemyMaxHealth = 40;
        }
        else
        {
            enemyMaxHealth = 20;
        }
        playerHealth = playerMaxHealth;
        enemyHealth = enemyMaxHealth;
        // Enables the on-screen visuals
        levelText.text = "Level " + varCheck.sceneNum;
        actionText.GetComponent<Text>().enabled = false;
        enemyAtking = true;
        attackIcon.SetActive(false);
        healthIcon.SetActive(false);
        randVar = Random.Range(0, 2);
        enemyHealthText.text = enemyHealth.ToString() + "/" + enemyMaxHealth.ToString();
        playerHealthText.text = playerHealth.ToString() + "/" + playerMaxHealth.ToString();
        // Sets Max Health of Health Bar
        enemyHealthBar.SetMaxHealth(enemyMaxHealth);
        playerHealthBar.SetMaxHealth(playerMaxHealth);
    }


    public void PlayerAttacks() // Player turn of Attack
    {
        
        if (playerHealth > 0)
        {
            playerPower = 5 + varCheck.upgAtk;
            enemyPower = Random.Range(4, 6);
            enemyHealth -= playerPower;
            enemyHealthBar.SetHealth(enemyHealth);
            enemyHealthText.text = enemyHealth.ToString() + "/" + enemyMaxHealth.ToString();
            if (enemyHealth < 0)
            {
                enemyHealth = 0;
            }
            Debug.Log("Player Attacked for " + playerPower);
            EnemyTurn();
        }
    }
    public void PlayerHeals() //Player turn of Healing
    {
        if (playerHealth > 0)
        {
            playerPower = 5 + varCheck.upgAtk;
            enemyPower = Random.Range(4, 6);
            playerHealth += (5 + varCheck.upgHeal);
            if (playerHealth > playerMaxHealth)
            {
                playerHealth = playerMaxHealth;
            }
            playerHealthBar.SetHealth(playerHealth);
            playerHealthText.text = playerHealth.ToString() + "/" + playerMaxHealth.ToString();
            EnemyTurn();
        }
    }

    public void EnemyTurn() //Enemy Turn
    {
        actionText.GetComponent<Text>().enabled = true;

        if (enemyHealth > 0) 
        {
            if(randVar == 1)
            {
                playerHealth -= enemyPower;
                randVar = Random.Range(0, 2);
                if (playerHealth < 0)
                {
                    playerHealth = 0;
                }
                playerHealthText.text = playerHealth.ToString() + "/" + playerMaxHealth.ToString();
                enemyAtking = true;
                Debug.Log("Enemy Attacked");
            }
            else
            {
                enemyHealth += 3;
                randVar = Random.Range(0, 2);
                enemyHealthText.text = enemyHealth.ToString() + "/" + enemyMaxHealth.ToString();
                enemyAtking = false;
                Debug.Log("Enemy Healed");
            }
            enemyHealthBar.SetHealth(enemyHealth);
            playerHealthBar.SetHealth(playerHealth);
        }
    }

    public void Update() //Constantly Checking
    {
        if (randVar == 1) //Changes enemy icon, depending on what their next move is
        {
            attackIcon.SetActive(true);
            healthIcon.SetActive(false);
        }
        else
        {
            attackIcon.SetActive(false);
            healthIcon.SetActive(true);
        }

        if (enemyAtking == true) // Displays the last action of the enemy
        {
            actionText.text = "Enemy Attacked for " + enemyPower;
        }
        else
        {
            actionText.text = "Enemy Healed for 5";
        }

        if (playerHealth <= 0) // Lose Condition
        {
            print("Game Lost!");
            actionText.text = "Game Lost";
            Debug.Log("Enemy Health: " + enemyHealth);
            this.enabled = false;
        }
        else if (enemyHealth <= 0) // Win Condition
        {
            print("Game Won!");
            actionText.text = "Game Won";
            Debug.Log("Player Health: " + playerHealth);
            SceneManager.LoadScene("Upgrades");
            this.enabled = false;
            varCheck.sceneNum++;
        }
        

        if (playerHealth >= playerMaxHealth) //Disables the Heal Button if player is on full health
        {
            GameObject.Find("Defend").GetComponent<Button>().interactable = false;
        }
        else
        {
            GameObject.Find("Defend").GetComponent<Button>().interactable = true;
        }

        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }

        if (enemyHealth >= enemyMaxHealth)
        {
            randVar = 1;
        }
    }


}

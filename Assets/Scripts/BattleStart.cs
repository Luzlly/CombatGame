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

    private VariableCheck varCheck;

    public Animator playerAnimator;
    public Animator enemyAnimator;

    public AudioClip playerAtkSnd;
    public AudioClip enemyAtkSnd;
    public AudioClip deathSnd;
    public AudioSource audioSource;

    public void Start()
    {
        actionText.text = "";
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
        actionText.GetComponent<Text>().enabled = true;
        attackIcon.SetActive(false);
        healthIcon.SetActive(false);
        randVar = Random.Range(0, 2);
        enemyHealthText.text = enemyHealth.ToString() + "/" + enemyMaxHealth.ToString();
        playerHealthText.text = playerHealth.ToString() + "/" + playerMaxHealth.ToString();

        // Sets Max Health of Health Bar
        enemyHealthBar.SetMaxHealth(enemyMaxHealth);
        playerHealthBar.SetMaxHealth(playerMaxHealth);
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        GameObject.Find("Attack").GetComponent<Button>().interactable = true;
        GameObject.Find("Defend").GetComponent<Button>().interactable = true;
    }

    public void PlayerAttacks() // Player turn of Attack
    {
        
        if (playerHealth > 0)
        {
            playerPower = 5 + varCheck.upgAtk;
            enemyPower = Random.Range(4, 6);
            playerAnimator.SetTrigger("playerAtk"); //Triggers Player Attack Animation
            StartCoroutine(WaitForEnemyTurn());
            enemyHealth -= playerPower;
            actionText.text = "Player Attacked for " + playerPower;
            enemyHealthBar.SetHealth(enemyHealth);
            enemyHealthText.text = enemyHealth.ToString() + "/" + enemyMaxHealth.ToString();
            if (enemyHealth <= 0)
            {
                enemyHealth = 0;
            }
            audioSource.PlayOneShot(playerAtkSnd, 0.7F);
            Debug.Log("Player Attacked for " + playerPower);
        }
    }
    public void PlayerHeals() //Player turn of Healing
    {
        if (playerHealth > 0)
        {
            playerPower = 5 + varCheck.upgAtk;
            enemyPower = Random.Range(4, 6);
            playerAnimator.SetTrigger("playerHeal"); //Triggers Player Heal Animation
            StartCoroutine(WaitForEnemyTurn());
            playerHealth += (5 + varCheck.upgHeal);
            actionText.text = "Player Healed for " + (5 + varCheck.upgHeal);
            if (playerHealth > playerMaxHealth)
            {
                playerHealth = playerMaxHealth;
            }
            playerHealthBar.SetHealth(playerHealth);
            playerHealthText.text = playerHealth.ToString() + "/" + playerMaxHealth.ToString();
        }
    }

    public void EnemyTurn() //Enemy Turn
    {
        actionText.GetComponent<Text>().enabled = true;

        if (enemyHealth <= 10) 
        {
            if(randVar == 1)
            {
                EnemyAttacks();
            }
            else
            {
                EnemyHeals();
            }
            enemyHealthBar.SetHealth(enemyHealth);
            playerHealthBar.SetHealth(playerHealth);
        }
        else
        {
            EnemyAttacks();
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

        if (playerHealth <= 0) // Lose Condition
        {
            playerAnimator.SetTrigger("playerDead");
            print("Game Lost!");
            actionText.text = "Game Lost";
            Debug.Log("Enemy Health: " + enemyHealth);
            audioSource.PlayOneShot(deathSnd, 0.7F);
            this.enabled = false;
        }
        else if (enemyHealth <= 0) // Win Condition
        {
            enemyAnimator.SetTrigger("enemyDead");
            print("Game Won!");
            actionText.text = "Game Won";
            Debug.Log("Player Health: " + playerHealth);
            audioSource.PlayOneShot(deathSnd, 0.7F);
            this.enabled = false;
            StartCoroutine(WaitForUpgradeLoad());
            varCheck.sceneNum++;
        }
        

        if (playerHealth >= playerMaxHealth) //Disables the Heal Button if player is on full health
        {
            GameObject.Find("Defend").GetComponent<Button>().interactable = false;
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

    public void EnemyAttacks()
    {
        enemyAnimator.SetTrigger("enemyAtk");
        playerHealth -= enemyPower;
        randVar = Random.Range(0, 2);
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }
        playerHealthText.text = playerHealth.ToString() + "/" + playerMaxHealth.ToString();
        StartCoroutine(WaitForPlayerTurn());
        actionText.text = "Enemy Attacked for " + enemyPower;
        audioSource.PlayOneShot(enemyAtkSnd, 0.7F);
        Debug.Log("Enemy Attacked");
    }

    public void EnemyHeals()
    {
        enemyAnimator.SetTrigger("enemyHeal");
        enemyHealth += 5;
        randVar = Random.Range(0, 2);
        enemyHealthText.text = enemyHealth.ToString() + "/" + enemyMaxHealth.ToString();
        StartCoroutine(WaitForPlayerTurn());
        actionText.text = "Enemy Healed for 5";
        Debug.Log("Enemy Healed");
    }

    private IEnumerator WaitForUpgradeLoad()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Upgrades");

    }

    private IEnumerator WaitForPlayerTurn()
    {
        yield return new WaitForSeconds(2);
        PlayerTurn();
    }

    private IEnumerator WaitForEnemyTurn()
    {
        GameObject.Find("Attack").GetComponent<Button>().interactable = false;
        GameObject.Find("Defend").GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(2);
        EnemyTurn();
    }


}

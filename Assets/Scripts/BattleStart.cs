using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleStart : MonoBehaviour
{
    // Integers used
    public int playerHealth;
    int playerMaxHealth;
    public int enemyHealth;
    int enemyMaxHealth;
    int playerPower;
    private int playerMana;
    private int enemyMana;
    public int randVar;

    public bool enemyAtking;

    public GameObject iconHealth;
    public GameObject iconAttack;

    public HealthBar enemyHealthBar;
    public HealthBar playerHealthBar;

    public Text playerHealthText;
    public Text enemyHealthText;
    public Text levelText;
    public Text actionText;
    public Text buttonHealText;

    private VariableCheck varCheck;

    public Animator playerAnimator;
    public Animator enemyAnimator;

    public AudioClip playerAtkSnd;
    public AudioClip enemyAtkSnd;
    public AudioClip deathSnd;
    public AudioSource audioSource;

    public void Start()
    {
        playerMana = 3;
        enemyMana = 3;
        enemyAtking = true;

        varCheck = GameObject.Find("Variables").GetComponent<VariableCheck>(); //Establishes Connection with Variables Script

        // Initialising Variables
        playerMaxHealth = 20 + varCheck.upgMH;
        if (varCheck.sceneNum % 5 == 0)
        {
            enemyMaxHealth = (int)(1.5 * varCheck.enemyMaxHP);
        }
        else
        {
            enemyMaxHealth = varCheck.enemyMaxHP;
        }
        playerHealth = playerMaxHealth;
        enemyHealth = enemyMaxHealth;

        // Enables the on-screen visuals
        actionText.GetComponent<Text>().enabled = true;
        iconAttack.SetActive(true);
        iconHealth.SetActive(false);
        actionText.text = "";
        levelText.text = "Level " + varCheck.sceneNum;
        enemyHealthText.text = enemyHealth.ToString() + "/" + enemyMaxHealth.ToString();
        playerHealthText.text = playerHealth.ToString() + "/" + playerMaxHealth.ToString();
        buttonHealText.text = "Heal (" + playerMana.ToString() + "/3)";

        // Sets Max Health of Health Bar
        enemyHealthBar.SetMaxHealth(enemyMaxHealth);
        playerHealthBar.SetMaxHealth(playerMaxHealth);
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        if (playerHealth >= playerMaxHealth || playerMana == 0) //Disables the Heal Button if player is on full health
        {
            GameObject.Find("Defend").GetComponent<Button>().interactable = false;
            GameObject.Find("Attack").GetComponent<Button>().interactable = true;
        }
        else
        {
            GameObject.Find("Defend").GetComponent<Button>().interactable = true;
            GameObject.Find("Attack").GetComponent<Button>().interactable = true;
        }
        
        if (randVar <= 4 && enemyHealth <= (int)(.25 * enemyMaxHealth) && enemyMana != 0) //Changes enemy icon, depending on what their next move is
        {
            iconAttack.SetActive(false);
            iconHealth.SetActive(true);
            enemyAtking = false;
        }
        else
        {
            iconAttack.SetActive(true);
            iconHealth.SetActive(false);
            enemyAtking = true;
        }
    }

    public void PlayerAttacks() // Player turn of Attack
    {
        
        if (playerHealth > 0)
        {
            playerPower = 5 + varCheck.upgAtk;
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
            playerAnimator.SetTrigger("playerHeal"); //Triggers Player Heal Animation
            StartCoroutine(WaitForEnemyTurn());
            playerHealth += (5 + varCheck.upgHeal);
            actionText.text = "Player Healed for " + (5 + varCheck.upgHeal);
            if (playerHealth > playerMaxHealth)
            {
                playerHealth = playerMaxHealth;
            }
            playerMana -= 1;
            buttonHealText.text = "Heal (" + playerMana.ToString() + "/3)";
            playerHealthBar.SetHealth(playerHealth);
            playerHealthText.text = playerHealth.ToString() + "/" + playerMaxHealth.ToString();
        }
    }

    public void EnemyTurn() //Enemy Turn Deciding
    {
        actionText.GetComponent<Text>().enabled = true;

        if (enemyAtking == true)
        {
            EnemyAttacks();
        }
        else if (enemyAtking == false)
        {
            EnemyHeals();
        }
        randVar = Random.Range(1, 6);
        enemyHealthBar.SetHealth(enemyHealth);
        playerHealthBar.SetHealth(playerHealth);
    }

    public void EnemyAttacks()
    {
        enemyAnimator.SetTrigger("enemyAtk");
        playerHealth -= varCheck.enemyAtk;
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }
        playerHealthText.text = playerHealth.ToString() + "/" + playerMaxHealth.ToString();
        StartCoroutine(WaitForPlayerTurn());
        actionText.text = "Enemy Attacked for " + varCheck.enemyAtk.ToString();
        audioSource.PlayOneShot(enemyAtkSnd, 0.7F);
        Debug.Log("Enemy Attacked");
    }

    public void EnemyHeals()
    {
        enemyAnimator.SetTrigger("enemyHeal");
        enemyHealth += 5;
        enemyHealthText.text = enemyHealth.ToString() + "/" + enemyMaxHealth.ToString();
        StartCoroutine(WaitForPlayerTurn());
        enemyMana -= 1;
        actionText.text = "Enemy Healed for 5";
        Debug.Log("Enemy Healed");
    }

    public void Update() //Constantly Checking
    {
        if (playerHealth <= 0) // Lose Condition
        {
            playerAnimator.SetTrigger("playerDead");
            print("Game Lost!");
            actionText.text = "Game Lost at Level " + varCheck.sceneNum.ToString();
            Debug.Log("Enemy Health: " + enemyHealth);
            audioSource.PlayOneShot(deathSnd, 0.7F);
            this.enabled = false;
            StartCoroutine(WaitForGameOver());
        }
        else if (enemyHealth <= 0) // Win Condition
        {
            enemyAnimator.SetTrigger("enemyDead");
            print("Fight Won!");
            actionText.text = "Fight Won";
            Debug.Log("Player Health: " + playerHealth);
            audioSource.PlayOneShot(deathSnd, 0.7F);
            this.enabled = false;
            StartCoroutine(WaitForUpgradeLoad());

            if (varCheck.sceneNum % 2 == 0)
            {
                varCheck.enemyMaxHP += 5;
            }
            if (varCheck.sceneNum % 2 != 0)
            {
                varCheck.enemyAtk += 1;
            }
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

    private IEnumerator WaitForUpgradeLoad()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Upgrades");

    }

    private IEnumerator WaitForGameOver()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameOver");

    }

    private IEnumerator WaitForPlayerTurn()
    {
        yield return new WaitForSeconds(1);
        PlayerTurn();
    }

    private IEnumerator WaitForEnemyTurn()
    {
        GameObject.Find("Attack").GetComponent<Button>().interactable = false;
        GameObject.Find("Defend").GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(1);
        EnemyTurn();
    }
}

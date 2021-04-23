using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEquips : MonoBehaviour
{
    #region Variables
    [SerializeField] Canvas Bar;
    [SerializeField] Slider healthBar;
    [SerializeField] Image health;
    [SerializeField] Image bar;
    [SerializeField] Animator animCharacter;

    GameManager manager;

    public bool playerIsDead = false;
    public bool faceRight = false;
    bool shouldDisplay = false;

    public int totalLives = 3;

    float maxHealth = 100f;
    float totalHealth = 100f;
    //float damageByPig = 10f;

    #endregion
    #region BuiltIn Methods
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        health.CrossFadeAlpha(0, 1f, false);
        bar.CrossFadeAlpha(0, 1f, false);
        healthBar.value = (totalHealth / maxHealth);
    }
    private void Update()
    {
        healthBar.value = (totalHealth / maxHealth);
    }
    #endregion

    #region Custom Methods
    public void DamagePig(float amountOfDamage)
    {
        totalHealth -= amountOfDamage;
        if (!playerIsDead)
        {
            animCharacter.SetTrigger("on_Hit");
            shouldDisplay = true;
        }
        if (totalHealth <= 0f && totalLives > 0)
        {
            totalHealth = maxHealth;
            totalLives -= 1;
            manager.HealthManager(totalLives);
        }
        if (totalLives <= 0)
        {
            playerIsDead = true;
            Bar.enabled = false;
            animCharacter.SetBool("on_dead", true);
            manager.KillPlayer();
        }
        if(shouldDisplay)
        {
            DisplayHealth();
        }
    }
    public void Die()
    {
        totalLives = 0;
        manager.HealthManager(totalLives);
        playerIsDead = true;
        animCharacter.SetBool("on_dead", true);
    }
    public void DisplayHealth()
    {
        health.CrossFadeAlpha(1, 1f, false);
        bar.CrossFadeAlpha(1, 1f, false);
        shouldDisplay = false;
        StartCoroutine(HealthDissapear());
    }
    public void Flip()
    {
        faceRight = !faceRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = Bar.transform.localScale;
        theScale.x *= -1;
        Bar.transform.localScale = theScale;
    }
    IEnumerator HealthDissapear()
    {
        yield return new WaitForSeconds(3f);
        health.CrossFadeAlpha(0, 1f, false);
        bar.CrossFadeAlpha(0, 1f, false);
    }
    #endregion
}

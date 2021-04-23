using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PigEquips : MonoBehaviour
{
    #region Variables
    [SerializeField] Canvas Bar;
    [SerializeField] Slider healthBar;
    [SerializeField] Image health;
    [SerializeField] Image bar;
    [SerializeField] Animator animPigs;

    public bool playerIsDead = false;
    public bool faceRight = true;
    public bool shouldDisplay = false;

    float maxHealth = 100f;
    float totalHealth = 100f;

    #endregion

    #region BuiltIn Methods
    private void Start()
    {
        health.CrossFadeAlpha(0, 1f, false);
        bar.CrossFadeAlpha(0, 1f, false);
        healthBar.value = (totalHealth / maxHealth);
    }
    private void Update()
    {
        healthBar.value = (totalHealth / maxHealth);
        if(shouldDisplay)
        {
            DisplayHealth();
        }
    }
    #endregion

    #region Custom Methods
    public void Damage()
    {
        totalHealth -= 35f;
        animPigs.SetTrigger("on_hit");
        shouldDisplay = true;
        if (totalHealth <= 0f)
        {
            playerIsDead = true;
            Bar.enabled = false;
            animPigs.SetBool("on_dead", true);
        }
        if (shouldDisplay)
        {
            DisplayHealth();
        }
    }
    public void DisplayHealth()
    {
        health.CrossFadeAlpha(1, 1f, false);
        bar.CrossFadeAlpha(1, 1f, false);
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
        shouldDisplay = false;
    }
    #endregion
}

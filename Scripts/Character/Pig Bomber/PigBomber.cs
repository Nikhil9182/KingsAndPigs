using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBomber : MonoBehaviour
{
    #region Variables
    [SerializeField] Animator animPig;
    [SerializeField] Animator animCannon;
    [SerializeField] Transform attakPoint;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject bomb;
    [SerializeField] GameManager manager;
    [SerializeField] LayerMask whoIsPlayer;
    [SerializeField] PigBomber pig;
    GameObject bombs;

    public float attackRange = 0.6f;
    public float attackForce = 300f;
    float attackRate = 2f;
    float nextAttackTime = 0f;

    #endregion

    #region BuiltIn Methods
    private void FixedUpdate()
    {
        if(manager.player != null)
        {
            if (Time.time >= nextAttackTime)
            {
                if (!bombs)
                {
                    Collider2D col = Physics2D.OverlapCircle(attakPoint.position, attackRange, whoIsPlayer);
                    if (col)
                    {
                        StartCoroutine(Fire());
                        nextAttackTime = Time.time + (4f / attackRate);
                    }
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attakPoint.position, attackRange);
    }
    #endregion

    #region Custom Methods
    IEnumerator Fire()
    {
        animPig.SetTrigger("fire");
        yield return new WaitForSeconds(0.3f);
        animCannon.SetTrigger("shoot");
        yield return new WaitForSeconds(0.3f);
        FireBall();
    }
    void FireBall()
    {
        if(manager.player != null)
        {
            bombs = Instantiate(bomb, spawnPoint.transform.position, spawnPoint.rotation);
            bombs.GetComponent<Bomb>().pig = pig;
            Vector2 direction = (manager.player.transform.position - transform.position);
            bombs.GetComponent<Rigidbody2D>().AddForce(direction * attackForce);
        }
    }
    public void DestroyBomb()
    {
        Destroy(bombs,0.5f);
    }
    #endregion
}

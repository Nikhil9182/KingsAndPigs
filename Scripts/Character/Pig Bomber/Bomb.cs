using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    #region Variables
    [SerializeField] Animator animBomb;
    [SerializeField] Rigidbody2D rbBomb;
    [SerializeField] LayerMask whoIsPlayer;

    public PigBomber pig;
    GameManager manager;

    float impactRadius = 0.29f;
    float damage = 60f;
    #endregion

    #region BuiltIn Methods
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rbBomb.isKinematic = true;
            rbBomb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rbBomb.constraints = RigidbodyConstraints2D.FreezePosition;
            animBomb.SetTrigger("boom");
            pig.DestroyBomb();
        }
        if(collision.gameObject.CompareTag("Ground"))
        {
            rbBomb.isKinematic = true;
            rbBomb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rbBomb.constraints = RigidbodyConstraints2D.FreezePosition;
            animBomb.SetTrigger("boom");
            pig.DestroyBomb();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
    #endregion

    #region Custom Methods
    public void Attack()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, impactRadius, whoIsPlayer);
        if (col && manager.player != null)
        {
            manager.player.GetComponent<CharacterEquips>().DamagePig(damage);
        }
    }
    #endregion
}

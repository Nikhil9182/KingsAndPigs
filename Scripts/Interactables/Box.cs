using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject[] boxParts;
    [SerializeField] GameObject spawnCollectables;
    [SerializeField] Animator animBox;
    [SerializeField] GameObject box;

    List<GameObject> spawnedParts = new List<GameObject>();
    GameObject spawnedCollectable;

    float forceapplied = 1000f;
    float randomSpace = 0.1f;
    float amountOfTime = 1.5f;

    int totalHits = 2;
    int hitsCount = 0;
    #endregion

    #region BuiltIn Methods
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, randomSpace);
    }
    #endregion

    #region Custom Methods
    private void OnDestroyed()
    {
        box.GetComponent<Rigidbody2D>().isKinematic = true;
        box.GetComponent<BoxCollider2D>().enabled = false;
        box.GetComponent<SpriteRenderer>().enabled = false;

        for (int i = 0; i < boxParts.Length; i++)
        {
            GameObject pieces = Instantiate(boxParts[i], transform.position + new Vector3(Random.Range(-randomSpace, randomSpace),Random.Range(-randomSpace, randomSpace),transform.position.z), transform.rotation);
            spawnedParts.Add(pieces);
        }
        if(spawnCollectables != null)
        {
            spawnedCollectable = Instantiate(spawnCollectables, transform.position + new Vector3(Random.Range(0, randomSpace), Random.Range(0, randomSpace), transform.position.z), transform.rotation);
        }
        for (int i = 0; i < boxParts.Length; i++)
        {
            Vector2 direction = spawnedParts[i].transform.position - transform.position;
            spawnedParts[i].GetComponent<Rigidbody2D>().AddForce(direction * forceapplied);
        }
        
        if(spawnedCollectable)
        {
            Vector2 direction = spawnedCollectable.transform.position - transform.position;
            spawnedCollectable.GetComponent<Rigidbody2D>().AddForce(direction * forceapplied);
            Destroy(spawnedCollectable, 5f);
        }
        for (int i = 0; i < boxParts.Length; i++)
        {
            Destroy(spawnedParts[0], amountOfTime);
            spawnedParts.RemoveAt(0);
        }
    }
    public void OnHit()
    {
        hitsCount += 1;
        animBox.SetTrigger("on_hit");
        if (hitsCount >= totalHits)
        {
            OnDestroyed();
        }
    }
    #endregion
}

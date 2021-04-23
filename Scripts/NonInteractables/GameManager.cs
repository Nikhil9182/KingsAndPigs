using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField] Transform spawnDoor;  // Take Spawn transform for spawning the character
    [SerializeField] Transform finalDoor;  // For detroying the character if it reaches to that point
    [SerializeField] GameObject character;  //take the prefab of the character for spawning
    [SerializeField] Animator animDoorOne;  // Take the animator for playing animation
    [SerializeField] Animator animDoorTwo;  // Take the animator for playing animation
    [SerializeField] Animator animCharacter; // Take the animator for playing animation
    [SerializeField] List<GameObject> enemyCounts = new List<GameObject>();
    [SerializeField] Image[] health;
    [SerializeField] Text diamonds;
    [SerializeField] Camera cam;
    [SerializeField] Button door;

    public GameObject player;
    CameraFollow follow;

    float waitTimeInSpawn = 1f;

    int killedCount = 0;
    int totalEnemies = 0;
    int totalDiamonds = 0;

    public bool killedAll = false;
    public bool inTouch = false;
    bool canPress = true;
    //bool canFollow = false;
    #endregion

    #region Builtin Methods
    public void Start()
    {
        door.interactable = false;
        follow = GetComponent<CameraFollow>();
        totalEnemies = enemyCounts.Count;
        StartCoroutine(SpawnCharacter());
    }
    private void Update()
    {
        for (int i = 0; i < enemyCounts.Count; i++)
        {
            if(enemyCounts[i].GetComponent<PigEquips>().playerIsDead)
            {
                killedCount += 1;
                enemyCounts.RemoveAt(i);
            }
        }
        if(killedCount == totalEnemies)
        {
            killedAll = true;
            door.interactable = true;
        }
        if(CrossPlatformInputManager.GetButtonDown("Fire2") && killedAll && canPress)
        {
            if(inTouch)
            {
                StartCoroutine(CloseDoor());
            }
        }
        diamonds.text = totalDiamonds.ToString();
    }
    #endregion

    #region Custom Methods
    private IEnumerator SpawnCharacter()
    {
        animDoorOne.SetTrigger("open");
        yield  return new WaitForSeconds(waitTimeInSpawn);
        player = Instantiate(character, spawnDoor.position, spawnDoor.rotation);
        yield return new WaitForSeconds(waitTimeInSpawn);
        animDoorOne.SetTrigger("close");
        yield return new WaitForSeconds(waitTimeInSpawn);
        player.GetComponent<CharacterMovement>().doMove = true;
    }
    public void OpenDoor()
    {
        animDoorTwo.SetTrigger("open");
    }
    public IEnumerator CloseDoor()
    {
        canPress = false;
        player.transform.position = finalDoor.position;
        player.GetComponent<CharacterMovement>().doMove = false;
        player.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Destroy(player);
        animDoorTwo.SetTrigger("close");
        yield return new WaitForSeconds(waitTimeInSpawn);
    }
    public void HealthManager(int totalLives)
    {
        if(totalLives == 3)
        {
            health[0].CrossFadeAlpha(1, 1f, false);
            health[1].CrossFadeAlpha(1, 1f, false);
            health[2].CrossFadeAlpha(1, 1f, false);
        }
        else if (totalLives == 2)
        {
            health[0].CrossFadeAlpha(1, 1f, false);
            health[1].CrossFadeAlpha(1, 1f, false);
            health[2].CrossFadeAlpha(0, 1f, false);
        }
        else if (totalLives == 1)
        {
            health[0].CrossFadeAlpha(1, 1f, false);
            health[1].CrossFadeAlpha(0, 1f, false);
            health[2].CrossFadeAlpha(0, 1f, false);
        }
        else if(totalLives == 0)
        {
            health[0].CrossFadeAlpha(0, 1f, false);
            health[1].CrossFadeAlpha(0, 1f, false);
            health[2].CrossFadeAlpha(0, 1f, false);
        }
    }
    public void DiamontCount()
    {
        totalDiamonds += 1;
    }
    public void KillPlayer()
    {
        Destroy(player, 1.5f);
    }
    #endregion
}

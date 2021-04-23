using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Variables
    [SerializeField] GameManager manager;
    [Range(1, 10)] [SerializeField] int smoothFactor;

    public float startPointY = 0f;
    public float endPointY = 0f;
    public float startPointX = 0f;
    public float endPointX = 0f;

    public bool directionY = false;
    public bool directionX = false;
    #endregion

    #region BuiltIn Methods
    private void FixedUpdate()
    {
        if(manager.player)
        {
            if (directionX)
            {
                if (transform.position.x >= startPointX && transform.position.x <= endPointX)
                {
                    Vector3 temp = transform.position;
                    temp.x = manager.player.transform.position.x;
                    Vector3 smoothPosition = Vector3.Lerp(transform.position, temp, smoothFactor * Time.fixedDeltaTime);
                    transform.position = smoothPosition;
                }
            }
            if (directionY)
            {
                if(transform.position.y >= startPointY && transform.position.y <= endPointY)
                {
                    Vector3 temp = transform.position;
                    temp.y = manager.player.transform.position.y;
                    Vector3 smoothPosition = Vector3.Lerp(transform.position, temp, smoothFactor * Time.fixedDeltaTime);
                    transform.position = smoothPosition;
                }
            }
        }
    }
    #endregion
}

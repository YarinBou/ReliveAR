using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineMovement : MonoBehaviour
{
    public Vector3 minPose, maxPose;
    public float speed, defaultYPose, defaultZPose;
    public bool isMoving, moveRight;

    // Start is called before the first frame update
    void Start()
    {
        StartLine();

    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (moveRight)
            {
                transform.Translate(speed, defaultYPose, defaultZPose);
            }
            else // move left
            {
                transform.Translate(-speed, defaultYPose, defaultZPose);
            }

            if ((transform.position.x > maxPose.x) || (transform.position.x < minPose.x)) //change direction
            {
                moveRight = !moveRight;
            }
        }
       
        
    }

    public void StartLine()
    {
        transform.Translate(minPose);
        isMoving = true;
        moveRight = true;

        defaultYPose = this.transform.position.y;
        defaultZPose = this.transform.position.z;
    }

    public void StopLine()
    {
        isMoving = false;
    }
}

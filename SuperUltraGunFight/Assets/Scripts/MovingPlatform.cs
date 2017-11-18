using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    #region MovingPlatform Members
    //the position of the two anchor points in world space
    Vector3 anchor1Pos;
    Vector3 anchor2Pos;
    //A reference to the platform itself
    GameObject platform;
    //a public variable to change the speed of the platform = 5/time to destination;
    public float speed;
    //the start time for lerp;
    float startTime;
    //set witch direction you are moveing;
    bool moveForward;

    //2 vector3 to show the change in movement to keep the character on the object;
    Vector3 previousPos;
    Vector3 movement;
    #endregion


    // Use this for initialization
    void Start () {
       //set the anchor pos and the gameobjects pos
        anchor1Pos = gameObject.transform.Find("Anchor1").position;
        anchor2Pos = gameObject.transform.Find("Anchor2").position;
        platform = gameObject.transform.Find("Platform").gameObject;
        platform.transform.position = anchor1Pos;

        //init default starttime & set moving forward to true
        startTime = Time.time;
        moveForward = true;
	}

    private void Awake()
    {
        //reset starttime on awake
        startTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        previousPos = platform.transform.position;
        //set % of dist
        float dist = (Time.time - startTime) * speed/5;

        //chose direction of movement
        if (moveForward)
        {
            //lerp between anchor points
            platform.transform.position = Vector3.Lerp(anchor1Pos, anchor2Pos, dist);
            //if you reached the other ancor point change directon and reset
            if (platform.transform.position == anchor2Pos)
            {
                moveForward = false;
                startTime = Time.time;
            }
        }
        else
        {
            platform.transform.position = Vector3.Lerp(anchor2Pos, anchor1Pos, dist);
            if (platform.transform.position == anchor1Pos)
            {
                moveForward = true;
                startTime = Time.time;

            }
        }
        //set movement
        movement = platform.transform.position - previousPos;
        
    }

    //get movement
    public Vector3 getMovement()
    {
        return movement;
    }

}

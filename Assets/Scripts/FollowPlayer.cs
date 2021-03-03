using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Camera cam;
    public GameObject player;

    public Vector3 zOffset;

    void Awake()
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector2((float)Screen.width/2f, (float)Screen.height/2f)) + zOffset;


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (IsWithinRange(play))
        this.transform.position = player.transform.position + zOffset;


    }

    bool IsWithinRange()
    {
        bool ret = true;


        return ret;
    }

}

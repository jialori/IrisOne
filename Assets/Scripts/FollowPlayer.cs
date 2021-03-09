using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Camera cam;
    public GameObject player;

    private float m_zOffset;

    void Awake()
    {
        // Vector3 worldPos = cam.ScreenToWorldPoint(new Vector2((float)Screen.width/2f, (float)Screen.height/2f)) + m_zOffset;
        m_zOffset = this.transform.position.z;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (IsWithinRange(play))
        Vector2 playerPos = player.transform.position;
        this.transform.position = new Vector3(playerPos.x, playerPos.y, m_zOffset);


    }

    bool IsWithinRange()
    {
        bool ret = true;


        return ret;
    }

}

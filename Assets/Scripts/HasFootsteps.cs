using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasFootsteps : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject footstepLPrototype;
    public GameObject footstepRPrototype;
    // public int footStepNum;
    public float timeBeforeFade;
    public float timeFade;
    public float footstepsSpacing = 1;

    public float offsetDistance;

    private float m_footstepsSpacingSqr;
    private float m_footstepsSpacingSqrCounter;


    private List<FootStep> m_pool;
    private int m_poolIter = -1;
    private Vector2 lastPosition;


    void Awake()
    {
        if (offsetDistance <= 0) offsetDistance = footstepsSpacing;
        m_footstepsSpacingSqr = Mathf.Pow(footstepsSpacing, 2);
        int poolSize = 10; // an even number required
        m_pool = new List<FootStep>();
        for (int i = 0; i < 10; i++)
        {
            GameObject footstepGameObject;
            if (i % 2 == 0)
            {
                footstepGameObject = Instantiate(footstepLPrototype);
            }
            else
            {
                footstepGameObject = Instantiate(footstepRPrototype);
            }
            FootStep footstepScript = footstepGameObject.GetComponent<FootStep>();
            footstepScript.Initialize(timeBeforeFade, timeFade);
            footstepScript.Deactivate();
            m_pool.Add(footstepScript);
        }
        OnGameStart();
        Debug.Log(m_pool.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_footstepsSpacingSqr == 0) return;

        Vector2 move = (Vector2)transform.position - lastPosition;
        m_footstepsSpacingSqrCounter += move.sqrMagnitude;
        if (m_footstepsSpacingSqrCounter > m_footstepsSpacingSqr)
        {
            if (m_poolIter < m_pool.Count - 1) {m_poolIter++;} else {m_poolIter = 0;}
            FootStep footstep = m_pool[m_poolIter];
            footstep.Reset();
            footstep.transform.position = this.transform.position - offsetDistance * (Vector3)move.normalized;
            footstep.transform.rotation = Quaternion.FromToRotation(Vector2.down, move);
            footstep.Activate();
            m_footstepsSpacingSqrCounter = 0;
        }
        lastPosition = this.transform.position;
    }

    public void OnGameStart()
    {
        lastPosition = this.transform.position;
    }

    public void OnPlayerPlacement()
    {
        lastPosition = this.transform.position;
    }

}

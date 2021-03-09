using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : MonoBehaviour
{
    public float timeLife;
    public float speedTravel = Mathf.NegativeInfinity;
    // public float lengthTravel = Single.negativeInfinity;
    private float m_timer;
    private bool m_isReady = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        m_timer = 0.0f;
        m_isReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isReady) return;
        
        m_timer += Time.deltaTime;
        if (m_timer > timeLife)
        {
            m_timer = 0.0f;
            Deactivate();
        }
        else
        {
            this.transform.position = this.transform.position + Time.deltaTime * speedTravel * this.transform.right; 
        }        
    }


    private void Deactivate()
    {
            gameObject.SetActive(false);
    }

    public void SetIsReady()
    {
        m_isReady = true;
    }

    public void SetSpeed(float speed)
    {
        this.speedTravel = speed;
    }
}

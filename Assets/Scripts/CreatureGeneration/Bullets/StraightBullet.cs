using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletLifeControlType
{
    ByTime,
    ByLength
}

public class StraightBullet : MonoBehaviour, IPoolObject
{
    public float speedTravel = Mathf.NegativeInfinity;
    public BulletLifeControlType lifeControlType;

    [DrawIf("lifeControlType", BulletLifeControlType.ByTime)]
    [SerializeField]
    private float m_lifeByTime;
    public float LifeByTime
    {
        get {return ( (lifeControlType == BulletLifeControlType.ByTime) ? m_lifeByTime : m_lifeByLengthTravel / speedTravel);}
        set {m_lifeByTime = value;}
    }
    [DrawIf("lifeControlType", BulletLifeControlType.ByLength)]
    [SerializeField]
    private float m_lifeByLengthTravel;
    public float LifeByLengthTravel
    {
        get {return ( (lifeControlType == BulletLifeControlType.ByLength) ? m_lifeByLengthTravel : m_lifeByTime * speedTravel);}
        set {m_lifeByLengthTravel = value;}
    }

    private float m_timer;
    private bool m_isReady = false;

    void Awake()
    {
        Deactivate();
    }

    void OnEnable()
    {
        m_timer = 0.0f;
        m_isReady = false;
    }

    void Update()
    {
        if (!m_isReady) return;
        
        m_timer += Time.deltaTime;
        if (m_timer > LifeByTime)
        {
            m_timer = 0.0f;
            Deactivate();
        }
        else
        {
            this.transform.position = this.transform.position + Time.deltaTime * speedTravel * this.transform.right; 
        }        
    }

    // IPoolObject methods
    public void Reset()
    {
        m_timer = 0.0f;
        m_isReady = false;

    }

    public void Activate()
    {
        gameObject.SetActive(true);
        m_isReady = true;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        m_isReady = false;
    }

    // IPoolObject related methods for reuse

    public void SetBulletLife(BulletLifeControlType inputType, float val)
    {
        if (inputType == BulletLifeControlType.ByTime)
        {
            LifeByTime = val;
        }
        else
        {
            LifeByLengthTravel = val;
        }
    }

    public void SetTransform(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    public void SetSpeed(float speed)
    {
        this.speedTravel = speed;
    }


}

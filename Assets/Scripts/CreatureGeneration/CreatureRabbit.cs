using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureRabbit : MonoBehaviour
{
    // static?
    public float bulletTravelSpeed = 3.0f;
    public float shootInterval = 3.0f;
    public float bulletCircleStartRadius = 1.0f;
    public int bulletCircleNumberOfPoints = 5;
    public BulletLifeControlType bulletLifeControlType;
    public float bulletLifeControl;
    public GameObject prototypeBullet;

    private float m_timerShootInterval = 0.0f;
    private List<StraightBullet> m_pool;
    private int m_poolIter = -1;
    
    void Awake()
    {
        int poolSize = 4 * bulletCircleNumberOfPoints; // an even number required
        m_pool = new List<StraightBullet>();
        for (int i = 0; i < 10; i++)
        {
            GameObject bulletGameObject;
            bulletGameObject = Instantiate(prototypeBullet);
            StraightBullet bulletScript = bulletGameObject.GetComponent<StraightBullet>();
            m_pool.Add(bulletScript);
        }
    }


    // Update is called once per frame
    void Update()
    {
        m_timerShootInterval += Time.deltaTime;
        if (m_timerShootInterval > shootInterval)
        {
            Shoot();
            m_timerShootInterval = 0.0f;    
        }
    }

    void Shoot()
    {
        Vector3 pos = transform.position;
        List<ClanBuilderUtilShape.BulletSpecification> genPoints;
        genPoints = ClanBuilderUtilShape.BuildCircleWithRealativeDirection(bulletCircleStartRadius, bulletCircleNumberOfPoints, pos.x, pos.y, pos.z, ClanBuilderUtilShape.RelativeDirection.Away);
        for (int i = 0; i < genPoints.Count; i++)
        {
            if (m_poolIter < m_pool.Count - 1) {m_poolIter++;} else {m_poolIter = 0;}
            StraightBullet bullet = m_pool[m_poolIter];
            bullet.Reset();
            bullet.SetTransform(genPoints[i].position, genPoints[i].rotation);
            bullet.SetBulletLife(bulletLifeControlType, bulletLifeControl);
            // bullet.transform.position = genPoints[i].position;
            // bullet.transform.rotation = genPoints[i].rotation;
            bullet.SetSpeed(bulletTravelSpeed);
            bullet.Activate();

            // GameObject bulletGameObject = Instantiate(prototypeBullet, genPoints[i].position, genPoints[i].rotation);
            // StraightBullet bulletScript = bulletGameObject.GetComponent<StraightBullet>();
            // bulletScript.SetSpeed(bulletTravelSpeed);
            // // bulletScript.SetIsReady();
        }
    }

    void OnEnable()
    {
        m_timerShootInterval = 0.0f;
    }
}

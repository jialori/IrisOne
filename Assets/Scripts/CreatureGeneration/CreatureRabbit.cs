using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureRabbit : MonoBehaviour
{
    // static?
    public float bulletTravelSpeed = 3.0f;
    public float shootInterval = 3.0f;
    
    private float m_timerShootInterval = 0.0f;

    public GameObject prototypeBullet;

    // Start is called before the first frame update
    void Start()
    {
        
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
        List<ClanBuilderUtilShape.BulletSpecification> genPoints = ClanBuilderUtilShape.BuildCircleWithRealativeDirection(1, 5, pos.x, pos.y, pos.z, ClanBuilderUtilShape.RelativeDirection.Away);
        for (int i = 0; i < genPoints.Count; i++)
        {
            GameObject bulletGameObject = Instantiate(prototypeBullet, genPoints[i].position, genPoints[i].rotation);
            StraightBullet bulletScript = bulletGameObject.GetComponent<StraightBullet>();
            bulletScript.SetSpeed(bulletTravelSpeed);
            bulletScript.SetIsReady();
        }
    }

    void OnEnable()
    {
        m_timerShootInterval = 0.0f;
    }
}

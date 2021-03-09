using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Func


public class Bullet : MonoBehaviour
{

    public float timeToSurvive;
    private float timer;
    Func<float, Vector3> funcMove;
    Func<float, Vector3> m_moverFunc;
    public BulletMover.Type moverType;
    bool isInitialized = false;
    Vector2 origin;

    void Start()
    {
        origin = this.transform.position;

        switch (moverType)
        {
            case BulletMover.Type.Spiral:
                m_moverFunc = BulletMover.SpiralImp;
                break;
            default:
                m_moverFunc = BulletMover.SpiralImp;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (!isInitialized) return;

        timer += Time.deltaTime;
        if (timer > timeToSurvive)
        {
            this.gameObject.SetActive(false);
        }
        // this.transform.position = (Vector3)origin + funcMove(timer);
        this.transform.position = (Vector3)origin + m_moverFunc(timer);
        
    }

    public void Initialize(Func<float, Vector3> funcMove, float time)
    {
        this.timeToSurvive = time;
        this.funcMove = funcMove;
        this.isInitialized = true;
        timer = 0f;
    }
}

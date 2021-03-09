using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Func
using System.Linq; 

public class GenerationPattern
{
    public enum Type
    {
        InFront,
        Spiral,
        Circular,
        Square   
    }

    public class Ticket
    {
        public struct InFront
        {
            float distance;
        } 

        public struct Spiral
        {
            // float ;
            bool isTimeDelayed;
            float timeDelay;
        }
    }

    static public class StrategyImp
    {
        static public Vector3 InFront(Transform charPos, float distance)
        {
            return charPos.position + distance * (Vector3) Vector2.one;
        }

    }
}

public class BulletMover
{
    public enum Type
    {
        Spiral,
        Linear,


    }


    public class Ticket
    {
        public struct InFront
        {
            float distance;
        } 

        public struct Spiral
        {
            // float ;
            bool isTimeDelayed;
            float timeDelay;
        }
    }

    // public Vector3 SpiralImp()
    // {

    // }

    // public class MoverImp
    // {

    static public Vector3 CircularImp(float aRad)
    {
        return new Vector3(Mathf.Sin(aRad), Mathf.Cos(aRad), 0);
    } 

    static public Vector3 SpiralImp(float a)
    {
        return new Vector3(a * Mathf.Sin(a), a * Mathf.Cos(a), 0);
    } 

    // }

}

public struct BulletGeneratorTicket
{
    GenerationPattern.Type genType;
    GenerationPattern.Ticket genTicket;
    BulletMover.Type moveType;
    BulletMover.Ticket moveTicket;
}


public class BulletGenerator : MonoBehaviour
{
    Func<float, Vector3> funcMove;
    private float timer;
    public GameObject prototypeBullet;
    public float interval;
    public GameObject testGenerationLocation;

    void Awake()
    {
        // this.funcMove = Spiral;
        // this.funcMove = Circular();
    }

    void Start()
    {
        timer = 0f;
        
    }

    void Update()
    {
        // timer += Time.deltaTime;
        // if (timer > interval)
        // {
        //     // for (int i = 0; i < 360; i = i + 10)
        //     // {
        //     //     GameObject bullet = Instantiate(prototypeBullet, testGenerationLocation.transform);
        //     //     bullet.GetComponent<Bullet>().Initialize(CircularRadiant(i), 10.0f);
        //     // }
        //     GameObject bullet = Instantiate(prototypeBullet);
        //     bullet.GetComponent<Bullet>().Initialize(Spiral, 10.0f);

        //     timer = 0f;
        // }
        
    }



    // public void GenerateBullet(, )
    // {

    // }




    Func<float, Func<float, Vector3>> CircularRadiant = theta => r => new Vector3(r * Mathf.Sin(theta), r * Mathf.Cos(theta), 0);

    // public Vector3 Circular(float a)
    // {
    //     return new Vector3(Mathf.Sin(a), Mathf.Cos(a), 0);
    // } 

    public Vector3 Spiral(float a)
    {
        return new Vector3(a * Mathf.Sin(a), a * Mathf.Cos(a), 0);
    } 


    public Vector3 SinWave(float a) 
    {
        return new Vector3(a, Mathf.Sin(a), 0);
    }

}

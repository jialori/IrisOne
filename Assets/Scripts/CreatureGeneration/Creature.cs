using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Func

// public interface Creature
public class Creature : MonoBehaviour
{
    // Func<float, List<Vector3>> bulletGenerationPattern;
    Mover mover;
    private int moveSpeedRank; // 0, 1, 2, 3

    float m_timer = 0;

    void Start()
    {
        
    }

    void Update()
    {
        m_timer += Time.deltaTime;
        // this.transform.position += (Vector3)moveDir * moveSpeed;
    
        GenerateBullet();
        mover.Move(transform, m_timer);
    }

    public void GenerateBullet() {}

}

public interface Mover
{
    void Move(Transform transform, float time);
}

public class RabbitMover : Mover
{
    public void Move(Transform transform, float time)
    {
        return ;
    }
// Func<float, Vector3>
}

    // public Vector2 moveDir;
    // private float moveSpeed;
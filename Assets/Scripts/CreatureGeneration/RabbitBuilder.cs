using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class RabbitBuilder : MonoBehaviour, ClanBuilder
public class RabbitBuilder : ClanBuilder
{
    GameObject m_prototypeCreature;


    public Clan Build(Vector2 createPoint)
    {
        return m_prototypeCreature.GetComponent<Clan>();
    }
    public CreatureRabbit Build(Vector2 createPoint, GameObject prototypeCreature)
    {
        GameObject creatureGameObject = Object.Instantiate(prototypeCreature, (Vector3) createPoint, Quaternion.identity);
        CreatureRabbit creatureScript = creatureGameObject.GetComponent<CreatureRabbit>();
        // creatureScript.Initialize(createPoint);
        return creatureScript;
    }

    // public List<Creature> BuildClanCreatures(Vector2 createPoint)
    // {
    //     List<Creature> creatures = new List<Creature>();
    //     List<Vector3> buildPoints = ClanBuilderUtilShape.BuildCircle(5, 10, createPoint.x, createPoint.y);
    //     foreach (Vector2 buildPoint in buildPoints)
    //     {
    //         GameObject creatureGameObject = Instantiate(prototypeCreature, buildPoint, Quaternion.identity);
    //         // creatureGameObject.GetComponent<Creature>().Initialize(CircularRadiant(i), 10.0f);
            
    //     }
    //     return creatures;
    // }
}

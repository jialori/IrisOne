using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitClanBuilder : MonoBehaviour, ClanBuilder
{
    GameObject prototypeCreature;


    public Clan Build(Vector2 createPoint)
    {
        GameObject clanGameObject = Instantiate(prototypeCreature, (Vector3) createPoint, Quaternion.identity);
        Clan clanScript = clanGameObject.GetComponent<Clan>();
        // clanScript.Initialize(createPoint);
        return clanScript;
    }

    public List<Creature> BuildClanCreatures(Vector2 createPoint)
    {
        List<Creature> creatures = new List<Creature>();
        List<Vector3> buildPoints = ClanBuilderUtilShape.BuildCircle(5, 10, createPoint.x, createPoint.y);
        foreach (Vector2 buildPoint in buildPoints)
        {
            GameObject creatureGameObject = Instantiate(prototypeCreature, buildPoint, Quaternion.identity);
            // creatureGameObject.GetComponent<Creature>().Initialize(CircularRadiant(i), 10.0f);
            
        }
        return creatures;

    
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

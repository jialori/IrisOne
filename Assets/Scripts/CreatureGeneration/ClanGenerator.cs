using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClanGenerator : MonoBehaviour
{
    public GameObject prototypeClan;
    // private ClanBuilder m_clanBuilder = new RabbitClanBuilder(); // extended to a list in the future

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Initialize(World world)
    {

    }

    public List<Clan> GenerateClans(World world, Road road, int numClans)
    {

        HashSet<int> genPointIdxs = new HashSet<int>();
        for (int i = 0; i < numClans; i++)
        {
            int genPointIdx;
            do
            {
                genPointIdx = Random.Range(0, road.PointsCount - 1);
            } 
            while (genPointIdxs.Contains(genPointIdx));
            genPointIdxs.Add(genPointIdx);
        }

        List<Clan> clans = new List<Clan>();
        foreach (int genPointIdx in genPointIdxs)
        {
            Vector2 genPoint = road.Points[genPointIdx];
            // Clan clan = m_clanBuilder.Build(genPoint);
            // Clan clan = CreateSimple(genPoint);
        }

        return clans;

    }


    public Clan CreateSimple(Vector2 createPoint)
    {
        GameObject clanGameObject = Instantiate(prototypeClan, (Vector3) createPoint, Quaternion.identity);
        Clan clanScript = clanGameObject.GetComponent<Clan>();
        // clanScript.Initialize(createPoint);
        return clanScript;
    }

    // public Clan CreateWithPattern(Vector2 createPoint, GenerationPattern.Type)
    // {}


}


enum ClanSize
{
    Small, Medium, Big
}

public interface ClanBuilder
{
    Clan Build(Vector2 createPoint);
    List<Creature> BuildClanCreatures(Vector2 createPoint);
    // List<Creature> BuildClanEnvironment(World world, Road road, int size);
}



public static class ClanBuilderUtilShape
{
    enum Shape
    {
        Circle,
        Spiral
    }

    public struct BulletSpecification
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    static public List<Vector3> BuildCircle(float radius, int numPoints, float offsetX=0.0f, float offsetY=0.0f, float offsetZ=0.0f)
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);
        float angleRadOffset = 360.0f / numPoints * Mathf.Deg2Rad;;
        for (int i = 0; i < numPoints; i++)
        {
            float angleRad = i * angleRadOffset;
            Vector3 point = offset + radius * new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);
            points.Add(point);
        }
        return points;
    }

    public enum RelativeDirection
    {
        Towards, Away
    }
    static public List<BulletSpecification> BuildCircleWithRealativeDirection(float radius, int numPoints, float offsetX, float offsetY, float offsetZ, RelativeDirection relDirType)
    {
        List<BulletSpecification> bsList = new List<BulletSpecification>();
        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);
        float angleRadOffset = 360.0f / numPoints * Mathf.Deg2Rad;;
        for (int i = 0; i < numPoints; i++)
        {
            BulletSpecification bs;
            float angleRad = i * angleRadOffset;
            Vector3 point = offset + radius * new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            bs.position = point;
            bs.rotation = Quaternion.Euler(0, 0, angleRad * Mathf.Rad2Deg);
            bsList.Add(bs);
        }
        return bsList;
    }



}


public static class ClanBuilderUtilMover
{
    static public void BuildMover()
    {

    }

}

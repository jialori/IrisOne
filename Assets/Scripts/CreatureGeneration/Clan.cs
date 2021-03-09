using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Func

public class Clan
{
    Func<float, List<Vector3>> creatureGenerationPattern;
    // Func<float, Vector3> creatureMover;
    public GenerationPattern.Type genType;

    private List<Creature> m_creatures;


    bool isMoveTogether;
    bool isMoveIndividually;


    public Clan()
    {

    }
    public Clan (List<Creature> creatures)
    {
        m_creatures = creatures;
    }

}

public class RabbitClan : Clan
{

}

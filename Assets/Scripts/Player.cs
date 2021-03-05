using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static System.Mathf;

using Util; // using static Vector2Ext.Vector2Ext;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.005f;
    bool isAlive = true;

    private Road m_road;
    private int m_navBlockIndexer; // between 0 and m_road.Count - 2

    // private Vector2 m_roadNavPoint; // 
    // private bool m_roadNavPointIsVertex;
    // Vector2 CurRoadPoint
    // {
    //     get {return m_road.Points[m_navBlockIndexer];}
    // }

    void Start()
    {
        
    }

    void Update()
    {
        if (isAlive)
        {
            // Movement
            Vector2 move = moveSpeed * (Input.GetAxis("Vertical") * Vector2.up + Input.GetAxis("Horizontal") * Vector2.right);
            if (move != Vector2.zero)
            {
                this.transform.position = (Vector3)TryMove(move, this.transform.position);
            }

        }
    }

    public void Initialize(Vector2 startPoint, Road road)
    {
        transform.position = startPoint;
        m_road = road;
        m_navBlockIndexer = 0;
        // m_roadNavPoint = m_road.StartPoint;
        // m_roadNavPointIsVertex = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("monster"))
        {
            isAlive = false;
        }
    }

    // Returns the result position
    // RISK m_road.Width
    private Vector2 TryMove(Vector2 move, Vector2 curPosition, bool isPotentialDeadCorner=false)
    {
        if (move == Vector2.zero)
        {
            return Vector2.zero;
        }

        // Debug.Log(move);
        Vector2 resultMove;
        Vector2 residualMove = Vector2.zero;

        float roadRadius = m_road.Width / 2;
        float RoadRadiusSqr = Mathf.Pow(roadRadius, 2);
        NavBlock curNavBlock = m_road.GetNavBlock(m_navBlockIndexer);
        Vector2 curNavBlockPivoted = curNavBlock.endPoint - curNavBlock.startPoint;
        Vector2 curPositionPivoted = curPosition - curNavBlock.startPoint;

        Vector2 curProjU, curProjV;
        Vector2Ext.Project(curPositionPivoted, curNavBlockPivoted, out curProjU, out curProjV);
        Vector2 moveProjU, moveProjV;
        Vector2Ext.Project(move, curNavBlockPivoted, out moveProjU, out moveProjV);

        float A = moveProjU.sqrMagnitude;
        float B = curProjU.sqrMagnitude;
        float C = (moveProjU + curProjU).sqrMagnitude;
        float D = curNavBlockPivoted.sqrMagnitude;
        float E = (moveProjU + curNavBlockPivoted).sqrMagnitude;
        if (A == 0)
        {
        // moving only in the V axis
            resultMove = Vector2.ClampMagnitude(curProjV + moveProjV, roadRadius) - curProjV;
        }        
        else if (E < D)
        {
        // moving towards the previous block
            if (A <= B)
            {
            // stay on the same nav block
                resultMove = moveProjU + Vector2.ClampMagnitude(curProjV + moveProjV, roadRadius) - curProjV;
            }
            else //contains the B==0 case
            {
            // trying to move from the current navBlock into the previous navBlock
                float a = moveProjU.magnitude;
                float b = curProjU.magnitude;
                Vector2 clampedMoveProjU = -curProjU;
                Vector2 clampedMoveProjV = (b / a) * moveProjV;
                resultMove = clampedMoveProjU +
                            (Vector2.ClampMagnitude(curProjV + clampedMoveProjV, roadRadius) - curProjV);

                Debug.Log(resultMove.sqrMagnitude);
                Debug.Log(!Vector2Ext.IsCloseToZero(resultMove));

                if (m_navBlockIndexer > 0 && 
                    ( (!isPotentialDeadCorner) || (!Vector2Ext.IsCloseToZero(resultMove)) ) // prevents infinite loop on dead corner
                    )
                {
                    residualMove = (moveProjU - clampedMoveProjU) 
                                    + (moveProjV - clampedMoveProjV);
                    // residualMove = (curProjU + moveProjU) + ((a - b) / a) * moveProjV;
                    m_navBlockIndexer -= 1;
                }                
            }
        }
        else
        {
        // moving towards the next block
            if (C <= D)
            {
            // stay on the same nav block
                resultMove = moveProjU + Vector2.ClampMagnitude(curProjV + moveProjV, roadRadius) - curProjV;
            }
            else    //contains the B==D case
            {
            // trying to move from the current navBlock into the next navBlock
                float a = moveProjU.magnitude;
                float b = curProjU.magnitude;
                float d = curNavBlockPivoted.magnitude;
                Vector2 clampedMoveProjU = curNavBlockPivoted - curProjU;
                Vector2 clampedMoveProjV = ((d - b) / a) * moveProjV;
                resultMove = clampedMoveProjU +
                            Vector2.ClampMagnitude(curProjV + clampedMoveProjV, roadRadius) - curProjV;

                Debug.Log(resultMove.sqrMagnitude);
                Debug.Log(!Vector2Ext.IsCloseToZero(resultMove));

                if (m_navBlockIndexer < m_road.Points.Count - 2 && 
                    ( (!isPotentialDeadCorner) || (!Vector2Ext.IsCloseToZero(resultMove)) ) // prevents infinite loop on dead corner
                    )
                {
                    residualMove = (moveProjU - clampedMoveProjU) 
                                    + (moveProjV - clampedMoveProjV);
                                    // + ((a - d + b) / a) * moveProjV;
                    m_navBlockIndexer += 1;
                }                                
            }

        }
        Debug.Log(m_navBlockIndexer);
        
        if (residualMove != Vector2.zero)
        {
        // Debug.Log("hi");
        Debug.Log(resultMove);
            return TryMove(residualMove, curPosition + resultMove, Vector2Ext.IsCloseToZero(resultMove));
        }
        else
        {
            return curPosition + resultMove;
        }


    }

}


        // // Vector2 nextPosition = this.transform.position + (Vector3)move;
        // Vector2 GetRoadPoint(int index) {return m_road.Points[index];}
        // Vector2 CurRoadPoint() {return m_road.Points[m_navBlockIndexer];}
        // bool HasPrevRoadPoint() {return m_navBlockIndexer > 0;}
        // Vector2 PrevRoadPoint() {return m_road.Points[m_navBlockIndexer - 1];}
        // bool HasNxtRoadPoint() {return m_navBlockIndexer < m_road.Points.Count - 1;}
        // Vector2 NxtRoadPoint() {return m_road.Points[m_navBlockIndexer + 1];}
        // bool CanFindNavPointInNxt(out Vector2 NavPoint)
        // {
        //     NavPoint = Vector2Ext.NegativeInfinity;
        //     return false;
        // }




        // if (m_roadNavPointIsVertex)
        // {
        //     if ((nextPosition - m_roadNavPoint).sqrMagnitude < RoadRadiusSqr)
        //     {
        //         isValidMove = true;
        //     }
        //     else
        //     {
        //         Vector2 proj, projOppo;
        //         Vector2 nextPositionPivoted = nextPosition - GetRoadPoint(m_navBlockIndexer);

        //         Vector2 curRoadSpine = GetRoadPoint(m_navBlockIndexer + 1) - GetRoadPoint(m_navBlockIndexer);
        //         Vector2Ext.Project(nextPositionPivoted, curRoadSpine, out proj, out projOppo);
        //         if (proj.sqrMagnitude < curRoadSpine.sqrMagnitude
        //             && proj.sqrMagnitude < (proj + curRoadSpine).sqrMagnitude
        //             )
        //         {
        //         // Nav point is in the current road block

        //             // projOppo.sqrMagnitude < Pow(m_road.Width / 2, 2)
        //         }
        //         else
        //         {
        //             if (Vector2.Dot(move, curRoadSpine) >= 0)
        //             {

        //             }

        //             float 
        //             if (HasPrevRoadPoint && HasNxtRoadPoint)
        //             {
        //                 Vector2 prevRoadSpine = GetRoadPoint(m_navBlockIndexer) - GetRoadPoint(m_navBlockIndexer - 1);
        //                 Vector2Ext.Project(nextPositionPivoted, prevRoadSpine, out proj, out projOppo);
        //                 float sqrPrevDistance

        //             }
        //             else if (HasPrevRoadPoint)
        //             {
        //                 if (Vector2.Dot(move, prevRoadSpine) >= 0)
        //                 {

        //                 }


        //                 Vector2 prevRoadSpine = GetRoadPoint(m_navBlockIndexer) - GetRoadPoint(m_navBlockIndexer - 1);
        //                 Vector2Ext.Project(nextPositionPivoted, prevRoadSpine, out proj, out projOppo);
        //                 if (projOppo.sqrMagnitude < RoadRadiusSqr
        //                     && proj.sqrMagnitude < curRoadSpine.sqrMagnitude
        //                     && proj.sqrMagnitude < (proj + curRoadSpine).sqrMagnitude
        //                     )
        //                 {
        //                 // Nav point is can be in the prev road block
        //                 }
                    
        //             }
        //             else if (HasNxtRoadPoint)
        //             {

        //             }
                    
        //         }
        //     }
        // }
        // else
        // {
        //     if ((nextPosition - m_roadNavPoint).sqrMagnitude < RoadRadiusSqr)
        //     {
        //         m_roadNavPointIsVertex = true;
        //         isValidMove = true;
        //     }
            
        // }

        // bool CanFindNavPointInPrev(out Vector2 NavPoint)
        // {
        //     NavPoint = Vector2Ext.NegativeInfinity;
        //     found = false;

        //     if (HasPrevRoadPoint())
        //     {
        //         if ((nextPosition - PrevRoadPoint()).sqrMagnitude < RoadRadiusSqr)
        //         {
        //             m_navBlockIndexer -= 1;
        //             m_roadNavPointIsVertex = true;
        //             found = true;
        //         }
        //         else
        //         {
        //             // Vector2 nextPositionPivoted = nextPosition - GetRoadPoint(m_navBlockIndexer);
        //             // Vector2 curRoadSpine = GetRoadPoint(m_navBlockIndexer + 1) - GetRoadPoint(m_navBlockIndexer);
        //             // Vector2 proj = Vector2Ext.VectorProjectionAdjacent(nextPositionPivoted, curRoadSpine);
        //             // Vector2 projOppo = nextPositionPivoted - proj;

        //             // if 
        //         }
        //     }
        //     return found;
        // }

        //     if (projOppo.sqrMagnitude < Pow(m_road.Width / 2, 2)
        //         && proj.sqrMagnitude < curRoadSpine.sqrMagnitude
        //         && proj.sqrMagnitude < (proj + curRoadSpine).sqrMagnitude
        //         )
        //     {
        //         // Naively within the current road block
        //         return true;






        // bool IsInRoadBlock(Vector2 point)
        // {
        //     Vector2 nextPositionPivoted = point - GetRoadPoint(m_navBlockIndexer);
        //     Vector2 curRoadSpine = GetRoadPoint(m_navBlockIndexer + 1) - GetRoadPoint(m_navBlockIndexer);
        //     Vector2 proj = Vector2Ext.VectorProjectionAdjacent(nextPositionPivoted, curRoadSpine);
        //     Vector2 projOppo = nextPositionPivoted - proj;


        //     if (projOppo.sqrMagnitude < Pow(m_road.Width / 2, 2)
        //         && proj.sqrMagnitude < curRoadSpine.sqrMagnitude
        //         && proj.sqrMagnitude < (proj + curRoadSpine).sqrMagnitude
        //         )
        //     {
        //         // Naively within the current road block
        //         return true;
        //     }
        //     else
        //     {
        //         if (projOppo.sqrMagnitude >= Pow(m_road.Width / 2, 2) && m_navBlockIndexer != 0)
        //         {
        //             return true;
        //         }
        //         else
        //         {
        //             if (projOppo.sqrMagnitude < Pow(m_road.Width / 2, 2))
        //             {
        //             // case 1. in the connection area to the previous block; no change of indexer
                        
        //             }
        //             if (proj.sqrMagnitude >= (proj + curRoadSpine).sqrMagnitude)

        //             else if ()
        //             {
        //             // case 2. in the previous block; change of indexer
        //                 if (m_navBlockIndexer > 0)
        //                 {
        //                     m_navBlockIndexer -= 1;
        //                     return IsInRoadBlock(point, numRecursion - 1);
        //                 }
        //                 else
        //                 {
        //                     return false;
        //                 }
        //             }
        //             else
        //             {
        //             // case 3. in the next block; change of indexer

                        
        //             }




        //         }
        //     }

        //     // if (proj.normalized - curRoadSpine.normalized == Vector2.zero)
        //     // {
        //     //     // going into the "previous" road block, as the vector projection is in the opposite direction
        //     // }
        //     // else
        //     // {
        //     //     if (proj.sqrMagnitude > curRoadSpine.sqrMagnitude)
        //     //     {
        //     //         if (projOppo.sqrMagnitude < Pow(m_road.Width, 2))
        //     //         {
        //     //             // In the road but road point assignment is unclear

        //     //         }
        //     //         else
        //     //         {
        //     //             // going into the "next" road block
                        
        //     //         }

        //     //         return true;
        //     //     }
        //     //     else
        //     //     {
                    
        //     //     }
                
        //     // }
        //     return false;
            
        // }

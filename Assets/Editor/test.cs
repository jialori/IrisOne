//  using UnityEngine;
//  using System.Collections;
//  using UnityEditor;
 
//  [CustomEditor(typeof(StraightBullet))]
//  public class test : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         StraightBullet bulletScript = (StraightBullet)target;
//         // bulletScript.lifeControlType = (BulletLifeControlType)EditorGUILayout.EnumPopup("My lifeControlType", bulletScript.lifeControlType);
//         if (bulletScript.lifeControlType == BulletLifeControlType.ByTime)
//         {
//             bulletScript.lifeByTime = EditorGUILayout.FloatField("Life By Time", bulletScript.lifeByTime);
//         }
//          else if (bulletScript.lifeControlType == BulletLifeControlType.ByLength)
//          {
//             bulletScript.lifeByLengthTravel = EditorGUILayout.FloatField("Life By Length Travel", bulletScript.lifeByLengthTravel);
//          }
//     }
// }
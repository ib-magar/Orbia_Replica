using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class checkPointData{
    public alignType _alignType=alignType.together;             //no need to fill
    public Vector2 _enemycount1,_enemycount2;               // enmies count for 1st and 2nd circle
    public Vector2 _radius1, _radius2;                      //radius of the first and second circle
    public Vector2 _rotationSpeed1,_rotationSpeed2;         // rotation speed for 1st and 2nd circle
    public bool _rotationSpeedType=true, _rotationSpeedType2=true;  //true means clockwise and false means anticlockwise
    public float _childCircleRadius=.3f;                    // the radius of the white circle of the checkpoint/minicheckpoint
    public int _circleCount=1;                              // the number of enemies radius
    public float _angleMeasurement1=20,_angleMeasurement2=15f;  // no need to fill
    public int _miniCheckPointCount = 5;                    // the number of miniCheckpoint counts
    public float _miniCheckPointDistance;                   //minicheckPoint distance
    public bool _hasAbility;                                //_if has ability
    public int _abilityType;                                // 0 means shield  
                                                            // 1 means coins
                                                            // 2 means treasure of coins
                                                            // 3 means chilli
                                                            // 4 means speedup
                                                            // 
}
public class levelManagement : MonoBehaviour
{
    public checkPointData[] _CheckPoints;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class checkPointData{
    public alignType _alignType=alignType.together;
    public Vector2 _enemycount1,_enemycount2;
    public float _radius11=1.2f, _radius2=1.8f;
    public Vector2 _rotationSpeed1,_rotationSpeed2;
    public bool _rotationSpeedType=true, _rotationSpeedType2=true;
    public float _childCircleRadius=.3f;
    public int _circleCount=1;
    public float _angleMeasurement1=20,_angleMeasurement2=15f;
    public int _miniCheckPointCount = 5;
    public float _miniCheckPointDistance;
    public checkPointData[] miniCheckPoints;
    checkPointData()
    {
        miniCheckPoints = new checkPointData[_miniCheckPointCount];
    }
}
public class levelManagement : MonoBehaviour
{
    public checkPointData[] _CheckPoints;

}

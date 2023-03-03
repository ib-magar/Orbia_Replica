using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int _Score;
    [Header("bools")]
    public bool _playerdead;

    [Header("checkPoints Management")]
    [SerializeField] Vector2 _checkPointDistance;
    [SerializeField] Vector2 _angleClamp;
    [SerializeField] Transform _gameobject1;
    [SerializeField] Transform _gameobject2;
    [SerializeField] Transform _angleGameobject;
    [SerializeField] Vector3 _gameobject1Direction;
    [SerializeField] Vector3 _gameobject2Direction;

    private Transform _checkPointHolderTransform;

    [Header("Checkpoint Gameobject")]
    [SerializeField] GameObject _checkPointGameObject;
    [SerializeField] GameObject _checkPointDummyObject;
   

    [Header("CheckPoint List")]
    [SerializeField] int _checkPointCount;
    [SerializeField] List<GameObject> _checkPointList = new List<GameObject>();

    [Header("external scripts")]
    [SerializeField] CharacterController _player;
    [SerializeField] Transform _levelManager;

    [Header("Enemy variation")]
    [SerializeField] List<GameObject> _enemies=new List<GameObject>();

    [Header("Mini Checkpoints")]
    [SerializeField] List<GameObject> _miniCheckPoints = new List<GameObject>();
    [SerializeField] public int _miniCheckPointCount;
    private bool _firstInstantiationCheckPoint = true;
    [Header("Abilities")]
    [SerializeField] GameObject[] _abilities;
    public GameObject shiel;
    [Header("Level Data Track")]
    public int _levelCount;
    public Transform getLastCheckPoint()
    {
        return _checkPointList[0].transform;
    }
    public void GetMiniCheckPointList(List<GameObject> _list)
    {
        _miniCheckPoints = _list;
        _miniCheckPointCount = 0;
    }
    private void Awake()
    {
        _firstInstantiationCheckPoint = true;

        _player=GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        _levelManager=GameObject.FindGameObjectWithTag("levelManager").transform;
        _checkPointHolderTransform = transform.Find("CheckPointHolder").transform;
       
        _checkPointList.Add(Instantiate(new GameObject("object"),Vector3.zero-Vector3.down*4f,Quaternion.identity,_checkPointHolderTransform));
        _checkPointList.Add(Instantiate(_checkPointDummyObject, Vector3.zero, Quaternion.identity, _checkPointHolderTransform));
    }
    private void Start()
    {
        GenerateUpcomingCheckpoints();
        //_player.Move(_checkPointList[0]);
        MoveCharacter();
        _Score = 1;
        
    }
    [ContextMenu("tools/removeFirstNode")]
    public void removeFirstPoint()
    {
        _checkPointList.RemoveAt(0);
    }
    void GenerateUpcomingCheckpoints()
    {
        if (_checkPointList.Count == 0 || _checkPointList == null)
        {
            Debug.LogError("_checkpoint list empty");
            return;
        }
        int _limit = _checkPointCount + 1;
        if (_firstInstantiationCheckPoint)
        {
            _limit = _checkPointCount + 1 + 1;
            _firstInstantiationCheckPoint = false;
        }
        else
        {
            _limit = _checkPointCount + 1;
        }
        while (_checkPointList.Count < _limit)
        {
            if (_miniCheckPoints.Count > 0)
                // 
                _checkPointList.Add(GenerateCheckPoint(_miniCheckPoints[_miniCheckPoints.Count - 1].transform));
            else
                _checkPointList.Add(GenerateCheckPoint(_checkPointList[_checkPointList.Count - 1].transform));
        }
    }
    GameObject GenerateCheckPoint(Transform _previousCheckpoint)
    {   
        _angleGameobject.eulerAngles = new Vector3(0, 0, Random.Range(_angleClamp.x, _angleClamp.y));
        Vector3 spawnPosition = _previousCheckpoint.position + _angleGameobject.up * Random.Range(_checkPointDistance.x,_checkPointDistance.y);
        //Vector3 spawnPosition = _previousCheckpoint.position +Vector3.up * Random.Range(_checkPointDistance.x,_checkPointDistance.y);
        
       GameObject g = Instantiate(_checkPointGameObject, spawnPosition,Quaternion.Euler(0,0,Random.Range(0,360f)),_checkPointHolderTransform);
       checkPointScript c = g.GetComponent<checkPointScript>();
        c._checkPointScore.text = (++_Score).ToString();

        _levelCount %= _levelManager.childCount;

        // do the level Data Implementation
        c.Init(_levelManager.GetChild(_levelCount++).GetComponent<levelManagement>()._CheckPoints[0]);
        //c.Init(alignType.together, __enemiesCount, __radius, __rotationspeed);
        return g;
    }
    public void CheckAndRemoveCheckPoint(GameObject g)
    {
        if(g.TryGetComponent<checkPointScript>(out checkPointScript c))
        {
            //_lastCheckPoint = c;
            c.DisableEnemies();
            c.GetComponent<CircleCollider2D>().enabled = false;
            GetMiniCheckPointList(c._miniCheckPointList);
        }
        //if (_checkPointList.Contains(g))
        //_checkPointList.Remove(g);
        _checkPointList.RemoveAt(0);
        //make the next checkpoint to rotate
        if(_miniCheckPoints.Count>0)
            _miniCheckPoints[0].GetComponent<checkPointScript>()._canRotate = true;
        else
        _checkPointList[1].GetComponent<checkPointScript>()._canRotate = true;

        if(_checkPointHolderTransform.childCount>_checkPointCount)
        {
            Destroy(_checkPointHolderTransform.GetChild(0).gameObject);
        }

        GenerateUpcomingCheckpoints();
    }
    public void CheckAndRemoveMiniCheckPoint(GameObject g)
    {
            _miniCheckPointCount++;
        if (g.TryGetComponent<checkPointScript>(out checkPointScript c))
        {
            //_lastCheckPoint = c._miniCheckPointList[_miniCheckPointCount - 1].GetComponent<checkPointScript>();
            c.DisableEnemies();
        }
        if (_miniCheckPointCount<_miniCheckPoints.Count)
        {
            _miniCheckPoints[_miniCheckPointCount].GetComponent<checkPointScript>()._canRotate = true;
        }
        else
        {
            GenerateUpcomingCheckpoints();
            _checkPointList[1].GetComponent<checkPointScript>()._canRotate= true;
        }
    }
    public void MoveCharacter()
    {
       // if (_checkPointList != null || _checkPointList.Count < 1)               // can create function to check the nullity of the list 
        //    return;

        if(_playerdead)
        {
        _player.Move(_checkPointList[0]);
            _playerdead = false;
        }
        else
        {
            if(_miniCheckPointCount<_miniCheckPoints.Count && _miniCheckPoints.Count>0)
            {
                
                    _player.Move(_miniCheckPoints[_miniCheckPointCount]);
               // _miniCheckPointCount++;
            }
            else
            {
               // if (_miniCheckPointCount == _miniCheckPoints.Count - 1)
               //     _player.Move(_miniCheckPoints[_miniCheckPoints.Count - 1]);
              //  else
                    _player.Move(_checkPointList[1]);
            }

        }

    }
    public void ActivateLastCheckPoint()
    {
        checkPointScript _lastCheckPoint = _checkPointList[0].GetComponent<checkPointScript>();
       // if(_lastCheckPoint._miniCheckPointList.Count>0 && _lastCheckPoint._hasMiniCheckPoint)
        //{
            int _childCount = _lastCheckPoint._miniCheckPointList.Count;
            for(int i=0;i<_childCount;i++)
            {
            _lastCheckPoint._miniCheckPointList[i].GetComponent<checkPointScript>().ResetEnemies();
            }
        //}
    }
    public void GetBackToLastCheckPoint()
    {
        checkPointScript _lastCheckPoint = _checkPointList[0].GetComponent<checkPointScript>();
        if (_miniCheckPointCount <= 1)
            _player.Move(_checkPointList[0].gameObject);
        else
        {
            _miniCheckPointCount--;
           _player.Move(_lastCheckPoint._miniCheckPointList[_miniCheckPointCount].gameObject);
           // StartCoroutine(changeValue());
        }
    }
    IEnumerator changeValue()
    {
        yield return new WaitForSeconds(.4f);
        --_miniCheckPointCount;
    }
    private void Update()
    {
        /*_gameobject1.eulerAngles = new Vector3(_gameobject1.eulerAngles.x, _gameobject1.eulerAngles.y, _angleClamp.x);
        _gameobject2.eulerAngles = new Vector3(_gameobject2.eulerAngles.x, _gameobject2.eulerAngles.y, _angleClamp.y);

        _gameobject1Direction = _gameobject1.forward;
        _gameobject2Direction = _gameobject2.forward;*/
        if(Input.GetKeyDown(KeyCode.Space))
            GetBackToLastCheckPoint();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        /*Gizmos.DrawLine(_gameobject1.position, _gameobject1.position + _gameobject1.up * 5f);
        Gizmos.DrawLine(_gameobject2.position, _gameobject2.position + _gameobject2.up * 5f);
*/
        //Gizmos.DrawLine(_angleGameobject.position, _angleGameobject.position + _angleGameobject.up * 5f);
    }
}

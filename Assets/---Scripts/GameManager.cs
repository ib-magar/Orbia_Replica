using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    [Header("Enemy variation")]
    [SerializeField] List<GameObject> _enemies=new List<GameObject>();
    private void Awake()
    {
        _player=GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        _checkPointHolderTransform = transform.Find("CheckPointHolder").transform;
       
        _checkPointList.Add(Instantiate(new GameObject("object"),Vector3.zero-Vector3.down*4f,Quaternion.identity,_checkPointHolderTransform));
        _checkPointList.Add(Instantiate(_checkPointDummyObject, Vector3.zero, Quaternion.identity, _checkPointHolderTransform));
    }
    private void Start()
    {
        GenerateUpcomingCheckpoints();
        //_player.Move(_checkPointList[0]);
        MoveCharacter();
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
        while (_checkPointList.Count < _checkPointCount)
        {
            // 
            _checkPointList.Add(GenerateCheckPoint(_checkPointList[_checkPointList.Count - 1].transform));
        }
    }
    GameObject GenerateCheckPoint(Transform _previousCheckpoint)
    {
        _angleGameobject.eulerAngles = new Vector3(0, 0, Random.Range(_angleClamp.x, _angleClamp.y));
        Vector3 spawnPosition = _previousCheckpoint.position + _angleGameobject.up * Random.Range(_checkPointDistance.x,_checkPointDistance.y);
        //Vector3 spawnPosition = _previousCheckpoint.position +Vector3.up * Random.Range(_checkPointDistance.x,_checkPointDistance.y);
        
        return (Instantiate(_checkPointGameObject, spawnPosition, Quaternion.identity,_checkPointHolderTransform));
    }
    public void CheckAndRemoveNode(GameObject g)
    {
        if(g.TryGetComponent<checkPointScript>(out checkPointScript c))
        {
            c.DisableEnemies();
            c.GetComponent<CircleCollider2D>().enabled = false;
        }

        //if (_checkPointList.Contains(g))
        //_checkPointList.Remove(g);
        _checkPointList.RemoveAt(0);

        //make the next checkpoint to rotate
        _checkPointList[1].GetComponent<checkPointScript>()._canRotate = true;

        if(_checkPointHolderTransform.childCount>_checkPointCount+1)
        {
            Destroy(_checkPointHolderTransform.GetChild(0).gameObject);
        }

        GenerateUpcomingCheckpoints();
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
        _player.Move(_checkPointList[1]);
        }

    }
    private void Update()
    {
        /*_gameobject1.eulerAngles = new Vector3(_gameobject1.eulerAngles.x, _gameobject1.eulerAngles.y, _angleClamp.x);
        _gameobject2.eulerAngles = new Vector3(_gameobject2.eulerAngles.x, _gameobject2.eulerAngles.y, _angleClamp.y);

        _gameobject1Direction = _gameobject1.forward;
        _gameobject2Direction = _gameobject2.forward;*/
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_gameobject1.position, _gameobject1.position + _gameobject1.up * 5f);
        Gizmos.DrawLine(_gameobject2.position, _gameobject2.position + _gameobject2.up * 5f);

        Gizmos.DrawLine(_angleGameobject.position, _angleGameobject.position + _angleGameobject.up * 5f);
    }
}

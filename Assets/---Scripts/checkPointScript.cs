using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.Sockets;

public enum alignType
{
    measured,together,random
}
public class checkPointScript : MonoBehaviour
{
    public bool _hasMiniCheckPoint;
    [SerializeField] public alignType _alignType;
    [SerializeField] public int _enemiesCount=3;
    [SerializeField] public int _enemiesCount2=3;
    [SerializeField] public float _radius;
    [SerializeField] public float _radius2;
    [Header("rotation")]
    [SerializeField] public bool _canRotate=false;
    [SerializeField] public float _rotationSpeed;

    [Header("Child")]
    [SerializeField] List<Transform> _enemieslist = new List<Transform>();
    [SerializeField] public float _childCircleRadius;
    [SerializeField] SpriteRenderer _circle;
    [SerializeField] GameObject _lineRendererObject;

    [Header("instantiation management")]
    [SerializeField] int _circleCount;
    [SerializeField] Transform _rotating_point;
    [SerializeField] Transform _EnemiesHolder;
    [SerializeField] Transform _EnemiesHolder2;
    [SerializeField] bool _rotationType1;
    [SerializeField] bool _rotationType2;
    [SerializeField] LayerMask _enemesLayer;

    [Header("Enemies")]
    [SerializeField] List<GameObject> _enemies=new List<GameObject>();
    [Header("angle measurement")]
    [SerializeField] float _angleMeasurement1;
    [SerializeField] float _angleMeasurement2;
    [Header("enemies disable")]
    [SerializeField] float _timeToFade;
    [SerializeField] float _scaleTarget;

    [Header("Text")]
    [SerializeField] public TMP_Text _checkPointScore;

    [Header("Child enemies")]
    [SerializeField] GameObject _miniCheckPointGameObect;
    [SerializeField] int _miniCheckPointCount;
    [SerializeField] float _miniCheckPointDistance;
    [SerializeField] Transform _miniCheckPointHolder;
    [SerializeField] public List<GameObject> _miniCheckPointList = new List<GameObject>();

    [Header("External Scripts")]
    private GameManager _gameManager;
    private Transform _levelManager;
    [Header("abilities")]
    [SerializeField] GameObject[] _abilities;
    private void Awake()
    {
     _gameManager=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
     _levelManager=GameObject.FindGameObjectWithTag("levelManager").transform;
    }
    public void Init(checkPointData _checkPointData)
    {
        _alignType = alignType.together;
        _circleCount = _checkPointData._circleCount;
        _enemiesCount =(int) Random.Range(_checkPointData._enemycount1.x, _checkPointData._enemycount1.y);
        _enemiesCount2=(int)Random.Range(_checkPointData._enemycount2.x, _checkPointData._enemycount2.y);
        _radius =Random.Range(_checkPointData._radius1.x, _checkPointData._radius1.y);
        _radius2 =Random.Range(_checkPointData._radius2.x, _checkPointData._radius2.y);
        _rotationType1 = _checkPointData._rotationSpeedType;
        _rotationType2 = _checkPointData._rotationSpeedType2;
        _childCircleRadius = _checkPointData._childCircleRadius;
<<<<<<< HEAD
        //_angleMeasurement1 = /*_checkPointData._angleMeasurement1;*///20f;
       // _angleMeasurement2=_checkPointData._angleMeasurement2;
=======
        _angleMeasurement1 = /*_checkPointData._angleMeasurement1;*/20f;
        _angleMeasurement2=_checkPointData._angleMeasurement2;
>>>>>>> main
        if (_hasMiniCheckPoint)
            _miniCheckPointCount = _checkPointData._miniCheckPointCount;
        _miniCheckPointDistance= _checkPointData._miniCheckPointDistance;
       // if (_checkPointData._hasAbility)
            //Instantiate(_abilities[_checkPointData._abilityType], transform.position, Quaternion.identity);      
    }
    IEnumerator  Start()
    {
        Physics2D.queriesStartInColliders = false;
        yield return new WaitForSeconds(.5f);
        _rotating_point.eulerAngles = Vector3.zero;
        switch (_alignType)
        {
            case alignType.measured:
                InsantiateEnemiesMeasureType();
                break;
            case alignType.together:
                InsantiateEnemiesTogetherType();
                break;
            case alignType.random:
                InsantiateEnemiesRandomType();
                break;
            default: break;
        }

        if(_hasMiniCheckPoint)
        {
            GenerateMiniCheckPoint();
        }

    }
    void GenerateMiniCheckPoint()
    {
        float _distance = _radius + _miniCheckPointDistance;
            Transform _rotateObject;
        for (int i = 0; i < _miniCheckPointCount; i++)
        {
            if (i == 0) _rotateObject = _rotating_point;
            else _rotateObject = _miniCheckPointList[i - 1].GetComponent<checkPointScript>()._rotating_point;

            float _angleOffset = Random.Range(-30f, 30f);
            _rotateObject.eulerAngles = Vector3.forward * _angleOffset;
            Vector3 _spawnPosition = _rotateObject.position + _rotateObject.transform.up * _distance;
            GameObject g = Instantiate(_miniCheckPointGameObect, _spawnPosition, Quaternion.Euler(0, 0, Random.Range(0, 360f)), _miniCheckPointHolder);
            g.GetComponent<checkPointScript>().Init(_levelManager.GetChild(_gameManager._levelCount - 1).GetChild(0).GetComponent<levelManagement>()._CheckPoints[i]);
            _miniCheckPointList.Add(g);
<<<<<<< HEAD
            if(i>0 && i<_miniCheckPointCount-1)
            {
                Vector3 dir = (_miniCheckPointList[i].transform.position - _miniCheckPointList[i-1].transform.position).normalized;
                GameObject line = Instantiate(_lineRendererObject, transform.position, Quaternion.identity,transform);
                LineRenderer _lineRenderer= line.GetComponent<LineRenderer>();
                _lineRenderer.SetPosition(0, _miniCheckPointList[i - 1].transform.position+dir*(_childCircleRadius+.25f));
                _lineRenderer.SetPosition(1, _miniCheckPointList[i].transform.position-dir*(_childCircleRadius+.25f));
            }
=======
>>>>>>> main
        }
        
    }
    void InsantiateEnemiesMeasureType()
    {
        _circle.transform.localScale = Vector3.one * _childCircleRadius;
        float _angleOffset = 360 / _enemiesCount;
        for(int i=0;i<_enemiesCount;++i)
        {
            Vector3 _spawnPosition = _rotating_point.position + _rotating_point.transform.up * _radius;
            GameObject g =  Instantiate(_enemies[Random.Range(0, _enemies.Count)], _spawnPosition, Quaternion.identity,_EnemiesHolder);
            _enemieslist.Add(g.transform);
            _rotating_point.eulerAngles += Vector3.forward * _angleOffset;
        }
    }
    void InsantiateEnemiesTogetherType()
    {
        _circle.transform.localScale = Vector3.one * _childCircleRadius;
        _rotating_point.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        float _angleOffset = _angleMeasurement1;
        for (int i = 0; i < _enemiesCount; ++i)
        {
            Vector3 _spawnPosition = _rotating_point.position + _rotating_point.transform.up * _radius;
            GameObject g = Instantiate(_enemies[0], _spawnPosition, Quaternion.identity, _EnemiesHolder);
            _enemieslist.Add(g.transform);
            _rotating_point.eulerAngles += Vector3.forward * _angleOffset;
        }
        if (_circleCount > 1)
        {
            _rotating_point.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
            for (int i = 0; i < _enemiesCount2; ++i)
            {
                Vector3 _spawnPosition = _rotating_point.position + _rotating_point.transform.up * _radius2;
<<<<<<< HEAD
                GameObject g = Instantiate(_enemies[1], _spawnPosition, Quaternion.identity, _EnemiesHolder2);
=======
                GameObject g = Instantiate(_enemies[Random.Range(0, _enemies.Count)], _spawnPosition, Quaternion.identity, _EnemiesHolder2);
>>>>>>> main
                _enemieslist.Add(g.transform);
                _rotating_point.eulerAngles += Vector3.forward * _angleOffset;
            }
        }
        
    }
    void InsantiateEnemiesRandomType()
    {
        _circle.transform.localScale = Vector3.one * _childCircleRadius;
        float _angleOffset;
        _rotating_point.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        for (int i = 0; i < _enemiesCount; i++)
        {
            Vector3 _spawnPosition = _rotating_point.position + _rotating_point.transform.up * _radius;
            RaycastHit2D hit = Physics2D.Raycast(_rotating_point.position, _rotating_point.up, _radius,_enemesLayer);
            if(hit.collider==null)
            {
             GameObject g = Instantiate(_enemies[Random.Range(0, _enemies.Count)], _spawnPosition, Quaternion.identity, _EnemiesHolder);
             _enemieslist.Add(g.transform);
            }else
            {
                // decrese the value of i
            }
            _angleOffset = Random.Range(_angleMeasurement1, _angleMeasurement1 * 3f);
            _rotating_point.eulerAngles += Vector3.forward * _angleOffset;
        }
    }
    public void DisableEnemies()
    {
        for (int i = 0; i < _EnemiesHolder.childCount; i++)
        {
            _EnemiesHolder.GetChild(i).GetComponent<TrailRenderer>().enabled = false;
        }
        if (_circleCount > 1)
        {
            for (int i = 0; i < _EnemiesHolder2.childCount; i++)
            {
                _EnemiesHolder2.GetChild(i).GetComponent<TrailRenderer>().enabled = false;
            }
        }
            _EnemiesHolder.DOScale(new Vector3(_scaleTarget, _scaleTarget, 1f), _timeToFade).OnComplete(() =>
        {
            _EnemiesHolder.gameObject.SetActive(false);
        });
        _EnemiesHolder2.DOScale(new Vector3(_scaleTarget, _scaleTarget, 1f), _timeToFade).OnComplete(() =>
        {
            _EnemiesHolder2.gameObject.SetActive(false);
        });
        
        //_EnemiesHolder.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (_canRotate)
        {
            if (_rotationType1)
                _EnemiesHolder.eulerAngles += Vector3.forward * _rotationSpeed * Time.deltaTime;
            else
                _EnemiesHolder.eulerAngles -= Vector3.forward * _rotationSpeed * Time.deltaTime;
            if (_rotationType2)
                _EnemiesHolder2.eulerAngles += Vector3.forward * _rotationSpeed * 1.5f * Time.deltaTime;
            else
                _EnemiesHolder2.eulerAngles -= Vector3.forward * _rotationSpeed * 1.5f * Time.deltaTime;
        }
    }
    private void OnDrawGizmos()
    {
    /*    Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
        //foreach(Transform t in _enemieslist)
            //Gizmos.DrawWireSphere(t.position, _childRadius);

        Gizmos.color= Color.red;
        Gizmos.DrawLine(_rotating_point.position,_rotating_point.position+ _rotating_point.up * _radius);
    */
    }
    public void ResetEnemies()
    {
       
        _EnemiesHolder.DOScale(Vector3.one, _timeToFade).OnStart(() =>
        {
            _EnemiesHolder.gameObject.SetActive(true);
        });
        if (_circleCount > 1)
        {
        _EnemiesHolder2.DOScale(Vector3.one, _timeToFade).OnStart(() =>
        {
            _EnemiesHolder2.gameObject.SetActive(true);
        });
        }
        StartCoroutine(trailGain());

    }
    IEnumerator trailGain()
    {
        yield return new WaitForSeconds(.4f);
        for (int i = 0; i < _EnemiesHolder.childCount; i++)
        {
            _EnemiesHolder.GetChild(i).GetComponent<TrailRenderer>().enabled = true;
        }
        if (_circleCount > 1)
        {
            for (int i = 0; i < _EnemiesHolder2.childCount; i++)
            {
                _EnemiesHolder2.GetChild(i).GetComponent<TrailRenderer>().enabled = true;
            }

        }
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum alignType
{
    measured, together, random
}
public class checkPointScript : MonoBehaviour
{
    public bool _hasMiniCheckPoint;
    [SerializeField] public alignType _alignType;
    [SerializeField] public int _enemiesCount = 3;
    [SerializeField] public int _enemiesCount2 = 3;
    [SerializeField] public float _radius;
    [Header("rotation")]
    [SerializeField] public bool _canRotate = false;
    [SerializeField] public float _rotationSpeed;

    [Header("Child")]
    [SerializeField] List<Transform> _enemieslist = new List<Transform>();
    [SerializeField] public float _childCircleRadius;
    [SerializeField] SpriteRenderer _circle;

    [Header("instantiation management")]
    [SerializeField] int _circleCount;
    [SerializeField] Transform _rotating_point;
    [SerializeField] Transform _EnemiesHolder;
    [SerializeField] Transform _EnemiesHolder2;
    [SerializeField] LayerMask _enemesLayer;

    [Header("Enemies")]
    [SerializeField] List<GameObject> _enemies = new List<GameObject>();
    [Header("angle measurement")]
    [SerializeField] float _angleMeasurement;
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
    private void Awake()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    public void Init(alignType _alignType = alignType.together, int enemies_count = 1, float _radius = 1.5f, float _rotationSpeed = 90f, float _child_radius = .35f)
    {
        this._alignType = _alignType;
        _enemiesCount = enemies_count;
        this._radius = _radius;
        this._rotationSpeed = _rotationSpeed;
        this._childCircleRadius = _child_radius;
    }
    IEnumerator Start()
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

        if (_hasMiniCheckPoint)
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
            _miniCheckPointList.Add(g);
            //_rotateObject.eulerAngles += Vector3.forward * _angleOffset;
            //_distance += _miniCheckPointDistance;
        }

    }
    void InsantiateEnemiesMeasureType()
    {
        _circle.transform.localScale = Vector3.one * _childCircleRadius;
        float _angleOffset = 360 / _enemiesCount;
        for (int i = 0; i < _enemiesCount; ++i)
        {
            Vector3 _spawnPosition = _rotating_point.position + _rotating_point.transform.up * _radius;
            GameObject g = Instantiate(_enemies[Random.Range(0, _enemies.Count)], _spawnPosition, Quaternion.identity, _EnemiesHolder);
            _enemieslist.Add(g.transform);
            _rotating_point.eulerAngles += Vector3.forward * _angleOffset;
        }
    }
    void InsantiateEnemiesTogetherType()
    {
        _circle.transform.localScale = Vector3.one * _childCircleRadius;
        _rotating_point.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        float _angleOffset = _angleMeasurement;
        for (int i = 0; i < _enemiesCount; ++i)
        {
            Vector3 _spawnPosition = _rotating_point.position + _rotating_point.transform.up * _radius;
            GameObject g = Instantiate(_enemies[Random.Range(0, _enemies.Count)], _spawnPosition, Quaternion.identity, _EnemiesHolder);
            _enemieslist.Add(g.transform);
            _rotating_point.eulerAngles += Vector3.forward * _angleOffset;
        }
        if (_circleCount > 1)
        {
            _rotating_point.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
            for (int i = 0; i < _enemiesCount2; ++i)
            {
                Vector3 _spawnPosition = _rotating_point.position + _rotating_point.transform.up * _radius * 2f;
                GameObject g = Instantiate(_enemies[Random.Range(0, _enemies.Count)], _spawnPosition, Quaternion.identity, _EnemiesHolder2);
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
            RaycastHit2D hit = Physics2D.Raycast(_rotating_point.position, _rotating_point.up, _radius, _enemesLayer);
            if (hit.collider == null)
            {
                GameObject g = Instantiate(_enemies[Random.Range(0, _enemies.Count)], _spawnPosition, Quaternion.identity, _EnemiesHolder);
                _enemieslist.Add(g.transform);
            }
            else
            {
                // decrese the value of i
            }
            _angleOffset = Random.Range(_angleMeasurement, _angleMeasurement * 3f);
            _rotating_point.eulerAngles += Vector3.forward * _angleOffset;
        }
    }
    public void DisableEnemies()
    {
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
            _EnemiesHolder.eulerAngles += Vector3.forward * _rotationSpeed * Time.deltaTime;
            _EnemiesHolder2.eulerAngles += Vector3.forward * _rotationSpeed * 1.5f * Time.deltaTime;

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
        //foreach(Transform t in _enemieslist)
        //Gizmos.DrawWireSphere(t.position, _childRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_rotating_point.position, _rotating_point.position + _rotating_point.up * _radius);
    }
    public void ResetEnemies()
    {
        _EnemiesHolder.DOScale(Vector3.one, _timeToFade).OnStart(() =>
        {
            _EnemiesHolder.gameObject.SetActive(true);
        });
        _EnemiesHolder2.DOScale(Vector3.one, _timeToFade).OnStart(() =>
        {
            _EnemiesHolder2.gameObject.SetActive(true);
        });
    }
}

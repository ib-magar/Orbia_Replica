using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum alignType
{
    measured,together,random
}
public class checkPointScript : MonoBehaviour
{
    [SerializeField] public alignType _alignType;

    [SerializeField] public int _enemiesCount=3;
    [SerializeField] public float _radius;
    [Header("rotation")]
    [SerializeField] public bool _canRotate=false;
    [SerializeField] public float _rotationSpeed;

    [Header("Child")]
    [SerializeField] List<Transform> _enemieslist = new List<Transform>();
    [SerializeField] float _childRadius;

    [Header("instantiation management")]
    [SerializeField] Transform _rotating_point;
    [SerializeField] Transform _EnemiesHolder;
    [SerializeField] LayerMask _enemesLayer;

    [Header("Enemies")]
    [SerializeField] List<GameObject> _enemies=new List<GameObject>();
    [Header("angle measurement")]
    [SerializeField] float _angleMeasurement;
    [Header("enemies disable")]
    [SerializeField] float _timeToFade;
    [SerializeField] float _scaleTarget;

    [Header("Text")]
    [SerializeField] public TMP_Text _checkPointScore;
    public void Init(alignType _alignType=alignType.together,int enemies_count=1,float _radius=1.5f,float _rotationSpeed=90f,float _child_radius=1f)
    {
        this._alignType = _alignType;
        _enemiesCount = enemies_count;
        this._radius = _radius;
        this._rotationSpeed = _rotationSpeed;
        this._childRadius = _child_radius;
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

    }
    void InsantiateEnemiesMeasureType()
    {
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
        float _angleOffset = _angleMeasurement;
        for (int i = 0; i < _enemiesCount; ++i)
        {
            Vector3 _spawnPosition = _rotating_point.position + _rotating_point.transform.up * _radius;
            GameObject g = Instantiate(_enemies[Random.Range(0, _enemies.Count)], _spawnPosition, Quaternion.identity, _EnemiesHolder);
            _enemieslist.Add(g.transform);
            _rotating_point.eulerAngles += Vector3.forward * _angleOffset;
        }
    }
    void InsantiateEnemiesRandomType()
    {
        float _angleOffset;
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
        //_EnemiesHolder.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(_canRotate)
        _EnemiesHolder.eulerAngles += Vector3.forward * _rotationSpeed*Time.deltaTime;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
        //foreach(Transform t in _enemieslist)
            //Gizmos.DrawWireSphere(t.position, _childRadius);

        Gizmos.color= Color.red;
        Gizmos.DrawLine(_rotating_point.position,_rotating_point.position+ _rotating_point.up * _radius);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEditor.ShaderKeywordFilter;

public enum _State
{
    idle,moving
}
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] _State state;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _moveTime;
    [Header("death")]
    [SerializeField] float _fallDownDistance;
    [SerializeField] float _fallTime;
    private Transform _player;
    [Header("Checkpoints")]
    [SerializeField] GameObject _currentCheckPoint;
    [Header("external Scripts")]
    [SerializeField] GameManager _gameManager;
    private void Awake()
    {
        _gameManager=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        state=_State.idle;
    }
    private void Start()
    {
        _player = GetComponent<Transform>();
    }
    public void Move(GameObject movePosition)
    {
        
        // move the player to the next checkpoint 
        if(state ==_State.idle)
        {
            state= _State.moving;
            /*if (_currentCheckPoint != null)
            _player.DOMove(_currentCheckPoint.transform.position, _moveTime).From(_player.position).OnComplete(() =>
            {
                state = _State.idle;
            });
            else*/
            _player.DOMove(movePosition.transform.position, _moveTime).From(_player.position).OnComplete(() =>
            {
                state= _State.idle;
            });
            
            //_currentCheckPoint = movePosition;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("checkPoint"))
        {
            _gameManager.CheckAndRemoveNode(collision.gameObject);
        }
        else if(collision.CompareTag("enemy"))
        {
            //gameObject.SetActive(false);
            GetComponent<CircleCollider2D>().enabled = false;
           // GetComponent<Rigidbody2D>().isKinematic = false;
            _player.DOMoveY(_player.position.y - _fallDownDistance, _fallTime).From(_player.position.y).OnComplete(() =>
            {
                //Game Restart Menu
                GetComponent<CircleCollider2D>().enabled = true;
            });
            _gameManager._playerdead = true;
        }
    }


}

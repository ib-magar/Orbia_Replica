using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System;

public enum _State
{
    idle,moving
}
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] int _life =11;
    //Events
    public event Action _playerFailed;
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
    private Fx_player _fxPlayer;

   
    private void Awake()
    {
        _gameManager=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        _fxPlayer=GetComponent<Fx_player>();
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
            _fxPlayer.DashSoundPlay();
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
            _fxPlayer.CheckPointReached();
            _gameManager.CheckAndRemoveCheckPoint(collision.gameObject);     
        }
        else if(collision.CompareTag("enemy"))
        {
            if (state == _State.moving)
            {
                if (_life <= 0)
                {
                    state = _State.moving;
                    _gameManager._playerdead = true;
                    if (_playerFailed != null) _playerFailed();         //subscribed by the UI Manager
                    _fxPlayer.FailCollision();
                    GetComponent<CircleCollider2D>().enabled = false;
                    // GetComponent<Rigidbody2D>().isKinematic = false;
                    _player.DOMove(_gameManager.getLastCheckPoint().position, _fallTime - .2f).OnComplete(() =>
                    {
                        //Game Restart Menu
                        GetComponent<CircleCollider2D>().enabled = true;
                        _gameManager._miniCheckPointCount = 0;      //reset the checkPoint count (mini)
                        state = _State.idle;
                    });
                    //Regenerate the last checkPoint Enemies.
                }
                else
                {
                    --_life;
                    //dodge sound
                    state = _State.idle;
                    _gameManager.GetBackToLastCheckPoint();
                    //get back to the last mini-point
                }
            }

        }
        else if(collision.CompareTag("child"))
        {
            _fxPlayer.MiniCheckPointReached();
            _gameManager.CheckAndRemoveMiniCheckPoint(collision.gameObject);
        }

        if(collision.CompareTag("shield"))
        {
            // shield sound
            _life += 1;
            Destroy(collision.gameObject);
        }
    }


}

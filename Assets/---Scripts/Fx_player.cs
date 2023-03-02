using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Fx_player : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource _fx_source;

    [Header("audioc clips")]
    [SerializeField] AudioClip _checkPoint_reach;
    [SerializeField] AudioClip _collide_and_fail;
    [SerializeField] AudioClip _dashMovement1;
    [SerializeField] AudioClip _dashMovement2;
    private void Start()
    {
        _fx_source = GetComponent<AudioSource>();
    }

    public void CheckPointReached()
    {
        _fx_source.pitch = Random.Range(.9f, 1.2f);
        _fx_source.PlayOneShot(_checkPoint_reach);
    } 
    public void FailCollision()
    {
        _fx_source.pitch = Random.Range(.9f, 1.2f);
        _fx_source.PlayOneShot(_collide_and_fail);
    }
    public void DashSoundPlay()
    {
        _fx_source.pitch = Random.Range(.9f, 1.2f);
        if (Random.value > .5f)
            _fx_source.PlayOneShot(_dashMovement1);
        else _fx_source.PlayOneShot(_dashMovement2);
    }
    public void MiniCheckPointReached()
    {
        //have to change the audioclip later.
        _fx_source.pitch = Random.Range(.9f, 1.2f);
        _fx_source.PlayOneShot(_checkPoint_reach);
    }
}

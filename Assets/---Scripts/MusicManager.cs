using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Audio sources")]
    [SerializeField] AudioSource _music_source;


    [Header("Audio Clips")]
    [SerializeField] AudioClip _music_clip;

    [Header("Initial volumes")]
    [SerializeField] float _initial_music_volume;
    private void Start()
    {
        _music_source.clip = _music_clip;     
        _music_source.volume= _initial_music_volume;
    }
   

}

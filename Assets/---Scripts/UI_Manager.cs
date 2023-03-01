using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    [Header("Buttons")]
    [SerializeField] Button _playButton;
    [SerializeField] Button _RestartButton;

    [Header("GameObjects")]
    [SerializeField] GameObject _GamePlayUI;
    
    GameManager _gameManager;
    CharacterController _characterController;
    private void Awake()
    {
        _gameManager=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        _characterController=GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }
    private void Start()
    {
        _characterController._playerFailed += PlayerFailed;
        _GamePlayUI.SetActive(false);
    }
    public void Play()
    {
        _playButton.gameObject.SetActive(false);
        _RestartButton.gameObject.SetActive(false);
        _GamePlayUI.SetActive(true);
    }
    public void Restart()
    {
        _RestartButton.gameObject.SetActive(false);
        _gameManager.MoveCharacter();
        StartCoroutine(gamePlaySetActive(true));
    }
    IEnumerator gamePlaySetActive(bool _isTrue)
    {
        yield return new WaitForSeconds(.4f);
        _GamePlayUI.SetActive(_isTrue);

    }
    public void PlayerFailed()
    {
        _RestartButton.gameObject.SetActive(true);
        _GamePlayUI.SetActive(false);
    }
}

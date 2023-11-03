using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoViewer : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private GameObject _startScreen;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _closeButton;
    [Space]
    [SerializeField] private Sprite _play;
    [SerializeField] private Sprite _paused;

    private bool _isPaused;

    private void Start()
    {
        _playButton.onClick.AddListener(PausedPlayVideo);
        _closeButton.onClick.AddListener(Closed);

        StartCoroutine(StartPlayVideo());
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(PausedPlayVideo);
        _closeButton.onClick.RemoveListener(Closed);
    }


    private void PausedPlayVideo()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
            PausedVideo();
        else
            PlayVideo();
    }

    private void PausedVideo()
    {
        _videoPlayer.Pause();
        _playButton.image.sprite = _play;
    }

    private void PlayVideo()
    {
        _videoPlayer.Play();
        _playButton.image.sprite = _paused;
    }


    private void Closed()
    {
        GameManager.Instance.ReturnInMenu();
    }

    private IEnumerator StartPlayVideo()
    {
        _startScreen.SetActive(true);

        _videoPlayer.url = GameManager.Instance.PathToVideo;
        PlayVideo();

        while (!_videoPlayer.isPlaying)
            yield return null;

        _startScreen.SetActive(false);
    }
}

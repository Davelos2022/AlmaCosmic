using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;
using AlmaSpace;
using VolumeBox.Utils;

public class VideoViewer : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private Image _fade;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _closeButton;
    [Space]
    [SerializeField] private Sprite _play;
    [SerializeField] private Sprite _paused;
    [Space]
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private Button _replay;
    [SerializeField] private Button _nextVideo;
    [SerializeField] private Button _exitToMenu;

    private bool _isPaused;
    private string _currentPathVideo;

    private void Start()
    {
        _playButton.onClick.AddListener(PausedPlayVideo);
        _closeButton.onClick.AddListener(Closed);
        _replay.onClick.AddListener(Replay);
        _nextVideo.onClick.AddListener(NextVideo);
        _exitToMenu.onClick.AddListener(Closed);

        _videoPlayer.loopPointReached += OnVideoEnd;
        _currentPathVideo = AlmaSpaceManager.Instance.CurrentVideo;

        StartCoroutine(StartPlayVideo());
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(PausedPlayVideo);
        _closeButton.onClick.RemoveListener(Closed);
        _replay.onClick.RemoveListener(Replay);
        _nextVideo.onClick.RemoveListener(NextVideo);
        _exitToMenu.onClick.RemoveListener(Closed);

        _videoPlayer.loopPointReached -= OnVideoEnd;
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
        AlmaSpaceManager.Instance.ShowMessageBox(new MessageBoxData
        {
            HeaderCaption = "Подтвердите действие",
            MainCaption = "Вы действительно хотите выйти в меню?",
            CancelCaption = "Отмена",
            OkCaption = "Да",
            OkAction = AlmaSpaceManager.Instance.ReturnInMenu
        });
    }

    private void OnVideoEnd(VideoPlayer videoPlayer)
    {
        ActivateDeactivateButton(false);
        ActivateDeactivateEndScreen(true);
    }

    private void Replay()
    {
        ActivateDeactivateEndScreen(false);
        StartCoroutine(StartPlayVideo());
    }

    private void NextVideo()
    {
        int indexVideo = Array.IndexOf(AlmaSpaceManager.Instance.CurrentVideos, _currentPathVideo);
        indexVideo++;

        if (indexVideo > AlmaSpaceManager.Instance.CurrentVideos.Length - 1)
            indexVideo = 0;

        _currentPathVideo = AlmaSpaceManager.Instance.CurrentVideos[indexVideo];

        ActivateDeactivateEndScreen(false);
        StartCoroutine(StartPlayVideo());
    }

    private void ActivateDeactivateEndScreen(bool active)
    {
        _endScreen.SetActive(active);
    }

    private void ActivateDeactivateButton(bool active)
    {
        _playButton.interactable = active;
        _closeButton.interactable = active;
    }

    private IEnumerator StartPlayVideo()
    {
        ActivateDeactivateButton(true);

        float fadeDuration = 1.0f;
        float alphaFade = 1.0f;
        float duration = 0.5f;

        Color newColor = _fade.color;
        newColor.a = alphaFade;
        _fade.color = newColor;

        _videoPlayer.url = _currentPathVideo;
        PlayVideo();

        while (!_videoPlayer.isPlaying)
            yield return null;

        for (float t = 0; t < fadeDuration; t += duration)
        {
            alphaFade -= duration;
            newColor.a = alphaFade;

            _fade.color = newColor;
            yield return null;
        }
    }
}

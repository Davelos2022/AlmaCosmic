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
    [SerializeField] private Button _soundButton;
    [Space]
    [SerializeField] private Sprite _play;
    [SerializeField] private Sprite _paused;
    [SerializeField] private Sprite _offMusic;
    [SerializeField] private Sprite _onMusic;
    [Space]
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private Button _replay;
    [SerializeField] private Button _prevVideo;
    [SerializeField] private Button _nextVideo;
    [SerializeField] private Button _exitToMenu;

    private bool _isPaused;
    private string _currentPathVideo;

    private void Awake()
    {
        _playButton.onClick.AddListener(PausedOrPlayVideo);
        _closeButton.onClick.AddListener(CloseVideoView);
        _replay.onClick.AddListener(Replay);
        _prevVideo.onClick.AddListener(PrevVideo);
        _nextVideo.onClick.AddListener(NextVideo);
        _exitToMenu.onClick.AddListener(CloseVideoView);
        _soundButton.onClick.AddListener(OnOffMusic);

        _videoPlayer.loopPointReached += OnVideoEnd;
        _videoPlayer.prepareCompleted += PrepareCompleted;

        _currentPathVideo = AlmaSpaceManager.Instance.CurrentVideo;

        StartCoroutine(PreparingForPlayback());
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(PausedOrPlayVideo);
        _closeButton.onClick.RemoveListener(CloseVideoView);
        _replay.onClick.RemoveListener(Replay);
        _prevVideo.onClick.RemoveListener(PrevVideo);
        _nextVideo.onClick.RemoveListener(NextVideo);
        _exitToMenu.onClick.RemoveListener(CloseVideoView);
        _soundButton.onClick.RemoveListener(OnOffMusic);

        _videoPlayer.loopPointReached -= OnVideoEnd;
        _videoPlayer.prepareCompleted -= PrepareCompleted;
    }

    //Buttons 
    private void PausedOrPlayVideo()
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

    private void OnOffMusic()
    {
        AlmaSpaceManager.Instance.AudioManager.OnOffMusic();

        if (!AlmaSpaceManager.Instance.AudioManager.Music)
            _soundButton.image.sprite = _offMusic;
        else
            _soundButton.image.sprite = _onMusic;
    }

    private void CloseVideoView()
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

    //End Screen
    private void OnVideoEnd(VideoPlayer videoPlayer)
    {
        int indexVideo = Array.IndexOf(AlmaSpaceManager.Instance.CurrentVideos, _currentPathVideo);

        if (indexVideo >= AlmaSpaceManager.Instance.CurrentVideos.Length - 1)
            _nextVideo.interactable = false;
        else
            _nextVideo.interactable = true;

        if (indexVideo <= 0)
            _prevVideo.interactable = false;
        else
            _prevVideo.interactable = true;

        ActivateDeactivateButton(false);
        ActivateDeactivateEndScreen(true);
    }

    private void Replay()
    {
        ActivateDeactivateEndScreen(false);
        StartCoroutine(PreparingForPlayback());
    }

    private void NextVideo()
    {
        int indexVideo = Array.IndexOf(AlmaSpaceManager.Instance.CurrentVideos, _currentPathVideo);
        indexVideo++;

        _currentPathVideo = AlmaSpaceManager.Instance.CurrentVideos[indexVideo];

        ActivateDeactivateEndScreen(false);
        StartCoroutine(PreparingForPlayback());
    }

    private void PrevVideo()
    {
        int indexVideo = Array.IndexOf(AlmaSpaceManager.Instance.CurrentVideos, _currentPathVideo);
        indexVideo--;

        _currentPathVideo = AlmaSpaceManager.Instance.CurrentVideos[indexVideo];

        ActivateDeactivateEndScreen(false);
        StartCoroutine(PreparingForPlayback());
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


    //Preparing Video
    public void PrepareCompleted(VideoPlayer videoPlayer)
    {
        if (videoPlayer.isPrepared)
            PlayVideo();
    }

    private IEnumerator PreparingForPlayback()
    {
        ActivateDeactivateButton(true);

        float fadeDuration = 1.0f;
        float alphaFade = 1.0f;
        float duration = 0.5f;

        Color newColor = _fade.color;
        newColor.a = alphaFade;
        _fade.color = newColor;

        _videoPlayer.url = _currentPathVideo;
        _videoPlayer.Prepare();

        while (!_videoPlayer.isPrepared)
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

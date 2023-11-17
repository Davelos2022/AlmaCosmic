using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;
using AlmaSpace;
using VolumeBox.Utils;

public class VideoViewer : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private Image _fade;
    [Header("Interface Screen Settings")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _soundButton;
    [Space]
    [SerializeField] private Sprite _playSprite;
    [SerializeField] private Sprite _pausedSprite;
    [SerializeField] private Sprite _offMusicSprite;
    [SerializeField] private Sprite _onMusicSprite;
    [Header("End Screen Video Settings")]
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private Button _replay;
    [SerializeField] private Button _prevVideo;
    [SerializeField] private Button _nextVideo;
    [SerializeField] private Button _exitToMenu;

    private bool _isPaused;
    private bool _isPrepare;
    private int _currentIndexVideo;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        _playButton.onClick.AddListener(PausedOrPlayVideo);
        _closeButton.onClick.AddListener(() => CloseVideoView(true));
        _replay.onClick.AddListener(Replay);
        _prevVideo.onClick.AddListener(PrevVideo);
        _nextVideo.onClick.AddListener(NextVideo);
        _exitToMenu.onClick.AddListener(() => CloseVideoView(false));
        _soundButton.onClick.AddListener(OnOffMusic);

        _videoPlayer.loopPointReached += OnVideoEnd;
        _videoPlayer.prepareCompleted += PrepareCompleted;

        MenuController._aboutCompany += PausedVideo;
        MenuController._hideCompany += PlayVideo;

        _currentIndexVideo = Array.IndexOf(AlmaSpaceManager.Instance.CurrentVideos, AlmaSpaceManager.Instance.CurrentVideo);

        StartCoroutine(PreparingForPlayback());
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(PausedOrPlayVideo);
        _closeButton.onClick.RemoveListener(() => CloseVideoView(true));
        _replay.onClick.RemoveListener(Replay);
        _prevVideo.onClick.RemoveListener(PrevVideo);
        _nextVideo.onClick.RemoveListener(NextVideo);
        _exitToMenu.onClick.RemoveListener(() => CloseVideoView(false));
        _soundButton.onClick.RemoveListener(OnOffMusic);

        _videoPlayer.loopPointReached -= OnVideoEnd;
        _videoPlayer.prepareCompleted -= PrepareCompleted;

        MenuController._aboutCompany -= PausedVideo;
        MenuController._hideCompany -= PlayVideo;
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
        _playButton.image.sprite = _playSprite;
    }

    private void PlayVideo()
    {
        _videoPlayer.Play();
        _playButton.image.sprite = _pausedSprite;
    }



    private void OnOffMusic()
    {
        AlmaSpaceManager.Instance.AudioManager.OnOffMusic();

        if (!AlmaSpaceManager.Instance.AudioManager.Music)
        {
            _soundButton.image.sprite = _offMusicSprite;
            _videoPlayer.SetDirectAudioMute(0, true);
        }
        else
        {
            _soundButton.image.sprite = _onMusicSprite;
            _videoPlayer.SetDirectAudioMute(0, false);
        }
    }

    private void CloseVideoView(bool message)
    {
        if (message)
        {
            PausedVideo();

            AlmaSpaceManager.Instance.ShowMessageBox(new MessageBoxData
            {
                HeaderCaption = "",
                MainCaption = "Завершить и выйти в главное меню?",
                CancelCaption = "Отмена",
                OkCaption = "Выйти",
                OkAction = AlmaSpaceManager.Instance.ReturnInMenu,
                CancelAction =  _isPaused ? null : PlayVideo
            });
        }
        else
        {
            AlmaSpaceManager.Instance.ReturnInMenu();
        }
    }

    //End Screen
    private void OnVideoEnd(VideoPlayer videoPlayer)
    {
        if (AlmaSpaceManager.Instance.TypeLesson == TypeLesson.Gymnastick)
        {
            if (_currentIndexVideo >= AlmaSpaceManager.Instance.CurrentVideos.Length - 1)
                _nextVideo.interactable = false;
            else
                _nextVideo.interactable = true;

            if (_currentIndexVideo <= 0)
                _prevVideo.interactable = false;
            else
                _prevVideo.interactable = true;
        }

        ActivateDeactivateEndScreen(true);
    }

    private void Replay()
    {
        ActivateDeactivateEndScreen(false);
        StartCoroutine(PreparingForPlayback());
    }

    private void NextVideo()
    {
        _currentIndexVideo++;

        if (_currentIndexVideo > AlmaSpaceManager.Instance.CurrentVideos.Length - 1)
            _currentIndexVideo = 0;

        ActivateDeactivateEndScreen(false);
        StartCoroutine(PreparingForPlayback());
    }

    private void PrevVideo()
    {
        _currentIndexVideo--;

        if (_currentIndexVideo < 0)
            _currentIndexVideo = AlmaSpaceManager.Instance.CurrentVideos.Length - 1;

        ActivateDeactivateEndScreen(false);
        StartCoroutine(PreparingForPlayback());
    }

    private void ActivateDeactivateEndScreen(bool active)
    {
        _endScreen.SetActive(active);
    }

    //Preparing Video
    public void PrepareCompleted(VideoPlayer videoPlayer)
    {
        PlayVideo();
        Resources.UnloadUnusedAssets();
    }

    private IEnumerator PreparingForPlayback()
    {
        if (!_isPrepare)
        {
            _isPrepare = true;

            float fadeDuration = 1.0f;
            float alphaFade = 1.0f;
            float duration = 0.1f;

            Color newColor = _fade.color;
            newColor.a = alphaFade;
            _fade.color = newColor;

            _videoPlayer.url = AlmaSpaceManager.Instance.CurrentVideos[_currentIndexVideo];
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

            _isPrepare = false;
        }
    }
}

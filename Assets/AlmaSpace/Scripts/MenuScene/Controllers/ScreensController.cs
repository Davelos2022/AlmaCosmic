using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TypeModeController
{
    Click,
    Swipe
}

[Serializable]
public class ScreenPanel //For Type Click 
{
    public GameObject Screen;
    public Button Button;
    public UnityEvent ShowSreen;
    public UnityEvent HideScreen;
}

public class ScreensController : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private TypeModeController _type;
    [SerializeField][Range(1, 50)] private int _startScreenIndex;
    [SerializeField] private ScreenPanel[] _screens;
    [Space]
    [Header("Type Click Settings")]
    [SerializeField] private bool _isFade;
    [SerializeField] private Image _fadeImage;
    [SerializeField][Range(0.2f, 2f)] private float _fadeOutDuration = 0.2f;
    [Space]
    [SerializeField] private Button _homeButton;
    [SerializeField] private bool _isPrevScreenButton;
    [SerializeField] private Button _prevScreenButton;
    [Space]
    [Header("Type Swipe Settings")]
    [SerializeField] private RectTransform _containerScreen;
    [SerializeField] private bool _navigationButton;
    [SerializeField] private Button _previousScreenButton;
    [SerializeField] private Button _nextScreenButton;
    [Space]
    [SerializeField][Range(150, 550)] private float _minSwipeDistance;
    [SerializeField][Range(0.3f, 1f)] private float _scrollDuration;

    private ScreenPanel _currentScreen;

    //Type Click Propirties
    private List<ScreenPanel> _screensPanels = new List<ScreenPanel>();

    //Type Swipe Propirties
    private RectTransform _screenRect;
    private int _currentScreenIndex;
    private Vector3 _startScreenPosition;
    private Vector2 _dragStartPosition;

    private void Start()
    {
        if (_type == TypeModeController.Click)
        {
            SettingScreensForClick();
        }
        else if (_type == TypeModeController.Swipe)
        {
            SettingsScreensForSwipe();
        }
    }

    private void OnDestroy()
    {
        if (_type == TypeModeController.Click)
        {
            _homeButton?.onClick.RemoveListener(ReturnInMainScreen);

            if (_isPrevScreenButton)
                _prevScreenButton?.onClick.RemoveListener(PrevScreen);

            for (int x = 0; x < _screens.Length; x++)
                _screens[x].Button?.onClick.RemoveListener(() => ShowScreen(_screens[x], true));
        }
        else if (_type == TypeModeController.Swipe && _navigationButton)
        {
            _previousScreenButton?.onClick.RemoveListener(() => NavigationScroll(-1));
            _nextScreenButton?.onClick.RemoveListener(() => NavigationScroll(+1));
        }
    }

    //Type Click methods
    private void SettingScreensForClick()
    {
        _homeButton?.onClick.AddListener(ReturnInMainScreen);

        if (_isPrevScreenButton)
            _prevScreenButton?.onClick.AddListener(PrevScreen);

        for (int x = 0; x < _screens.Length; x++)
        {
            int index = x;
            _screens[x].Button?.onClick.AddListener(() => ShowScreen(_screens[index], true));
            _screens[x].Screen?.SetActive(false);
        }

        _startScreenIndex = _startScreenIndex > _screens.Length - 1 ? _screens.Length - 1 : _startScreenIndex - 1;
        _currentScreen = _screens[_startScreenIndex];
        _currentScreen.Screen.SetActive(true);

        if (_isFade)
            StartCoroutine(FadeAnim());
    }
    private void ShowScreen(ScreenPanel screenPanel, bool newScreen)
    {
        if (_isFade)
            StartCoroutine(FadeAnim());

        if (newScreen)
            _screensPanels.Add(_currentScreen);

        _currentScreen.HideScreen?.Invoke();
        _currentScreen.Screen.SetActive(false);

        _currentScreen = screenPanel;
        _currentScreen.Screen.SetActive(true);
        _currentScreen.ShowSreen?.Invoke();

        CheckStateButton();
    }

    private void CheckStateButton()
    {
        if (_isPrevScreenButton)
        {
            if (_screensPanels.Count > 0 && !_prevScreenButton.gameObject.activeSelf && _isPrevScreenButton)
                _prevScreenButton.gameObject.SetActive(true);
            else if (_screensPanels.Count <= 0 && _prevScreenButton.gameObject.activeSelf && _isPrevScreenButton)
                _prevScreenButton.gameObject.SetActive(false);
        }

        if (_currentScreen != _screens[_startScreenIndex] && !_homeButton.gameObject.activeSelf)
            _homeButton.gameObject.SetActive(true);
        else if (_currentScreen == _screens[_startScreenIndex] && _homeButton.gameObject.activeSelf)
            _homeButton.gameObject.SetActive(false);
    }

    private void PrevScreen()
    {
        ScreenPanel prevScreen = _screensPanels[_screensPanels.Count - 1];
        _screensPanels.Remove(prevScreen);
        ShowScreen(prevScreen, false);
    }

    private void ReturnInMainScreen()
    {
        _screensPanels.Clear();
        ShowScreen(_screens[_startScreenIndex], false);
    }

    private IEnumerator FadeAnim()
    {
        StopCoroutine(nameof(FadeAnim));

        float alphaFade = 1.0f;
        float stack = 0;

        Color newColor = _fadeImage.color;
        newColor.a = alphaFade;
        _fadeImage.color = newColor;

        while (_fadeImage.color.a > 0)
        {
            stack += Time.deltaTime / _fadeOutDuration;
            newColor.a = Mathf.Lerp(1, 0, stack);
            _fadeImage.color = newColor;
            yield return null;
        }
    }

    //Type Swipe methods
    private void SettingsScreensForSwipe()
    {
        _screenRect = GetComponent<RectTransform>();
        _startScreenPosition = _screenRect.anchoredPosition;
        _currentScreenIndex = _startScreenIndex > _screens.Length - 1 ? _screens.Length - 1 : _startScreenIndex - 1;

        if (_navigationButton)
        {
            _previousScreenButton?.onClick.AddListener(() => NavigationScroll(-1));
            _nextScreenButton?.onClick.AddListener(() => NavigationScroll(+1));
            _previousScreenButton?.gameObject.SetActive(true);
            _nextScreenButton?.gameObject.SetActive(true);
        }

        RectTransform previousScreen;
        RectTransform nextScreen;
        Vector2 positionScreen;
        float offset = 60f;

        for (int x = 1; x < _containerScreen.childCount; x++)
        {
            previousScreen = _containerScreen.GetChild(x - 1).GetComponent<RectTransform>();
            nextScreen = _containerScreen.GetChild(x).GetComponent<RectTransform>();

            positionScreen = new Vector2((previousScreen.anchoredPosition.x + Screen.currentResolution.width) + offset, 0);
            nextScreen.anchoredPosition = positionScreen;
            nextScreen.gameObject.SetActive(true);
        }

        _currentScreen = _screens[_currentScreenIndex];
        _currentScreen.Screen.SetActive(true);
        ScrollTo(_containerScreen, _currentScreenIndex);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_type == TypeModeController.Swipe)
        {
            _dragStartPosition = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_type == TypeModeController.Swipe)
        {
            if (eventData.delta.x < 0 && _currentScreenIndex < _screens.Length - 1 || eventData.delta.x > 0 && _currentScreenIndex > 0)
                _screenRect.anchoredPosition = new Vector2(_screenRect.anchoredPosition.x + eventData.delta.x, _screenRect.anchoredPosition.y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_type == TypeModeController.Swipe)
        {
            float dragDistance = _dragStartPosition.x - eventData.position.x;

            if (Mathf.Abs(dragDistance) >= _minSwipeDistance)
            {
                if (dragDistance > 0.5f && _currentScreenIndex < _screens.Length - 1)
                {
                    Debug.Log("Next");
                    _currentScreen.HideScreen?.Invoke();
                    _currentScreenIndex++;
                    _currentScreen = _screens[_currentScreenIndex];
                    _currentScreen.ShowSreen?.Invoke();

                }
                else if (dragDistance < 0f && _currentScreenIndex > 0)
                {
                    Debug.Log("Back");
                    _currentScreen.HideScreen?.Invoke();
                    _currentScreenIndex--;
                    _currentScreen = _screens[_currentScreenIndex];
                    _currentScreen.ShowSreen?.Invoke();
                }

                ScrollTo(_containerScreen, _currentScreenIndex);
            }

            ScrollTo(_screenRect, _startScreenPosition.x);
        }
    }

    private void ScrollTo(RectTransform rectTransform, float targetIndex)
    {
        float targetPosition = -targetIndex * _containerScreen.rect.width;
        rectTransform.DOAnchorPosX(targetPosition, _scrollDuration).SetEase(Ease.OutQuad);
    }

    private void NavigationScroll(int index)
    {
        if ((_currentScreenIndex + index) > _screens.Length - 1 || (_currentScreenIndex + index) < 0) return;

        _currentScreenIndex += index;
        ScrollTo(_containerScreen, _currentScreenIndex);
    }
}

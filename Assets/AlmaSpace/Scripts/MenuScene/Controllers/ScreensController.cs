using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ScreenPanel //For Type Click 
{
    public GameObject Screen;
    public Button Button;
}

public enum TypeModeController
{
    Click,
    Swipe
}

public class ScreensController : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private TypeModeController _type;
    [Space]
    [Header("Typpe Click Settings")]
    [SerializeField] private GameObject _startScreen;
    [SerializeField] private Button _returnMainScreenButton;
    [SerializeField] private Animator _fafeAnimator;
    [SerializeField] private ScreenPanel[] _screens;
    [Header("Type Swipe Settings")]
    [SerializeField] private RectTransform _containerScreen;
    [SerializeField] private bool _navigationButton;
    [SerializeField] private Button _previousScreenButton;
    [SerializeField] private Button _nextScreenButton;
    [SerializeField][Range(1, 10)] private int _startCreen;
    [SerializeField][Range(150, 550)] private float _minSwipeDistance;
    [SerializeField][Range(0.3f, 1f)] private float _scrollDuration;

    //Type Click Propirties
    private GameObject _currentScreen;
    private const string _fadeAnimName = "Fade_mainMenu";

    //Type Swipe Propirties
    private RectTransform _screenRect;
    private int _screenCount;
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
            _returnMainScreenButton.onClick.RemoveListener(ReturnInMainScreen);

            for (int x = 0; x < _screens.Length; x++)
                _screens[x].Button.onClick.RemoveListener(() => ShowScreen(_screens[x]));
        }
        else if (_type == TypeModeController.Swipe && _navigationButton)
        {
            _previousScreenButton.onClick.RemoveListener(() => NavigationScroll(-1));
            _nextScreenButton.onClick.RemoveListener(() => NavigationScroll(+1));
        }
    }

    //Type Click methods
    private void SettingScreensForClick()
    {
        _returnMainScreenButton.onClick.AddListener(ReturnInMainScreen);

        for (int x = 0; x < _screens.Length; x++)
        {
            int index = x;
            _screens[x].Button.onClick.AddListener(() => ShowScreen(_screens[index]));
        }
    }
    private void ShowScreen(ScreenPanel screenPanel)
    {
        _currentScreen = screenPanel.Screen;
        ActivateDeActivateScreen(true);
    }

    private void ReturnInMainScreen()
    {
        _returnMainScreenButton.gameObject.SetActive(false);
        ActivateDeActivateScreen(false);
    }

    private void ActivateDeActivateScreen(bool activate)
    {
        _fafeAnimator.Play(_fadeAnimName);
        _startScreen.SetActive(!activate);
        _returnMainScreenButton.gameObject.SetActive(activate);
        _currentScreen.SetActive(activate);
    }

    //Type Swipe methods
    private void SettingsScreensForSwipe()
    {
        _screenRect = GetComponent<RectTransform>();
        _startScreenPosition = _screenRect.anchoredPosition;
        _screenCount = _containerScreen.childCount;
        _currentScreenIndex = _startCreen > _screenCount - 1 ? _screenCount - 1 : _startCreen - 1;

        if (_navigationButton)
        {
            _previousScreenButton.onClick.AddListener(() => NavigationScroll(-1));
            _nextScreenButton.onClick.AddListener(() => NavigationScroll(+1));
            _previousScreenButton.gameObject.SetActive(true);
            _nextScreenButton.gameObject.SetActive(true);
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
            if (eventData.delta.x < 0 && _currentScreenIndex < _screenCount - 1 || eventData.delta.x > 0 && _currentScreenIndex > 0)
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
                if (dragDistance > 0.5f && _currentScreenIndex < _screenCount - 1)
                {
                    Debug.Log("Next");
                    _currentScreenIndex++;
                }
                else if (dragDistance < -0f && _currentScreenIndex > 0)
                {
                    Debug.Log("Back");
                    _currentScreenIndex--;
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
        if ((_currentScreenIndex + index) > _screenCount - 1 || (_currentScreenIndex + index) < 0) return;

        _currentScreenIndex += index;
        ScrollTo(_containerScreen, _currentScreenIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreensController : MonoBehaviour
{
    [SerializeField] private GameObject _startScreen;
    [SerializeField] private GameObject _oneScreen;
    [SerializeField] private GameObject _twoSreen;
    [Space]
    [SerializeField] private Button _oneScreenButton;
    [SerializeField] private Button _twoScreenButton;
    [SerializeField] private Button _mainScreenButton;


    private GameObject _currentScreen;

    private void Start()
    {
        _oneScreenButton.onClick.AddListener(ShowOneScreen);
        _twoScreenButton.onClick.AddListener(ShowTwoScreen);
        _mainScreenButton.onClick.AddListener(ReturnInMainScreen);

        _startScreen.SetActive(true);
    }


    private void OnDestroy()
    {
        _oneScreenButton.onClick.RemoveListener(ShowOneScreen);
        _twoScreenButton.onClick.RemoveListener(ShowTwoScreen);
        _mainScreenButton.onClick.RemoveListener(ReturnInMainScreen);
    }

    private void ShowOneScreen()
    {
        _currentScreen = _oneScreen;

        ActivateDeActivateScreen(true);
    }

    private void ShowTwoScreen()
    {
        _currentScreen = _twoSreen;

        ActivateDeActivateScreen(true);
    }

    private void ReturnInMainScreen()
    {
        ActivateDeActivateScreen(false);
    }

    private void ActivateDeActivateScreen(bool activate)
    {
        _startScreen.SetActive(!activate);
        _currentScreen.SetActive(activate);
    }
}

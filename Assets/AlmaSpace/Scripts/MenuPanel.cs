using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _aboutButton;
    [Space]
    [SerializeField] private Button _soundButton;
    [SerializeField] private TextMeshProUGUI _textSoundButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TextMeshProUGUI _textExitButton;
    [Space]
    [SerializeField] private Animator _animatorControllerMenu;

    const string _animMenu = "ShowMenu";

    const string _textOnMusic = "Включить музыку";
    const string _textOffMusic = "Выключить музыку";
    const string _textExitApp = "Выйти из программы";
    const string _textExitToMenu = "Выйти в меню";

    private void Start()
    {
        _menuButton.onClick.AddListener(ShowMenu);
        _aboutButton.onClick.AddListener(AboutCompany);
        _soundButton.onClick.AddListener(ActiveDeactiveSound);
    }

    private void OnDestroy()
    {
        _menuButton.onClick.RemoveListener(ShowMenu);
        _aboutButton.onClick.RemoveListener(AboutCompany);
        _soundButton.onClick.RemoveListener(ActiveDeactiveSound);
    }

    private void ShowMenu()
    {
        _animatorControllerMenu.SetBool(_animMenu, true);

        if (GameManager.Instance.InMenu)
        {
            _textExitButton.text = _textExitApp;
            _exitButton.onClick.AddListener(MessageBoxForExitApp);
        }
        else
        {
            _textExitButton.text = _textExitToMenu;
            _exitButton.onClick.AddListener(ExitToMenu);
        }
    }

    public void HideMenu()
    {
        _animatorControllerMenu.SetBool(_animMenu, false);

        if (GameManager.Instance.InMenu)
            _exitButton.onClick.RemoveListener(MessageBoxForExitApp);
        else
            _exitButton.onClick.RemoveListener(ExitToMenu);
    }

    private void AboutCompany()
    {

    }

    private void ActiveDeactiveSound()
    {
        GameManager.Instance.AudioSettings.OnOffMusic();

        if (!GameManager.Instance.AudioSettings.Music)
            _textSoundButton.text = _textOnMusic;
        else
            _textSoundButton.text = _textOffMusic;
    }

    private void ExitToMenu()
    {
        GameManager.Instance.ReturnInMenu();
        HideMenu();
    }

    private void MessageBoxForExitApp()
    {

    }

    private void ExitApp()
    {
        Application.Quit();
    }
}

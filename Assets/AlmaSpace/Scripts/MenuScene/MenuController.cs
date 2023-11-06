using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VolumeBox.Utils;
using UnityEngine.EventSystems;
using AlmaSpace;
using System;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject _backGroundPanel;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _aboutButton;
    [Space]
    [SerializeField] private Button _soundButton;
    [SerializeField] private TextMeshProUGUI _textSoundButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TextMeshProUGUI _textExitButton;
    [Space]
    [SerializeField] private Animator _animatorControllerMenu;

    private const string _animMenu = "ShowMenu";
    private const string _animAbout = "About";

    private const string _textOnMusic = "Включить музыку";
    private const string _textOffMusic = "Выключить музыку";
    private const string _textExitApp = "Выйти из программы";
    private const string _textExitToMenu = "Выйти в меню";

    private void Start()
    {
        _menuButton.onClick.AddListener(ShowMenu);
        _aboutButton.onClick.AddListener(ShowAboutCompany);
        _soundButton.onClick.AddListener(ActiveDeactiveSound);
    }

    private void OnDestroy()
    {
        _menuButton.onClick.RemoveListener(ShowMenu);
        _aboutButton.onClick.RemoveListener(ShowAboutCompany);
        _soundButton.onClick.RemoveListener(ActiveDeactiveSound);
    }

    private void ShowMenu()
    {
        _backGroundPanel.SetActive(true);
        _animatorControllerMenu.SetBool(_animMenu, true);

        if (AlmaSpaceManager.Instance.InMenu)
        {
            _soundButton.interactable = true;
            _textExitButton.text = _textExitApp;
            _exitButton.onClick.AddListener(MessageBoxForExitApp);
        }
        else
        {
            _soundButton.interactable = false;
            _textExitButton.text = _textExitToMenu;
            _exitButton.onClick.AddListener(MessageBoxForExitToMenu);
        }
    }

    public void HideMenu()
    {
        _backGroundPanel.SetActive(false);
        _animatorControllerMenu.SetBool(_animMenu, false);

        if (AlmaSpaceManager.Instance.InMenu)
            _exitButton.onClick.RemoveListener(MessageBoxForExitApp);
        else
            _exitButton.onClick.RemoveListener(MessageBoxForExitToMenu);
    }

    private void ShowAboutCompany()
    {
        _animatorControllerMenu.SetBool(_animAbout, true);
    }

    public void HideAboutCompany()
    {
        _animatorControllerMenu.SetBool(_animAbout, false);
    }

    private void ActiveDeactiveSound()
    {
        AlmaSpaceManager.Instance.AudioManager.OnOffMusic();

        if (!AlmaSpaceManager.Instance.AudioManager.Music)
            _textSoundButton.text = _textOnMusic;
        else
            _textSoundButton.text = _textOffMusic;
    }

    private void ExitToMenu()
    {
        AlmaSpaceManager.Instance.ReturnInMenu();
        HideMenu();
    }

    private void MessageBoxForExitToMenu()
    {
        MessageBox.Show(new MessageBoxData
        {
            HeaderCaption = "Подтвердите действие",
            MainCaption = "Вы действительно хотите выйти?",
            CancelCaption = "Отмена",
            OkCaption = "Да",
            OkAction = ExitApp
        });
    }

    private void MessageBoxForExitApp()
    {
        MessageBox.Show(new MessageBoxData
        {
            HeaderCaption = "Подтвердите действие",
            MainCaption = "Вы действительно хотите выйти в меню?",
            CancelCaption = "Отмена",
            OkCaption = "Да",
            OkAction = ExitToMenu
        });
    }

    private void ExitApp()
    {
        Application.Quit();
    }
}

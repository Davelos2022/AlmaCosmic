using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AlmaSpace;
using VolumeBox.Utils;

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
    [Space]
    [SerializeField] private Sprite _offMusicSprite;
    [SerializeField] private Sprite _onMusicSprite;

    private const string _animMenu = "ShowMenu";
    private const string _animAbout = "About";

    private const string _textOnMusic = "Включить музыку";
    private const string _textOffMusic = "Выключить музыку";
    private const string _textExitApp = "Выйти из программы";
    private const string _textExitToMenu = "Выйти в меню";

    private void Start()
    {
        _menuButton.onClick.AddListener(ShowAboutCompany);
        _soundButton.onClick.AddListener(ActiveDeactiveSound);

        //_menuButton.onClick.AddListener(ShowMenu);
        //_aboutButton.onClick.AddListener(ShowAboutCompany);
    }

    private void OnDestroy()
    {
        _menuButton.onClick.RemoveListener(ShowAboutCompany);
        _soundButton.onClick.RemoveListener(ActiveDeactiveSound);

        //_menuButton.onClick.RemoveListener(ShowMenu);
        //_aboutButton.onClick.RemoveListener(ShowAboutCompany);
    }

    private void ShowMenu()
    {
        _backGroundPanel.SetActive(true);
        _animatorControllerMenu.SetBool(_animMenu, true);

        if (AlmaSpaceManager.Instance.InMenu)
        {
            _soundButton.interactable = true;
            _textExitButton.text = _textExitApp;
            _exitButton.onClick.AddListener(ExitApp);
        }
        else
        {
            _soundButton.interactable = false;
            _textExitButton.text = _textExitToMenu;
            _exitButton.onClick.AddListener(ExitToMenu);
        }
    }

    public void HideMenu()
    {
        _backGroundPanel.SetActive(false);
        _animatorControllerMenu.SetBool(_animMenu, false);

        if (AlmaSpaceManager.Instance.InMenu)
            _exitButton.onClick.RemoveListener(ExitApp);
        else
            _exitButton.onClick.RemoveListener(ExitToMenu);
    }

    private void ShowAboutCompany()
    {
        _backGroundPanel.SetActive(true);
        _animatorControllerMenu.SetBool(_animAbout, true);
    }

    public void HideAboutCompany()
    {
        _backGroundPanel.SetActive(false);
        _animatorControllerMenu.SetBool(_animAbout, false);
    }

    private void ActiveDeactiveSound()
    {
        AlmaSpaceManager.Instance.AudioManager.OnOffMusic();

        if (!AlmaSpaceManager.Instance.AudioManager.Music)
        {
            //_textSoundButton.text = _textOnMusic;
            _soundButton.image.sprite = _offMusicSprite;
        }
        else
        {
            //_textSoundButton.text = _textOffMusic;
            _soundButton.image.sprite = _onMusicSprite;
        }
    }

    private void ExitToMenu()
    {
        HideMenu();

        AlmaSpaceManager.Instance.ShowMessageBox(new MessageBoxData
        {
            HeaderCaption = "Подтвердите действие",
            MainCaption = "Вы действительно хотите выйти в меню?",
            CancelCaption = "Отмена",
            OkCaption = "Да",
            OkAction = AlmaSpaceManager.Instance.ReturnInMenu
        });
    }

    private void ExitApp()
    {
        AlmaSpaceManager.Instance.ShowMessageBox(new MessageBoxData
        {
            HeaderCaption = "Подтвердите действие",
            MainCaption = "Вы действительно хотите выйти?",
            CancelCaption = "Отмена",
            OkCaption = "Да",
            OkAction = AlmaSpaceManager.Instance.ExitApp
        });
    }
}

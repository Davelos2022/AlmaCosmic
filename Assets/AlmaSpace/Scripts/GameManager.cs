using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private AudioSettings _audioSettings;

    public AudioSettings AudioSettings => _audioSettings;
    private bool _inMenu; public bool InMenu => _inMenu;
    private string _pathToVideo; public string PathToVideo => _pathToVideo;

    private void Start()
    {
        _inMenu = true;
    }

    public async UniTask PlayGymnastics(string pathToFile)
    {
        _inMenu = false;
        _pathToVideo = pathToFile;
        _audioSettings.StopMusic();

        await _sceneController.LoadSceneAsync(SceneName.Gymnastics);
    }

    public async UniTask PlayAcrobatick()
    {
        _inMenu = false;
        _audioSettings.StopMusic();

        await _sceneController.LoadSceneAsync(SceneName.Acrobatics);
    }

    public void ReturnInMenu()
    {
        _inMenu = true;
        _audioSettings.PlauMusic();
        _sceneController.UnLoadScene();
    }
}

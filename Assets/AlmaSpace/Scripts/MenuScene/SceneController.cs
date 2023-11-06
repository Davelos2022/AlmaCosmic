using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum SceneName
{
    Gymnastics,
    Acrobatics
}

public class SceneController : MonoBehaviour
{
    private const string _sceneGymnastics = "Gymnastics_Scene";
    private const string _sceneAcrobatics = "Acrobatics_Scene";

    private Scene _loaderScene;

    public async UniTask LoadSceneAsync(SceneName sceneName)
    {
        AsyncOperation asyncLoad;
        string _currentActiveScene;

        switch (sceneName)
        {
            case SceneName.Acrobatics:
                asyncLoad = SceneManager.
             LoadSceneAsync(_sceneAcrobatics, LoadSceneMode.Additive);
                _currentActiveScene = _sceneAcrobatics;
                break;
            case SceneName.Gymnastics:
                asyncLoad = SceneManager.
                LoadSceneAsync(_sceneGymnastics, LoadSceneMode.Additive);
                _currentActiveScene = _sceneGymnastics;
                break;
            default:
                return;
        }

        while (!asyncLoad.isDone)
        {
            await UniTask.Yield();
        }

        _loaderScene = SceneManager.GetSceneByName(_currentActiveScene);
    }

    public async void UnLoadScene()
    {
        if (_loaderScene.isLoaded)
        {
            await SceneManager.UnloadSceneAsync(_loaderScene);
        }
    }
}

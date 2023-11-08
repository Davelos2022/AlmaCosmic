using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum SceneName
{
    VideoViwer
}

public class SceneController : MonoBehaviour
{
    private const string _sceneVideoViwer = "VideoViwer_Scene";

    private Scene _loaderScene;

    public async UniTask LoadSceneAsync(SceneName sceneName)
    {
        AsyncOperation asyncLoad;
        string _currentActiveScene;

        switch (sceneName)
        {
            case SceneName.VideoViwer:
                asyncLoad = SceneManager.
                LoadSceneAsync(_sceneVideoViwer, LoadSceneMode.Additive);
                _currentActiveScene = _sceneVideoViwer;
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

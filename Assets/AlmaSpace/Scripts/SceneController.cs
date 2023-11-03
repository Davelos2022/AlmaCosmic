using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum SceneName
{
    Gymnastics,
    Acrobatics
}

public class SceneController : MonoBehaviour
{
    const string _sceneGymnastics = "Gymnastics_Scene";
    const string _sceneAcrobatics = "Acrobatics_Scene";

    private Scene _loaderScene;
    public async UniTask LoadSceneAsync(SceneName sceneName)
    {
        AsyncOperation asyncLoad;
        string _curretnActiveScene;

        switch (sceneName)
        {
            case SceneName.Acrobatics:
                asyncLoad = SceneManager.
             LoadSceneAsync(_sceneAcrobatics, LoadSceneMode.Additive);
                _curretnActiveScene = _sceneAcrobatics;
                break;
            case SceneName.Gymnastics:
                asyncLoad = SceneManager.
                LoadSceneAsync(_sceneGymnastics, LoadSceneMode.Additive);
                _curretnActiveScene = _sceneGymnastics;
                break;
            default:
                return;
        }

        while (!asyncLoad.isDone)
        {
            await UniTask.Yield();
        }

        _loaderScene = SceneManager.GetSceneByName(_curretnActiveScene);
    }

    public async void UnLoadScene()
    {
        if (_loaderScene.isLoaded)
        {
            await SceneManager.UnloadSceneAsync(_loaderScene);
        }
    }
}

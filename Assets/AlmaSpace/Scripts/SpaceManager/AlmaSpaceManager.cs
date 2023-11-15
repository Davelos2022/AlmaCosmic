using Cysharp.Threading.Tasks;
using UnityEngine;
using VolumeBox.Utils;

namespace AlmaSpace
{
    public enum TypeLesson
    {
        Gymnastick,
        Zvukoreche
    }

    public class AlmaSpaceManager : Singleton<AlmaSpaceManager>
    {
        [SerializeField] private SceneController _sceneController;
        [SerializeField] private AudioManager _audioManager;

        public TypeLesson TypeLesson { get; set; }
        public AudioManager AudioManager => _audioManager;
        //private bool _inMenu; public bool InMenu => _inMenu;
        private string[] _currentVideosPaths; public string[] CurrentVideos => _currentVideosPaths;
        private string _currentVideoPath; public string CurrentVideo => _currentVideoPath;

        private void Start()
        {            
            //_inMenu = true;
        }

        public async UniTask PlayVideoViwer(string[] pathsToVideoFiles, int indexVideo)
        {
            //_inMenu = false;
            _currentVideosPaths = pathsToVideoFiles;
            _currentVideoPath = pathsToVideoFiles[indexVideo];
            _audioManager.StopClip();

            await _sceneController.LoadSceneAsync(SceneName.VideoViwer);
        }
        public void ShowMessageBox(MessageBoxData data)
        {
            MessageBox.Show(data);
        }

        public void ReturnInMenu()
        {
            _sceneController.UnLoadScene();
            //_inMenu = true;

            if (!AudioManager.Music)
                AudioManager.OnOffMusic();
        }

        public void ExitApp()
        {
            Application.Quit();
        }
    }
}

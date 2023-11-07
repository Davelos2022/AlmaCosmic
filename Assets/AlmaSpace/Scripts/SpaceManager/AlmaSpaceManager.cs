using Cysharp.Threading.Tasks;
using UnityEngine;
using VolumeBox.Utils;
using UnityEngine.Android;
using System.Collections.Generic;
using System.Collections;
using System;

namespace AlmaSpace
{
    public class AlmaSpaceManager : Singleton<AlmaSpaceManager>
    {
        [SerializeField] private SceneController _sceneController;
        [SerializeField] private AudioManager _audioManager;

        public AudioManager AudioManager => _audioManager;
        private bool _inMenu; public bool InMenu => _inMenu;

        //Gymnastick Propirties
        private string[] _currentVideosPaths; public string[] CurrentVideos => _currentVideosPaths;
        private string _pathPlayVideo; public string CurrentVideo => _pathPlayVideo;

        //Acrobatick Propirties


        private void Start()
        {
            //StartCoroutine(RequestStoragePermission(CallBack));
            _inMenu = true;
        }

        public async UniTask PlayGymnastics(string[] pathsToVideoFiles, int indexVideo)
        {
            _inMenu = false;
            _currentVideosPaths = pathsToVideoFiles;
            _pathPlayVideo = pathsToVideoFiles[indexVideo];
            _audioManager.StopMusic();

            await _sceneController.LoadSceneAsync(SceneName.Gymnastics);
        }

        public async UniTask PlayAcrobatick()
        {
            _inMenu = false;
            _audioManager.StopMusic();

            await _sceneController.LoadSceneAsync(SceneName.Acrobatics);
        }

        public void ShowMessageBox(MessageBoxData data)
        {
            MessageBox.Show(data);
        }

        public void ReturnInMenu()
        {
            _sceneController.UnLoadScene();
            _inMenu = true;

            if (_audioManager.Music)
                _audioManager.PlauMusic();
        }

        public void ExitApp()
        {
            Application.Quit();
        }

        private IEnumerator RequestStoragePermission(Action<bool> callback)
        {
            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                callback?.Invoke(true);
            }
            else
            {
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
                yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead));
                callback?.Invoke(Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead));
            }
        }

        private void CallBack(bool data)
        {
            if (data)
                Debug.Log("sddsds");
            else
                Debug.Log("sdsdsds");
        }
    }

}

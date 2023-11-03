using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OneScreenMenu : MonoBehaviour
{
    [SerializeField] private Button[] _buttons;

    private string _pathToVideo = Application.streamingAssetsPath + "/" + "Video";
    private string[] _videosPath;


    private void Start()
    {
        _videosPath = GetCountVideoFiles();

        for (int x = 0; x < _videosPath.Length; x++)
        {
            int path = x;
            _buttons[x].onClick.AddListener(() => ClickButton(path));
        }
    }

    private void OnDestroy()
    {
        for (int x = 0; x < _videosPath.Length; x++)
        {
            _buttons[x].onClick.RemoveListener(() => ClickButton(x));
        }
    }

    private async void ClickButton(int index)
    {
        await GameManager.Instance.PlayGymnastics(_videosPath[index]);
    }



    private string[] GetCountVideoFiles()
    {
        var checkFormats = new[] { ".mp4" };

        var countFiles = Directory
            .GetFiles(_pathToVideo)
            .Where(file => checkFormats.Any(file.ToLower().EndsWith))
            .ToArray();

        return countFiles;
    }
}

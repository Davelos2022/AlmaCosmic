using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using AlmaSpace;

public class GymnasticsMenu : MonoBehaviour
{
    [SerializeField] private Button[] _buttonsJuniorGroup;
    [SerializeField] private Button[] _buttonsSeniorGroup;

    private string _pathToVideo = Application.streamingAssetsPath + "/" + "Video/";
    private const string _juniorGroup = "JuniorGroup";
    private const string _seniorGroup = "SeniorGroup";

    private string[] _videosPathsJunior;
    private string[] _videosPathsSenior;

    private void Start()
    {
        _videosPathsJunior = GetCountVideoFiles(_juniorGroup);
        _videosPathsSenior = GetCountVideoFiles(_seniorGroup);

        InsalizationButton(_buttonsJuniorGroup, _videosPathsJunior);
        InsalizationButton(_buttonsSeniorGroup, _videosPathsSenior);
    }

    private void OnDestroy()
    {
        ClearListenerButtons(_buttonsJuniorGroup);
        ClearListenerButtons(_buttonsSeniorGroup);
    }

    private void InsalizationButton(Button[] buttons, string[] paths)
    {
        for (int x = 0; x < paths.Length; x++)
        {
            int index = x;

            if (x > buttons.Length - 1)
            {
                Debug.Log($"Файлов больше чем кнопок, добавьте кнопки для {buttons}");
                break;
            }

            buttons[x].onClick.AddListener(() => ClickButton(paths, index));
        }
    }

    private void ClearListenerButtons(Button[] buttons)
    {
        for (int x = 0; x < buttons.Length; x++)
            buttons[x].onClick.RemoveAllListeners();

    }

    private async void ClickButton(string[] paths, int index)
    {
        await AlmaSpaceManager.Instance.PlayGymnastics(paths, index);
    }

    private string[] GetCountVideoFiles(string folder)
    {
        var checkFormats = new[] { ".mp4" };

        var countFiles = Directory
            .GetFiles(_pathToVideo + folder)
            .Where(file => checkFormats.Any(file.ToLower().EndsWith))
            .ToArray();

        return countFiles;
    }
}
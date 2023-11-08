using UnityEngine;
using UnityEngine.UI;
using AlmaSpace;

public class GymnasticsMenu : MonoBehaviour
{
    [SerializeField] private Button[] _buttonsJuniorGroup;
    [SerializeField] private Button[] _buttonsSeniorGroup;
    [Space]
    [SerializeField] private string[] _nameFileJuniorForAndroid;
    [SerializeField] private string[] _nameFileSeniorForAndroid;

    private string[] _videosPathsJunior;
    private string[] _videosPathsSenior;

    private void Start()
    {
        InsalizationsVideoPath();

        InsalizationButton(_buttonsJuniorGroup, _videosPathsJunior);
        InsalizationButton(_buttonsSeniorGroup, _videosPathsSenior);
    }

    private void OnDestroy()
    {
        ClearListenerButtons(_buttonsJuniorGroup);
        ClearListenerButtons(_buttonsSeniorGroup);
    }

    private void InsalizationsVideoPath()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            _videosPathsJunior = FileManager.GetCountVideoFiles(FileManager._pathToVideoGymnastics + FileManager._juniorGroupGymnastics);
            _videosPathsSenior = FileManager.GetCountVideoFiles(FileManager._pathToVideoGymnastics + FileManager._seniorGroupGymnastics);
        }
        else
        {
            _videosPathsJunior = FileManager.GetCountVideoFiles(FileManager._pathToVideoGymnastics + FileManager._juniorGroupGymnastics, _nameFileJuniorForAndroid);
            _videosPathsSenior = FileManager.GetCountVideoFiles(FileManager._pathToVideoGymnastics + FileManager._seniorGroupGymnastics, _nameFileSeniorForAndroid);
        }
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
        await AlmaSpaceManager.Instance.PlayVideoViwer(paths, index);
    }

}
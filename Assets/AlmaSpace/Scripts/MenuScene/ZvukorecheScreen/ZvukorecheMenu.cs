using AlmaSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZvukorecheMenu : MonoBehaviour
{
    [SerializeField] private Button[] _buttonsCard;
    [Space]
    [SerializeField] private AudioClip _startClip;
    [SerializeField] private ZvukorecheData _zvukorecheData;
    [SerializeField] private Button _startButton;

    private string[] _videosPaths;
    private int _currentIndexCard;
    private List<string> _nameFilesVideo = new List<string>();

    void Start()
    {
        InsalizationButton();
    }

    private void OnEnable()
    {
        AlmaSpaceManager.Instance.AudioManager.PlayClip(_startClip);
    }

    private void InsalizationsVideoPath()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            _videosPaths = FileManager.GetCountVideoFiles(FileManager._pathToVideoZvukoreche);
        }
        else
        {
            _videosPaths = FileManager.GetCountVideoFiles(FileManager._pathToVideoZvukoreche, _nameFilesVideo.ToArray());
        }
    }

    private void InsalizationButton()
    {
        _startButton.onClick.AddListener(StartPlay);

        for (int x = 0; x < _buttonsCard.Length; x++)
        {
            int index = x;

            _buttonsCard[x].GetComponent<ButtonCard>().SetInfoCard(_zvukorecheData.datas[x]);
            _buttonsCard[x].onClick.AddListener(() => ShowInfoCard(index));

            _nameFilesVideo.Add(_zvukorecheData.datas[x]._videoVileName);
        }

        InsalizationsVideoPath();
    }

    private void ShowInfoCard(int index)
    {
        _currentIndexCard = index;
    }


    private async void StartPlay()
    {
        await AlmaSpaceManager.Instance.PlayVideoViwer(_videosPaths, _currentIndexCard);
    }

   
}

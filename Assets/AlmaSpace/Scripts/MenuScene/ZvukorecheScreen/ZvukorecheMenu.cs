using AlmaSpace;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZvukorecheMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private PreviewBox _previewBox;
    [SerializeField] private AudioClip _startClip;
    [SerializeField] private ButtonCard[] _buttonsCard;
    [Space]
    [SerializeField] private ZvukorecheScr _zvukorecheScr;

    private string[] _videosPaths;
    private int _currentIndexCard;

    void Start()
    {
        InsalizationButton();
        ShowInfo(0);
    }

    private void OnEnable()
    {
        AlmaSpaceManager.Instance.AudioManager.PlayClip(_startClip);
    }

    private void OnDisable()
    {
        AlmaSpaceManager.Instance.AudioManager.StopClip();
    }

    private void InsalizationButton()
    {
        _startButton.onClick.AddListener(StartPlay);
        List<string> paths = new List<string>();

        for (int x = 0; x < _buttonsCard.Length; x++)
        {
            int index = x;

            _buttonsCard[x].SetInfoCardZvukoreche(_zvukorecheScr.Datas[x]);
            _buttonsCard[x].GetComponent<Button>().onClick.AddListener(() => GetInfoButtonCard(index));

            paths.Add(Application.streamingAssetsPath + "/" + _zvukorecheScr.Datas[x].VideoVilePath);
        }

        _videosPaths = paths.ToArray();
    }

    private void GetInfoButtonCard(int indexCard)
    {
        _buttonsCard[_currentIndexCard].SelectedCard(false);
        _buttonsCard[indexCard].SelectedCard(true);

        ShowInfo(indexCard);
        AlmaSpaceManager.Instance.AudioManager.PlayClip(_zvukorecheScr.Datas[indexCard].AudioClip);
        _currentIndexCard = indexCard;
    }

    private void ShowInfo(int indexData)
    {
        _previewBox.SetInfoPreview(_zvukorecheScr.Datas[indexData]);
    }

    private async void StartPlay()
    {
        await AlmaSpaceManager.Instance.PlayVideoViwer(_videosPaths, _currentIndexCard);
    }
}

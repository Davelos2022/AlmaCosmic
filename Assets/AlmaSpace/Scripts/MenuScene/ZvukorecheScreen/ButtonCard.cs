
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonCard : MonoBehaviour
{
    [SerializeField] private Image imageCard;
    [SerializeField] private TextMeshProUGUI _headerTMP;

    private AudioClip _clipCard;
    private string _descriptionText;
    private string _headerText;  public string HeaderText => _headerText;
    private string _videoFile; public string VideoFile => _videoFile;


    public void SetInfoCard(DataInfo dataInfo)
    {
        imageCard.sprite = dataInfo._sprite;
        _headerText = dataInfo._headerText;
        _descriptionText = dataInfo._descriptionText;
        _videoFile = dataInfo._videoVileName;
        _clipCard = dataInfo.audioClip;

        _headerTMP.text = _headerText;
    }
}

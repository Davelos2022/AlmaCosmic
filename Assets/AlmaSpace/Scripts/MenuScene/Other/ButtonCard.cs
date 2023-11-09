using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ButtonCard : MonoBehaviour
{
    [SerializeField] private Image imageCard;
    [SerializeField] private TextMeshProUGUI _headerTMP;
    [SerializeField] private Image _selectedImage;

    private AudioClip _clipCard;
    private string _descriptionText;
    private string _headerText;
    private string _videoFile;

    public AudioClip AudioClip => _clipCard;
    public string DescriptionText => _descriptionText;
    public string HeaderText => _headerText;
    public string VideoFile => _videoFile;

    public void SetInfoCardZvukoreche(DataZvukoreche dataZvukoreche)
    {
        imageCard.sprite = dataZvukoreche.SpriteButton;
        _headerText = dataZvukoreche.HeaderText;
        _descriptionText = dataZvukoreche.DescriptionText;
        _videoFile = dataZvukoreche.VideoVilePath;
        _clipCard = dataZvukoreche.AudioClip;

        _headerTMP.text = _headerText;
    }

    public void SetInfoCardGymnastick(DataGymnastics dataGymnastics)
    {
        imageCard.sprite = dataGymnastics.SpriteButton;
        _videoFile = dataGymnastics.VideoPath;
    }

    private void ClearListenerButtons(ButtonCard buttonCard)
    {
        buttonCard.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void SelectedCard(bool selected)
    {
        _selectedImage.gameObject.SetActive(selected);
    }

    private void OnDestroy()
    {
        ClearListenerButtons(this);
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PreviewBox : MonoBehaviour
{
    [SerializeField] private Image _imageBoxPreview;
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    public void SetInfoPreview(DataZvukoreche dataZvukoreche)
    {
        _imageBoxPreview.sprite = dataZvukoreche.SpriteButton;
        _headerText.text = dataZvukoreche.HeaderText;
        _descriptionText.text = dataZvukoreche.DescriptionText;

        _imageBoxPreview.preserveAspect = true;
    }
}

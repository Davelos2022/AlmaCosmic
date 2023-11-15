using UnityEngine;
using UnityEngine.UI;
using AlmaSpace;
using System.Collections.Generic;

public enum TypeGroup
{
    Junior,
    Semior
}

public class GymnasticsScreen : MonoBehaviour
{
    [SerializeField] private ButtonCard[] _buttonsCardsJuniorGroup;
    [SerializeField] private ButtonCard[] _buttonsCardsSeniorGroup;
    [SerializeField] private GymnasticsScr _gymnasticsScr;

    private string[] _videosPathsJunior;
    private string[] _videosPathsSenior;

    private void Start()
    {
        InsalizationButton(_buttonsCardsJuniorGroup, _videosPathsJunior, TypeGroup.Junior);
        InsalizationButton(_buttonsCardsSeniorGroup, _videosPathsSenior, TypeGroup.Semior);
    }

    private void InsalizationButton(ButtonCard[] buttonsCards, string[] videoPaths, TypeGroup typeGroup)
    {
        List<string> paths = new List<string>();

        for (int x = 0; x < buttonsCards.Length; x++)
        {
            int index = x;

            switch (typeGroup)
            {
                case TypeGroup.Junior:
                    buttonsCards[x].SetInfoCardGymnastick(_gymnasticsScr.JuniorGroup[x]);
                    paths.Add(Application.streamingAssetsPath + "/" + _gymnasticsScr.JuniorGroup[x].VideoPath);
                    break;
                case TypeGroup.Semior:
                    buttonsCards[x].SetInfoCardGymnastick(_gymnasticsScr.SeniorGroup[x]);
                    paths.Add(Application.streamingAssetsPath + "/" + _gymnasticsScr.SeniorGroup[x].VideoPath);
                    break;
                default:
                    break;
            }

            buttonsCards[x].GetComponent<Button>().onClick.AddListener(() => ClickButton(videoPaths, index));
        }

        videoPaths = paths.ToArray();
    }

    private async void ClickButton(string[] paths, int index)
    {
        AlmaSpaceManager.Instance.TypeLesson = TypeLesson.Gymnastick;
        await AlmaSpaceManager.Instance.PlayVideoViwer(paths, index);
    }
}
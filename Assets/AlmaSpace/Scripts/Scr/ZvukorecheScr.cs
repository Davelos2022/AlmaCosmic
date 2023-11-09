using UnityEngine;


[CreateAssetMenu(fileName = "DataZvukoreche", menuName = "Data Zvukoreche", order = 51)]
public class ZvukorecheScr : ScriptableObject
{
    public DataZvukoreche[] Datas;
}

[System.Serializable]
public class DataZvukoreche
{
    public Sprite SpriteButton;
    public string HeaderText;
    public string DescriptionText;
    public AudioClip AudioClip;
    public string VideoVilePath;
}

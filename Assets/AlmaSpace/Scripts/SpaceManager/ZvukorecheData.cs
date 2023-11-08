using UnityEngine;


[CreateAssetMenu(fileName = "DataZvukoreche", menuName = "Data", order = 51)]
public class ZvukorecheData : ScriptableObject
{
    public DataInfo[] datas;
}


[System.Serializable]
public class DataInfo
{
    public Sprite _sprite;
    public string _headerText;
    public string _descriptionText;
    public AudioClip audioClip;
    public string _videoVileName;
}

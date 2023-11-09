using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataGymnastics", menuName = "Data Gymnastics", order = 51)]
public class GymnasticsScr : ScriptableObject
{
    public DataGymnastics[] JuniorGroup;
    public DataGymnastics[] SeniorGroup;
}

[System.Serializable]
public class DataGymnastics
{
    public Sprite SpriteButton;
    public string VideoPath;
}

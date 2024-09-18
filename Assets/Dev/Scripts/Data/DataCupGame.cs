using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataCup
{
    public Sprite sprImgSelect;
    public GameObject prefabCup;
    public Sprite sprImgChange;
}

[CreateAssetMenu(fileName = "DataCupGame", menuName = "DataConfig/DataCupGame", order = 1)]
public class DataCupGame : ScriptableObject
{
    public List<DataCup> dataCups = new();
}

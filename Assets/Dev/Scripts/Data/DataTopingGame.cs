using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum TYPE_TOPING
{
    DEFAULT,
    DA
}

[Serializable]
public class DataToping
{
    public Sprite spr;
    public TYPE_TOPING type;
}

[CreateAssetMenu(fileName = "DataToping", menuName = "DataConfig/DataToping", order = 1)]
public class DataTopingGame : ScriptableObject
{
    public List<DataToping> dataToping = new();
}


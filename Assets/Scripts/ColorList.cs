using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ColorList
{
    public List<ColorObject> colors;
}

[System.Serializable]
public struct ColorObject
{
    public string id;
    public Color color;
}
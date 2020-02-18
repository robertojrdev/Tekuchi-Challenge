using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CombinationList
{
    public List<Combination> combinations;
}

[System.Serializable]
public struct Combination
{
    public string id;
    public List<CombinationItem> items;
}

[System.Serializable]
public struct CombinationItem
{
    public TransformData transformData;
    public string colorID;
}

[System.Serializable]
public struct TransformData
{
    public Vector3 position;
    public Vector3 scale;
    public Vector3 rotation;
}
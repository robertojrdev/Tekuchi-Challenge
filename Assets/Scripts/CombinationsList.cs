using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CombinationList
{
    const string WARNING_EMPTY = "The combination list is empty";
    const string WARNING_NOT_FOUND = "The combination list does not contain the id: ";

    public List<Combination> combinations;

    public bool GetCombination(string id, out Combination combination)
    {
        if(combinations == null || combinations.Count == 0)
        {
            Debug.LogWarning(WARNING_EMPTY);
            combination = default;
            return false;
        }

        var index = combinations.FindIndex(x => x.id == id);

        if(index == -1)
        {
            Debug.LogWarning(WARNING_NOT_FOUND + id);
            combination = default;
            return false;
        }

        combination = combinations[index];
        return true;
    }
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
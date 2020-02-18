using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ColorList
{
    const string WARNING_EMPTY = "The color list is empty";
    const string WARNING_NOT_FOUND = "The color list does not contain the id: ";

    public List<ColorObject> colors;

    public bool GetColor(string id, out Color color)
    {
        if(colors == null || colors.Count == 0)
        {
            Debug.LogWarning(WARNING_EMPTY);
            color = Color.magenta;
            return false;
        }

        var index = colors.FindIndex(x => x.id == id);

        if(index == -1)
        {
            Debug.LogWarning(WARNING_NOT_FOUND + id);
            color = Color.magenta;
            return false;
        }
        
        color = colors[index].color;
        return true;
    }
}

[System.Serializable]
public struct ColorObject
{
    public string id;
    public Color color;
}
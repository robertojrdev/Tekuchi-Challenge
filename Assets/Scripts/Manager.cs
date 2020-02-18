using UnityEngine;
// using Newtonsoft.Json;

public class Manager : MonoBehaviour
{
    public TextAsset jsonObjects;
    public TextAsset jsonColors;

    public ColorList colors;
    public CombinationList combinations;

    private void Start()
    {
        colors = JsonUtility.FromJson<ColorList>(
            jsonColors.ToString());

        combinations = JsonUtility.FromJson<CombinationList>(
            jsonObjects.ToString());
    }
}

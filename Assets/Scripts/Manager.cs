using UnityEngine;
// using Newtonsoft.Json;

public class Manager : MonoBehaviour
{
    public Material objectsMaterial;
    public TextAsset jsonObjects;
    public TextAsset jsonColors;

    public ColorList colors;
    public CombinationList combinations;

    private void Start()
    {
        LoadFiles();
        GenerateObject("mario");
    }

    private void LoadFiles()
    {
        colors = JsonUtility.FromJson<ColorList>(
            jsonColors.ToString());

        combinations = JsonUtility.FromJson<CombinationList>(
            jsonObjects.ToString());
    }

    private void GenerateObject(string id)
    {
        Combination combination;

        if(combinations.GetCombination(id, out combination))
        {
            foreach (var item in combination.items)
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                cube.transform.localPosition = item.transformData.position;
                cube.transform.localScale = item.transformData.scale;
                cube.transform.eulerAngles = item.transformData.rotation;
                
                Color color;
                colors.GetColor(item.colorID, out color);

                var renderer = cube.GetComponent<MeshRenderer>();
                renderer.sharedMaterial = objectsMaterial;
                renderer.material.SetColor("_Color", color);
            }
        }
    }
}

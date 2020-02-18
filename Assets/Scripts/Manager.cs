using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Material objectsMaterial;
    public TextAsset jsonObjects;
    public TextAsset jsonColors;

    [Space(10), Header("UI")]
    public Transform buttonsContainer;
    public ItemButton itemButtonPrefab;

    public ColorList colors;
    public CombinationList combinations;

    private Dictionary<string, GameObject> instantiatedSculptures = 
        new Dictionary<string, GameObject>();

    private GameObject currentSculpture;


    private void Start()
    {
        LoadFiles();
        PopulateUIListItems();
    }

    private void LoadFiles()
    {
        colors = JsonUtility.FromJson<ColorList>(
            jsonColors.ToString());

        combinations = JsonUtility.FromJson<CombinationList>(
            jsonObjects.ToString());
    }

    private void PopulateUIListItems()
    {
        foreach (var c in combinations.combinations)
        {
            var itemButton = Instantiate(itemButtonPrefab, buttonsContainer);
            itemButton.SetId(c.id);
            itemButton.gameObject.name = "Item Button - " + c.id;

            itemButton.button.onClick.
                AddListener( () => DisplaySculpture(c.id));
        }
    }

    private void DisplaySculpture(string id)
    {
        //hide current sculpture
        if(currentSculpture)
        {
            currentSculpture.SetActive(false);
            currentSculpture = null;
        }

        //check if already has instantiated this object
        if(instantiatedSculptures.ContainsKey(id))
        {
            currentSculpture = instantiatedSculptures[id];
            currentSculpture.SetActive(true);
        }
        else
        {
            var sculpture = GenerateSculpture(id);
            if(sculpture)
            {
                currentSculpture = sculpture.gameObject;
                instantiatedSculptures.Add(id, sculpture.gameObject);
            }
            else
                Debug.LogWarning("Failed to create sculpture");
        }

    }

    private Transform GenerateSculpture(string id)
    {
        Combination combination;

        if(combinations.GetCombination(id, out combination))
        {
            var parent = new GameObject("id").transform;

            foreach (var item in combination.items)
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.parent = parent.transform;

                cube.transform.localPosition = item.transformData.position;
                cube.transform.localScale = item.transformData.scale;
                cube.transform.eulerAngles = item.transformData.rotation;
                
                Color color;
                colors.GetColor(item.colorID, out color);

                var renderer = cube.GetComponent<MeshRenderer>();
                renderer.sharedMaterial = objectsMaterial;
                renderer.material.SetColor("_Color", color);
            }

            return parent;
        }

        return null;
    }
}

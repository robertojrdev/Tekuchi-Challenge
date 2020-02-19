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

    private Dictionary<string, Sculpture> instantiatedSculptures = 
        new Dictionary<string, Sculpture>();

    private Sculpture currentSculpture;


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
            currentSculpture.Hide();
            currentSculpture = null;
        }

        //check if already has instantiated this object
        if(instantiatedSculptures.ContainsKey(id))
        {
            currentSculpture = instantiatedSculptures[id];
            currentSculpture.Show();
        }
        else
        {
            var sculpture = GenerateSculpture(id);
            if(sculpture)
            {
                currentSculpture = sculpture;
                instantiatedSculptures.Add(id, sculpture);
                sculpture.Show();
            }
            else
                Debug.LogWarning("Failed to create sculpture");
        }

    }

    private Sculpture GenerateSculpture(string id)
    {
        Combination combination;

        if(combinations.GetCombination(id, out combination))
        {
            var sculpture = new GameObject("id").AddComponent<Sculpture>();
            sculpture.material = objectsMaterial;

            foreach (var item in combination.items)
            {                
                Color color;
                colors.GetColor(item.colorID, out color);

                sculpture.AddPiece(item.transformData, color);
            }

            return sculpture;
        }

        return null;
    }
}

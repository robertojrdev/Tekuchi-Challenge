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

    private Sculpture? currentSculpture;
    private List<Cube> pool = new List<Cube>();
    private Transform poolParent;

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
                AddListener(() => DisplaySculpture(c.id));
        }
    }

    private void DisplaySculpture(string id)
    {
        //hide current sculpture
        if (currentSculpture.HasValue)
        {
            StartCoroutine(Sculpture.HideAnimation(currentSculpture.Value));

            currentSculpture = null;
        }

        //allocate cubes for new sculpture
        Combination combination;
        if (combinations.GetCombination(id, out combination))
        {
            var size = combination.items.Count;
            var cubes = AllocateCubes(size);

            currentSculpture = new Sculpture()
            {
                cubes = cubes,
                combination = combination
            };

            StartCoroutine(Sculpture.ShowAnimation(currentSculpture.Value, colors));
        }

    }

    private Cube[] AllocateCubes(int amount)
    {
        if(poolParent == null)
            poolParent = new GameObject("Pool").transform;

        var cubes = new Cube[amount];

        var cubesAvailable = 0;
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].busy)
            {
                pool[i].busy = true;
                cubes[cubesAvailable] = pool[i];
                cubesAvailable++;
                
                if(cubesAvailable == amount)
                    return cubes;
            }
        }
        
        var newCubesRequired = amount - cubesAvailable;

        for (int i = 0; i < newCubesRequired; i++)
        {
            var cube = Cube.GetCube(objectsMaterial);
            cube.busy = true;
            cube.transform.parent = poolParent;

            pool.Add(cube);
            cubes[i + cubesAvailable] = cube;
        }

        return cubes;
    }
}

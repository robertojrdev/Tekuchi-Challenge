using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemButton : MonoBehaviour
{
    public Button button;
    [SerializeField] private Text text;

    public string Id {get; private set;}

    private void Reset()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
    }

    public void SetId(string id)
    {
        Id = id;
        text.text = Id;
    }

}
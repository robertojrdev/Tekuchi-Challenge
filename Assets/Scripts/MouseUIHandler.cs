using UnityEngine;
using UnityEngine.EventSystems;

public class MouseUIHandler : MonoBehaviour
{
    public static bool isOverUI { get; private set; }

    private void Update()
    {
        isOverUI = EventSystem.current.IsPointerOverGameObject();
    }
}
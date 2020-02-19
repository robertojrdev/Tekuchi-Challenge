using UnityEngine;

public class Cube : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public bool busy = false;

    public static Cube GetCube(Material material)
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        var renderer = obj.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = material;

        obj.SetActive(false);

        var cube = obj.AddComponent<Cube>();
        cube.meshRenderer = renderer;
        return cube;
    }
}
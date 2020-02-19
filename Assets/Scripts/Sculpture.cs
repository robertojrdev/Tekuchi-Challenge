using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sculpture : MonoBehaviour
{
    List<TransformData> dataSets = new List<TransformData>();
    List<Transform> transforms = new List<Transform>();
    List<MeshRenderer> renderers = new List<MeshRenderer>();
    public Material material;

    public void AddPiece(TransformData data, Color color)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = transform;
        cube.SetActive(false);

        cube.transform.localPosition = data.position;
        cube.transform.localScale = data.scale;
        cube.transform.eulerAngles = data.rotation;

        var renderer = cube.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = material;

        var block = new MaterialPropertyBlock(); //this is more efficient than having one distinct material per obj
        block.SetColor("_Color", color);
        renderer.SetPropertyBlock(block);

        dataSets.Add(data);
        transforms.Add(cube.transform);
        renderers.Add(renderer);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ShowAnimation());
    }

    private IEnumerator ShowAnimation()
    {
        
        var count = transforms.Count;
        var block = new MaterialPropertyBlock();
        for (int i = 0; i < count; i++)
        {
            var randPos = Random.insideUnitSphere * 5 + Vector3.up * 5;
            transforms[i].position = randPos;
            transforms[i].gameObject.SetActive(true);
        }

        var originalPositions = new Vector3[count];
        var originalRotations = new Quaternion[count];

        for (int i = 0; i < count; i++)
        {
            originalPositions[i] = transforms[i].position;
            originalRotations[i] = transforms[i].rotation;
        }

        var time = 1f;
        var counter = 0f;
        while (counter <= time)
        {
            counter += Time.deltaTime;
            var t = Mathf.InverseLerp(0, time, counter);
            t = Easing.EaseOutBounce(0, 1, t);

            for (int i = 0; i < count; i++)
            {
                var newPos = Vector3.Slerp(
                    originalPositions[i], dataSets[i].position, t);

                var newRot = Quaternion.Lerp(
                    originalRotations[i], Quaternion.Euler(dataSets[i].rotation), t);


                renderers[i].GetPropertyBlock(block);
                block.SetFloat("_Cutoff", 1 - t);
                renderers[i].SetPropertyBlock(block);

                transforms[i].position = newPos;
                transforms[i].rotation = newRot;
            }

            yield return null;
        }
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(HideAnimation());
    }

    private IEnumerator HideAnimation()
    {
        var count = transforms.Count;
        var block = new MaterialPropertyBlock();

        var originalPositions = new Vector3[count];
        var originalRotations = new Quaternion[count];

        for (int i = 0; i < count; i++)
        {
            originalPositions[i] = transforms[i].position;
            originalRotations[i] = transforms[i].rotation;
        }

        var time = 2f;
        var counter = 0f;
        while (counter <= time)
        {
            counter += Time.deltaTime;
            var t = Mathf.InverseLerp(0, time, counter);
            t = Easing.EaseOutCirc(0, 1, t);

            for (int i = 0; i < count; i++)
            {
                var newPos = Vector3.Slerp(
                    originalPositions[i], originalPositions[i] + Vector3.up * 5, t);

                var newRot = Quaternion.Lerp(
                    originalRotations[i], Quaternion.Inverse(originalRotations[i]), t);


                renderers[i].GetPropertyBlock(block);
                block.SetFloat("_Cutoff", t);
                renderers[i].SetPropertyBlock(block);

                transforms[i].position = newPos;
                transforms[i].rotation = newRot;
            }

            yield return null;
        }
        
        gameObject.SetActive(false);
    }
}
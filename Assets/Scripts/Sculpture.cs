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

        //place objects in a random position above the stage
        for (int i = 0; i < count; i++)
        {
            var randPos = Random.insideUnitSphere * 5 + Vector3.up * 5;
            transforms[i].position = randPos;
            transforms[i].gameObject.SetActive(true);
        }

        //store random positions to lerp
        var originalPositions = new Vector3[count];
        var originalRotations = new Quaternion[count];
        for (int i = 0; i < count; i++)
        {
            originalPositions[i] = transforms[i].localPosition;
            originalRotations[i] = transforms[i].localRotation;
        }

        //start animation loop
        var time = 1f;
        var counter = 0f;
        while (counter <= time)
        {
            counter += Time.deltaTime;
            //lerp using an ease function
            var t = Easing.EaseOutBounce(0, time, counter);

            //apply animation to each object individually
            for (int i = 0; i < count; i++)
            {
                //slerp cause a circular movement effect
                var newPos = Vector3.Slerp(
                    originalPositions[i], dataSets[i].position, t);

                var newRot = Quaternion.Lerp(
                    originalRotations[i], Quaternion.Euler(dataSets[i].rotation), t);

                //fade-in
                renderers[i].GetPropertyBlock(block);
                block.SetFloat("_Cutoff", 1 - t);

                //apply animation
                renderers[i].SetPropertyBlock(block);
                transforms[i].localPosition = newPos;
                transforms[i].localRotation = newRot;
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

        //store initial positions to lerp
        var originalPositions = new Vector3[count];
        var originalRotations = new Quaternion[count];
        for (int i = 0; i < count; i++)
        {
            originalPositions[i] = transforms[i].localPosition;
            originalRotations[i] = transforms[i].localRotation;
        }

        //start animation loop
        var time = .9f;
        var counter = 0f;
        while (counter <= time)
        {
            counter += Time.deltaTime;
            //lerp using an ease function
            var t = Easing.EaseOutExpo(0, time, counter);

            //apply animation to each object individually
            for (int i = 0; i < count; i++)
            {
                //slerp cause a circular movement effect
                var newPos = Vector3.Slerp(
                    originalPositions[i], originalPositions[i] + Vector3.up * 5, t);

                var newRot = Quaternion.Lerp(
                    originalRotations[i], Quaternion.Inverse(originalRotations[i]), t);

                //fade-out
                renderers[i].GetPropertyBlock(block);
                block.SetFloat("_Cutoff", t);

                //apply animation
                renderers[i].SetPropertyBlock(block);
                transforms[i].localPosition = newPos;
                transforms[i].localRotation = newRot;
            }

            yield return null;
        }
        
        gameObject.SetActive(false);
    }
}
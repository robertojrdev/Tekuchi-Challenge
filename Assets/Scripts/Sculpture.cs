using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sculpture
{
    public Cube[] cubes;
    public Combination combination;

    public static IEnumerator ShowAnimation(Sculpture sculpture, ColorList colors)
    {
        var count = sculpture.cubes.Length;
        var block = new MaterialPropertyBlock();

        //setup and place objects in a random position above the stage
        Color color;
        for (int i = 0; i < count; i++)
        {
            var item = sculpture.combination.items[i];
            var randPos = item.transformData.position + Random.insideUnitSphere + Vector3.up * 1.5f;
            sculpture.cubes[i].transform.position = randPos;
            sculpture.cubes[i].gameObject.SetActive(true);

            //set color
            sculpture.cubes[i].meshRenderer.GetPropertyBlock(block);
            colors.GetColor(sculpture.combination.items[i].colorID, out color);
            block.SetColor("_Color", color);
            sculpture.cubes[i].meshRenderer.SetPropertyBlock(block);
        }

        //store random positions to lerp
        var originalPositions = new Vector3[count];
        var originalRotations = new Quaternion[count];
        for (int i = 0; i < count; i++)
        {
            originalRotations[i] = sculpture.cubes[i].transform.localRotation;
            originalPositions[i] = sculpture.cubes[i].transform.localPosition;
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
                var item = sculpture.combination.items[i];

                //slerp cause a circular movement effect
                var newPos = Vector3.Slerp(
                    originalPositions[i], item.transformData.position, t);

                var newRot = Quaternion.Lerp(
                    originalRotations[i], Quaternion.Euler(item.transformData.rotation), t);

                var newScale = Vector3.Slerp(
                    Vector3.zero, item.transformData.scale, t);

                //fade-in
                sculpture.cubes[i].meshRenderer.GetPropertyBlock(block);
                block.SetFloat("_Cutoff", 1 - t);

                //apply animation
                sculpture.cubes[i].meshRenderer.SetPropertyBlock(block);
                sculpture.cubes[i].transform.localPosition = newPos;
                sculpture.cubes[i].transform.localRotation = newRot;
                sculpture.cubes[i].transform.localScale = newScale;
            }

            yield return null;
        }
    }

    public static IEnumerator HideAnimation(Sculpture sculpture)
    {
        var count = sculpture.cubes.Length;
        var block = new MaterialPropertyBlock();

        //store initial positions to lerp
        var originalPositions = new Vector3[count];
        var originalRotations = new Quaternion[count];
        for (int i = 0; i < count; i++)
        {
            originalPositions[i] = sculpture.cubes[i].transform.localPosition;
            originalRotations[i] = sculpture.cubes[i].transform.localRotation;
        }

        //start animation loop
        var time = 1f;
        var counter = 0f;
        while (counter <= time)
        {
            counter += Time.deltaTime;
            //lerp using an ease function
            var t = Easing.EaseOutCirc(0, time, counter);

            //apply animation to each object individually
            for (int i = 0; i < count; i++)
            {
                var item = sculpture.combination.items[i];

                //slerp cause a circular movement effect
                var newPos = Vector3.Slerp(
                    originalPositions[i], originalPositions[i] + Vector3.up * 2, t);

                var newRot = Quaternion.Lerp(
                    originalRotations[i], Quaternion.Inverse(originalRotations[i]), t);
                    
                var newScale = Vector3.Slerp(
                    item.transformData.scale, Vector3.zero, t);

                //fade-out
                sculpture.cubes[i].meshRenderer.GetPropertyBlock(block);
                block.SetFloat("_Cutoff", t + 0.1f);

                //apply animation
                sculpture.cubes[i].meshRenderer.SetPropertyBlock(block);
                sculpture.cubes[i].transform.localPosition = newPos;
                sculpture.cubes[i].transform.localRotation = newRot;
                sculpture.cubes[i].transform.localScale = newScale;
            }

            yield return null;
        }

        //free cubes
        for (int i = 0; i < count; i++)
            sculpture.cubes[i].busy = false;
    }
}
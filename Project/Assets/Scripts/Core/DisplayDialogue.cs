using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Core;

public class DisplayDialogue : MonoBehaviour
{
    public TextMeshProUGUI TextMesh;
    public string Text = "Jaap";

    public float Interval = 1f;

    private float timer;
    private int index;

    void Update()
    {
        if (timer >= Interval)
        {
            if (index == Text.Length)
            {
                TextMesh.text = "";
                index = 0;
            }

            TextMesh.text += Text[index];
            index++;

            timer = 0f;
        }
        else
            timer += Time.deltaTime;
    }
}

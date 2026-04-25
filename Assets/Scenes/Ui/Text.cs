using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Text : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    void Start()
    {
        text.text = new string("Score: 0");
    }

    public void UpdateText(float points)
    {
        text.text = new string("Score: " + points);
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveEventUI : MonoBehaviour
{
    public static ActiveEventUI instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private TextMeshProUGUI activeEventText;


    public void SetActiveEventText(string name, string desc)
    {
        activeEventText.text = "Event: " + name + "\n Description: " + desc;
    }
}

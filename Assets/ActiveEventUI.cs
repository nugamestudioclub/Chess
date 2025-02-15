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


    public void SetActiveEventText(RandomStatus evt)
    {
        activeEventText.text = "Event: " + evt + "\n Description: " + RandomGameEvent.GetDescription(evt);
    }
}

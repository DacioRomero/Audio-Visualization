using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MicrophonePopulateDropdown : MonoBehaviour
{
    private Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();

        List<Dropdown.OptionData> deviceOptions = new List<Dropdown.OptionData>();

        foreach (string device in Microphone.devices)
        {
            deviceOptions.Add(new Dropdown.OptionData(device));
        }

        dropdown.options = deviceOptions;
        dropdown.RefreshShownValue();

        Destroy(this);
    }
}

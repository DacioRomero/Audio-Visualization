using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VisualizerPopulateDropdown : MonoBehaviour
{
    private Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();

        List<Dropdown.OptionData> modeOptions = new List<Dropdown.OptionData>();

        foreach (string name in System.Enum.GetNames(typeof(Visualizer.Modes)))
        {
            modeOptions.Add(new Dropdown.OptionData(name));
        }

        dropdown.options = modeOptions;
        dropdown.RefreshShownValue();

        Destroy(this);
    }
}

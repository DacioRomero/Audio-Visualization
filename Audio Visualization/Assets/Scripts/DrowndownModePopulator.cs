using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DrowndownModePopulator : MonoBehaviour
{
    [SerializeField] private ModeChanger moder;

    private Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.options = moder.GetModes();
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.AddListener(delegate { moder.SetMode(dropdown); });
    }
}

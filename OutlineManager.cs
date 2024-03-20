using UnityEngine;
using TMPro;

public class OutlineManager : MonoBehaviour
{
    [Range(0, 1)]
    public float Thickness;
    public Color color = new(0, 0, 0, 1);

    void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        text.outlineWidth = Thickness;
        text.outlineColor = color;
    }
}

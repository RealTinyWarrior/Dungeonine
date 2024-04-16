using TMPro;
using UnityEngine;

public class ShowRandomQuote : MonoBehaviour
{
    public string[] quotes;

    void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = quotes[Random.Range(0, quotes.Length - 1)];
    }
}

using System.Net.Mime;
using TMPro;
using UnityEngine;

public class LearnMoveUI : MonoBehaviour
{
    public void SetText(string text)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
}

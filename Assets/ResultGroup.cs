using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultGroup : MonoBehaviour
{
    [SerializeField]
    Image image;

    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    Color victoryColor;

    [SerializeField]
    Color defeatColor;

    [SerializeField]
    CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup.alpha = 0;
    }

    public void ShowVictory()
    {
        image.color = victoryColor;
        text.text = "Victory";
        canvasGroup.alpha = 1;
    }

    public void ShowDefeat()
    {
        image.color = defeatColor;
        text.text = "Defeat";
        canvasGroup.alpha = 1;
    }
}
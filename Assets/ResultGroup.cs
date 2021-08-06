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

    public void ShowVictory(int fireCount)
    {
        var score = 10000.0f / fireCount;
        image.color = victoryColor;
        text.text = $"Victory\nScore: {score:F0}";
        canvasGroup.alpha = 1;
    }

    public void ShowDefeat()
    {
        image.color = defeatColor;
        text.text = "Defeat";
        canvasGroup.alpha = 1;
    }
}
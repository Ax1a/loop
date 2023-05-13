using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour
{
    public GameObject lifeCountParent;
    private Image[] lifeImages;
    private int maxLives = 3;
    private int currentLives;
    private Color fullLifeColor = new Color(173f / 255f, 0f, 0f);
    private Color emptyLifeColor = new Color(22f / 255f, 22f / 255f, 22f / 255f);
    public GameObject GameOverPanel;
    private void Start()
    {
        StartGame();
    }
    private void SetLifeColors()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (i < currentLives)
            {
                lifeImages[i].color = fullLifeColor;
            }
            else
            {
                lifeImages[i].color = emptyLifeColor;
            }
        }
    }
    public void ReduceLife()
    {
        currentLives--;
        SetLifeColors();
        GameOver();
    }

    public void GameOver()
    {
        if (currentLives != 0 || GameOverPanel == null)
        {
            return;
        }
        GameOverPanel.SetActive(true);
    }
    public void StartGame()
    {
        lifeImages = lifeCountParent.GetComponentsInChildren<Image>();
        currentLives = maxLives;
        SetLifeColors();

        GameOverPanel?.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class HygieneSystem : MonoBehaviour
{
    public Slider hygieneSlider;
    public float maxHygiene = 100f;
    public float hygieneDecrement = 40f;
    // public float hygieneIncrement = 20f;
    public float currentHygiene;
    public float timeElapsed;

    private void Start()
    {
        currentHygiene = maxHygiene;
        hygieneSlider.maxValue = maxHygiene;
        hygieneSlider.value = currentHygiene;
    }

    private void Update()
    {
        // decrease hygiene every 5 seconds, then reset timer.
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= 5f)
        {
            DecreaseHygiene();
            timeElapsed = 0f;
        }

        // regenerate hygiene to max if L is pressed
        if (Input.GetKeyDown(KeyCode.L))
        {
            IncreaseHygiene();
        }

        // decrease hygiene to 40 if K is pressed
        if (Input.GetKeyDown(KeyCode.K))
        {
            DecreaseHygiene(40f);
        }

        // change color of slider fill based on hygiene level
        if (currentHygiene >= 50f)
        {
            hygieneSlider.fillRect.GetComponent<Image>().color = Color.green;
        }
        else if (currentHygiene < 50f && currentHygiene >= 25f)
        {
            hygieneSlider.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            hygieneSlider.fillRect.GetComponent<Image>().color = Color.red;
        }
    }

    //used for decreasing hygiene level 
    private void DecreaseHygiene(float amount = -1f)
    {
        if (amount == -1f)
        {
            amount = hygieneDecrement;
        }

        if (currentHygiene > 0f)
        {
            currentHygiene = Mathf.Max(currentHygiene - amount, 0f);
            hygieneSlider.value = currentHygiene;
        }
        timeElapsed = 0f;
    }

    //function for increasing hygiene to max
    private void IncreaseHygiene()
    {
        currentHygiene = maxHygiene;
        hygieneSlider.value = currentHygiene;
        timeElapsed = 0f;
    }
}

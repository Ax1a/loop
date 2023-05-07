using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ScreenSettingsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    void Start()
    {
        // Get the available resolutions
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();


        // Create a list of resolution options
        List<string> options = new List<string>();

        // Add each resolution to the options list
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == 1280 && resolutions[i].height == 720)
            {
                currentResolutionIndex = i;
            }
        }

        // Add the options to the dropdown
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set the default resolution to 1280 x 720
        Screen.SetResolution(1280, 720, Screen.fullScreen);

    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}

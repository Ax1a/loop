using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxAudio : MonoBehaviour
{
    public void btnAudio ()
    {
        AudioManager.Instance.PlaySfx("Button");
    }

    public void clickAudio ()
    {
        AudioManager.Instance.PlaySfx("Click");
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FunctionPlaySound : BEInstruction
{
    AudioClip soundToPlay;
    
    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        soundToPlay = BeController.GetSound(beBlock.BeInputs.stringValues[0]);
        targetObject.beAudioSource.clip = soundToPlay;
        targetObject.beAudioSource.Play();

        BeController.PlayNextOutside(beBlock);
    }
}

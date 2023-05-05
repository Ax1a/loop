using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTime : MonoBehaviour
{
    private void OnEnable() {
        StartCoroutine(DelayPause());
    }

    private void OnDisable() {
        Time.timeScale = 1;
    }

    private IEnumerator DelayPause() {
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 0;
    }
}

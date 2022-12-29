using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    public Text gameScore;
    public static int score_count;
    // Start is called before the first frame update
    void Start() {
        gameScore = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update(){
        gameScore.text = "Score: " + score_count;
    }
}

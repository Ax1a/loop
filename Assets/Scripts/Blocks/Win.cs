using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public int ptsToWin= 0;
    private int currPts = 0;
    public GameObject myBlocks;
    public Toggle checkBox;
    public GameObject gameOverPanel;

    public quizTimer timer;
    public initialPosition blocksInitPos;
    public GameObject startPanel;
    public GameObject WinLosePanel;

    //[HideInInspector] public Drag drag;


    // Start is called before the first frame update
    void Start()
    {
       // ptsToWin = myBlocks.transform.childCount;
        gameOverPanel.SetActive(false);
        timer = GameObject.Find("StartPanel").GetComponent<quizTimer>();
        blocksInitPos = GameObject.Find("resetPos").GetComponent<initialPosition>();
        //drag = GameObject.Find("DragScript").GetComponent<Drag>();
    }
    public void retry(){
        startPanel.transform.SetAsLastSibling();
        timer.resetTime();
        startPanel.transform.localScale = new Vector3(1,1,1); 
        transform.GetChild(1).gameObject.SetActive(false);
        gameOverPanel.SetActive(false);
        blocksInitPos.ResetPositions();
        Debug.Log("Retry");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void resPos(){
       // myBlocks.transform.position = initPos;
    }

    public void validate(){
        if(currPts  >= ptsToWin)
        {
            //Win Message
            //Check Box
            //transform.GetChild(0).gameObject.SetActive(true);
            WinLosePanel.transform.gameObject.SetActive(true);
            WinLosePanel.transform.SetAsLastSibling();
          //  WinLosePanel.transform.GetChild(1).gameObject.SetActive(false);
            checkBox.GetComponent<Toggle>().isOn= true;
            timer.stopTime();   
            Debug.Log("Winner");
            Debug.Log("Points: " + currPts);
        }else{
            Debug.Log("Losser");
            // WinLosePanel.transform.GetChild(0).gameObject.SetActive(false);
            // WinLosePanel.transform.GetChild(1).gameObject.SetActive(true);
            gameOverPanel.transform.SetAsLastSibling();
            //transform.GetChild(1).gameObject.SetActive(true);
            gameOverPanel.SetActive(true);
            timer.stopTime();
        }
    }

    public void AddPoints(){
        currPts += 1;
        Debug.Log(currPts);
    }
  
}

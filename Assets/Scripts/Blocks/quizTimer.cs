using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class quizTimer : MonoBehaviour
{
    public enum Difficulty {
        easy, medium, hard
    }
    public Difficulty difficulty;
    public float currTime= 0f;
    public float startingTime = 0f;
 
    public GameObject gameOverPanel;
    public GameObject startPanel;
    [HideInInspector] public bool isStart = false;
    [SerializeField]public TextMeshProUGUI countDownText;
    void Start()
    {
        //Define starting time based on difficulty levels.. 
        if(difficulty == Difficulty.easy)
        {
            startingTime = 20f;
        }
        else if (difficulty == Difficulty.medium)
        {
            startingTime = 30f;
        }
        else
        {
            startingTime = 60f;
        }

        currTime = startingTime;
        gameOverPanel.SetActive(false);

    }

    // Update is called once per frame    
    public void startGame()
    {  
        //Check if timer is not starting
        if(!isStart)
        {
            isStart = true;
            startPanel.transform.localScale = new Vector3(0,0,0);
            Debug.Log("Timer Start");
        }
    }

    public void stopTime()
    {  
        //Check if timer is starting
        if(isStart)
        {
            Debug.Log("Timer Stop");
            isStart = false;   
        } 
    }

    public void resetTime()
    {  
      
        currTime = startingTime;
        isStart = true;
        Debug.Log("Reset the timer");
             
       
    }

    public void res()
    {
        isStart = false;
    }

    void Update()
    {  
        //Decrease the time when the timer is starting. 
        //The float type is converted to string
        //When current time is equal to zero, it activate game over panel.
        if(isStart)
        {  
            currTime -= 1 * Time.deltaTime;
            float time = currTime;
            countDownText.text = time.ToString("0");   

            if(currTime <= 0 )
            {
                currTime = 0;
                gameOverPanel.SetActive(true);
                gameOverPanel.transform.SetAsLastSibling();
                isStart = false;
            }               
        } 
 
    }
    
}

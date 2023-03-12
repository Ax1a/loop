using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public List<Reward> rewards = new List<Reward>();
    public static RewardManager Instance;
    
    [HideInInspector] public int _money;
    [HideInInspector] public int _exp;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void GiveRewards (string name)
    {
        Reward reward = rewards.Find(r => r.rewardName == name);
        if (reward != null)
        {
            DataManager.AddMoney(reward.money);
            DataManager.AddExp(reward.exp);
            _money = reward.money;
            _exp = reward.exp;
        }
    }
    public void AssessReward ()
    {
        if (quizTimer.Instance.difficulty == quizTimer.Difficulty.easy)
        {
            GiveRewards("Easy Level");
            Debug.Log("Rewards Added");
        }   
        else if (quizTimer.Instance.difficulty == quizTimer.Difficulty.medium)
        {
            GiveRewards("Medium Level");
            Debug.Log("Rewards Added");

        }
        else
        {
            GiveRewards("Difficult Level");
            Debug.Log("Rewards Added");

        }
    }
}

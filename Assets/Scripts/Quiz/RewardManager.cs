using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public List<Reward> rewards = new List<Reward>();
    public static RewardManager Instance;

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
        }
    }
    public void AssessReward ()
    {
        if (quizTimer.Instance.difficulty == quizTimer.Difficulty.easy)
        {
            GiveRewards("Easy Level");
        }   
        else if (quizTimer.Instance.difficulty == quizTimer.Difficulty.medium)
        {
            GiveRewards("Medium Level");
        }
        else
        {
            GiveRewards("Difficult Level");
        }
    }

}

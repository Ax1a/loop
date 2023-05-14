using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [SerializeField] LifeSystem lifeSystem;
    [SerializeField] MazePlayerMovement mazePlayerMovement;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (lifeSystem != null)
            {
                lifeSystem.ReduceLife();
                mazePlayerMovement.ResetPosition();
            }
        }
    }
}

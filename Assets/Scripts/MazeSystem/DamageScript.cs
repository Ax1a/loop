using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [SerializeField] LifeSystem lifeSystem;
    [SerializeField] MazePlayerMovement mazePlayerMovement;
    private bool died;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !died)
        {
            if (lifeSystem != null)
            {
                //Play sfx
                AudioManager.Instance.PlaySfx("ElectricAttack");
                died = true;
                StartCoroutine(AnimateDamage());
            }
        }
    }

    private IEnumerator AnimateDamage() {
        // Add death sound effect
        ThirdPersonCamera.Instance.ToggleControl(false);
        lifeSystem.AnimateRobot("isDying", true);
        lifeSystem.ReduceLife();
        yield return new WaitForSeconds(2f);
        mazePlayerMovement.ResetPosition();
        lifeSystem.AnimateRobot("isDying", false);
        ThirdPersonCamera.Instance.ToggleControl(true);
        died = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float moveSpeedMultiplier;
    [SerializeField] private ParticleSystem waterParticles;

    public void managerAnimation(Vector3 moveVector)
    {
        if (moveVector.magnitude > 0)
        {
            anim.SetFloat("moveSpeed", moveVector.magnitude * moveSpeedMultiplier);
            playRunAnimation();
            anim.transform.forward = moveVector.normalized;
        }
        else playIdleAnimation();
    }
    private void playRunAnimation()
    {
        anim.Play("Run");
    }
    private void playIdleAnimation()
    {
        anim.Play("Idle");
    }
    public void playSowAnimation()
    {
        anim.SetLayerWeight(1, 1);
        anim.Play("Sow");
    }
    public void stopSowAnimation()
    {
        anim.SetLayerWeight(1, 0);
    }
    public void playWaterAnimation()
    {
        anim.SetLayerWeight(2, 1);
    }
    public void stopWaterAnimation()
    {
        anim.SetLayerWeight(2, 0);
        waterParticles.Stop();
    }
    public void playHarvestAnimation()
    {
        anim.SetLayerWeight(3, 1);
    }
    public void stopHarvestAnimation()
    {
        anim.SetLayerWeight(3, 0);
    }
    public void playShakeTreeAnimation()
    {
        anim.SetLayerWeight(4, 1);
        anim.Play("Shake Tree");
    }
    public void stopShakeTreeAnimation()
    {
        anim.SetLayerWeight(4, 0);
    }
}

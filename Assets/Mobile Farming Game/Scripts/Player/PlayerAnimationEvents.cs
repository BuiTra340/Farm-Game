using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private ParticleSystem seedParticles;
    [SerializeField] private ParticleSystem waterParticles;
    [Header("Events")]
    [SerializeField] private UnityEvent startHarvestingEnvent;
    [SerializeField] private UnityEvent stoptHarvestingEnvent;
    private void playSeedParticles()
    {
        seedParticles.Play();
    }
    private void playWaterParticles()
    {
        waterParticles.Play();
    }
    private void startHarvestingCallback()
    {
        startHarvestingEnvent?.Invoke();
    }
    private void stopHarvestingCallback()
    {
        stoptHarvestingEnvent?.Invoke();
    }    
}

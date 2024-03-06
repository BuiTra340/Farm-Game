using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SeedParticles : MonoBehaviour
{
    public static Action<Vector3[]> onSeedCollided;
    private void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        int collisionAmout = ps.GetCollisionEvents(other, collisionEvents);
        Vector3[] collisionPositions = new Vector3[collisionAmout];
        for(int i=0;i<collisionAmout;i++)
            collisionPositions[i] = collisionEvents[i].intersection;

        onSeedCollided?.Invoke(collisionPositions);
    }
}

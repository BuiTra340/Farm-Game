using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionEffectManager : MonoBehaviour
{
    public static TransactionEffectManager instance;
    [Header("Elements")]
    [SerializeField] private ParticleSystem coinPS;
    [SerializeField] private RectTransform coinRectTransform;
    private Camera camera;
    [Header("Settings")]
    [SerializeField]private float moveSpeed;
    private int coinAmount;
    private void Awake()
    {
        if(instance == null)
           instance = this;
        else Destroy(instance.gameObject);
    }
    private void Start()
    {
        camera = Camera.main;
    }

    [NaughtyAttributes.Button]
    private void playTest()
    {
        playCoinParticles(30);
    }
    public void playCoinParticles(int amount)
    {
        if (coinPS.isPlaying)
            return;
        ParticleSystem.Burst burst = coinPS.emission.GetBurst(0);
        burst.count = amount;

        ParticleSystem.MainModule main = coinPS.main;
        main.gravityModifier = 2;

        coinPS.emission.SetBurst(0, burst);

        coinPS.Play();
        coinAmount = amount;
        StartCoroutine(playCoinParticlesCoroutine());
    }
    IEnumerator playCoinParticlesCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        ParticleSystem.MainModule main = coinPS.main;
        main.gravityModifier = 0;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[coinAmount];
        coinPS.GetParticles(particles);
        Vector3 direction = (coinRectTransform.position - camera.transform.position).normalized;
        Vector3 targetPosition = camera.transform.position + direction * (Vector3.Distance(camera.transform.position, coinPS.transform.position));
        while(coinPS.isPlaying) 
        {
            coinPS.GetParticles(particles);
            for(int i =0;i<particles.Length;i++)
            {
                if (particles[i].remainingLifetime <= 0)
                    continue;

                particles[i].position = Vector3.MoveTowards(particles[i].position, targetPosition, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(particles[i].position, targetPosition) < .01f)
                {
                    CashManager.instance.addCoin(1);
                    particles[i].position += Vector3.up * 10000;
                }
            }
            coinPS.SetParticles(particles);
            yield return null;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    enum State { Ready, Growing }
    private State state;
    [SerializeField] private Renderer renderer;
    [SerializeField] private float shakeMultipliers;
    private Rigidbody rb;
    [Header("Actions")]
    public static Action<CropType> onAppleReleased;

    [Header("Settings")]
    private Vector3 originPosition;
    private Quaternion originRotation;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originPosition = transform.position;
        originRotation = transform.rotation;
        state = State.Ready;
    }

    public void updateMaterials(float shakeMagnitude)
    {
        float realShakeMagnitude = shakeMultipliers * shakeMagnitude;
        renderer.material.SetFloat("_Magnitude", realShakeMagnitude);
    }
    public bool isFree()
    {
        return !rb.isKinematic;
    }
    public void releaseApple()
    {
        rb.isKinematic = false;
        renderer.material.SetFloat("_Magnitude", 0);
        onAppleReleased?.Invoke(CropType.Apple);
        forceReset();
        state = State.Growing;
    }
    private void forceReset()
    {
        LeanTween.scale(gameObject, Vector3.zero, 1).setDelay(2).setOnComplete(forceReady);
    }
    private void forceReady()
    {
        rb.isKinematic = true;
        transform.position = originPosition;
        transform.rotation = originRotation;
        LeanTween.scale(gameObject, Vector3.one, 1);
        LeanTween.delayedCall(2, () => state = State.Ready);
    }
    public bool isReady() => state == State.Ready;
}

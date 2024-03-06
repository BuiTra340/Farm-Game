using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShakeTreeAbility : MonoBehaviour
{
    private AppleTree currentTree;
    [SerializeField] private float distanceToTree;
    PlayerAnimator playerAnim;
    private bool isActive;
    private Vector2 previousMousePosition;
    [Range(0f, 1f)]
    [SerializeField] private float shakeThreshold;
    private void Start()
    {
        playerAnim = GetComponent<PlayerAnimator>();
        AppleTreeManager.onTreeModeStarted += treeModeStartedCallback;
        AppleTreeManager.onTreeModeEnded += treeModeEndedCallback;
    }
    private void OnDestroy()
    {
        AppleTreeManager.onTreeModeStarted -= treeModeStartedCallback;
        AppleTreeManager.onTreeModeEnded -= treeModeEndedCallback;
    }

    private void Update()
    {
        if (isActive)
            manageShakeTree();
    }
    private void treeModeStartedCallback(AppleTree tree)
    {
        currentTree = tree;
        moveTowardsTree();
        isActive = true;
    }
    private void treeModeEndedCallback()
    {
        currentTree = null;
        isActive = false;
        LeanTween.delayedCall(.1f,() => playerAnim.stopShakeTreeAnimation());
    }
    private void moveTowardsTree()
    {
        Vector3 direction = transform.position - currentTree.transform.position;
        direction.y = 0;
        Vector3 targetPos = currentTree.transform.position + direction.normalized * distanceToTree;
        //playerAnim.managerAnimation(targetPos);
        LeanTween.move(gameObject, targetPos, .5f);
    }
    private void manageShakeTree()
    {
        if (!Input.GetMouseButton(0))
        {
            currentTree.stopShake();
            return;
        }
        float shakeMagnitude = Vector2.Distance(Input.mousePosition, previousMousePosition);
        if (shouldShake(shakeMagnitude))
            Shake();
        else currentTree.stopShake();

        previousMousePosition = Input.mousePosition;
    }
    private void Shake()
    {
        currentTree.Shake();
        playerAnim.playShakeTreeAnimation();
    }
    private bool shouldShake(float magnitude)
    {
        float screenPercent = magnitude / Screen.width;
        return screenPercent >= shakeThreshold;
    }
}

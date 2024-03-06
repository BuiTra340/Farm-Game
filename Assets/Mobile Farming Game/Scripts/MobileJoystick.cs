using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileJoystick : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private RectTransform joystickOutline;
    [SerializeField] private RectTransform joystickKnob;

    [Header("Settings")]
    private Vector3 clickedPos;
    [SerializeField] private float moveFactor;
    private bool canControl;
    private Vector3 move;
    public void clickedOnJoystickZoneCallback()
    {
        clickedPos = Input.mousePosition;
        joystickOutline.position = clickedPos;
        showJoystick();
    }
    private void Start()
    {
        hideJoystick();
    }
    private void Update()
    {
        if (canControl)
            controlJoystick();
    }
    private void showJoystick()
    {
        joystickOutline.gameObject.SetActive(true);
        canControl = true;
    }
    private void hideJoystick()
    {
        joystickOutline.gameObject.SetActive(false);
        canControl = false;
        move = Vector3.zero;
    }
    private void controlJoystick()
    {
        Vector3 currentPosition = Input.mousePosition;
        Vector3 direction = currentPosition - clickedPos;
        float moveMagnitude = direction.magnitude * moveFactor / Screen.width;
        moveMagnitude = Mathf.Min(moveMagnitude,joystickOutline.rect.width / 2);
        move = direction.normalized * moveMagnitude;
        Vector3 targetPosition = clickedPos + move;
        joystickKnob.position = targetPosition;
        if (Input.GetMouseButtonUp(0))
            hideJoystick();
    }
    public Vector3 getMoveVector() => move;
}

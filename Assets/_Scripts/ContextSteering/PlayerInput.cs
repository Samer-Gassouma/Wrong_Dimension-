using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;
    public UnityEvent OnAttack;
    public static float speed = 1;
    //[SerializeField]
    //private InputActionReference movement, attack, pointerPosition;

    private void Update()
    {
        //OnMovementInput?.Invoke(movement.action.ReadValue<Vector2>().normalized);
        OnMovementInput?.Invoke((new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"))) * speed);
        OnPointerInput?.Invoke(GetPointerInput()); 
        if(Input.GetMouseButtonDown(0))
            OnAttack?.Invoke();
    }

    public void ActivateSpeedBoost(int powerUpDuration)
    {
        float oldSpeed = speed;
        speed = 2;
        StartCoroutine(DeactivateSpeedBoost(powerUpDuration, oldSpeed));

    }
    IEnumerator DeactivateSpeedBoost(int powerUpDuration, float oldSpeed)
    {
        yield return new WaitForSeconds(powerUpDuration);
        speed = oldSpeed;
    }


    public void ActivateSpeedSlower(int powerUpDuration)
    {
        float oldSpeed = speed;
        speed -= 0.5f;
        StartCoroutine(DeactivateSpeedSlower(powerUpDuration, oldSpeed));

    }

    IEnumerator DeactivateSpeedSlower(int powerUpDuration, float oldSpeed)
    {
        yield return new WaitForSeconds(powerUpDuration);
        speed = oldSpeed;
    }

    


    private Vector2 GetPointerInput()
    {
        //Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    //private void OnEnable()
    //{
    //    attack.action.performed += PerformAttack;
    //}

    //private void PerformAttack(InputAction.CallbackContext obj)
    //{
    //    OnAttack?.Invoke();
    //}

    //private void OnDisable()
    //{
    //    attack.action.performed -= PerformAttack;
    //}
}

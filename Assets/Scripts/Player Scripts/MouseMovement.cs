using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] private Transform playerRoot , lookRoot;
    [SerializeField] private bool invert;
    // [SerializeField] private bool canUnlock = true;
    [SerializeField] private float sensitivity = 5f;
    // [SerializeField] private int smoothSteps = 10;
    // [SerializeField] private float smoothWeight = 0.4f;
    [SerializeField] private float rollAngle = 0f;
    [SerializeField] private Vector2 lookLimits = new Vector2(-70f, 80f);
    private Vector2 currentMouseLook , lookAngles , smoothMove , smoothLook;
    private float currentRollAngle , rollSpeed = 3f;
    private int lastLookFrame;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LockAndUnlockCursor();
        if(Cursor.lockState == CursorLockMode.Locked){
            LookAround();
        }
    }
    void LockAndUnlockCursor(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(Cursor.lockState == CursorLockMode.Locked){
                Cursor.lockState = CursorLockMode.None;
            }else{
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void LookAround(){
        currentMouseLook = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        lookAngles.x += currentMouseLook.x * sensitivity * (invert ? 1f : -1f);
        lookAngles.y += currentMouseLook.y * sensitivity;
        lookAngles.x = Mathf.Clamp(lookAngles.x, lookLimits.x, lookLimits.y);
        currentRollAngle = Mathf.Lerp(currentRollAngle, Input.GetAxisRaw("Mouse X") * rollAngle, Time.deltaTime * rollSpeed);
        lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, currentRollAngle);
        playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
    }
}

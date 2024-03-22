using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    public PlayerMovementScript playerMovementScript;
    public float crouchSpeed = 1.5f , moveSpeed = 4f , sprintSpeed = 8f;
    private Transform LookTransform;
    private float standHeight = 1.6f , crouchHeight = 1f;
    private bool isCrouching;
    private PlayerFootsteps playerFootsteps;
    private PlayerStats playerStats;
    private float sprintVolume = 1f, crouchVolume = 0.1f, walkVolumeMin = 0.2f, walkVolumeMax = 0.6f , walkStepDistance = 0.4f , sprintStepDistance = 0.25f, crouchStepDistance = 0.5f;
    private float sprintVal = 100f;
    private float sprintThreshold = 10f;
    void Awake()
    {
        playerMovementScript = GetComponent<PlayerMovementScript>();
        LookTransform = transform.GetChild(0);
        playerFootsteps = GetComponentInChildren<PlayerFootsteps>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Start()
    {
        playerFootsteps.volumeMin = walkVolumeMin;
        playerFootsteps.volumeMax = walkVolumeMax;
        playerFootsteps.stepDistance = walkStepDistance;
    }

    void Update()
    {
        Sprint();
        Crouch();   
    }

    void Sprint(){
        if(sprintVal > 0){
            if(Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching){
                playerMovementScript.speed = sprintSpeed;
                playerFootsteps.stepDistance = sprintStepDistance;
                playerFootsteps.volumeMin = sprintVolume;
                playerFootsteps.volumeMax = sprintVolume;
            }
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching){
            playerMovementScript.speed = moveSpeed;
            playerFootsteps.stepDistance = walkStepDistance;
            playerFootsteps.volumeMin = walkVolumeMin;
            playerFootsteps.volumeMax = walkVolumeMax;
        }
        if(Input.GetKey(KeyCode.LeftShift) && !isCrouching){
            sprintVal -= sprintThreshold * Time.deltaTime;
            if(sprintVal <= 0){
                sprintVal = 0f;
                playerMovementScript.speed = moveSpeed;
                playerFootsteps.stepDistance = walkStepDistance;
                playerFootsteps.volumeMin = walkVolumeMin;
                playerFootsteps.volumeMax = walkVolumeMax;
            }
            playerStats.DisplayStaminaStats(sprintVal);
        }
        else {
            if(sprintVal != 100){
                sprintVal += (sprintThreshold / 2f) * Time.deltaTime;
                playerStats.DisplayStaminaStats(sprintVal);
                if(sprintVal > 100){
                    sprintVal = 100;
                }
            }
        }
    }

    void Crouch(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            if(isCrouching){
                LookTransform.localPosition = new Vector3(0f, standHeight, 0f);
                playerMovementScript.speed = moveSpeed;
                playerFootsteps.stepDistance = walkStepDistance;
                playerFootsteps.volumeMin = walkVolumeMin;
                playerFootsteps.volumeMax = walkVolumeMax;
                isCrouching = false;
            }
            else {
                LookTransform.localPosition = new Vector3(0f, crouchHeight, 0f);
                playerMovementScript.speed = crouchSpeed;
                playerFootsteps.stepDistance = crouchStepDistance;
                playerFootsteps.volumeMin = crouchVolume;
                playerFootsteps.volumeMax = crouchVolume;
                isCrouching = true;
            }
       }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [Header("Camera to Assign")]
    public GameObject aimCam;
    public GameObject aimCanvas;
    public GameObject thirdPersonCam;
    public GameObject thirdPersonCanvas;
    public PlayerScript player;

    [Header("Camera Animator")]
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (player.mobileInputs == true)
        {
            if (CrossPlatformInputManager.GetButton("Aim") && player.currentPlayerSpeed > 0)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("AimWalk", true);
                animator.SetBool("Walk", true);

                thirdPersonCam.SetActive(false);
                thirdPersonCanvas.SetActive(false);
                aimCam.SetActive(true);
                aimCanvas.SetActive(true);
            }
            else if (CrossPlatformInputManager.GetButton("Aim"))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("AimWalk", false);
                animator.SetBool("Walk", false);

                thirdPersonCam.SetActive(false);
                thirdPersonCanvas.SetActive(false);
                aimCam.SetActive(true);
                aimCanvas.SetActive(true);
            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetBool("IdleAim", false);
                animator.SetBool("AimWalk", false);

                thirdPersonCam.SetActive(true);
                thirdPersonCanvas.SetActive(true);
                aimCam.SetActive(false);
                aimCanvas.SetActive(false);
            }
        }
        else
        {
            if (Input.GetButton("Fire2") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("AimWalk", true);
                animator.SetBool("Walk", true);

                thirdPersonCam.SetActive(false);
                thirdPersonCanvas.SetActive(false);
                aimCam.SetActive(true);
                aimCanvas.SetActive(true);
            }
            else if (Input.GetButton("Fire2"))
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("AimWalk", false);
                animator.SetBool("Walk", false);

                thirdPersonCam.SetActive(false);
                thirdPersonCanvas.SetActive(false);
                aimCam.SetActive(true);
                aimCanvas.SetActive(true);
            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetBool("IdleAim", false);
                animator.SetBool("AimWalk", false);

                thirdPersonCam.SetActive(true);
                thirdPersonCanvas.SetActive(true);
                aimCam.SetActive(false);
                aimCanvas.SetActive(false);
            }
        }
    }
}

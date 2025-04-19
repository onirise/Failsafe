using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crouch : MonoBehaviour
{
    public CharacterController playerController;
    public float crouchSpeed, normalHeight, crouchHeight;
    public Vector3 offset;
    public Transform player;
    bool crouching;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouching = !crouching;
        }
        if (crouching == true)
        {
            playerController.height = playerController.height - crouchSpeed * Time.deltaTime;
            if (playerController.height <= crouchHeight)
            {
                playerController.height = crouchHeight;
            }
        }
        if (crouching == false)
        {
            playerController.height = playerController.height + crouchSpeed * Time.deltaTime;
            if (playerController.height < normalHeight)
            {
                player.position = player.position + offset * Time.deltaTime;
            }
            if (playerController.height >= normalHeight)
            {
                playerController.height = normalHeight;
            }
        }
    }
}
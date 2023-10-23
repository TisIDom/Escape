﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
    public class PlayerMovement : MonoBehaviour
    {
        public GameObject mPlayer;


        public CharacterController controller;

        

        public float speed = 5f;
        public float gravity = -15f;

        Vector3 velocity;

        bool isGrounded;

        // Update is called once per frame
        void Update()
        {

            PlayerHiding playerHiding = mPlayer.GetComponent<PlayerHiding>();

            if (playerHiding.movingAllowed)
            {
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");

                Vector3 move = transform.right * x + transform.forward * z;

                controller.Move(move * speed * Time.deltaTime);

                velocity.y += gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime);

            }
            

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
    public class PlayerMovement : MonoBehaviour
    {

        public CharacterController controller;
        public PlayerHiding playerHiding;

        public float speed = 5f;
        public float gravity = -15f;

        Vector3 velocity;

        // Update is called once per frame
        void Update()
        {
            if(!playerHiding.isUnderTable) 
            {

                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");

                Vector3 move = transform.right * x + transform.forward * z;

                controller.Move(move * speed * Time.deltaTime);

                velocity.y += gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime);
            }

            //if(controller.transform.position.y < -5 ) {
            //    transform.position = new Vector3(-22.2f, 1.0f, 21.7f);
            //}

        }
    }
}
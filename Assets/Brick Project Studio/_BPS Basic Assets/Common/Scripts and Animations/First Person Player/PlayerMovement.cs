using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SojaExiles

{
    public class PlayerMovement : MonoBehaviour
    {

        public CharacterController controller;
        public PlayerHiding playerHiding;

        public float speed = 5f;
        public float gravity = -15f;
        public GameObject youWin; 

        Vector3 velocity;

        // Update is called once per frame

        void Update()
        {
            if(transform.position.z < 1.0f)
            {
                InitializeYouWin();
            }

            if(!playerHiding.isUnderTable && this.isActiveAndEnabled) 
            {

                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");

                Vector3 move = transform.right * x + transform.forward * z;

                controller.Move(move * speed * Time.deltaTime);

                velocity.y += gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime);
            }

            if (controller.transform.position.y < -5)
            {
                transform.position = new Vector3(-22.2f, 1.0f, 21.7f);
            }

        }
        void InitializeYouWin()
        {
            youWin.SetActive(true);
            Time.timeScale = 0f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }


}
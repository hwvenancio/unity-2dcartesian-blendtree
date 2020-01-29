using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Experimental.PlayerLoop;


namespace HatMan
{
    

    public class PlayerWalkControl : MonoBehaviour
    {
        private Rigidbody body;
        private Animator animationFSM;
        private static readonly int Strafe = Animator.StringToHash("Strafe");
        private static readonly int Sprint = Animator.StringToHash("Sprint");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly float TOLERANCE = 0.001f;
        private static readonly float DEFAULT_SPEED = 0.666f;
        private static readonly float ANIMATION_FACTOR = 2.0f;

        void Awake()
        {
            body = GetComponent<Rigidbody>();
            animationFSM = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var run = Input.GetAxis("Run");
            
            animationFSM.SetBool(IsMoving, IsNotZero(horizontal) || IsNotZero(vertical));
            animationFSM.SetFloat(Strafe, body.velocity.x * ANIMATION_FACTOR);
            animationFSM.SetFloat(Sprint, body.velocity.z * ANIMATION_FACTOR);

            var speed = (run > 0.0f) ? DEFAULT_SPEED * 2.0f : DEFAULT_SPEED;
            
            var velocity = new Vector3(horizontal, 0.0f, vertical);
            body.velocity = (horizontal + vertical > 1.0f ? velocity.normalized : velocity) * speed;
        }

        static bool IsNotZero(float value)
        {
            return Math.Abs(value) > TOLERANCE;
        }

        static bool IsZero(float value) {
            return Math.Abs(value) < TOLERANCE;
        }

    }

}
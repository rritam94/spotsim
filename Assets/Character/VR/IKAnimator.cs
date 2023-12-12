using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class IKAnimator : MonoBehaviour
{

    protected Animator animator;

    [SerializeField]
    private bool ikActive = true;
    [SerializeField]
    private Transform rightHandObj = null;
    [SerializeField]
    private Transform leftHandObj = null;
    [SerializeField]
    private Transform headObj = null;
    [SerializeField]
    private Transform rootMovement = null;
    [SerializeField]
    private float rootOffset = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

      //  Vector3 rootForwardGrounded = new Vector3(rootMovement.forward.x, 0, rootMovement.forward.z).normalized;

      //  transform.position = new Vector3(rootMovement.position.x, 0, rootMovement.position.z) - rootForwardGrounded * rootOffset;

      //  transform.rotation = Quaternion.Euler(new Vector3(
       //     transform.rotation.eulerAngles.x, 
       //     rootMovement.rotation.eulerAngles.y, 
        //    transform.rotation.eulerAngles.z));
        //    transform.rotation.eulerAngles.z));
    }

    //a callback for calculating IK
    void OnAnimatorIK(int layeridx)
    {
        if (animator)
        {
            //if the IK is active, set the position and rotation directly to the goal.
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                if (headObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(headObj.position + headObj.forward);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (leftHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                }

            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }
}
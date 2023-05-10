using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{

    public Transform groundCheckTransform;
    public float groundCheckRadius = 0.2f;

    private Transform targetTransform;
    public LayerMask mouseAimMask;
    public LayerMask groundMask;

    public GameObject bulletPrefab;
    public Transform muzzleTransform;

    public AnimationCurve recoilCurve;
    public float recoilDuration = 0.25f;
    public float recoilMaxRotation = 45f;
    private float nextRound = 0f;
    public float firerate;
    public Transform rightLowerArm;
    public Transform rightHand;
    public Transform leftUpperArm;
    public Transform leftLowerArm;
    public Transform leftHand;
    public Transform leftHint;
    public Transform leftElbowHint;
    [SerializeField] private Vector3 leftHintPos;
    [SerializeField] private Vector3 leftElbowHintPos;

    /*public AvatarMask upperBody;
    public AvatarMask baseBody;*/

    private Animator animator;
    private Rigidbody rbody;
    private bool isGrounded;
    private float recoilTimer;

    private Enemy enemyscrpt;

    private int FacingSign
    {
        get
        {
            Vector3 perp = Vector3.Cross(transform.forward, Vector3.forward);
            float dir = Vector3.Dot(perp, transform.up);
            return dir > 0f ? -1 : dir < 0f ? 1 : 0;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        enemyscrpt = GetComponent<Enemy>();
    }

    private void Awake()
    {
        targetTransform = FindObjectOfType<Cowboy>().transform;
    }


    void Update()
    {

        firerate = UnityEngine.Random.Range(0.5f, 10f);
        
            if (Time.time > nextRound)
            {
                nextRound = Time.time + firerate;
                Fire();
                
            }

        //adjusting the position of IK Hands
        leftHint.transform.position = rightHand.position + leftHintPos;
        leftElbowHint.transform.position = leftElbowHint.position + leftElbowHintPos;
    }

    private void Fire()
    {
        if (enemyscrpt.isDead)
        {
            return;
        }
        else if (!enemyscrpt.isDead && targetTransform!=null)
        {
            recoilTimer = Time.time;

            var go = Instantiate(bulletPrefab);
            go.transform.position = muzzleTransform.position;
            var bullet = go.GetComponent<EnemyBullet>();
            bullet.Fire(go.transform.position, muzzleTransform.eulerAngles, gameObject.layer);
        }
        
    }

    private void LateUpdate()
    {
        // Recoil Animation
        if (recoilTimer < 0)
        {
            return;
        }

        float curveTime = (Time.time - recoilTimer) / recoilDuration;
        if (curveTime > 1f)
        {
            recoilTimer = -1;
        }
        else
        {
            rightLowerArm.Rotate(Vector3.forward, recoilCurve.Evaluate(curveTime) * recoilMaxRotation, Space.Self);
        }


    }

    private void FixedUpdate()
    {


        // Facing Rotation
        if (enemyscrpt.isDead)
        {
            return;
        }
        else
        {
        rbody.MoveRotation(Quaternion.Euler(new Vector3(0, 90 * Mathf.Sign(targetTransform.position.x - transform.position.x), 0)));

        }

        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void OnAnimatorIK()
    {
        // Weapon Aim at Target IK with hands
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, targetTransform.position);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHint.position);

        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1);
        animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowHint.position);

        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHint.rotation);


        // Look at target IK
        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(targetTransform.position);
    }

}

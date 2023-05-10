using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody mainRb;
    public Collider mainCol;

    public Rigidbody[] ragdoll_Rb;
    public Collider[] ragdoll_Col;

    private Animator enemyAnim;

    public float hurtForce;
    public Rigidbody pelvis_rb;

    [HideInInspector]
    public bool isDead;
    private void Awake()
    {
        mainRb = GetComponent<Rigidbody>();
        mainCol = GetComponent<Collider>();

        ragdoll_Rb = GetComponentsInChildren<Rigidbody>();
        ragdoll_Col = GetComponentsInChildren<Collider>();

        enemyAnim = GetComponent<Animator>();


        //Turning off ragdoll on awake
        RagdollDisable();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            RagdollEnable();
        }
    }

    void RagdollEnable()
    {
        isDead = true;

        foreach (var rb in ragdoll_Rb)
        {
            rb.isKinematic = false;


            mainRb.isKinematic = true;

            enemyAnim.enabled = false;

            StartCoroutine(DisableDead());
        }

        if (transform.localRotation.y <= 0)
        {
            pelvis_rb.AddForce(Vector3.right.normalized * hurtForce * Time.deltaTime * 100f, ForceMode.Impulse);
        }
        if (transform.localRotation.y >= 0)
        {
            pelvis_rb.AddForce(Vector3.right.normalized * -hurtForce * Time.deltaTime * 100f, ForceMode.Impulse);
        }

        foreach (var col in ragdoll_Col)
        {
            col.enabled = true;
            mainCol.enabled = false;

        }
    }

    void RagdollDisable()
    {
       

        foreach (var rb in ragdoll_Rb)
        {
            rb.isKinematic = true;


            mainRb.isKinematic = false;

            enemyAnim.enabled = true;
        }

        foreach (var col in ragdoll_Col)
        {
            col.enabled = false;
            mainCol.enabled = true;

        }
    }

    IEnumerator DisableDead()
    {
        yield return new WaitForSeconds(1.5f);
        foreach (var rb in ragdoll_Rb)
        {
            rb.isKinematic = true;
        }
        foreach (var col in ragdoll_Col)
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bulletass"))
        {


            RagdollEnable();

            Destroy(collision.gameObject);
       
        }
    }
}

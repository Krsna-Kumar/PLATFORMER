using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float velocity;
    public float life;

    private int firedByLayer;
    private float lifeTimer;

    
    
    void Update()
    {
        float bulletVelocity = UnityEngine.Random.Range(5f, 20f);
        float bulletLife = UnityEngine.Random.Range(2f, 5f);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, bulletVelocity * Time.deltaTime, ~(1<< firedByLayer)))
        {
            transform.position = hit.point;
            Vector3 reflected = Vector3.Reflect(transform.forward, hit.normal);
            Vector3 direction = transform.forward;
            Vector3 vop = Vector3.ProjectOnPlane(reflected, Vector3.forward);
            transform.forward = vop;
            transform.rotation = Quaternion.LookRotation(vop, Vector3.forward);
            Hit(transform.position, reflected, direction, hit.collider);
        }
        else
        {
            transform.Translate(Vector3.forward * bulletVelocity * Time.deltaTime);
        }

        

        if (Time.time > lifeTimer + bulletLife)
        {
            Destroy(gameObject);
        }
    }

    private void Hit(Vector3 position, Vector3 reflected ,Vector3 direction, Collider collider)
    {
        // Do something here with the object that was hit (collider), e.g. collider.gameObject 

        Destroy(gameObject,0.01f);

        /*if (collider.gameObject.name =="ENEMY")
        {
            Destroy(gameObject, 0.001f);
        }*/

        if (collider.gameObject.CompareTag("Player"))
        {
            print("surprise madafaka");
        }

    }

    public void Fire(Vector3 position, Vector3 euler, int layer)
    {
        lifeTimer = Time.time;
        transform.position = position;
        transform.eulerAngles = euler;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        Vector3 vop = Vector3.ProjectOnPlane(transform.forward, Vector3.forward);
        transform.forward = vop;
        transform.rotation = Quaternion.LookRotation(vop, Vector3.forward);

    }


}

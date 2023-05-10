using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public float Damage;
    public float fireRate;
    public Transform muzzle;
    public GameObject bulletPrefab;

    public AnimationCurve recoilCurve;
    public float recoilDuration = 0.25f;
    public float recoilMaxRotation = 45f;

    
}

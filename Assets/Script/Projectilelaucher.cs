using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectilelaucher : MonoBehaviour
{
    public Transform lauchPoint;
    public GameObject projectilePrefab;

    public void FireProjectile()
    {
        GameObject projectile= Instantiate(projectilePrefab,lauchPoint.position,projectilePrefab.transform.rotation);
        Vector3 origScale=projectile.transform.localScale;
        projectile.transform.localScale = new Vector3(
            origScale.x * transform.localScale.x > 0 ? 1 : -1,
            origScale.y * transform.localScale.x > 0 ? 1 : -1,
            origScale.z
            );
    }
}

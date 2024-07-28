using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int healthRestore = 20;
    public Vector3 spinRoatateSpeed = new Vector3(0, 180, 0);

    AudioSource pickupSource;
    private void Start()
    {
        pickupSource=GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damgeable damgeable=collision.GetComponent<Damgeable>();

        if (damgeable)
        {
            if(pickupSource)
                AudioSource.PlayClipAtPoint(pickupSource.clip,gameObject.transform.position,pickupSource.volume);
            damgeable.Heal(healthRestore);
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        transform.eulerAngles += spinRoatateSpeed * Time.deltaTime;
    }
}

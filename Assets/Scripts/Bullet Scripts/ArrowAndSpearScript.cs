using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAndSpearScript : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 30f;
    public int damage = 15;
    public float deactivateTimer = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    void Start()
    {
        Invoke("DeactivateGameObject", deactivateTimer);
    }

    void DeactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    public void Launch(Camera mainCamera){
        rb.velocity = mainCamera.transform.forward * speed;
        transform.LookAt(transform.position + rb.velocity);
    }
    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Enemy")
        {
            target.GetComponent<HealthScript>().ApplyDamage(damage);
            gameObject.SetActive(false);
        }
    }
}

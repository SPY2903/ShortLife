using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletDecal;
    [SerializeField] private float speed = 50f;
    [SerializeField] private float timeToDestroy = 5f;
    private Vector3 target;
    private bool hit;
    public Vector3 GetTarget()
    {
        return target;
    }
    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }
    public bool GetHit()
    {
        return hit;
    }
    public void SetHit(bool hit)
    {
        this.hit = hit;
    }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if(!hit && Vector3.Distance(transform.position, target) < .01f)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.GetContact(0);
        Instantiate(bulletDecal, contactPoint.point + contactPoint.normal * .0001f, Quaternion.LookRotation(contactPoint.normal));
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabable : MonoBehaviour
{
    [SerializeField] float grabDistance = 1.5f;
    [SerializeField] float moveSpeed = 10.0f;
    [Header("그랩 했을 때 상대 높이")]
    public float grabY = -0.25f;
    [Header("그랩 레이어(플레이어와 충돌x)")]
    [SerializeField] int grabLayer = 6;

    Grabber grabOwner = null;
    Transform owner = null;
    bool isGrab = false;
    Rigidbody rb;
    int originalLayer = 0;
    float originalAngularDrag = 0.0f;
    bool originalGravity = true;

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalLayer = gameObject.layer;
        originalAngularDrag = rb.angularDrag;
        originalGravity = rb.useGravity;
    }
    void FixedUpdate()
    {

    }

    protected virtual void Update()
    {
        if (isGrab)
            GrabMove();
    }

    void GrabMove()
    {
        if (owner == null)
            return;

        Vector3 goal = owner.position + owner.forward * grabDistance;
        goal.y += grabY;
        Vector3 v = goal - transform.position;

        rb.velocity = moveSpeed * v;
        //rb.MoveRotation(owner.rotation);
    }

    public virtual void Grab(Transform owner, Grabber grabOwner)
    {
        isGrab = true;
        this.grabOwner = grabOwner;
        this.owner = owner;
        gameObject.layer = grabLayer;
        rb.useGravity = false;
        rb.angularDrag = 10.0f;
    }
    public virtual void Drop(float charge = 0)
    {
        isGrab = false;
        grabOwner.GrabClear();
        Vector3 ownerFoward = grabOwner.cam.forward;
        grabOwner = null;
        owner = null;
        gameObject.layer = originalLayer;

        if(charge > 1.0f)
        {
            Vector3 force = ownerFoward * charge;
            rb.AddForce(force, ForceMode.Impulse);
        }
        rb.angularDrag = originalAngularDrag;
        rb.useGravity = originalGravity;
        rb.velocity /= rb.mass;
    }

    public bool IsGrab()
    {
        return isGrab;
    }

    public Transform GetOwner()
    {
        return owner;
    }
    public Grabber GetGrabber()
    {
        return grabOwner;
    }

    public void SetOwner(Transform owner)
    {
        this.owner = owner;
    }
}

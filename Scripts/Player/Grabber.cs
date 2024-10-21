using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grabber : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] PlayerController player;
    [SerializeField] public Transform cam;
    [Header("상호작용 거리")]
    public float interactLength = 1.0f;
    [Header("최대 날리는 힘")]
    [SerializeField] float throwPower = 10.0f;
    [Header("최대 날리기 위해 필요한 시간(초)")]
    [SerializeField] float chargeSpeed = 2.0f;

    public delegate void OnInteract();
    public OnInteract onInteract = () => { };
    public delegate void OnUseItem(PickAble heldItem);
    public OnUseItem onUseItem;

    RaycastHit hit;
    Grabable grab = null;
    Rigidbody grabRigidBody = null;
    IInteractable interacter = null;

    KeyCode rotateKey = KeyCode.R;

    Quaternion playerRotOriginal;
    Quaternion grabRotOriginal;
    Quaternion grabRotation;

    float charge = 0.0f;

    PickAble heldItem = null;

    void Awake()
    {
        if (player != null)
            rotateKey = player.keyRotate;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ObjectProcess();
        ThrowObject();
        RotateObject();
    }

    void ObjectProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Trigger"));
            if (Physics.Raycast(cam.position, cam.forward, out hit, interactLength, layerMask))
            {
                if (heldItem != null) //아이템 사용
                {
                    bool success = heldItem.Use(hit.collider.gameObject);

                    if (success)
                    {
                        if(onUseItem != null)
                            onUseItem(heldItem);
                    }
                    heldItem = null;
                    return;
                }

                if (hit.rigidbody == null)
                    return;

                if (grab == null)
                {
                    grab = hit.rigidbody.GetComponent<Grabable>();
                    interacter = hit.rigidbody.GetComponent<IInteractable>();
                    if (grab != null)
                    {
                        grabRigidBody = hit.rigidbody;
                        playerRotOriginal = cam.transform.rotation;
                        grabRotOriginal = grab.transform.rotation;
                        grabRotation = Quaternion.identity;

                        grab.Grab(cam.transform, this);
                    }
                    if (interacter != null)
                    {
                        interacter.Interact();
                        onInteract();
                    }
                }
            }
            else
            {
                if(heldItem != null)
                {
                    heldItem = null;
                    if(onUseItem != null)
                        onUseItem(heldItem);
                }
            }
        }
    }
        void ThrowObject()
    {
        if (Input.GetMouseButton(1))
        {
            if (charge < throwPower)
                charge += Time.deltaTime * (throwPower / chargeSpeed);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Drop(charge);
            charge = 0;
        }
    }

    void RotateObject()
    {
        if (grab != null)
        {
            Quaternion rotdif = cam.transform.rotation * Quaternion.Inverse(playerRotOriginal);

            Quaternion grabRot = rotdif * grabRotOriginal;
            if (Input.GetKey(rotateKey))
            {
                float mouseX = Input.GetAxisRaw("Mouse X") * player.mouseSensivityX;
                float mouseY = Input.GetAxisRaw("Mouse Y") * player.mouseSensivityY;

                grabRotation = grabRotation * Quaternion.Euler(0, -mouseX, -mouseY);

            }
            grabRot = grabRot * grabRotation;
            grabRigidBody.MoveRotation(grabRot);
        }
    }

    public Grabable GetGrabObject()
    {
        return grab;
    }
    public IInteractable GetInteractObject()
    {
        return interacter;
    }

    public void Drop(float charge = 0)
    {
        if (grab == null)
            return;

        grab.Drop(charge);
        grab = null;
        grabRigidBody = null;
    }

    public void GrabClear()
    {
        grab = null;
    }

    public void SetHeldItem(PickAble obj)
    {
        heldItem = obj;
    }

    public PickAble GetHeldItem()
    {
        return heldItem;
    }
}

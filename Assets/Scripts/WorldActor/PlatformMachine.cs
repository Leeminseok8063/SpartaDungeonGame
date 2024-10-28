using System.Collections.Generic;
using UnityEngine;

enum PLATTYPE
{
    NONE,
    MOVE,
    FORCE,
    TRAP,
}

public class PlatformMachine : MonoBehaviour
{

    bool ObjectOnPanel = false;
    [Header("MOD")][SerializeField]PLATTYPE type = PLATTYPE.NONE;
    BoxCollider collider;
    Rigidbody rb;
    Animator animator;
    
    [Header("Common")]
    public float speed = 0;

    [Header("ForceMode")]
    public float power = 0;
    public float delay = 0;
   
    [Header("MoveMode")]
    public Vector3 startPos = Vector3.zero;
    public Vector3 destPos = Vector3.zero;

    private float forceModeTimer = 0f;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        transform.position = startPos;

        if(type == PLATTYPE.FORCE)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch(type)
        {
            case PLATTYPE.NONE:
                break;
            case PLATTYPE.MOVE:
                MoveModeUpdate();
                break;
            case PLATTYPE.FORCE:
                ForceModeUpdate();
                break;
            case PLATTYPE.TRAP:
                break;
        }
    }

    List<GameObject> FindObjectOnPanel()
    {

        Vector3 boxSize = new Vector3(collider.transform.localScale.x, 0.1f, collider.transform.localScale.z);
        Vector3 boxCenter = transform.position + new Vector3(0, collider.transform.localScale.y / 2, 0);
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, boxSize / 2, Vector3.up, Quaternion.identity, 0.1f, LayerMask.GetMask("Player"));

        List<GameObject> objects = new List<GameObject>();
        foreach(RaycastHit hit in hits )
        {
            objects.Add(hit.collider.gameObject);
        }
        
        // 박스의 모서리를 계산
        Vector3[] corners = new Vector3[8];
        Vector3 halfBoxSize = boxSize / 2;
        Funtions.DrawBoxCasting(boxSize, boxCenter);
        return objects;
    }
    void ForceModeUpdate()
    {
        List<GameObject> objects = FindObjectOnPanel();
        if(objects.Count == 0)
        {
            forceModeTimer = 0;
            return;
        }

        if(forceModeTimer > delay)
        {
            foreach (GameObject obj in objects)
            {
                obj.GetComponent<Rigidbody>().AddForce(Vector3.up * power, ForceMode.Impulse);
            }
            animator.SetTrigger("isPop");
            forceModeTimer = 0;
        }

        forceModeTimer += Time.deltaTime;
        objects.Clear();
    }
    
    void MoveModeUpdate()
    {
        List<GameObject> objects = FindObjectOnPanel();

        Vector3 dest = destPos - transform.position;
        rb.velocity = dest.normalized * speed;     
        if (dest.sqrMagnitude < 0.1f)
        {
            Vector3 temp = startPos;
            startPos = destPos;
            destPos = temp;
        }

        foreach(GameObject obj in objects)
        {
            obj.GetComponent<IOnPlatformable>().PlatformPos(transform.position);
        }

        objects.Clear();
    }
}

using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemInfo info;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public Vector3 GetCheckPointPos()
    {
        if(info.type == ITEMTYPE.CHECKPOINT)
            return this.gameObject.transform.position + (Vector3.up * 5f);

        return new Vector3();
    }
    public void TriggerInteractItem()
    {
        if (info.type == ITEMTYPE.INTERACTABLE)
            animator.SetBool("isInteract", true);
    }
}

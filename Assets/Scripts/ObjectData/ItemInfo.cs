using UnityEngine;

public enum ITEMTYPE
{
    CONSUME,
    JUMPER,
    SHIELD,
    CHECKPOINT,
    INTERACTABLE,
    WEAPON
}

[CreateAssetMenu(fileName = "Item", menuName = "Create Item")]
public class ItemInfo : ScriptableObject
{
    public GameObject itemPrefab;
    public ITEMTYPE type;
    public string itemName;
    public string itemDesc;
    public int power;
}

using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public ItemType itemType;
    public int damage;
    public float attackSpeed;
    public float arc;
    public float range;
    public int rayAmount;



    public int extraHealth;
}

public enum ItemType
{
    Weapon,
    Equipment,
}

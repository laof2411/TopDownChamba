using UnityEngine;

public class TreeInteractable : MonoBehaviour, IInteractable, IDamagable
{
    [field: SerializeField] public int Health { get; set; }
    [field: SerializeField] public LayerMask GetHitBy { get; set; }

    [SerializeField] private GameObject resourcesDrop;
    public void Die()
    {
        Instantiate(resourcesDrop, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public void Interact()
    {
        Debug.Log("Soy un arbol, deja de toquetearme");
    }
}

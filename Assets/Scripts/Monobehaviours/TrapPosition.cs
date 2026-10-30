using UnityEngine;

public class TrapPosition : MonoBehaviour, IInteractable
{
    [SerializeField] private int cost;
    [SerializeField] private bool activated;
    [SerializeField] private int damage;


    public void Interact()
    {
        if(activated) { return; }

        if(FindAnyObjectByType<PlayerController>().resources >= cost)
        {
            activated = true;
            FindAnyObjectByType<PlayerController>().resources -= cost;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated) { return; }

        Debug.Log("uwu");

        if(other.gameObject.layer == 6)
        {
            Debug.Log("uwu2");
            //3 is the player layer
            other.GetComponent<IDamagable>().Damage(damage, 3);
        }
    }
}

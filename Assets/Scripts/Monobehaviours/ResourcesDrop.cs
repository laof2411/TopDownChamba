using UnityEngine;

public class ResourcesDrop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.resources += 1;
            Destroy(gameObject);
        }
    }
}

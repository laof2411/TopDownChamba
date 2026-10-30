using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour, IDamagable
{
    public int resources;

    [SerializeField] CharacterController characterController;
    [SerializeField] private float speed;

    [Header("Melee Attack")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private float arc;
    [SerializeField] private int rayAmount;
    [SerializeField] private float range;
    [SerializeField] private int weaponDamage;
    [SerializeField] private bool canAttack = true;

    [Header("Interactables")]
    [SerializeField] private float interactableRadius;


    [Header("Inventory")]
    [SerializeField] private List<ItemSO> inventory;
    [SerializeField] private ItemSO currentWeapon;

    [field: SerializeField] public int Health { get; set; }
    [field: SerializeField] public LayerMask GetHitBy { get; set; }


    //Input System
    private InputAction moveAction;

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        Movement();
        Looking();
        Attacking();
        Interaction();

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            int currentWeaponIndex = inventory.IndexOf(currentWeapon);
            int startingIndex = inventory.IndexOf(currentWeapon) + 1;
            bool foundItem = false;

            for(int i = startingIndex; foundItem == false; i++)
            {
                if(i >= inventory.Count)
                {
                    i = 0;
                }

                if (inventory[i].itemType == ItemType.Weapon && inventory[i] != currentWeapon)
                {
                    currentWeapon = inventory[i];
                    foundItem = true;
                }
                else    if(i == currentWeaponIndex)
                {
                    foundItem = true;   
                }
            }
        }
    }

    private void Interaction()
    {
        IInteractable closestInteractable = null;
        List<Collider> interactableColliders = Physics.OverlapSphere(transform.position + transform.forward, interactableRadius).ToList();
        float closestDistance = 1000f;

        if (interactableColliders.Count > 0)
        {
            foreach (Collider collider in interactableColliders)
            {
                if (collider.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    // Si nuestro collider con el componente de interactuable esta mas cerca que la distancia mas cercana
                    if (Vector3.Distance(collider.transform.position, transform.position) < closestDistance)
                    {
                        closestInteractable = interactable;
                        closestDistance = Vector3.Distance(collider.transform.position, transform.position);
                    }
                }
            }
        }
        else
        {
            closestInteractable = null;
        }


        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            //El "?" esta checando si es null
            closestInteractable?.Interact();
        }
    }

    private void Attacking()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!canAttack) { return; }

            canAttack = false;
            StartCoroutine(ResetAttack());
            float rayAngle = arc / rayAmount;

            //This only works properly on even numbers (2,4,6,8,10) Will need to make another logic for uneven numbers - Luis Ortiz
            for (int i = (rayAmount / 2) * -1; i < (rayAmount / 2); i++)
            {
                Quaternion angleRotation = Quaternion.AngleAxis(rayAngle * i, Vector3.up);
                Ray ray = new Ray(transform.position + Vector3.up, angleRotation * transform.forward);
                RaycastHit enemyInfo;

                Debug.DrawRay(ray.origin, ray.direction * range, Color.green, 5f);

                if (Physics.Raycast(ray.origin, ray.direction, out enemyInfo, range))
                {
                    if (enemyInfo.transform.TryGetComponent<IDamagable>(out IDamagable damagable))
                    {
                        //Debug.Log("Hola");
                        //3 is the player layer
                        damagable.Damage(weaponDamage, 3);
                    }
                }
            }
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    private void Looking()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100f))
        {
            Vector3 position = hitInfo.point;
            position.y = transform.position.y;

            transform.LookAt(position);
        }
    }

    private void Movement()
    {
        Vector3 movementVector = Vector3.zero;

        //Direccion
        movementVector.x = moveAction.ReadValue<Vector2>().x; // -1 y 1
        movementVector.y = 0;
        movementVector.z = moveAction.ReadValue<Vector2>().y; // -1 y 1

        characterController.Move(movementVector * Time.deltaTime * speed);
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }
}

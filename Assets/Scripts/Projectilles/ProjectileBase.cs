using UnityEngine;
using UnityEngine.Events;

public class ProjectileBase : MonoBehaviour
{
    public GameObject owner { get; private set; }
    public WeaponController weapon { get; private set; }
    public float speed { get; set; }
    public Vector3 initialPosition { get; private set; }
    public Vector3 initialDirection { get; private set; }
    public Vector3 inheritedMuzzleVelocity { get; private set; }
    



    public UnityAction onShoot;

    public void Shoot(WeaponController controller)
    {
        weapon = controller;
        owner = controller.owner;
        speed = controller.projectileVelocity;
        initialPosition = transform.position;
        initialDirection = transform.forward;

        inheritedMuzzleVelocity = controller.muzzleWorldVelocity;
        if (onShoot != null)
        {
            onShoot.Invoke();
        }
    }
}

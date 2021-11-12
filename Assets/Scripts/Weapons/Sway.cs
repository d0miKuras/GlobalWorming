using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    #region Variables

    #region Weapon Sway

    [Header("Weapon Sway")]
    public float weaponSwayIntensity;
    public float weaponSwaySmoothing;

    public float swayClampX;
    public float swayClampY;


    private PlayerInputs _input;
    private MyWeaponManager _weaponManager;
    private Quaternion originRotation;

    #endregion

    #endregion

    #region Monobehaviour Callbacks

    private void Start()
    {
        _input = transform.parent.parent.parent.parent.GetComponent<PlayerInputs>();

        // set origin rotation
        originRotation = transform.localRotation;
    }
    private void Update()
    {
        UpdateSway();
    }

    #endregion

    #region Private Methods

    private void UpdateSway()
    {
        Vector2 look = _input.GetLook();

        // clamping
        look.x = Mathf.Clamp(look.x, -swayClampX, swayClampX);
        look.y = Mathf.Clamp(look.y, -swayClampY, swayClampY);
        // calculate target rotation
        Quaternion t_adj_x = Quaternion.AngleAxis(weaponSwayIntensity * -look.x, Vector3.up);
        Quaternion t_adj_y = Quaternion.AngleAxis(weaponSwayIntensity * look.y, Vector3.right);
        Quaternion targerRotation = t_adj_x * t_adj_y * originRotation;

        // rotate towards target rotation
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targerRotation, weaponSwaySmoothing * Time.deltaTime);
    }

    #endregion
}

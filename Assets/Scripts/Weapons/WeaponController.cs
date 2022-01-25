using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponShootType
{
    Manual,
    Automatic,
    Charged
}

public class WeaponController : MonoBehaviour
{
    #region Variables

    #region General

    [Header("Information")]
    [Tooltip("The name that will be displayed in the UI for this weapon.")]
    public string weaponName;
    public WeaponShootType shootType;




    #endregion

    #region Internal References
    [Header("Internal References")]
    [Tooltip("The root object for the weapon, this is what will be deactivated when the weapon isn't active.")]
    public GameObject weaponRoot;
    [Tooltip("Tip of the weapon, where the projectiles are shot.")]
    public Transform weaponMuzzle;
    #endregion

    #region Shoot Parameters

    [Space(10)]
    [Header("Shoot Parameters")]
    public float damage = 10.0f;
    public float delayBetweenShots;
    public int maxAmmo = 10;

    [Tooltip("The 'speed' at which the bullet travels through the world.")]
    public float projectileVelocity;
    [Tooltip("The strength of the randomness of the direction of the bullet on hipfire.")] 
    public float hipBloom;
    public float reloadTime = 0.5f;
    [Tooltip("The projectile prefab.")] public ProjectileBase projectilePrefab;



    #endregion


    #region Camera Recoil
    [Space(10)]
    [Header("Camera Recoil Settings:")]
    public float rotationSpeed = 6f;
    public float returnSpeed = 25f;

    [Space(5)]
    [Tooltip("The recoil vector added to the camera rotation when hipfiring.")]
    public Vector3 hipfireCameraRecoilRotation = new Vector3(2.0f, 2.0f, 0.0f);
    [Tooltip("The recoil vector added to the camera rotation when shooting while aiming.")]
    public Vector3 adsCameraRecoilRotation = new Vector3(0.5f, 0.5f, 0.0f);

    #endregion

    #region Weapon Recoil
    [Space(10)]
    [Header("Weapon Recoil:")]
    public Transform weaponRecoil_Position;
    public Transform weaponRecoil_Rotation;
    public float weaponRecoil_PositionalSpeed = 8f;
    public float weaponRecoil_RotationalSpeed = 8f;
    public float weaponRecoil_PositionalReturnSpeed = 18f;
    public float weaponRecoil_RotationalReturnSpeed = 38f;

    public Vector3 weaponRecoil_RecoilRotation = new Vector3(10f, 5, 7);
    public Vector3 weaponRecoil_RecoilKickBack = new Vector3(0.015f, 0f, -0.2f);
    public Vector3 weaponRecoil_RotationAim = new Vector3(10, 4, 6);
    public Vector3 weaponRecoil_RecoilKickBackAim = new Vector3(0.015f, 0f, -0.2f);
    #endregion


    public float aimSpeed;

    public GameObject owner { get; set; }
    public Vector3 muzzleWorldVelocity { get; private set; }
    public GameObject sourcePrefab { get; set; }



    public bool isAiming { get; set; }

    PauseMenu pauseMenu;
    AudioSource shootingSound;


    #region Private Variables
    private bool m_isShooting;
    private int m_CurrentAmmo;
    private float m_LastTimeShot = Mathf.NegativeInfinity;
    private bool m_wantsToShoot;
    public Vector3 m_RecoilRemaining;
    public Vector3 m_CameraRecoilRotation;
    Vector3 m_LastMuzzlePosition;
    private PlayerCharacterController m_Controller;

    Vector3 wr_RotationalRecoil;
    Vector3 wr_PositionalRecoil;
    Vector3 wr_Rot;
    #endregion

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        m_LastMuzzlePosition = weaponMuzzle.position;
        m_CurrentAmmo = maxAmmo;
    }
    void Start()
    {
        pauseMenu = GameObject.Find("HUD").GetComponent<PauseMenu>();
        shootingSound = gameObject.GetComponent<AudioSource>();
        // m_Controller = owner.transform.GetComponent<PlayerCharacterController>(); 
    }

    private void FixedUpdate()
    {
        HandleRecoilCamera();
        HandleWeaponRecoil();
    }

    private void LateUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Time.deltaTime > 0)
        {
            muzzleWorldVelocity = (weaponMuzzle.position - m_LastMuzzlePosition) / Time.deltaTime;
            m_LastMuzzlePosition = weaponMuzzle.position;
        }
    }


    public void UseAmmo(int amount)
    {
        m_CurrentAmmo = (int)Mathf.Clamp(m_CurrentAmmo - amount, 0f, maxAmmo);
        m_LastTimeShot = Time.time;

    }

    public bool HandleShotInputs(bool inputDown, bool inputHeld, bool inputUp)
    {
        // m_wantsToShoot = inputDown || inputDown;
        switch (shootType)
        {
            case WeaponShootType.Manual:
                if (inputDown)
                {
                    return TryShoot();
                }
                return false;

            case WeaponShootType.Automatic:
                if (inputHeld)
                {
                    return TryShoot();
                }
                return false;
            default:
                return false;
        }

    }

    bool TryShoot()
    {
        if (pauseMenu.gamePaused)
            return false;
        if (m_CurrentAmmo > 0 && m_LastTimeShot + delayBetweenShots < Time.time)
        {
            var playerController = owner.transform.GetComponent<PlayerCharacterController>();
            Shoot();
            UseAmmo(1);
            return true;
        }
        return false;
    }

    #endregion

    #region Public Methods

    public void Shoot()
    {
        var shotDirection = isAiming ? weaponMuzzle.forward : GetShotDirectionWithinSpread(weaponMuzzle);
        ProjectileBase newProjectile = Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(shotDirection));
        // RECOIL
        if (isAiming)
        {
            m_RecoilRemaining += new Vector3(-adsCameraRecoilRotation.x, Random.Range(-adsCameraRecoilRotation.y, adsCameraRecoilRotation.y), Random.Range(-adsCameraRecoilRotation.z, adsCameraRecoilRotation.z)); // camera recoil
            wr_RotationalRecoil += new Vector3(-weaponRecoil_RotationAim.x, Random.Range(-weaponRecoil_RotationAim.y, weaponRecoil_RotationAim.y), Random.Range(-weaponRecoil_RotationAim.z, weaponRecoil_RotationAim.z)); // weapon rotational recoil
            wr_PositionalRecoil += new Vector3(Random.Range(-weaponRecoil_RecoilKickBackAim.x, weaponRecoil_RecoilKickBackAim.x), Random.Range(-weaponRecoil_RecoilKickBackAim.y, weaponRecoil_RecoilKickBackAim.y), weaponRecoil_RecoilKickBackAim.z); // weapon recoil kick back
        }
        else
        {
            m_RecoilRemaining += new Vector3(-hipfireCameraRecoilRotation.x, Random.Range(-hipfireCameraRecoilRotation.y, hipfireCameraRecoilRotation.y), Random.Range(-hipfireCameraRecoilRotation.z, hipfireCameraRecoilRotation.z));
            wr_RotationalRecoil += new Vector3(-weaponRecoil_RecoilRotation.x, Random.Range(-weaponRecoil_RecoilRotation.y, weaponRecoil_RecoilRotation.y), Random.Range(-weaponRecoil_RecoilRotation.z, weaponRecoil_RecoilRotation.z)); // weapon rotational recoil
            wr_PositionalRecoil += new Vector3(Random.Range(-weaponRecoil_RecoilKickBack.x, weaponRecoil_RecoilKickBack.x), Random.Range(-weaponRecoil_RecoilKickBack.y, weaponRecoil_RecoilKickBack.y), weaponRecoil_RecoilKickBack.z); // weapon recoil kick back
        }

        if (shootingSound != null)
            shootingSound.Play();
        newProjectile.Shoot(this);
    }

    public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
    {
        float spreadAngleRatio = hipBloom / 180f;
        Vector3 spreadWorldDirection = Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere, spreadAngleRatio);

        return spreadWorldDirection;
    }

    public void ShowWeapon(bool show)
    {
        weaponRoot.SetActive(show);
    }

    void HandleRecoilCamera()
    {
        var playerController = owner.transform.GetComponent<PlayerCharacterController>(); // TODO: figure out if I gotta call it every frame or not
        m_RecoilRemaining = Vector3.Lerp(m_RecoilRemaining, Vector3.zero, returnSpeed * Time.deltaTime);
        m_CameraRecoilRotation = Vector3.Slerp(m_CameraRecoilRotation, m_RecoilRemaining, rotationSpeed * Time.deltaTime);
        playerController.CameraRoot.transform.localRotation = Quaternion.Euler(m_CameraRecoilRotation);
    }

    void HandleWeaponRecoil()
    {
        wr_RotationalRecoil = Vector3.Lerp(wr_RotationalRecoil, Vector3.zero, weaponRecoil_RotationalReturnSpeed * Time.deltaTime);
        wr_PositionalRecoil = Vector3.Lerp(wr_PositionalRecoil, Vector3.zero, weaponRecoil_PositionalReturnSpeed * Time.deltaTime);

        weaponRecoil_Position.localPosition = Vector3.Slerp(weaponRecoil_Position.transform.localPosition, wr_PositionalRecoil, weaponRecoil_PositionalSpeed * Time.fixedDeltaTime);
        wr_Rot = Vector3.Slerp(wr_Rot, wr_RotationalRecoil, weaponRecoil_RotationalSpeed * Time.deltaTime);
        weaponRecoil_Rotation.localRotation = Quaternion.Euler(wr_Rot);
    }


    public float GetAmmoRatio()
    {
        return (float)m_CurrentAmmo/maxAmmo;
    }

    public IEnumerator HandleReload()
    {
        yield return new WaitForSeconds(reloadTime);
        m_CurrentAmmo = maxAmmo;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class M4 : Gun
{
    //gun properties
    [SerializeField] private int damage;

    [SerializeField] private float timeBetweenShooting;
    [SerializeField] private float range;
    [SerializeField] private float reloadTime;
    [SerializeField] private float timeBetweenShot;
    [SerializeField] private int magazineSize;
    [SerializeField] private int bulletPerTap;
    [SerializeField] private int bulletsLeft, bulletsShot;
    private float timeUntilNextShot;

    //bools
    [SerializeField] private bool shooting, readyToShoot, reloading;

    //references
    [SerializeField] public PlayerInput playerInputMap;

    [SerializeField] private Camera fpsCam;
    private RaycastHit rayHit;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private WeaponSway swayFXScript;
    [SerializeField] private ParticleSystem gunFlash;
    [SerializeField] private BulletPool bulletPool;
    private List<IAmmoObserver> observers = new List<IAmmoObserver>();
    [SerializeField] private AmmoDisplay ammoDisplay;
    GameObject bullet;


    [SerializeField] private Enemy targetEnemy;


    IEnumerator WaitDeploy(GameObject bullet, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        bullet.SetActive(false);
    }

    private void Start()
    {
        idleState = new IdleState(swayFXScript);
        shootingState = new ShootingState(swayFXScript, gunFlash);
        reloadingState = new ReloadingState(swayFXScript);

        playerInputMap = new PlayerInput();
        playerInputMap.Enable();
        playerInputMap.FPS.Shoot.started += ShotAttempted;
        playerInputMap.FPS.Shoot.canceled += ShotCanceled;
        playerInputMap.FPS.Reload.performed += ReloadAttempted;
        Initialize(idleState);


        
        observers = new List<IAmmoObserver>();
        AddObserver(ammoDisplay);
        SetAmmoCount(bulletsLeft);
    }

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case IdleState idleState:
                {
                    CurrentState.Update();
                }
                break;

            case ShootingState shootingState:
                {
                    CurrentState.Update();
                    PerformShoot();
                }
                break;

            case ReloadingState reloadingState:
                {
                    CurrentState.Update();
                    PerformReload();
                }
                break;

            default:
                {
                    //Debug.Log("Null state");
                }
                break;
        }
    }

    public override void ShotAttempted(InputAction.CallbackContext ctx)
    {
        if (bulletsLeft > 0 && CurrentState != reloadingState && readyToShoot == true)
        {
            SetState(shootingState);
        }
        else
        {
            Debug.Log("No ammo left");
        }
    }

    public override void ShotCanceled(InputAction.CallbackContext ctx)
    {
        shooting = false;
        SetState(idleState);
    }

    public override void ReloadAttempted(InputAction.CallbackContext ctx)
    {
        if (bulletsLeft < magazineSize && CurrentState != reloadingState)
        {
            SetState(reloadingState);
        }
    }

    private void PerformShoot()
    {
        if (bulletsLeft > 0 && timeUntilNextShot < Time.time)
        {
            Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * 100, Color.blue, 100f);
            shooting = true;
            SetAmmoCount(bulletsLeft);
            NotifyObservers();

            bullet = BulletPool.SharedInstance.GetPooledObject();
            if (bullet != null)
            {
                bullet.SetActive(true);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                StartCoroutine(WaitDeploy(bullet, 3.0f));
            }

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, range, enemyMask))
            {
                if (rayHit.collider.CompareTag("Enemy"))
                {
                    //Debug.Log("hit");
                    targetEnemy = rayHit.collider.GetComponent<Enemy>();
                    targetEnemy.TakeDamage(damage);
                }
            }
            timeUntilNextShot = Time.time + timeBetweenShooting;
            bulletsLeft--;
        }
        else if (bulletsLeft == 0)
        {
            SetState(idleState);
            SetAmmoCount(bulletsLeft);
        }
    }

    private void PerformReload()
    {
        if (reloading == false)
        {
            StartCoroutine(ReloadingFinished());
        }
    }

    private IEnumerator ReloadingFinished()
    {
        readyToShoot = false;
        reloading = true;

        yield return new WaitForSeconds(reloadTime);

        bulletsLeft = magazineSize;
        reloading = false;
        readyToShoot = true;
        SetState(idleState);
        SetAmmoCount(bulletsLeft);
    }

    public void AddObserver(IAmmoObserver observer)
    {
        observers.Add(observer);
    }


    public void RemoveObserver(IAmmoObserver observer)
    {
        observers.Remove(observer);
    }


    private void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.UpdateAmmo(bulletsLeft, magazineSize);
        }
    }

    public void SetAmmoCount(int newAmmoCount)
    {
        bulletsLeft = newAmmoCount;
        NotifyObservers();
    }
}
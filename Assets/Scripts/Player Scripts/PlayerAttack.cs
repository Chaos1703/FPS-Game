using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weaponManager;
    public float fireRate = 15f;
    private float nextTimeToFire;
    public int damage = 20;
    private Animator zoomCameraAnim;
    private Camera mainCamera;
    private GameObject crosshair;
    private bool isAiming;
    [SerializeField] private GameObject arrowPrefab , spearPrefab;
    [SerializeField] private Transform arrowBowStartPosition;
    void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        zoomCameraAnim = transform.Find("Look Root").transform.Find("FP Camera").GetComponent<Animator>();
        crosshair = GameObject.FindWithTag("Crosshair");
        mainCamera = Camera.main;
    }

    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }

    void WeaponShoot(){
        WeaponHandler currentWeapon = weaponManager.GetCurrentSelectedWeapon();
        if (currentWeapon.fireType == weaponFireType.single)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (currentWeapon.tag == "Axe")
                {
                   currentWeapon.ShootAnimation();
                }
                
                else if(currentWeapon.bulletType == weaponBulletType.bullet)
                {
                    currentWeapon.ShootAnimation();
                    BulletFired();
                }
                else{
                    if(isAiming)
                    {
                        currentWeapon.ShootAnimation();
                        if(currentWeapon.bulletType == weaponBulletType.arrow)
                        {
                            ThrowArrowOrSpear(true);
                        }
                        if(currentWeapon.bulletType == weaponBulletType.spear)
                        {
                            ThrowArrowOrSpear(false);
                        }
                    }
                }
            }

        }
        else if (currentWeapon.fireType == weaponFireType.multiple)
        {
            if (Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                currentWeapon.ShootAnimation();
                BulletFired();
            }
        }
    }

    void ZoomInAndOut(){
        WeaponHandler currentWeapon = weaponManager.GetCurrentSelectedWeapon();
        if(currentWeapon.weaponAim == weaponAim.aim)
        {
            if(Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play("Zoom In Animation");
                crosshair.SetActive(false);
            }
            if(Input.GetMouseButtonUp(1))
            {
                zoomCameraAnim.Play("Zoom Out Animation");
                crosshair.SetActive(true);
            }
        }

        if(currentWeapon.weaponAim == weaponAim.selfAim)
        {
            if(Input.GetMouseButtonDown(1))
            {
                currentWeapon.Aim(true);
                isAiming = true;
            }
            if(Input.GetMouseButtonUp(1))
            {
                currentWeapon.Aim(false);
                isAiming = false;
            }
        }
    }
    void ThrowArrowOrSpear(bool throwArrow){
        if(throwArrow)
        {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.position = arrowBowStartPosition.position;
            arrow.GetComponent<ArrowAndSpearScript>().Launch(mainCamera);
        }
        else{
            GameObject spear = Instantiate(spearPrefab);
            spear.transform.position = arrowBowStartPosition.position;
            spear.GetComponent<ArrowAndSpearScript>().Launch(mainCamera);
        }
    }

    void BulletFired(){
        RaycastHit hit;
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
        {
            if(hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }
        }
    }
}
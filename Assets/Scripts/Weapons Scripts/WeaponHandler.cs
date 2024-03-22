using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum weaponAim { none , selfAim , aim};
public enum weaponFireType { single , multiple};
public enum weaponBulletType { bullet , arrow , spear , none};
public class WeaponHandler : MonoBehaviour
{
    private Animator anim;
    public weaponAim weaponAim;
    public weaponFireType fireType;
    public weaponBulletType bulletType;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private AudioSource shootSound, reloadSound;
    public GameObject attackPoint;

    void Awake()
    {
        anim = GetComponent<Animator>();

    }

    public void ShootAnimation()
    {
        anim.SetTrigger("Shoot");
    }

    public void Aim(bool canAim)
    {
        anim.SetBool("Aim", canAim);
    }

    void TurnOnMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
    }

    void TurnOffMuzzleFlash()
    {
        muzzleFlash.SetActive(false);
    }

    void PlayShootSound()
    {
        shootSound.Play();
    }

    void PlayReloadSound()
    {
        reloadSound.Play();
    }

    void TurnOnAttackPoint()
    {
        attackPoint.SetActive(true);
    }

    void TurnOffAttackPoint()
    {
        if (attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(false);
        }
    }
}

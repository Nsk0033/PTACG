using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : CharacterComponents
{
    public static Action OnStartShooting;

    [Header("Weapon Settings")]
    [SerializeField] private Weapon weaponToUse;
    [SerializeField] private Transform weaponHolderPosition;

    // Reference of the Weapon we are using
    public Weapon CurrentWeapon { get; set; }

    // Store the reference to the second weapon
    public Weapon SecondaryWeapon { get; set; }

    // Returns the reference to our Current Weapon Aim
    public WeaponAim WeaponAim { get; set; }

    protected override void Start()
    {
        base.Start();
        EquipWeapon(weaponToUse, weaponHolderPosition);
    }
    /*private void Update()
    {
        if(CurrentWeapon.gameObject)
    }*/
    public void EquipWeapon(Weapon weapon, Transform weaponPosition)
    {

        if (CurrentWeapon != null)
        {
            CurrentWeapon.WeaponAmmo.SaveAmmo();
            WeaponAim.DestroyReticle();       // Each weapon has its own Reticle component
            Destroy(GameObject.Find("Pool"));
            Destroy(CurrentWeapon.gameObject);
        }

        CurrentWeapon = Instantiate(weapon, weaponPosition.position, weaponPosition.rotation);
        CurrentWeapon.transform.parent = weaponPosition;
        CurrentWeapon.SetOwner(character);
        WeaponAim = CurrentWeapon.GetComponent<WeaponAim>();
    }
}

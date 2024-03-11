using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{   
	private Weapon weapon; 

    private readonly string WEAPON_AMMO_SAVELOAD = "Weapon_";   
    
    private void Awake()  // Because we need to retrieve the update ammo value first
    {
        weapon = GetComponent<Weapon>(); 
        // RefillAmmo(); --- Delete this
        LoadWeaponMagazineSize();
    }

    // Consume our ammo when we shoot
    public void ConsumeAmmo()
    {
        if (weapon.UseMagazine)
        {
            weapon.CurrentAmmo -= 1;
        }
    }

    // Returns true if we have if we have enough ammo
    public bool CanUseWeapon()
    {
        if (weapon.CurrentAmmo > 0)
        {
            return true;
        }

        return false;
    }

    // Refills our weapon with ammo
    public void RefillAmmo()
    {
        if (weapon.UseMagazine)
        {
            weapon.CurrentAmmo = weapon.MagazineSize;
            //weapon.CurrentAmmo = LoadAmmo();
        }
	}

    public void LoadWeaponMagazineSize()
    {
        int savedAmmo = LoadAmmo();
        weapon.CurrentAmmo = savedAmmo < weapon.MagazineSize ? LoadAmmo() : weapon.MagazineSize;
    }

    public void SaveAmmo()
    {
        PlayerPrefs.SetInt(WEAPON_AMMO_SAVELOAD + weapon.WeaponName, weapon.CurrentAmmo);
    }

    public int LoadAmmo()
    {
        return PlayerPrefs.GetInt(WEAPON_AMMO_SAVELOAD + weapon.WeaponName, weapon.MagazineSize);
    }
}
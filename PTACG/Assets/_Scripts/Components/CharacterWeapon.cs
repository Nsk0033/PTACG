using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : CharacterComponents
{   
    public static Action OnStartShooting;
	
    [Header("Weapon Settings")]
    //[SerializeField] private Weapon weaponToUse;
    [SerializeField] private Transform weaponHolderPosition;
	[SerializeField] private Weapon BasicBow;
	[SerializeField] private Weapon BasicSword;
	[SerializeField] private Weapon UpgradeBow;
	[SerializeField] private Weapon UpgradeSword;
	[SerializeField] private Weapon Staff;
	[SerializeField] private Weapon Yamato;
	
	[SerializeField] private bool isBowUpgraded;
	[SerializeField] private bool isSwordUpgraded;
	[SerializeField] private bool isStaffOwned;
	[SerializeField] private bool isYamatoOwned;

    // Reference of the Weapon we are using
    public Weapon CurrentWeapon  { get; set; }

    // Store the reference to the second weapon
    public Weapon SecondaryWeapon { get; set; }

    // Returns the reference to our Current Weapon Aim
    public WeaponAim WeaponAim { get; set; }

    public bool IsBowUpgraded
    {
        get { return isBowUpgraded; }
        set { isBowUpgraded = value; }
    }
    public bool IsSwordUpgraded 
    {
        get { return isSwordUpgraded; }
        set { isSwordUpgraded = value; }
    }
    public bool IsStaffOwned 
    {
        get { return isStaffOwned; }
        set { isStaffOwned = value; }
    }
    public bool IsYamatoOwned
    {
        get { return isYamatoOwned; }
        set { isYamatoOwned = value;}
    }


    protected override void Start()
    {
        base.Start();

        if (!isBowUpgraded)
		{
			EquipWeapon(BasicBow, weaponHolderPosition);
		}	
		else
		{
			EquipWeapon(UpgradeBow, weaponHolderPosition);
		}
    }

    protected override void Update()
    {
        base.Update();
        IsSwordUpgraded = isSwordUpgraded;
        IsYamatoOwned = isYamatoOwned;
        Debug.Log("Character Weapon = " + isStaffOwned);
    }

    protected override void HandleInput()
    {
        if (character.CharacterType == Character.CharacterTypes.Player)
        {
            if (Input.GetMouseButton(0))
            {
                Shoot();
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopWeapon();    
            }
        
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				if(!isBowUpgraded)
				{
					EquipWeapon(BasicBow, weaponHolderPosition);
				}	
                else
				{
					EquipWeapon(UpgradeBow, weaponHolderPosition);
				}
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if(!isSwordUpgraded)
				{
					EquipWeapon(BasicSword, weaponHolderPosition);
				}	
                else
				{
					EquipWeapon(UpgradeSword, weaponHolderPosition);
				}
            }
			if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if(isStaffOwned)
				{	
					EquipWeapon(Staff, weaponHolderPosition);
				}
				else
				{
					if(isYamatoOwned)
					{
						EquipWeapon(Yamato, weaponHolderPosition);
					}
				}
            }
			if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if(isYamatoOwned && isStaffOwned)
				{	
					EquipWeapon(Yamato, weaponHolderPosition);
				}
				else
				{
					return;
				}
            }
        }   
    }
    
    // Fire our Weapon
    public void Shoot()
    {
        if (CurrentWeapon == null)
        {
            return;
        } 

        CurrentWeapon.UseWeapon();
        if (character.CharacterType == Character.CharacterTypes.Player)
        {	
            OnStartShooting?.Invoke();
            UIManager.Instance.UpdateAmmo(CurrentWeapon.CurrentAmmo, CurrentWeapon.MagazineSize);
        }     
    }

    // When we stop shooting we stop using our Weapon
    public void StopWeapon()
    {
        if (CurrentWeapon == null)
        {
            return;
        }
        
        CurrentWeapon.StopWeapon();
    }

    // Reload our Weapon
    public void Reload()
    {         
        if (CurrentWeapon == null)
        {
            return;
        }
        
        CurrentWeapon.Reload();
        if (character.CharacterType == Character.CharacterTypes.Player)
        {
            UIManager.Instance.UpdateAmmo(CurrentWeapon.CurrentAmmo, CurrentWeapon.MagazineSize);
        }
    }

    // Equip the weapon we specify
    public void EquipWeapon(Weapon weapon, Transform weaponPosition)
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.WeaponAmmo.SaveAmmo();
            WeaponAim.DestroyReticle();       // Each weapon has its own Reticle component
            // Destroy(GameObject.Find("Pool")); // --- DELETE THIS
			Destroy(CurrentWeapon.WeaponPooler.PoolContainer); // ---- NEW
            Destroy(CurrentWeapon.gameObject);
        }

        CurrentWeapon = Instantiate(weapon, weaponPosition.position, weaponPosition.rotation);
        CurrentWeapon.transform.parent = weaponPosition;
        CurrentWeapon.SetOwner(character);     
        WeaponAim = CurrentWeapon.GetComponent<WeaponAim>(); 

        if (character.CharacterType == Character.CharacterTypes.Player)
        {
			if(Lvl4UIManager.Instance != null)
			{
				Lvl4UIManager.Instance.UpdateWeaponSprite(CurrentWeapon.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite);
			}
            else if(UIManager.Instance != null)
			{
				UIManager.Instance.UpdateWeaponSprite(CurrentWeapon.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite);
			}
        }
    }
	public void SetIsBowUpgraded()
    {
		isBowUpgraded = true;
    }
	
	public void SetIsSwordUpgraded()
    {
		isSwordUpgraded = true;
    }
	
	public void SetIsStaffOwned()
    {
		isStaffOwned = true;
    }

	
	public void SetIsYamatoOwned()
    {
		isYamatoOwned = true;
    }
}

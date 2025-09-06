using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;
    public GameObject firstPassiveItemTest, secondPassiveItemTest;
    public GameObject secondWeaponTest;

    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentHealthRegen;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentStrength;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentPickUpRange;

    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    [Header("I-Frames")]
    public float invincibilityDuration;
    float invinciblityTimer;
    bool isInvincible;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    [System.Serializable]
    public class LevelRange 
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    public List<LevelRange> levelRanges;

    void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;

        inventory = GetComponent<InventoryManager>();

        currentHealth = characterData.MaxHealth;
        currentHealthRegen = characterData.HealthRegen;
        currentMoveSpeed = characterData.MoveSpeed;
        currentStrength = characterData.Strength;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentPickUpRange = characterData.PickUpRange;

        SpawnWeapon(characterData.StartingWeapon);
        SpawnWeapon(secondWeaponTest);
        SpawnPassiveItem(firstPassiveItemTest);
        SpawnPassiveItem(secondPassiveItemTest);
    }

    void Start() 
    {
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    void Update() 
    {
        if (invinciblityTimer > 0)
        {
            invinciblityTimer -= Time.deltaTime;
        }
        else if (isInvincible) 
        {
            isInvincible = false;
        }

        PassiveHealthRegen();
    }

    public void IncreaseExperience(int amount) 
    {
        experience += amount;
        LevelUpChecker();
    }

    void LevelUpChecker() 
    {
        if (experience >= experienceCap) 
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges) 
            {
                if (level >= range.startLevel && level <= range.endLevel) 
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible) 
        {
            currentHealth -= dmg;
            invinciblityTimer = invincibilityDuration;
            isInvincible = true;
            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        Debug.Log("PLAYER DIED LULW");
    }

    public void RestoreHealth(float amount) 
    {
        if(currentHealth < characterData.MaxHealth) 
        {
            currentHealth += amount;
            if(characterData.MaxHealth < currentHealth) 
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }
 
    void PassiveHealthRegen() 
    {
        if (currentHealth < characterData.MaxHealth) 
        {
            currentHealth += currentHealthRegen * Time.deltaTime;
            if(currentHealth> characterData.MaxHealth) 
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon) 
    {
        if (weaponIndex >= inventory.weaponSlots.Count - 1) 
        {
            Debug.LogError("Inventory is full");
            return;
        }

        GameObject spawnedWeapon = Instantiate(weapon,transform.position,Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);

        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());
        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.LogError("Inventory is full");
            return;
        }

        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);

        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());
        passiveItemIndex++;
    }
}

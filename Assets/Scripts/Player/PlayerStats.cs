using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    InputSystemActions playerControls;
    CharacterScriptableObject characterData;

    float currentHealth;
    float currentHealthRegen;
    float currentMoveSpeed;
    float currentStrength;
    float currentProjectileSpeed;
    float currentPickUpRange;

    public ParticleSystem damageEffect;
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }

        set
        {
            if (currentHealth != value)
            {
                currentHealth = value;
                if (GameManager.instance != null) 
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
                }
            }
        }
    }

    public float CurrentHealthRegen
    {
        get { return currentHealthRegen; }

        set
        {
            if (currentHealthRegen != value)
            {
                currentHealthRegen = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthRegenDisplay.text = "Health Regen: " + currentHealthRegen;
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }

        set
        {
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
                }
            }
        }
    }

    public float CurrentStrength
    {
        get { return currentStrength; }

        set
        {
            if (currentStrength != value)
            {
                currentStrength = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentStrengthDisplay.text = "Strength: " + currentStrength;
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }

        set
        {
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
                }
            }
        }
    }

    public float CurrentPickUpRange
    {
        get { return currentPickUpRange; }

        set
        {
            if (currentPickUpRange != value)
            {
                currentPickUpRange = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentPickUpRangeDisplay.text = "PickUp Range: " + currentPickUpRange;
                }
            }
        }
    }
    #endregion

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

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public Text levelText;

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

        CurrentHealth = characterData.MaxHealth;
        CurrentHealthRegen = characterData.HealthRegen;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentStrength = characterData.Strength;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentPickUpRange = characterData.PickUpRange;

        SpawnWeapon(characterData.StartingWeapon); 
        playerControls = new InputSystemActions();
    }

    void Start() 
    {
        experienceCap = levelRanges[0].experienceCapIncrease;
        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentHealthRegenDisplay.text = "Health Regen: " + currentHealthRegen;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
        GameManager.instance.currentStrengthDisplay.text = "Strength: " + currentStrength;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
        GameManager.instance.currentPickUpRangeDisplay.text = "PickUp Range: " + currentPickUpRange;

        GameManager.instance.AssignChosenCharacterUI(characterData);
        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
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
        bool isDieKeyPressed = playerControls.Player.KillSelf.triggered;
        if (isDieKeyPressed) 
        {
            Kill();
        }
        PassiveHealthRegen();
    }

    public void IncreaseExperience(int amount) 
    {
        experience += amount;
        LevelUpChecker();
        UpdateExpBar();
    }

    void UpdateExpBar() 
    {
        expBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText() 
    {
        levelText.text = "Lv " + level.ToString();
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
            UpdateLevelText();
            GameManager.instance.StartLevelUp(); 
        }

    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible) 
        {
            CurrentHealth -= dmg;

            if (damageEffect) Instantiate(damageEffect, transform.position, Quaternion.identity);

            invinciblityTimer = invincibilityDuration;
            isInvincible = true;
            if (CurrentHealth <= 0)
            {
                Kill();
            }
        }
        UpdateHealthBar();
    }

    void UpdateHealthBar() 
    {
        healthBar.fillAmount = currentHealth / characterData.MaxHealth;
    }

    public void Kill()
    {
        if (!GameManager.instance.isGameOver) 
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float amount) 
    {
        if(CurrentHealth < characterData.MaxHealth) 
        {
            CurrentHealth += amount;
            if(characterData.MaxHealth < CurrentHealth) 
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
        UpdateHealthBar();
    }
 
    void PassiveHealthRegen() 
    {
        if (CurrentHealth < characterData.MaxHealth) 
        {
            CurrentHealth += CurrentHealthRegen * Time.deltaTime;
            if(CurrentHealth> characterData.MaxHealth) 
            {
                CurrentHealth = characterData.MaxHealth;
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

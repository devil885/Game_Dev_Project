using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    float currentHealth;
    float currentHealthRegen;
    float currentMoveSpeed;
    float currentStrength;
    float currentProjectileSpeed;

    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    [Header("I-Frames")]
    public float invincibilityDuration;
    float invinciblityTimer;
    bool isInvincible;

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
        currentHealth = characterData.MaxHealth;
        currentHealthRegen = characterData.HealthRegen;
        currentMoveSpeed = characterData.MoveSpeed;
        currentStrength = characterData.Strength;
        currentProjectileSpeed = characterData.ProjectileSpeed;

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
 
}

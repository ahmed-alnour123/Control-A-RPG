using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerXP : MonoBehaviour {

    // xp system
    [HideInInspector]
    public int xp;
    private int lastLevel = 1; // get it from save

    public int level { get { return (xp / 50) <= 1 ? 1 : (xp / 50); } }
    public float levelProgress { get { return ((float)xp - GetLevelXP(level)) / (level <= 1 ? 100 : 50); } }

    // health system
    public int maxHealth;
    [HideInInspector]
    public int currentHealth;
    [HideInInspector]
    public float healthProgress { get { return (float)currentHealth / maxHealth; } }

    // energy system
    public int maxEnergy;
    public int energyDropAmount;
    public int energyDropTimeout;
    [HideInInspector]
    public int currentEnergy;
    [HideInInspector]
    public float energyProgress { get { return (float)currentEnergy / maxEnergy; } }

    void Start() {
        currentHealth = maxHealth; // TODO: get it from saved data
        currentEnergy = maxEnergy; // TODO: get it from saved data
        lastLevel = level;
        // no need for it, but I'll leave it just in case
        // StartCoroutine(DropEnergy());
    }

    // XP
    public void AddXP(int xp) {
        this.xp += xp;
        Debug.Log($"Player XP is {xp}");
        CheckLevel();
    }

    private void CheckLevel() {
        if (lastLevel != level) {
            lastLevel = level;
            Debug.Log($"Level UP {{{level}}}");
        }
    }

    private int GetLevelXP(int level) => level == 1 ? 0 : level * 50; // level 1 is special, so we don't start at level 0

    // Health
    public void ChangeHealth(int amount) {
        if (amount >= 0) {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            Debug.Log($"current health {currentHealth}");
        } else {
            currentHealth -= amount;
            if (currentHealth <= 0) {
                Die();
            }
        }
    }

    private void Die() {
        // TODO: implement function
        throw new System.NotImplementedException();
    }

    // Energy
    public void ChangeEnergy(int amount) {
        if (amount >= 0) {
            currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy);
        } else {
            currentEnergy += amount; // adding here because amount is minus
            if (currentEnergy <= 0) {
                GetTired();
            }
        }
        Debug.Log($"current energy {currentEnergy}");
    }

    private void GetTired() {
        // TODO: implement function
        throw new System.NotImplementedException();
    }

    [System.Obsolete("Don't Use this method, it's here just in case")]
    IEnumerator DropEnergy() {
        while (true) {
            ChangeEnergy(-energyDropAmount);
            yield return new WaitForSeconds(energyDropTimeout);
        }
    }
}

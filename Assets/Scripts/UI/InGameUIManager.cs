using DuloGames.UI;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour {
    public UIProgressBar healthBar;
    public UIProgressBar energyBar;
    public Image levelBar;
    public Text playerLevel;

    private PlayerXP playerXP;

    void Start() {
        playerXP = FindObjectOfType<PlayerXP>();
    }


    void Update() {
        healthBar.fillAmount = playerXP.healthProgress;
        energyBar.fillAmount = playerXP.energyProgress;
        levelBar.fillAmount = playerXP.levelProgress;
        playerLevel.text = "" + playerXP.level;
    }
}

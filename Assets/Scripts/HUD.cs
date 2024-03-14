using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    public enum infoType
    {
        Exp,
        Level,
        Kill,
        Time,
        Health
    }

    public infoType type;

    Text uiText;
    Slider uiSlider;

    private void Awake()
    {
        uiText = GetComponent<Text>();
        uiSlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch(type)
        {
            case infoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                uiSlider.value = curExp / maxExp;
                break;
            case infoType.Level:
                uiText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case infoType.Kill:
                uiText.text = string.Format("{0:F0}", GameManager.instance.Kill);
                break;
            case infoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                uiText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case infoType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                uiSlider.value = curHealth / maxHealth;
                break;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Player : MonoBehaviour
{
    public int level;
    public int health;
    public int maxHealth;
    public TextMeshProUGUI Text_health;
    public Image healthBar;
   // [SerializeField] private FMODUnity.EventReference healSound;
   // [SerializeField] private FMODUnity.EventReference damageSound;
    void Start()
    {
        level = 1;
        maxHealth = 100;
        health = maxHealth;
    }
    private void Update()
    {
        if(Text_health != null)
        {
            Text_health.text = health.ToString();
            healthBar.fillAmount = health * 0.01f;

        }
        health = Mathf.Clamp(health, 0, maxHealth);
        

    }
    public int GetHealth(int amount)
    {
        if(health < maxHealth)
        {
            health += amount;
           // PlayHealSound();

        }
        return health;
    }

    public int GetDamage(int amount)
    {
        if (health > 0)
        {
            health -= amount;
           // PlayDamageSound();
        }
        return health;
    }

    // void PlayHealSound()
    //{
    //    if (!healSound.IsNull)
    //    {
    //        FMOD.Studio.EventInstance healInstance = FMODUnity.RuntimeManager.CreateInstance(healSound);
    //        healInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    //        healInstance.start();
    //        healInstance.release();
    //    }
    //}

    // void PlayDamageSound()
    //{
    //    if (!damageSound.IsNull)
    //    {
    //        FMOD.Studio.EventInstance damageInstance = FMODUnity.RuntimeManager.CreateInstance(damageSound);
    //        damageInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
    //        damageInstance.start();
    //        damageInstance.release();
    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;


public abstract class PowerUpData : ScriptableObject
{
    #region Fields
    
    public float timer;

    public abstract void ApplyPowerUp(Agent agent);

    public abstract void DeletePowerUp(Agent agent);

    #endregion
}

[CreateAssetMenu(fileName = "StatUp", menuName = "PowerUp/StatUp")]
public class StatUp : PowerUpData
{
    public int extraDamage;
    public float extraSpeed;
    
    public override void ApplyPowerUp(Agent agent)
    {
        if (agent.currentPowerUp != null)
        {
            agent.DeletePowerUp();
        }
        agent.currentPowerUp = this;
        agent.extraDamage = extraDamage;
        agent.extraSpeed = extraSpeed;
        agent.ActivatePowerUpTimer();
    }

    public override void DeletePowerUp(Agent agent)
    {
        agent.currentPowerUp = null;
        agent.extraDamage = 0;
        agent.extraSpeed = 0;
    }
}

//[CreateAssetMenu(fileName = "Shield", menuName = "PowerUp/Shield")]
public class Shield : PowerUpData
{
    public GameObject shieldPrefab;
    
    public override void ApplyPowerUp(Agent agent)
    {
        if (agent.currentPowerUp != null)
        {
            agent.DeletePowerUp();
        }
        agent.currentPowerUp = this;
        agent.ActivatePowerUpTimer();
    }

    public override void DeletePowerUp(Agent agent)
    {
        agent.currentPowerUp = null;
    }
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "PowerUp/NewWeapon")]
public class NewWeapon : PowerUpData
{
    public WeaponData weaponData;
    
    public override void ApplyPowerUp(Agent agent)
    {
        if (agent.currentPowerUp != null)
        {
            agent.DeletePowerUp();
        }
        agent.currentPowerUp = this;
        foreach (WeaponControl weaponControl in agent.input.weapons.GetComponentsInChildren<WeaponControl>())
        {
            weaponControl.temporaryWeapon = weaponData;
        }
        agent.ActivatePowerUpTimer();
    }

    public override void DeletePowerUp(Agent agent)
    {
        agent.currentPowerUp = null;
        foreach (WeaponControl weaponControl in agent.input.weapons.GetComponentsInChildren<WeaponControl>())
        {
            weaponControl.temporaryWeapon = null;
        }
    }
}

//[CreateAssetMenu(fileName = "SlowMotion", menuName = "PowerUp/SlowMotion")]
public class SlowMotion : PowerUpData
{
    public float slowMoValue = .5f;
    
    public override void ApplyPowerUp(Agent agent)
    {
        if (agent.currentPowerUp != null)
        {
            agent.DeletePowerUp();
        }
        agent.currentPowerUp = this;
        agent.ActivatePowerUpTimer();
    }

    public override void DeletePowerUp(Agent agent)
    {
        agent.currentPowerUp = null;
    }
}

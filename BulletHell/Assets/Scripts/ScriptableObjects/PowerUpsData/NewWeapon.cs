using UnityEngine;

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
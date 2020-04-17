using UnityEngine;

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
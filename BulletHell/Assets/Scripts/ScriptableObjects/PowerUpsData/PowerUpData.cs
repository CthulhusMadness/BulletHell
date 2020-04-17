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

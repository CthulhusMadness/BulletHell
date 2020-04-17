using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "PowerUp/Shield")]
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
        GameObject shield = Instantiate(shieldPrefab, agent.transform);
        shield.transform.localPosition = Vector3.zero;
        shield.name = "Shield";
        agent.ActivatePowerUpTimer();
    }

    public override void DeletePowerUp(Agent agent)
    {
        agent.currentPowerUp = null;
        Destroy(agent.transform.Find("Shield").gameObject);
    }
}
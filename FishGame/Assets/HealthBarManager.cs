using Microlight.MicroBar;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] MicroBar healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.Initialize(100f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage()
    {
        float damageAmount = Random.Range(5f,15f);
        healthBar.UpdateBar(healthBar.CurrentValue- damageAmount);
    }
    public void Heal()
    {
        float healAmount = Random.Range(5f,15f);
        healthBar.UpdateBar(healthBar.CurrentValue + healAmount);
    }
}

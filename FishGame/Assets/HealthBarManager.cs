using Microlight.MicroBar;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] MicroBar healthBar;
    const float MAX_HP = 100f;
    float hp = MAX_HP;

    float HP{
       get => hp;
       set{
            bool isHeal = value > hp;
            hp = Mathf.Clamp(value,0f, MAX_HP);
            if(isHeal){
                healthBar.UpdateBar(hp,UpdateAnim.Heal);
            }
            else{
                healthBar.UpdateBar(hp,UpdateAnim.Damage);
            }
            healthBar.UpdateBar(hp, UpdateAnim.Damage);

        }
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.Initialize(100f);

        InvokeRepeating("Damage",0f,2f);
        InvokeRepeating("Heal",0f,5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage()
    {
        float damageAmount = Random.Range(5f,10f);
        healthBar.UpdateBar(healthBar.CurrentValue- damageAmount);
       
    }
    public void Heal()
    {
        float healAmount = Random.Range(5f,15f);
        healthBar.UpdateBar(healthBar.CurrentValue + healAmount);

    }
}

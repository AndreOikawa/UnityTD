using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private float maxHealth = 100f;
    private float currentHealth = 100f;
    public void SetMaxHealth(float val) {
        slider.maxValue = val;
        maxHealth = val;
        slider.value = val;
    }
    private void LateUpdate() {
        
        transform.LookAt(GameObject.Find("/Player/Camera").transform.forward + transform.position);
        // transform.Rotate(0,180,0);
    }

    public bool IsDead() {
        return currentHealth <= 0;
    }
    public void DamageTaken(float val) {
        currentHealth -= val;
        slider.value = Mathf.Max(currentHealth, 0);
    }
}

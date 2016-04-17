using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {
    public float amount = 0.21f;

    void OnTriggerEnter2D (Collider2D col) {
        var getter = col.GetComponent<IDamagable>();
        if(getter != null) {
            getter.GetDamage(amount);
        }
    }
}

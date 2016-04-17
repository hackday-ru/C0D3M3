using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Damage : NetworkBehaviour {
    public float amount = 0.21f;
    public GameObject damage;

    void OnTriggerEnter2D (Collider2D col) {
        var getter = col.GetComponent<IDamagable>();
        if(getter != null) {
            getter.GetDamage(amount);
        }

        Instantiate(damage, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

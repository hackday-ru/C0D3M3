using UnityEngine;
using System.Collections;

public class Absorber : MonoBehaviour, IDamagable {
    public Controller player;
    public float multiply;
    
    public void GetDamage(float amount) {
        player.GetDamage(amount * multiply);
    }
}

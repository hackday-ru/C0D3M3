using UnityEngine;
using System.Collections;

public class BatteryState : MonoBehaviour {
    public Sprite[] states;
    public float value = 1.0f;

	void Start () {
	}

	void Update () {
        int i = (int)(Mathf.Min(value, 0.999f) * states.Length);
        GetComponent<SpriteRenderer>().sprite = states[i];
	}
}

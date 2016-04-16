using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Interface : NetworkBehaviour {
    public Rigidbody2D rigid;
    public Controller stats;

	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        stats = GetComponent<Controller>();
	}

	void Update () {
	}

    public float[] velocity {
        get {
            float[] res = new float[2];
            res[0] = rigid.velocity.x;
            res[1] = rigid.velocity.y;
            return res;
        } set {
            rigid.velocity = new Vector2(value[0], value[1]);
        }
    }
}

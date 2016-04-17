using UnityEngine;
using UnityEngine.Networking;
using IronPython.Runtime;
using System.Collections;

public class Interface : NetworkBehaviour {
    public Rigidbody2D rigid;
    public Controller stats;
    public GameObject bullet;

    private float bulletRest = 0.0f;

	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        stats = GetComponent<Controller>();
	}

	void Update () {
        bulletRest = Mathf.Max(0, bulletRest - 1);
	}

    public bool attack () {
        float consume = 0.4f;
        Debug.Log(stats.battery + " > " + (consume + 0.01f));
        if (stats.battery > consume + 0.01f && bulletRest == 0) {
            bulletRest = 60;

            stats.battery -= consume;

            Transform gun = transform.Find("gun");
            GameObject b = Instantiate(bullet, gun.position - gun.right, gun.rotation) as GameObject;
            b.GetComponent<Rigidbody2D>().velocity = -gun.right;
            NetworkServer.Spawn(b);

            stats.battery -= consume;
            return true;
        } return false;
    }

    public bool move (float[] vec) {
        float consume = 0.006f;
        if(stats.battery > consume) {
            var force = (vec[1] * transform.up - vec[0] * transform.right);
            rigid.AddForce(force);
            stats.battery -= force.magnitude * consume;
            return true;
        } return false;
    }

    public bool rotate (float angle) {
        float consume = 0.01f*Mathf.Abs(angle);
        if (stats.battery > consume) {
            rigid.AddTorque(angle, ForceMode2D.Force);
            stats.battery -= consume;
            return true;
        }
        return false;
    }

    public PythonTuple opponent {
        get {
            if (stats.opponent == null) {
                return null;
            }

            var v = stats.opponent.transform.position - transform.position;
            var r = Vector3.Angle(-transform.right, v);
            return new PythonTuple(new float [] { v.x, v.y, r });
        }
    }
}

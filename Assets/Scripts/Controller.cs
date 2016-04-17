using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class Controller : NetworkBehaviour, IDamagable {
    public BatteryState EPBar;
    public BatteryState HPBar;

    public GameObject Overs;

    public GameObject opponent;

    [SyncVar]
    public float speed = 1.0f;
    [SyncVar]
    public float battery = 1.0f;
    [SyncVar]
    public float health = 1.0f;
    [SyncVar]
    public string code = "";

    [Command]
    public void CmdSyncCode(string code) {
        GameObject.Find("OpponentCode").GetComponent<InputField>().text = code;
    }

    private Rigidbody2D rigid;

	void Start () {
        opponent = null;

        rigid = GetComponent<Rigidbody2D>();
        Overs = GameObject.Find("OverCanvas");
    }
	
    void UISync () {
        EPBar.value = battery;
        HPBar.value = health;

        if(!isLocalPlayer) {
            GameObject.Find("OpponentCode").GetComponent<InputField>().text = code;
        }
    }

    void Halt() {
        if (isLocalPlayer && health <= 0.01f) {
            Overs.transform.GetChild(0).gameObject.SetActive(true);
        } else if (!isLocalPlayer && health <= 0.01f) {
            Overs.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    void Controles () {
        Interface self = GetComponent<Interface>();
        if(self != null) {
            self.ExecInPython((GameObject.Find("MineCode").GetComponent<InputField>()).text);
        }
    }

    void Restores () {
        battery = Mathf.Max(0.002f, Mathf.Min(battery + 0.002f, 1));
    }

    void AttachOpponent() {
        if(opponent != null) {
            return;
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
            if (player.GetInstanceID() != gameObject.GetInstanceID()) {
                opponent = player;
            }
        }
    }

    void Update () {
        if (!isServer) {
            CmdSyncCode(GameObject.Find("OpponentCode").GetComponent<InputField>().text);
        }

        AttachOpponent();
        UISync();

        if (!isLocalPlayer)
            return;

        Restores();
        Controles();
        Halt();
    }

    public void GetDamage (float amount) {
        health -= amount;
    }

    public override void OnStartLocalPlayer() {
        GetComponent<SpriteRenderer>().color = Color.red;
        if(GameObject.FindGameObjectsWithTag("Player").Length == 1) {
            transform.position -= transform.right;
            transform.rotation = new Quaternion(0, 0, -1, 0);
        } else {
            transform.position += 3*transform.right;
        }

        RectTransform mine = GameObject.Find("MineCode").GetComponent<RectTransform>(),
            oponnent = GameObject.Find("OpponentCode").GetComponent<RectTransform>();

        mine.localPosition += 2 * new Vector3(mine.sizeDelta.x, 0);
        oponnent.localPosition -= 2 * new Vector3(mine.sizeDelta.x, 0);
    }
}

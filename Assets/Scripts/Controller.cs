using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class Controller : NetworkBehaviour {
    public Slider EnergyBar;
    public Slider HPBar;

    [SyncVar]
    public float speed = 1.0f;
    [SyncVar]
    public float battery = 1.0f;
    [SyncVar]
    public float health = 1.0f;

    private Rigidbody2D rigid;

	void Start () {
        rigid = GetComponent<Rigidbody2D>();
    }
	
    void UISync () {
        EnergyBar.value = battery;
    }

    void Halt () {
        if(battery < 0.001) {
            rigid.velocity = Vector2.zero;
        }
    }

    void Controles () {
        /*if (battery > 0) {
            var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (direction.sqrMagnitude > 1) {
                direction.Normalize();
            } rigid.velocity = speed * direction;
        } else {
            rigid.velocity = Vector2.zero;
        }*/
        
        Interface self = GetComponent<Interface>();
        if(self != null) {
            self.ExecInPython((GameObject.Find("MineCode").GetComponent<InputField>()).text);
        }
    }

    void Consumes () {
        battery -= rigid.velocity.magnitude / 100;

        battery = Mathf.Max(battery, 0);
    }

    void Restores () {
        battery += 0.01f;

        battery = Mathf.Min(battery, 1);
    }

	void Update () {
        UISync();

        if (!isLocalPlayer)
            return;

        Restores();
        Consumes();
        Controles();
        Halt();
    }

    public override void OnStartLocalPlayer() {
        GetComponent<SpriteRenderer>().color = Color.red;

        RectTransform mine = GameObject.Find("MineCode").GetComponent<RectTransform>(),
            oponnent = GameObject.Find("OponnentCode").GetComponent<RectTransform>();

        mine.localPosition += 2 * new Vector3(mine.sizeDelta.x, 0);
        oponnent.localPosition -= 2 * new Vector3(mine.sizeDelta.x, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    ETCJoystick joystick;
    public float speed = 15f;
    private Animator animator;
    private int state = AnimaState.IDLE;
    public int seat;
    private bool canMove = true;
    private float distance = 2f;
    private float hp = 20;
    private float mp = 0;
    [SerializeField]
    private TextMesh hpText;
    [SerializeField]
    private TextMesh mpText;
    // Start is called before the first frame update
    void Start()
    {
        hpText.text = "hp:" + hp.ToString();
        mpText.text = "mp:" + mp.ToString();
        animator = GetComponent<Animator>();
        animator.SetInteger(AnimaState.state, state);
    }
    public void ChangeHp(float value)
    {
        if (hp - value > 0)
        {
            hp -= value;
        }
        else
        {
            hp = 0;
        }
        hpText.text = "hp:" + hp.ToString();
    }
    public void ChangeMp(float value)
    {
        // 暂时写这里
        if (mp + value <= 5)
        {
            mp += value;
        }
        else
        {
            mp = 5;
        }
        mpText.text = "mp:" + mp.ToString();
    }
    public void Init(int seatId)
    {
        seat = seatId;
        if (seat == GameData.seat)
        {
            joystick = GameObject.Find("Joystick").GetComponent<ETCJoystick>();
            joystick.onMove.AddListener(onMoveHandler);
            joystick.onMoveEnd.AddListener(onMoveEndHandler);
        }
    }
    void onMoveHandler(Vector2 position)
    {
        SendMoveMsg(position.x, position.y, Time.deltaTime);
        return;
        float angle = Mathf.Atan2(position.x, position.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        // 球形射线检测
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * speed * Time.deltaTime, distance);
        bool judge = false;
        for (int i = 0; i < hitColliders.Length; ++i)
        {
            if (hitColliders[i].gameObject.tag == "Wall")
            {
                judge = true;
                break;
            }
        }
        if (!judge)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
        /* 
        // 纵向,横向射线检测
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance)
            || Physics.Raycast(transform.position, transform.right, out hit, distance)
            || Physics.Raycast(transform.position, -transform.right, out hit, distance)
        {
            if (hit.collider.tag != "Wall")
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
            }
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
        */
    }
    void onMoveEndHandler()
    {
        SendMoveMsg(0, 0, 0);
    }
    void SendMoveMsg(float x, float y, float deltaTime)
    {
        MoveDTO move = new MoveDTO();
        move.Roomid = GameData.room.Roomid;
        move.Seat = GameData.seat;
        move.X = x;
        move.Y = y;
        move.DeltaTime = deltaTime;
        this.WriteMessage((int)MsgTypes.TypeFight, (int)FightTypes.MoveCreq, move.ToByteArray());
    }
    public void onMoveMsgHandler(float x, float y, float deltaTime)
    {
        if (x != 0 || y != 0)
        {
            if (state != AnimaState.RUN)
            {
                state = AnimaState.RUN;
                animator.SetInteger(AnimaState.state, AnimaState.RUN);
            }
            float angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            // 球形射线检测
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * speed * deltaTime, distance);
            bool judge = false;
            for (int i = 0; i < hitColliders.Length; ++i)
            {
                if (hitColliders[i].gameObject.tag == "Wall")
                {
                    judge = true;
                    break;
                }
            }
            if (!judge)
            {
                transform.Translate(Vector3.forward * speed * deltaTime, Space.Self);
            }
        }
        else
        {
            if (state != AnimaState.IDLE)
            {
                state = AnimaState.IDLE;
                animator.SetInteger(AnimaState.state, AnimaState.IDLE);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        float val = Vector3.Dot(transform.forward, other.gameObject.transform.position);
        // 点积结果为正=>正面
        if (val > 0)
        {
            ChangeHp(2);
            ChangeMp(1);
        }
        else
        {
            ChangeHp(7);
        }
    }
}

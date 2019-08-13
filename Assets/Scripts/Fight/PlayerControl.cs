using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    ETCJoystick joystick;
    public float speed = 20f;
    private Animator animator;
    private int state = AnimaState.IDLE;
    public int seat;
    private float distance = 2f;
    private float repulseDistance = 10f;
    private float repulseSpeed = 1;
    private Coroutine repulseCoroutine;
    private bool canMove = true;
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
        // animator = GetComponent<Animator>();
        // animator.SetInteger(AnimaState.state, state);
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
            FrameActions.Instance().Init(seat);
        }
    }
    void onMoveHandler(Vector2 position)
    {
        SendMoveMsg(position.x, position.y, Time.deltaTime);
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
        if (!FrameActions.Instance().isLock)
        {
            FrameInfo move = new FrameInfo();
            FrameActions.Instance().Add(move);
        }
    }
    public void onMoveMsgHandler(Google.Protobuf.Collections.RepeatedField<FrameInfo> frameInfo)
    {
        List<FrameInfo> frames = new List<FrameInfo>();
        for (int i = 0; i < frameInfo.Count; ++i)
        {
            frames.Add(frameInfo[i]);
        }
        // 顺序重排
        frames.Sort((a, b) => a.Frame.CompareTo(b.Frame));
        for (int i = 0; i < frames.Count; ++i)
        {
            // 判断是技能还是移动
            if (true)
            {
                Move(frames[i].Move);
            }
            else
            {

            }
        }





    }
    private void Move(DeltaDirection direction)
    {
        if (direction.X != 0 || direction.Y != 0)
        {
            if (state != AnimaState.RUN)
            {
                state = AnimaState.RUN;
                // animator.SetInteger(AnimaState.state, AnimaState.RUN);
            }
            float angle = Mathf.Atan2(direction.X, direction.Y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            // 球形射线检测
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * direction.DeltaTime * speed, distance);
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
                transform.Translate(Vector3.forward * direction.DeltaTime * speed, Space.Self);
            }
        }
        else
        {
            if (state != AnimaState.IDLE)
            {
                state = AnimaState.IDLE;
                // animator.SetInteger(AnimaState.state, AnimaState.IDLE);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator Repulse(Vector3 direction, float distance)
    {
        int index = 0;
        int time = Mathf.CeilToInt(distance / repulseSpeed);
        while (index < time - 1)
        {
            transform.Translate(direction.normalized * repulseSpeed, Space.World);
            ++index;
            yield return null;
        }
        transform.Translate(direction.normalized * (distance - index * repulseSpeed), Space.World);
        canMove = true;
        StopCoroutine(repulseCoroutine);
    }
    public void LightCollision(Vector3 direction)
    {
        float val = Vector3.Dot(transform.forward, direction.normalized);
        // 点积结果为负=>正面
        if (val < 0)
        {
            float moveDistance = repulseDistance;
            ChangeHp(2);
            ChangeMp(1);
            // 更改人物状态为击退(一个不可移动的状态)
            canMove = false;
            // 播放击退动画
            // 射线相交计算
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, distance + repulseDistance))
            {
                if (hit.collider.tag == "Wall")
                {
                    // ps:乘以1.5,否则会卡在无法移动的区域里面
                    moveDistance = Vector3.Distance(transform.position, hit.point) - distance * (float)1.5; ;
                }
            }
            repulseCoroutine = StartCoroutine(Repulse(direction, moveDistance));
        }
        else
        {
            ChangeHp(7);
        }
    }
}

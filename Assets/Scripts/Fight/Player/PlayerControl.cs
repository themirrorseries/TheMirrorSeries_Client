using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class PlayerControl : MonoBehaviour
{
    ETCJoystick joystick;
    public float speed = 20f;
    private PlayerAttribute playerAttribute;
    private AnimationControl animationControl;
    public int seat;
    private float wallDistance = 2f;
    private float repulseDistance = 10f;
    private float repulseSpeed = 1;
    private Coroutine repulseCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        animationControl = GetComponent<AnimationControl>();
        playerAttribute = GetComponent<PlayerAttribute>();
    }
    public void Init(int seatId)
    {
        seat = seatId;
        if (seat == GameData.seat)
        {
            joystick = GameObject.Find("Joystick").GetComponent<ETCJoystick>();
            joystick.onMove.AddListener(onMoveHandler);
            joystick.onMoveEnd.AddListener(onMoveEndHandler);
            FrameActions.instance.Init(seat);
        }
        // 防止未获取到组件
        if (playerAttribute == null)
        {
            playerAttribute = GetComponent<PlayerAttribute>();
        }
        playerAttribute.Init();
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
        if (!FrameActions.instance.isLock)
        {
            DeltaDirection direction = new DeltaDirection();
            direction.X = x;
            direction.Y = y;
            direction.DeltaTime = deltaTime;
            FrameInfo move = new FrameInfo();
            move.Move = direction;
            move.Move.DeltaTime = deltaTime;
            FrameActions.instance.Add(move);
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
            animationControl.Run();
            float angle = Mathf.Atan2(direction.X, direction.Y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            if (!playerAttribute.canMove()) return;
            // 球形射线检测
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * direction.DeltaTime * speed, wallDistance, LayerMask.GetMask("Wall"));
            if (hitColliders.Length == 0)
            {
                transform.Translate(Vector3.forward * direction.DeltaTime * speed, Space.Self);
            }
        }
        else
        {
            animationControl.Idle();
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
            transform.Translate(direction * repulseSpeed, Space.World);
            ++index;
            yield return null;
        }
        transform.Translate(direction * (distance - index * repulseSpeed), Space.World);
        playerAttribute.isRepulse = false;
        StopCoroutine(repulseCoroutine);
    }
    public void LightCollision(Vector3 direction)
    {
        float val = Vector3.Dot(transform.forward, direction);
        // 点积结果为负=>正面
        if (val < 0)
        {
            float moveDistance = repulseDistance;
            playerAttribute.ChangeHp(-2);
            playerAttribute.ChangeMp(1);
            playerAttribute.isRepulse = true;
            animationControl.Repulse();
            // 射线相交计算
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, wallDistance + repulseDistance, LayerMask.GetMask("Wall")))
            {
                // ps:乘以2,否则会卡在无法移动的区域里面
                moveDistance = Vector3.Distance(transform.position, hit.point) - wallDistance * (float)2;
            }
            repulseCoroutine = StartCoroutine(Repulse(direction.normalized, moveDistance));
        }
        else
        {
            playerAttribute.ChangeHp(-7);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class PlayerControl : MonoBehaviour
{
    ETCJoystick joystick;
    public float speed = 10f;
    private PlayerAttribute playerAttribute;
    private AnimationControl animationControl;
    private PlayerSkill playerSkill;
    private float wallDistance = 2f;
    private float repulseDistance = 10f;
    private float repulseSpeed = 1f;
    private Coroutine repulseCoroutine;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void Init(int seatId)
    {
        attr.seat = seatId;
        if (seatId == GameData.seat)
        {
            joystick = GameObject.Find("Joystick").GetComponent<ETCJoystick>();
            joystick.onMoveStart.AddListener(onMoveStartHandler);
            joystick.onMove.AddListener(onMoveHandler);
            joystick.onMoveEnd.AddListener(onMoveEndHandler);
            FrameActions.instance.Init();
        }
        attr.Init();
        skill.Init();
    }

    public void Ack()
    {
        SendSkillMsg(SkillEunm.ack, 0, 0);
    }
    public void Skill1()
    {
        SendSkillMsg(SkillEunm.skill1, 0, 0);
    }

    public void Skill2()
    {
        SendSkillMsg(SkillEunm.skill2, 0, 0);
    }

    public void Skill3()
    {
        SendSkillMsg(SkillEunm.skill3, 0, 0);
    }
    void SendSkillMsg(int skillNum, float x, float y)
    {
        if (!FrameActions.instance.isLock)
        {
            FrameInfo skillInfo = new FrameInfo();
            skillInfo.Skillid = skillNum;
            FrameActions.instance.Add(skillInfo);
        }
    }
    void onMoveStartHandler()
    {
        FrameActions.instance.needAdd = false;
    }
    void onMoveHandler(Vector2 position)
    {
        SendMoveMsg(position.x, position.y, Time.deltaTime);
    }
    void onMoveEndHandler()
    {
        SendMoveMsg(0, 0, 0);
        FrameActions.instance.needAdd = true;
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
            move.Skillid = SkillEunm.notSkill;
            move.Move = direction;
            move.Move.DeltaTime = deltaTime;
            FrameActions.instance.Add(move);
        }
    }
    public void onMsgHandler(Google.Protobuf.Collections.RepeatedField<FrameInfo> frameInfo)
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
            // 空帧
            if (frames[i].Skillid == SkillEunm.empty)
            {
                continue;
            }
            // 判断是技能还是移动
            else if (frames[i].Skillid == SkillEunm.notSkill)
            {
                Move(frames[i].Move);
            }
            else
            {
                Skill(frames[i].Skillid, frames[i].SkillDir);
            }
        }
    }
    private void Move(DeltaDirection direction)
    {
        if (direction.X != 0 || direction.Y != 0)
        {
            anim.Run();
            float angle = Mathf.Atan2(direction.X, direction.Y) * Mathf.Rad2Deg;
            // 混乱状态下,操作与移动方向相反
            if (attr.isChaos)
            {
                angle = -angle;
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            if (!playerAttribute.canMove()) return;
            // 球形射线检测
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * direction.DeltaTime * speed, wallDistance, LayerMask.GetMask(LayerEunm.WALL));
            if (hitColliders.Length == 0)
            {
                transform.Translate(Vector3.forward * direction.DeltaTime * speed, Space.Self);
            }
        }
        else
        {
            anim.Idle();
        }
    }
    private void Skill(int skillNum, DeltaDirection direction)
    {
        playerSkill.Release(skillNum, direction);
    }
    // Update is called once per frame
    void Update()
    {
    }
    // 击退协程
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
            playerAttribute.ChangeMp(5);
            // 击退期间有新的碰撞,则停止上一次的击退
            if (playerAttribute.isRepulse)
            {
                StopCoroutine(repulseCoroutine);
            }
            else
            {
                playerAttribute.isRepulse = true;
            }
            animationControl.Repulse();
            // 射线相交计算
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, wallDistance + repulseDistance, LayerMask.GetMask(LayerEunm.WALL)))
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
    public PlayerAttribute attr
    {
        get
        {
            if (playerAttribute == null)
            {
                playerAttribute = GetComponent<PlayerAttribute>();
            }
            return playerAttribute;
        }
    }
    public AnimationControl anim
    {
        get
        {
            if (animationControl == null)
            {
                animationControl = GetComponent<AnimationControl>();
            }
            return animationControl;
        }
    }
    public PlayerSkill skill
    {
        get
        {
            if (playerSkill == null)
            {
                playerSkill = GetComponent<PlayerSkill>();
            }
            return playerSkill;
        }
    }
}

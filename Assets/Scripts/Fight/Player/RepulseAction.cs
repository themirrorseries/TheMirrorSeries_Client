using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulseAction : MonoBehaviour
{
    // 默认击退距离
    private float defaultDistance = 10f;
    // 击退过程移动速度
    private float speed = 10f;
    // 击退方向
    private Vector3 repulseDirection;
    // 击退距离
    private float distance;
    // 当前击退距离
    private float curDistance;
    private PlayerAttribute attr;
    // Start is called before the first frame update
    void Start()
    {
        attr = GetComponent<PlayerAttribute>();
    }
    public void Check(float wallDistance, Vector3 direction)
    {
        // 射线相交计算
        RaycastHit hit;
        float moveDistance = defaultDistance;
        if (Physics.Raycast(transform.position, direction, out hit, wallDistance + defaultDistance, LayerMask.GetMask(LayerEunm.WALL)))
        {
            // 如果击退到墙壁,会二段扣血
            attr.ChangeHp(-2);
            if (attr.isDied)
            {
                AnimationControl anim = GetComponent<AnimationControl>();
                anim.Death();
                FightScene.instance.AddDeath(attr.seat, attr.bounces);
                return;
            }
            // ps:乘以2,否则会卡在无法移动的区域里面
            moveDistance = Vector3.Distance(transform.position, hit.point) - wallDistance * (float)2;
        }
        curDistance = 0;
        repulseDirection = direction.normalized;
        distance = moveDistance;
        attr.isRepulse = true;
    }
    public void Repulse(float deltaTime)
    {
        if (attr.isRepulse == false)
        {
            return;
        }
        else
        {
            if ((curDistance + speed * deltaTime) < distance)
            {
                curDistance += speed * deltaTime;
                transform.Translate(repulseDirection * speed * deltaTime, Space.World);
            }
            else
            {
                attr.isRepulse = false;
            }
        }
    }
}

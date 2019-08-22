using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    ////////// 击退 //////////
    ///       start       ///
    // 默认击退距离
    private float defaultRepulseDistance = 5f;
    // 击退过程移动速度
    private float repulseSpeed = 10f;
    // 击退方向
    private Vector3 repulseDirection;
    // 击退距离
    private float repulseDistance;
    // 当前击退距离
    private float curRepulseDistance;
    ///       end       ///

    ////////// 死亡 //////////
    ///       start       ///
    // 死亡移动标志位
    private bool isDeathMove = false;
    // 默认死亡距离
    private float defaultDeathDistance = 10f;
    // 死亡过程移动速度
    private float deathSpeed = 10f;
    // 死亡方向
    private Vector3 deathDirection;
    // 死亡距离
    private float deathDistance;
    // 当前死亡距离
    private float curDeathDistance;
    ///       end       ///

    private PlayerAttribute attr;
    // Start is called before the first frame update
    void Start()
    {
        attr = GetComponent<PlayerAttribute>();
    }
    public void UpdateState(float deltaTime)
    {
        Repulse(deltaTime);
        Death(deltaTime);
    }
    public void CheckRepulse(float wallDistance, Vector3 direction)
    {
        // 射线相交计算
        RaycastHit hit;
        float moveDistance = defaultRepulseDistance;
        if (Physics.Raycast(transform.position, direction, out hit, wallDistance + defaultRepulseDistance, LayerMask.GetMask(LayerEunm.WALL)))
        {
            // 如果击退到墙壁,会二段扣血
            attr.ChangeHp(-7);
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
        curRepulseDistance = 0;
        repulseDirection = direction.normalized;
        repulseDistance = moveDistance;
        attr.isRepulse = true;
    }

    public void CheckDeath(float wallDistance, Vector3 direction)
    {
        // 进入这个函数后,角色死亡
        // 先关闭碰撞盒,防止重复计算
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
        // 射线相交计算
        RaycastHit hit;
        float moveDistance = defaultDeathDistance;
        if (Physics.Raycast(transform.position, direction, out hit, wallDistance + defaultDeathDistance, LayerMask.GetMask(LayerEunm.WALL)))
        {
            moveDistance = Vector3.Distance(transform.position, hit.point) - wallDistance;
        }
        curDeathDistance = 0;
        deathDirection = direction.normalized;
        deathDistance = moveDistance;
        isDeathMove = true;
    }
    public void Repulse(float deltaTime)
    {
        if (attr.isRepulse == false)
        {
            return;
        }
        else
        {
            if ((curRepulseDistance + repulseSpeed * deltaTime) < repulseDistance)
            {
                curRepulseDistance += repulseSpeed * deltaTime;
                transform.Translate(repulseDirection * repulseSpeed * deltaTime, Space.World);
            }
            else
            {
                attr.isRepulse = false;
            }
        }
    }
    public void Death(float deltaTime)
    {
        if (isDeathMove == false)
        {
            return;
        }
        else
        {
            if ((curDeathDistance + deathSpeed * deltaTime) < deathDistance)
            {
                curDeathDistance += deathSpeed * deltaTime;
                transform.Translate(deathDirection * deathSpeed * deltaTime, Space.World);
            }
            else
            {
                isDeathMove = false;
                attr.isEnd = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    // 初始速度
    private float speed;
    private float curSpeed;
    // 碰撞次数
    private int count;
    private int index = 0;
    private Vector3 direction;
    private float distance = 1f;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init(float _speed, int _count, float _x, float _z)
    {
        speed = _speed;
        curSpeed = speed;
        count = _count;
        direction = new Vector3(_x, 0, _z);
    }

    public void Move(float deltaTime)
    {
        if (index < count)
        {
            // 射线相交计算
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, distance, LayerMask.GetMask(LayerEunm.WALL) | LayerMask.GetMask(LayerEunm.PLAYER)))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    // ps:防止光线先改变方向
                    PlayerAttribute playerAttribute = hit.collider.gameObject.GetComponent<PlayerAttribute>();
                    // 反弹次数暂定每次直接++, 1:每次 2:正面 3:反面 4:保护罩期间怎么计算
                    playerAttribute.bounces++;
                    // 保护罩期间直接反弹
                    if (!playerAttribute.hasProtection)
                    {
                        PlayerControl playerControl = hit.collider.gameObject.GetComponent<PlayerControl>();
                        playerControl.LightCollision(gameObject, direction.normalized);
                    }
                }
                Reflect(hit.collider.gameObject.transform.forward.normalized);
                transform.Translate(direction.normalized * deltaTime * curSpeed);
            }
            else
            {
                transform.Translate(direction.normalized * deltaTime * curSpeed);
            }
        }
    }

    private void Reflect(Vector3 forward)
    {
        // 计算反射方向
        direction = direction - 2 * Vector3.Dot(direction, forward) * forward;
        // 碰撞次数++
        index++;
        curSpeed = speed + Mathf.Log(index + 1, 1.2f);
        if (index == count)
        {
            FightScene.instance.RomoveLight(gameObject);
        }
    }
}

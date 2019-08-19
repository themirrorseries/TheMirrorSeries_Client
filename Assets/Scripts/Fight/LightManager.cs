using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    // 速度
    private float speed = 5f;
    // 碰撞次数
    private int count = 0;
    private int index = 0;
    private Vector3 direction;
    private float distance = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init(float _speed, int _count, float _x, float _z)
    {
        speed = _speed;
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
                    // 保护罩期间直接反弹
                    if (!playerAttribute.hasProtection)
                    {
                        PlayerControl playerControl = hit.collider.gameObject.GetComponent<PlayerControl>();
                        playerControl.LightCollision(direction);
                    }
                }
                Reflect(hit.collider.gameObject.transform.forward.normalized);
            }
            else
            {
                transform.Translate(direction.normalized * deltaTime * speed);
            }
        }
    }

    private void Reflect(Vector3 forward)
    {
        Debug.Log("第" + (index + 1).ToString() + "次碰撞," + "原来的方向:" + direction + "法线:" + forward);
        // 计算反射方向
        direction = direction - 2 * Vector3.Dot(direction, forward) * forward;
        Debug.Log("第" + (index + 1).ToString() + "次反弹方向为:" + direction);
        // 碰撞次数++
        index++;
        if (index == count)
        {
            FightScene.instance.RomoveLight(gameObject);
        }
    }
}

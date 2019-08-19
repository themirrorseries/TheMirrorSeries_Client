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
            transform.Translate(direction.normalized * deltaTime * speed);
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
        if (index == 0)
        {
            FightScene.instance.RomoveLight(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // ps:防止光线先改变方向
            PlayerAttribute playerAttribute = other.gameObject.GetComponent<PlayerAttribute>();
            // 保护罩期间直接反弹
            if (!playerAttribute.hasProtection)
            {
                PlayerControl playerControl = other.gameObject.GetComponent<PlayerControl>();
                playerControl.LightCollision(direction);
            }
        }
        Reflect(other.gameObject.transform.forward.normalized);
    }
}

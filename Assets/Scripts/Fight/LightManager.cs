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

    // Update is called once per frame
    void Update()
    {
        if (index < count)
        {
            transform.Translate(direction.normalized * Time.deltaTime * speed);
        }
    }
    private void Collide(Collision other)
    {
        // 获取接触点
        ContactPoint contactPoint = other.contacts[0];
        // 计算反射角
        direction = Vector3.Reflect(direction.normalized, contactPoint.normal);
        // 碰撞次数++
        index++;
    }
    private void OnCollisionEnter(Collision other)
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
        Collide(other);
    }
}

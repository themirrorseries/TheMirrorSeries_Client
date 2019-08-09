using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    // 速度
    private float speed = 5f;
    // 碰撞次数
    private int count = 0;
    private int index = 0;
    private Vector3 direction;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private TextMesh speedText;
    [SerializeField]
    private ParticleSystem[] particleSystems;
    private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
    }

    public void Init(float _speed, int _count, float _x, float _z)
    {
        speed = _speed;
        count = _count;
        direction = new Vector3(_x, 0, _z);
    }

    void FixedUpdate()
    {
        if (index < count)
        {
            rb.MovePosition(transform.position + direction * Time.deltaTime * speed);
            speedText.text = speed.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void Collide(Collision other)
    {
        // 获取接触点
        ContactPoint contactPoint = other.contacts[0];
        // 计算反射角
        direction = Vector3.Reflect(direction, contactPoint.normal);
        // 碰撞次数++
        index++;
    }
    private void OnCollisionEnter(Collision other)
    {
        Collide(other);
        if (other.gameObject.tag == "Wall")
        {

        }
        else if (other.gameObject.tag == "Wall")
        {

        }
    }
}

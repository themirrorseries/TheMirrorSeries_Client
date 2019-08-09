using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    // 速度
    private float speed = 5f;
    // 碰撞次数
    private int count = 20;
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
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
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
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "GlassesPlayer")
        {

        }
        else if (other.gameObject.tag == "Glasses")
        {
            HPManager hP = other.gameObject.GetComponent<HPManager>();
            if (!hP.isDead())
            {
                hP.HpChange(1f);
                if (hP.isDead())
                {
                    if (FightScene.instance.isEnd())
                    {
                        ps = (ParticleSystem)GameObject.Instantiate(particleSystems[Random.Range(0, particleSystems.Length)], transform.position, Quaternion.identity);
                        ps.Play();
                    }
                }
            }
        }
    }
}

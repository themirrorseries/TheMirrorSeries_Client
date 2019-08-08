using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    // 初始速度
    public float v0 = 5f;
    // 速度公式常数
    public float m = 1, n = 1, b = 2, c = 1, j = 1;
    // 碰撞次数
    private int t1 = 0;
    // 时间
    public float t2;
    private float speed;
    private Vector3 direction;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private TextMesh speedText;
    [SerializeField]
    private Button reStartBtn;
    [SerializeField]
    private ParticleSystem[] particleSystems;
    private ParticleSystem ps;
    private bool isEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        FightScene.instance.Init();
        speed = v0;
        rb = GetComponent<Rigidbody>();
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        t2 = Time.time;
        isEnd = false;
    }

    public void RestartLight()
    {
        gameObject.GetComponent<TrailRenderer>().Clear();
        transform.position = new Vector3(0, 0.25f, 0);
        if (ps != null)
        {
            ps.Stop();
        }
        speed = v0;
        t1 = 0;
        t2 = Time.time;
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }

    void FixedUpdate()
    {
        if (!isEnd)
        {
            speed = v0 + Mathf.Log((m * t1 + n * (Time.time - t2) + c), b) + j * FightScene.instance.deathCount;
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
        t1++;
    }
    private void OnCollisionEnter(Collision other)
    {
        Collide(other);
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "GlassesPlayer")
        {
            // 暂时没有需要处理的
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
                        isEnd = true;
                        ps = (ParticleSystem)GameObject.Instantiate(particleSystems[Random.Range(0, particleSystems.Length)], transform.position, Quaternion.identity);
                        ps.Play();
                        reStartBtn.gameObject.SetActive(true);
                    }
                    else
                    {
                        RestartLight();
                    }
                }
            }
        }
    }
}

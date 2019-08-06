using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    public float startSpeed = 5f;
    private float speed;
    private Vector3 direction;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private TextMesh countText;
    [SerializeField]
    private Button reStartBtn;
    [SerializeField]
    private ParticleSystem[] particleSystems;
    private ParticleSystem ps;
    private int count = 0;
    private bool isEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        FightScene.instance.Init();
        speed = startSpeed;
        rb = GetComponent<Rigidbody>();
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
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
        speed = startSpeed;
        count = 0;
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }

    void FixedUpdate()
    {
        if (!isEnd)
        {
            rb.MovePosition(transform.position + direction * Time.deltaTime * speed);
            countText.text = speed.ToString();
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
        count++;
        speed = Mathf.Log(count, 2) + startSpeed;
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

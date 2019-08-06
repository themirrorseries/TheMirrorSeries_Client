using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    public float speedLeft = 3f;
    public float speedRight = 5f;
    private float speed;
    private float startSpeed;
    private int count;
    public float time;
    public int countLeft = 10;
    public int countRight = 15;
    public float timeLeft = 3f;
    public float timeRight = 5f;
    private bool isStart = false;
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
    [SerializeField]
    private GameObject[] glasses;
    private int startCount;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(speedLeft, speedRight);
        startSpeed = speed;
        rb = GetComponent<Rigidbody>();
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        time = Random.Range(timeLeft, timeRight);
        for (int i = 0; i < glasses.Length; ++i)
        {
            glasses[i].SetActive(true);
        }
    }

    public void Restart()
    {
        transform.position = new Vector3(0, 0.25f, 0);
        reStartBtn.gameObject.SetActive(false);
        if (ps != null)
        {
            ps.Stop();
        }
        countText.gameObject.SetActive(false);
        speed = Random.Range(speedLeft, speedRight);
        startSpeed = speed;
        time = Random.Range(timeLeft, timeRight);
        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        for (int i = 0; i < glasses.Length; ++i)
        {
            glasses[i].SetActive(true);
        }
        isStart = false;
    }

    void FixedUpdate()
    {
        if (count > 0 || !isStart)
        {
            rb.MovePosition(transform.position + direction * Time.deltaTime * speed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                isStart = true;
                count = Random.Range(countLeft, countRight);
                startCount = count;
                countText.gameObject.SetActive(true);
                countText.text = count.ToString();
            }
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "GlassesPlayer")
        {
            // 获取接触点
            ContactPoint contactPoint = other.contacts[0];
            // 计算反射角
            direction = Vector3.Reflect(direction, contactPoint.normal);
            if (isStart)
            {
                count--;
                countText.text = count.ToString();
                speed = startSpeed + Mathf.Log(startCount - count, 2) + 1;
                if (count <= 0)
                {
                    direction = Vector3.zero;
                    ps = (ParticleSystem)GameObject.Instantiate(particleSystems[Random.Range(0, particleSystems.Length)], transform.position, Quaternion.identity);
                    ps.Play();
                    reStartBtn.gameObject.SetActive(true);
                }
            }
        }
        else if (other.gameObject.tag == "Glasses")
        {
            other.gameObject.SetActive(false);
        }
    }
}

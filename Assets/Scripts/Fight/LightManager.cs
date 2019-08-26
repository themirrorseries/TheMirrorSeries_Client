using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    // 初始速度
    private float speed;
    private float curSpeed;
    private float maxSpeed = 55;
    // 碰撞次数
    private int count;
    private int index = 0;
    private Vector3 direction;
    private float distance = 1.5f;
    // 延迟时间
    private float delay = 0.33f;
    // 延迟计时
    private float delayTime;
    // 是否初始化
    private bool isInit;
    // 由那个玩家创建,用于延时显示
    private GameObject player;
    private bool isPlayerCreate = false;
    [SerializeField]
    // 材质
    public Material material;
    [SerializeField]
    // 拖尾材质
    public Material trailMaterial;
    [SerializeField]
    public ParticleSystem particle;
    public string tintColor = "_TintColor";
    private string Emission = "_EMISSION";
    // 透明
    public Color transparent = new Color(128, 128, 128, 0);
    // 不透明
    public Color nottransparent = new Color(128, 128, 128, 128);
    // Start is called before the first frame update
    void Start()
    {
        material.DisableKeyword(Emission);
        trailMaterial.SetColor(tintColor, nottransparent);
        particle.gameObject.SetActive(true);
    }

    public void Init(float _speed, int _count, GameObject _player)
    {
        speed = _speed;
        curSpeed = speed;
        count = _count;
        player = _player;
        delayTime = 0;
        isInit = false;
        gameObject.SetActive(false);
        isPlayerCreate = true;
    }
    public void Init(float _speed, int _count, float _x, float _z)
    {
        speed = _speed;
        curSpeed = speed;
        count = _count;
        direction = new Vector3(_x, 0, _z);
        isPlayerCreate = false;
    }

    public void Move(float deltaTime)
    {
        if (isPlayerCreate)
        {
            // 延迟初始化
            if (delayTime <= delay)
            {
                delayTime += deltaTime;
                return;
            }
            if (!isInit)
            {
                // 固定光线初始位置y方向
                transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z)
                                            + player.transform.forward.normalized * 2;
                direction = new Vector3(player.transform.forward.x, 0, player.transform.forward.z);
                isInit = true;
                gameObject.SetActive(true);
                return;
            }
        }
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
        if (curSpeed + 1 <= maxSpeed)
        {
            curSpeed++;
        }
        if (index == count)
        {
            FightScene.instance.RomoveLight(gameObject);
            Destroy(gameObject);
        }
    }
}

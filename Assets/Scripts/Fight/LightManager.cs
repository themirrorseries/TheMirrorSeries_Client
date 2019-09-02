using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    // 初始速度
    private float speed;
    private float curSpeed;
    public float[] speedRange ={
        40f,
        50f,
        55f
    };
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
    // 拖尾材质
    public Material trailMaterial;
    [SerializeField]
    public ParticleSystem particle;
    public string tintColor = "_TintColor";
    // 颜色变化表
    public Color[] colors;
    // 透明颜色表
    public Color[] transparent;
    // 当前颜色下标
    public int colorIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        InitColor();
        trailMaterial.SetColor(tintColor, colors[colorIndex]);
        Material new_trailMaterial = new Material(trailMaterial);
        trailMaterial = new_trailMaterial;
        gameObject.GetComponent<TrailRenderer>().material = trailMaterial;
        particle.gameObject.SetActive(true);
    }

    // 防止报空
    private void InitColor()
    {
        colors = new Color[] {
            new Color(255f/255f, 201f / 255f, 26f / 255f, 1f),
            new Color(202f/255f, 37/255f, 185/255f, 1f),
            new Color(255f / 255f, 0/255f, 0f / 255f, 1f)
        };
        transparent = new Color[] {
            new Color(255f/255f, 201f / 255f, 26f / 255f, 0),
            new Color(202f/255f, 37/255f, 185/255f, 0),
            new Color(255f / 255f, 0/255f, 0f / 255f,0)
        };
    }

    public void Init(float _speed, int _count, GameObject _player)
    {
        InitColor();
        speed = _speed;
        curSpeed = speed;
        if (curSpeed >= speedRange[0] && curSpeed < speedRange[1])
        {
            colorIndex++;
            trailMaterial.SetColor(tintColor, colors[colorIndex]);
        }
        else if (curSpeed >= speedRange[1])
        {
            colorIndex += 2;
            trailMaterial.SetColor(tintColor, colors[colorIndex]);
        }
        count = _count;
        player = _player;
        delayTime = 0;
        isInit = false;
        gameObject.SetActive(false);
        isPlayerCreate = true;
    }
    public void Init(float _speed, int _count, float _x, float _z)
    {
        InitColor();
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
            // 先进行射线相交,计算出下次是否需要反射,移动的方向,然后进行球形检查,确定透明度,再移动
            // 射线相交计算
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, distance, LayerMask.GetMask(LayerEunm.WALL) | LayerMask.GetMask(LayerEunm.PLAYER)))
            {
                bool isDeath = false;
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer(LayerEunm.PLAYER))
                {
                    PlayerAttribute playerAttribute = hit.collider.gameObject.GetComponent<PlayerAttribute>();
                    if (playerAttribute.isDied)
                    {
                        isDeath = true;
                    }
                    else
                    {
                        // 反弹次数暂定每次直接++, 1:每次 2:正面 3:反面 4:保护罩期间怎么计算
                        playerAttribute.bounces++;
                        // 保护罩期间直接反弹
                        if (!playerAttribute.hasProtection)
                        {
                            PlayerControl playerControl = hit.collider.gameObject.GetComponent<PlayerControl>();
                            playerControl.LightCollision(gameObject, direction.normalized);
                        }
                    }
                }
                if (!isDeath)
                {
                    Reflect(hit.collider.gameObject.transform.forward.normalized);
                }
            }
            if (FightScene.instance.nightScope != -1)
            {
                // 球形射线检测
                Collider[] hitColliders = Physics.OverlapSphere(transform.position + direction * deltaTime * curSpeed, FightScene.instance.nightScope, LayerMask.GetMask(LayerEunm.PLAYER));
                if (hitColliders.Length == 0)
                {
                    if (particle.gameObject.activeInHierarchy)
                    {
                        trailMaterial.SetColor(tintColor, transparent[colorIndex]);
                        particle.gameObject.SetActive(false);
                    }
                }
                else
                {
                    bool isIn = false;
                    for (int i = 0; i < hitColliders.Length; ++i)
                    {
                        PlayerAttribute attr = hitColliders[i].gameObject.GetComponent<PlayerAttribute>();
                        if (RoomData.isMainRole(attr.seat))
                        {
                            if (!particle.gameObject.activeInHierarchy)
                            {
                                trailMaterial.SetColor(tintColor, colors[colorIndex]);
                                particle.gameObject.SetActive(true);
                            }
                            isIn = true;
                            break;
                        }
                    }
                    if (!isIn)
                    {
                        if (particle.gameObject.activeInHierarchy)
                        {
                            trailMaterial.SetColor(tintColor, transparent[colorIndex]);
                            particle.gameObject.SetActive(false);
                        }
                    }
                }
            }
            transform.Translate(direction.normalized * deltaTime * curSpeed);
        }
    }

    private void Reflect(Vector3 forward)
    {
        // 计算反射方向
        direction = direction - 2 * Vector3.Dot(direction, forward) * forward;
        // 碰撞次数++
        index++;
        curSpeed = SpeedFormula(curSpeed);
        if (curSpeed == speedRange[0])
        {
            colorIndex++;
            trailMaterial.SetColor(tintColor, colors[colorIndex]);
        }
        else if (curSpeed == speedRange[1])
        {
            colorIndex++;
            trailMaterial.SetColor(tintColor, colors[colorIndex]);
        }
        if (index == count)
        {
            FightScene.instance.RomoveLight(gameObject);
            Destroy(gameObject);
        }
    }

    private float SpeedFormula(float curSpeed)
    {
        if (curSpeed < speedRange[0])
        {
            curSpeed += 1;
        }
        else if (curSpeed < speedRange[1])
        {
            curSpeed += 0.5f;
        }
        else if (curSpeed < speedRange[2])
        {
            curSpeed += 0.5f;
        }
        return curSpeed;
    }
}

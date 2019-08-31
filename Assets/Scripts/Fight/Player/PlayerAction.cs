﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    ////////// 击退 //////////
    ///       start       ///
    // 默认击退距离
    private float defaultRepulseDistance = 8f;
    // 击退过程移动速度
    private float repulseSpeed = 10f;
    // 击退方向
    private Vector3 repulseDirection;
    // 击退距离
    private float repulseDistance;
    // 当前击退距离
    private float curRepulseDistance;
    private bool isRepulseWall = false;
    ///       end       ///

    ////////// 死亡 //////////
    ///       start       ///
    // 死亡移动标志位
    private bool isDeathMove = false;
    // 默认死亡距离
    private float defaultDeathDistance = 10f;
    // 死亡过程移动速度
    private float deathSpeed = 10f;
    // 死亡方向
    private Vector3 deathDirection;
    // 死亡距离
    private float deathDistance;
    // 当前死亡距离
    private float curDeathDistance;
    ///       end       ///

    //////// 黑夜降临 ////////
    ///       start       ///
    // 环境光
    private GameObject directionalLight;
    [SerializeField]
    // 聚光灯
    private GameObject spotLight;
    [SerializeField]
    // 材质
    public Material material;
    private string Emission = "_EMISSION";
    ///       end       ///

    ////////  掉血   /////////
    ///       start       ///
    // 掉血协程
    public Coroutine bleedingCoroutine;
    // 是否在掉血
    private bool isInBleeding = false;
    private float bleedingTime = 0.5f;
    ///       end       ///

    private PlayerAttribute attr;
    // Start is called before the first frame update
    void Start()
    {
        attr = GetComponent<PlayerAttribute>();
        directionalLight = GameObject.Find("Directional Light");
        material.DisableKeyword(Emission);
    }
    public void UpdateState(float deltaTime)
    {
        Repulse(deltaTime);
        Death(deltaTime);
    }
    public void DoBleeding()
    {
        if (isInBleeding)
        {
            StopCoroutine(bleedingCoroutine);
        }
        isInBleeding = true;
        bleedingCoroutine = StartCoroutine(Bleeding());
    }
    public IEnumerator Bleeding()
    {
        FightScene.instance.bleedingImg.gameObject.SetActive(true);
        yield return new WaitForSeconds(bleedingTime);
        FightScene.instance.bleedingImg.gameObject.SetActive(false);
        isInBleeding = false;
    }
    public void CheckRepulse(float wallDistance, Vector3 direction)
    {
        // 射线相交计算
        RaycastHit hit;
        float moveDistance = defaultRepulseDistance;
        isRepulseWall = false;
        if (Physics.Raycast(transform.position, direction, out hit, wallDistance + defaultRepulseDistance, LayerMask.GetMask(LayerEunm.WALL)))
        {
            isRepulseWall = true;
            // 如果击退到墙壁,会二段扣血
            attr.ChangeHp(attr.damage_repel);
            if (attr.isDied)
            {
                AnimationControl anim = GetComponent<AnimationControl>();
                anim.Death();
                if (RoomData.isMainRole(attr.seat))
                {
                    FightScene.instance.audioController.SoundPlay(AudioEunm.death);
                }
                FightScene.instance.AddDeath(attr.seat, attr.bounces);
                return;
            }
            // ps:乘以2,否则会卡在无法移动的区域里面
            moveDistance = Vector3.Distance(transform.position, hit.point) - wallDistance * (float)2;
        }
        curRepulseDistance = 0;
        repulseDirection = direction.normalized;
        repulseDistance = moveDistance;
        attr.isRepulse = true;
    }

    public void CheckDeath(float wallDistance, Vector3 direction)
    {
        // 进入这个函数后,角色死亡
        // 先关闭碰撞盒,防止重复计算
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
        // 射线相交计算
        RaycastHit hit;
        float moveDistance = defaultDeathDistance;
        if (Physics.Raycast(transform.position, direction, out hit, wallDistance + defaultDeathDistance, LayerMask.GetMask(LayerEunm.WALL)))
        {
            moveDistance = Vector3.Distance(transform.position, hit.point) - wallDistance;
        }
        curDeathDistance = 0;
        deathDirection = direction.normalized;
        deathDistance = moveDistance;
        isDeathMove = true;
    }
    public void Repulse(float deltaTime)
    {
        if (!attr.isRepulse)
        {
            return;
        }
        else
        {
            if ((curRepulseDistance + repulseSpeed * deltaTime) < repulseDistance)
            {
                curRepulseDistance += repulseSpeed * deltaTime;
                transform.Translate(repulseDirection * repulseSpeed * deltaTime, Space.World);
            }
            else
            {
                if (RoomData.isMainRole(attr.seat) && isRepulseWall)
                {
                    FightScene.instance.audioController.SoundPlay(AudioEunm.repulse);
                }
                attr.isRepulse = false;
            }
        }
    }
    public void Death(float deltaTime)
    {
        if (!isDeathMove)
        {
            return;
        }
        else
        {
            if ((curDeathDistance + deathSpeed * deltaTime) < deathDistance)
            {
                curDeathDistance += deathSpeed * deltaTime;
                transform.Translate(deathDirection * deathSpeed * deltaTime, Space.World);
            }
            else
            {
                PlayerChildren children = GetComponent<PlayerChildren>();
                children.title.gameObject.SetActive(false);
                isDeathMove = false;
                attr.isEnd = true;
            }
        }
    }
    public void BeforeNight(float skillScope)
    {
        if (attr.inSelfNight)
        {
            directionalLight.GetComponent<Light>().color = Color.grey;
        }
        else if (attr.inNight)
        {
            if (!RoomData.isMainRole(attr.seat)) return;
            FightScene.instance.nightScope = skillScope;
            directionalLight.SetActive(false);
            spotLight.SetActive(true);
            // 开启自己的自发光,关闭别人的自发光
            List<GameObject> players = FightScene.instance.Players;
            for (int i = 0; i < players.Count; ++i)
            {
                PlayerAction action = players[i].GetComponent<PlayerAction>();
                PlayerChildren children = players[i].GetComponent<PlayerChildren>();
                if (RoomData.isMainRole(action.attr.seat))
                {
                    action.material.EnableKeyword(Emission);
                    children.title.gameObject.SetActive(true);
                }
                else
                {
                    action.material.DisableKeyword(Emission);
                    children.title.gameObject.SetActive(false);
                }
            }
            // 关闭光线的自发光和透明化拖尾材质
            List<GameObject> lights = FightScene.instance.Lights;
            for (int i = 0; i < lights.Count; ++i)
            {
                LightManager lightManager = lights[i].GetComponent<LightManager>();
                lightManager.material.DisableKeyword(Emission);
                lightManager.trailMaterial.SetColor(lightManager.tintColor, lightManager.transparent[lightManager.colorIndex]);
                lightManager.particle.gameObject.SetActive(false);
            }
            Night();
        }
    }
    public void Night()
    {
        if (!attr.inSelfNight && !attr.inNight)
        {
            return;
        }
        if (attr.inNight)
        {
            if (!RoomData.isMainRole(attr.seat)) return;
            // 球形射线检测
            Collider[] hitColliders = Physics.OverlapSphere(transform.position,
                        FightScene.instance.nightScope, LayerMask.GetMask(LayerEunm.PLAYER) | LayerMask.GetMask(LayerEunm.LIGHT));
            for (int i = 0; i < hitColliders.Length; ++i)
            {
                if (hitColliders[i].gameObject.layer == LayerMask.NameToLayer(LayerEunm.PLAYER))
                {
                    PlayerAction action = hitColliders[i].gameObject.GetComponent<PlayerAction>();
                    if (RoomData.isMainRole(action.attr.seat))
                    {
                        continue;
                    }
                    if (!action.material.IsKeywordEnabled(Emission))
                    {
                        action.material.EnableKeyword(Emission);
                        PlayerChildren children = hitColliders[i].GetComponent<PlayerChildren>();
                        children.title.gameObject.SetActive(true);
                    }
                }
                else if (hitColliders[i].gameObject.layer == LayerMask.NameToLayer(LayerEunm.LIGHT))
                {
                    LightManager lightManager = hitColliders[i].gameObject.GetComponent<LightManager>();
                    if (!lightManager.material.IsKeywordEnabled(Emission))
                    {
                        lightManager.material.EnableKeyword(Emission);
                        lightManager.trailMaterial.SetColor(lightManager.tintColor, lightManager.colors[lightManager.colorIndex]);
                        lightManager.particle.gameObject.SetActive(true);
                    }
                }
            }
            for (int i = 0; i < FightScene.instance.Players.Count; ++i)
            {
                bool isIn = false;
                for (int j = 0; j < hitColliders.Length; ++j)
                {
                    if (hitColliders[j].gameObject == FightScene.instance.Players[i])
                    {
                        isIn = true;
                        break;
                    }
                }
                if (!isIn)
                {
                    PlayerAction action = FightScene.instance.Players[i].GetComponent<PlayerAction>();
                    if (action.material.IsKeywordEnabled(Emission))
                    {
                        action.material.DisableKeyword(Emission);
                        PlayerChildren children = FightScene.instance.Players[i].GetComponent<PlayerChildren>();
                        children.title.gameObject.SetActive(false);
                    }
                }
            }
            for (int i = 0; i < FightScene.instance.Lights.Count; ++i)
            {
                bool isIn = false;
                for (int j = 0; j < hitColliders.Length; ++j)
                {
                    if (hitColliders[j].gameObject == FightScene.instance.Lights[i])
                    {
                        isIn = true;
                        break;
                    }
                }
                if (!isIn)
                {
                    LightManager lightManager = FightScene.instance.Lights[i].GetComponent<LightManager>();
                    if (lightManager.material.IsKeywordEnabled(Emission))
                    {
                        lightManager.material.DisableKeyword(Emission);
                        lightManager.trailMaterial.SetColor(lightManager.tintColor, lightManager.transparent[lightManager.colorIndex]);
                        lightManager.particle.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    public void AfterNight()
    {
        if (attr.inSelfNight)
        {
            directionalLight.GetComponent<Light>().color = Color.white;
        }
        else if (attr.inNight)
        {
            if (!RoomData.isMainRole(attr.seat)) return;
            FightScene.instance.nightScope = -1;
            directionalLight.SetActive(true);
            spotLight.SetActive(false);
            // 关闭角色自发光和角色头顶
            List<GameObject> players = FightScene.instance.Players;
            for (int i = 0; i < players.Count; ++i)
            {
                PlayerAction action = players[i].GetComponent<PlayerAction>();
                action.material.DisableKeyword(Emission);
                PlayerChildren children = players[i].GetComponent<PlayerChildren>();
                children.title.gameObject.SetActive(true);
            }
            // 关闭光线自发光
            List<GameObject> lights = FightScene.instance.Lights;
            for (int i = 0; i < lights.Count; ++i)
            {
                LightManager lightManager = lights[i].GetComponent<LightManager>();
                lightManager.material.DisableKeyword(Emission);
                lightManager.trailMaterial.SetColor(lightManager.tintColor, lightManager.colors[lightManager.colorIndex]);
                lightManager.particle.gameObject.SetActive(true);
            }
        }
    }
}

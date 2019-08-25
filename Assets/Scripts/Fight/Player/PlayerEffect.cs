using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField]
    // 混乱
    private ParticleSystem chaos;
    [SerializeField]
    // 保护罩
    private ParticleSystem cover;
    public void Play(string effect)
    {
        ParticleSystem ps = name2Ps(effect);
        if (ps != null)
        {
            ps.Play();
        }
    }
    public void Stop(string effect)
    {
        ParticleSystem ps = name2Ps(effect);
        if (ps != null)
        {
            ps.Stop();
        }
    }
    public ParticleSystem name2Ps(string name)
    {
        switch (name)
        {
            case EffectEunm.CHAOS: return chaos;
            case EffectEunm.COVER: return cover;
            default:
                return null;
        }
    }
}

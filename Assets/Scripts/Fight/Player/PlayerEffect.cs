using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField]
    // 混乱
    private ParticleSystem chaos;
    [SerializeField]
    // 雷电
    private ParticleSystem thunder;
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
            case EffectEunm.THUNDER: return thunder;
            default:
                return null;
        }
    }
}

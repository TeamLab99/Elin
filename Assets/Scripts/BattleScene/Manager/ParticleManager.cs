using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Inst { get; private set; }
    void Awake() => Inst = this;

    public bool playAura = true; //파티클 제어 bool
    public ParticleSystem[] particleObjects; //파티클시스템

    void Update()
    {
        if (playAura)
            PlayParticle();
        else if (!playAura)
            StopParticle();
    }

    void PlayParticle()
    {
        for (int i = 0; i < particleObjects.Length; i++)
        {
            particleObjects[i].Play();
        }

        playAura = false;
    }

    void StopParticle()
    {
        for (int i = 0; i < particleObjects.Length; i++)
        {
            particleObjects[i].Stop();
        }
    }
}



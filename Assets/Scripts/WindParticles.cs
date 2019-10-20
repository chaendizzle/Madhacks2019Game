using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticles : MonoBehaviour
{
    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ClimateEvents.GetInstance().wind)
        {
            if (!ps.isEmitting)
            {
                ps.Play(false);
            }
        }
        else if (ps.isEmitting)
        {
            ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}

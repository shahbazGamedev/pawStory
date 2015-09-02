/**
Script Author : Peter Stirling (Sourced from net) 
Description   : Combo - Floating Origin
**/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FloatingOrigin : MonoBehaviour
{
    public float threshold = 100.0f;
    public float physicsThreshold = 1000.0f; // Set to zero to disable
    public float defaultSleepVelocity = 0.14f;
    public float defaultAngularVelocity = 0.14f;

    GameObject tempObj;
    ParticleEmitter pe;
    Rigidbody r;
    Transform t;

    void LateUpdate()
    {
        Vector3 cameraPosition = gameObject.transform.position;
        cameraPosition.y = 0f;
        cameraPosition.x = 0f;
        if (cameraPosition.magnitude > threshold)
        {
            Object[] objects = FindObjectsOfType(typeof(Transform));
            for(int i=0;i<objects.Length;i++)
            {
                t = (Transform)objects[i];
                if(t.tag == "Stationary")
                {
                    continue;
                }
                if(t.parent == null || t.parent.name=="Pooler")
                {
                    t.position -= cameraPosition;
                }
            }

            objects = FindObjectsOfType(typeof(ParticleEmitter));
            for (int i = 0; i < objects.Length; i++)
            {
                pe = (ParticleEmitter)objects[i];
                Particle[] emitterParticles = pe.particles;
                for (int j = 0; j < emitterParticles.Length; j++)
                {
                    emitterParticles[j].position -= cameraPosition;
                }
                pe.particles = emitterParticles;
            }

            if (physicsThreshold >= 0f)
            {
                objects = FindObjectsOfType(typeof(Rigidbody));
                for (int i = 0; i < objects.Length; i++)
                {
                    r = (Rigidbody)objects[i];
                    if (r.gameObject.transform.position.magnitude > physicsThreshold)
                    {
                        r.sleepAngularVelocity = float.MaxValue;
                        r.sleepVelocity = float.MaxValue;
                    }
                    else
                    {
                        r.sleepAngularVelocity = defaultSleepVelocity;
                        r.sleepVelocity = defaultAngularVelocity;
                    }
                }
            }
        }
    }
}
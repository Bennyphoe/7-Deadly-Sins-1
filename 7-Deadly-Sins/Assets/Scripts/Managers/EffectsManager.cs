﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{


    #region Singelton

    public static EffectsManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {

            instance = this;


        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
    public GameObject EffectsPool;

    public Transform returnEffect(int number)
    {
        for (int i = 0; i < EffectsPool.transform.childCount; i++)
        {
            if (i == number)
            {
                return EffectsPool.transform.GetChild(number);
            } 

        }
        return null;
    }

    public void ActivateParticleSystem(Transform particleSystem)
    {
        ParticleSystem[] particleSystems = particleSystem.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem ps in particleSystems)
        {
            ps.Stop();
            if (ps.isStopped)
            {
                ps.Play();
            }
           
               
            
            
        }
    }

    public void DeactivateParticleSystem(Transform particleSystem)
    {
        ParticleSystem[] particleSystems = particleSystem.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            
            ps.Stop();
        }
    }

    public void EnableEffectObject(int number)
    {
        for (int i = 0; i < EffectsPool.transform.childCount; i++)
        {
            if (i == number)
            {
                EffectsPool.transform.GetChild(number).gameObject.SetActive(true);
                
            }
            

        }
        
    }

    public void DisableEffectObject(int number)
    {
        for (int i = 0; i < EffectsPool.transform.childCount; i++)
        {
            if (i == number)
            {
                EffectsPool.transform.GetChild(number).gameObject.SetActive(false);
            }


        }

    }

    public void DisableAll()
    {
        for (int i = 0; i < EffectsPool.transform.childCount; i++)
        {
           
             EffectsPool.transform.GetChild(i).gameObject.SetActive(false);
            
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        DisableAll();
    }

    
}

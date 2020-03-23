using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectileControl : MonoBehaviour
{
    #region Fields

    [NonSerialized] public string targetTag;
    
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Collider collider;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private string sfxKey;
    
    private IEnumerator coroutine;

    #endregion

    #region UnityCallbacks

    private void Awake()
    {
        if (!collider)
        {
            collider = GetComponent<Collider>();
        }
        if (!meshRenderer)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Agent target = other.GetComponent<Agent>();
            Hit(target);
        }

        if (other.CompareTag("Projectile"))
        {
            if (other.GetComponent<ProjectileControl>().targetTag != targetTag)
            {
                Stop();
            }
        }
    }

    #endregion

    #region Methods

    public void StartLife()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = Life();
        StartCoroutine(coroutine);
        SFXPlayer.Instance.PlayOneShot(sfxKey);
    }

    private IEnumerator Life()
    {
        float timer = 0;
        while (timer < lifeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        
        Stop();
    }

    public void Stop()
    {
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
    }

    [Button(ButtonSizes.Medium)]
    private void Hit(Agent target)
    {
        target.Hit();
        collider.enabled = false;
        meshRenderer.enabled = false;
        rb.velocity = Vector3.zero;
        particle.Play();
    }

    public void PushToDirection(Vector3 direction)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * speed, ForceMode.VelocityChange);
    }

    #endregion
}

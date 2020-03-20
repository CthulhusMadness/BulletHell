using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectileControl : MonoBehaviour
{
    #region Fields

    public string targetTag;
    
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Collider collider;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private float speed = 10f;
    
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
            Hit();
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
    }

    private IEnumerator Life()
    {
        float timer = 0;
        while (timer < lifeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        
        FinishLife();
    }

    public void FinishLife()
    {
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
    }

    [Button(ButtonSizes.Medium)]
    private void Hit()
    {
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

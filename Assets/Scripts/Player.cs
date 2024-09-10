using System;
using System.Collections;
using System.Collections.Generic;
using Animators;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class Player : MonoBehaviour
{
    private PlayerMover _mover;
    private void Awake()
    {
        _mover = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        _mover.CalculateDirection();
    }

    private void FixedUpdate()
    {
        _mover.Move();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Coin coin))
            Destroy(coin.gameObject);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using HyperCore;
using UnityEngine;

public class CameraController : SingletonBehaviour<CameraController>
{
    [SerializeField] private Transform _playerFollower;
    [SerializeField] private CinemachineVirtualCamera _endgameCam;

    public Transform PlayerFollower => _playerFollower;

    public void Start()
    {
        _endgameCam.enabled = false;
    }

    public void ActivateEndgameCamera()
    {
        _endgameCam.enabled = true;
    }
}

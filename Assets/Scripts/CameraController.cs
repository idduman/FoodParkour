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
    [SerializeField] private float _cameraClipDistance;

    public Transform PlayerFollower => _playerFollower;
    public float CameraClipDistance => _cameraClipDistance;

    public void Start()
    {
        _endgameCam.enabled = false;
    }

    public void ActivateEndgameCamera()
    {
        _endgameCam.enabled = true;
    }
}

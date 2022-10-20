using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public static GameObject pointOfInterest;

    private float _cameraZCoord;

    private float _cameraSlowMotion = 0.05f;

    private Vector2 _camPositionLim = Vector2.zero;

    private void Awake()
    {
        _cameraZCoord = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        if (pointOfInterest == null) return;

        Vector3 destination = pointOfInterest.transform.position;
        destination = Vector3.Lerp(transform.position, destination, _cameraSlowMotion);
        
        destination.x = Mathf.Max(_camPositionLim.x, destination.x);
        destination.y = Mathf.Max(_camPositionLim.y, destination.y);
        destination.z = _cameraZCoord;

        transform.position = destination;

        Camera.main.orthographicSize = destination.y + 10;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

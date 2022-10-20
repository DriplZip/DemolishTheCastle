using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    public static ProjectileLine projectileLine;

    private float _minDistance = 0.1f;

    private LineRenderer _line;
    private GameObject _pointOfInterest;
    private List<Vector3> _points;

    private GameObject PointOfInterest
    {
        get
        {
            return _pointOfInterest;
        }
        set
        {
            _pointOfInterest = value;

            if (_pointOfInterest == null) return;
            
            _line.enabled = false;
            _points = new List<Vector3>();
            AddPoint();
        }
    }
    
    private Vector3 LastPoint => _points == null ? Vector3.zero : _points[_points.Count - 1];
    
    private void Awake()
    {
        projectileLine = this;

        _line = GetComponent<LineRenderer>();
        _line.enabled = false;
        _points = new List<Vector3>();
    }

    private void Clear()
    {
        _pointOfInterest = null;
        _line.enabled = false;
        _points = new List<Vector3>();
    }

    private void AddPoint()
    {
        Vector3 point = _pointOfInterest.transform.position;
        
        if (_points.Count > 0 && (point - LastPoint).magnitude < _minDistance) return;

        if (_points.Count == 0)
        {
            Vector3 launchPositionDiff = point - SlingShot.LaunchPosition;
            
            _points.Add(point + launchPositionDiff);
            _points.Add(point);

            _line.positionCount = 2;
            _line.SetPosition(0, _points[0]);
            _line.SetPosition(1, _points[1]);
        }
        else
        {
            _points.Add(point);

            _line.positionCount = _points.Count;
            _line.SetPosition(_points.Count - 1, LastPoint);
        }
        
        _line.enabled = true;
    }

    private void FixedUpdate()
    {
        if (PointOfInterest is null)
        {
            if (FollowCam.pointOfInterest is null) return;
          
            if (!FollowCam.pointOfInterest.CompareTag("Projectile")) return;
            
            PointOfInterest = FollowCam.pointOfInterest;
        }
        
        AddPoint();

        if (FollowCam.pointOfInterest is null) PointOfInterest = null;
    }
}

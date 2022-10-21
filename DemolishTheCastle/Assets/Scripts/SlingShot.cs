using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    private static SlingShot _slingShot;
    
    private GameObject _launchPoint;
    private Vector3 _launchPointPosition;
    
    private GameObject _projectile;
    private Rigidbody _projectileRigidbody;

    private bool _aimingState;

    [Header("Set in Inspector")] 
    public GameObject projectilePrefab;

    public float velocityMulti = 8f;
    
    public static Vector3 LaunchPosition => _slingShot == null ? Vector3.zero : _slingShot._launchPointPosition;

    private void Awake()
    {
        _slingShot = this;
        
        Transform launchPointTransform = transform.Find("LaunchPoint");
        
        _launchPoint = launchPointTransform.gameObject;
        _launchPoint.SetActive(false);

        _launchPointPosition = launchPointTransform.position;
    }

    private void OnMouseEnter()
    {
        _launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        _launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        _aimingState = true;

        _projectile = Instantiate(projectilePrefab);
        _projectile.transform.position = _launchPointPosition;

        _projectileRigidbody = _projectile.GetComponent<Rigidbody>();
        _projectileRigidbody.isKinematic = true;
    }

    private void Update()
    {
        if(!_aimingState) return;

        Vector3 mousePosition2d = Input.mousePosition;
        mousePosition2d.z = -Camera.main.transform.position.z;

        Vector3 mousePosition3d = Camera.main.ScreenToWorldPoint(mousePosition2d);

        Vector3 launchPointPositionDiffWithMouse = mousePosition3d - _launchPointPosition;
        float maxDiffMagnitude = this.GetComponent<SphereCollider>().radius;

        if (launchPointPositionDiffWithMouse.magnitude > maxDiffMagnitude)
        {
            launchPointPositionDiffWithMouse.Normalize();
            launchPointPositionDiffWithMouse *= maxDiffMagnitude;
        }

        Vector3 projectilePosition = _launchPointPosition + launchPointPositionDiffWithMouse;
        _projectile.transform.position = projectilePosition;

        if (Input.GetMouseButtonUp(0))
        {
            _aimingState = false;
            _projectileRigidbody.isKinematic = false;
            _projectileRigidbody.velocity = -launchPointPositionDiffWithMouse * velocityMulti;
            
            FollowCam.pointOfInterest = _projectile;
            
            _projectile = null;
            
            GameManager.ShotFired();
            ProjectileLine.projectileLine.PointOfInterest = _projectile;
        }
    }
}

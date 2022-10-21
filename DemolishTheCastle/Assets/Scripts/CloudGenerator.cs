using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudGenerator : MonoBehaviour
{
    private int _numberOfClouds = 50;

    private Vector3 _minCloudPosition = new Vector3(-50, 15, 10);
    private Vector3 _maxCloudPosition = new Vector3(300, 100, 10);

    private float _minCloudScale = 1f;
    private float _maxCloudScale = 2f;

    private float _cloudSpeedMulti = 0.5f;

    private GameObject[] _cloudInstances;
    
    public GameObject cloudPrefab;

    private void Awake()
    {
        _cloudInstances = new GameObject[_numberOfClouds];

        GameObject generator = GameObject.Find("CloudGenerator");

        for (int i = 0; i < _numberOfClouds; i++)
        {
            _cloudInstances[i] = CreateCloud(generator);
        }
    }

    private GameObject CreateCloud(GameObject generator)
    {
        GameObject cloud = Instantiate(cloudPrefab);
        
        Vector3 cloudPosition = Vector3.zero;
        cloudPosition.x = Random.Range(_minCloudPosition.x, _maxCloudPosition.x);
        cloudPosition.x = Random.Range(_minCloudPosition.y, _maxCloudPosition.y);

        float scale = Random.value;
        float scaleValue = Mathf.Lerp(_minCloudScale, _maxCloudScale, scale);

        cloudPosition.y = Mathf.Lerp(_minCloudPosition.y, cloudPosition.y, scale);
        cloudPosition.z = 100 - 90 * scale;

        cloud.transform.position = cloudPosition;
        cloud.transform.localScale = Vector3.one * scaleValue;
        
        cloud.transform.SetParent(generator.transform);

        return cloud;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject cloud in _cloudInstances)
        {
            float scaleValue = cloud.transform.localScale.x;
            Vector3 cloudPosition = cloud.transform.position;

            cloudPosition.x -= scaleValue * Time.deltaTime * _cloudSpeedMulti;

            if (cloudPosition.x <= _minCloudPosition.x)
            {
                cloudPosition.x = _maxCloudPosition.x;
            }

            cloud.transform.position = cloudPosition;
        }
    }
}

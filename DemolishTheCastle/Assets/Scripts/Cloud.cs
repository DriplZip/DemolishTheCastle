using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private int _minNumSpheres = 6;
    private int _maxNumSpheres = 10;

    private Vector3 _sphereOffsetScale = new Vector3(5, 2, 1);
    private Vector2 _scaleRangeX = new Vector2(4, 8);
    private Vector2 _scaleRangeY = new Vector2(3, 4);
    private Vector2 _scaleRangeZ = new Vector2(2, 4);

    private float _scaleYMin = 2f;

    private List<GameObject> _spheres;

    public GameObject cloudSphere;
    
    // Start is called before the first frame update
    void Start()
    {
        _spheres = new List<GameObject>();

        int numSpheres = Random.Range(_minNumSpheres, _maxNumSpheres);

        for (int i = 0; i < numSpheres; i++)
        {
            GameObject sphere = Instantiate(cloudSphere);
            _spheres.Add(sphere);

            Transform sphereTransform = sphere.transform;
            sphereTransform.SetParent(transform);

            Vector3 offset = Random.insideUnitSphere;
            offset.x *= _sphereOffsetScale.x;
            offset.y *= _sphereOffsetScale.y;
            offset.z *= _sphereOffsetScale.z;
            sphereTransform.localPosition = offset;

            Vector3 scale = Vector3.one;
            scale.x = Random.Range(_scaleRangeX.x, _scaleRangeX.y);
            scale.y = Random.Range(_scaleRangeY.x, _scaleRangeY.y);
            scale.z = Random.Range(_scaleRangeZ.x, _scaleRangeZ.y);

            scale.y *= 1 - (Mathf.Abs(offset.x) / _sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, _scaleYMin);

            sphereTransform.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Restart(); 
    }

    void Restart()
    {
        _spheres.ForEach(Destroy);
        
        Start();
    }
}

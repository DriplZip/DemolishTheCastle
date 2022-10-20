using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static bool goalMet;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Goal.goalMet = true;

            Material goalMaterial = GetComponent<Renderer>().material;
            Color goalColor = goalMaterial.color;

            goalColor.a = 1;
            goalMaterial.color = goalColor;
        }
    }
}

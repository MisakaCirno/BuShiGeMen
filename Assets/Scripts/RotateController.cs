using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour
{
    private Camera myCam;
    private Vector3 screenPos;
    private float angleOfset;
    private Collider2D col;

    void Start()
    {
        myCam = Camera.main;
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Vector3 mousePos = myCam.ScreenToWorldPoint(Input.mousePosition);

        bool isDown = false;

        if (Input.GetMouseButtonDown(0))
        {
            isDown = true;
            if (col == Physics2D.OverlapPoint(mousePos))
            {
                screenPos = myCam.WorldToScreenPoint(transform.position);
                Vector3 vec3 = Input.mousePosition - screenPos;
                angleOfset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(vec3.y, vec3.x)) * Mathf.Rad2Deg;
            }
        }

        if (Input.GetMouseButton(0))
        {
            isDown = true;
            if (col == Physics2D.OverlapPoint(mousePos))
            {
                Vector3 vec3 = Input.mousePosition - screenPos;
                float angle = Mathf.Atan2(vec3.y, vec3.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, angle + angleOfset);
            }
        }

        // 如果鼠标松开了，则将物体的旋转角度根据最近的角度，进行吸附
        List<float> targetAngles = new() { 0, 90, -90};
        if (Input.GetMouseButtonUp(0) && isDown == false)
        {
            float resultAngle = transform.eulerAngles.z;
            float minAngleDistance = float.MaxValue;

            foreach (float targetAngle in targetAngles)
            {
                float convertEulerZ = transform.eulerAngles.z;
                if (convertEulerZ > 180)
                {
                    convertEulerZ = convertEulerZ - 360;
                }

                float angleDistance = Mathf.Abs(targetAngle - convertEulerZ);
                Debug.Log("now is " + convertEulerZ + " distance to " + targetAngle + " is " + angleDistance);

                if (angleDistance < minAngleDistance)
                {
                    minAngleDistance = angleDistance;
                    resultAngle = targetAngle;
                }
            }

            transform.eulerAngles = new Vector3(0, 0, resultAngle);
        }
    }
}

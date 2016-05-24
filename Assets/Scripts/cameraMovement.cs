using UnityEngine;
using System;
 
[AddComponentMenu("Camera-Control/WoW Camera")]
public class cameraMovement : MonoBehaviour
{
    public Transform target;
    public float targetHeight = 2.0f;
    public float distance = 15.0f;
    public float maxDistance = 25.0f;
    public float minDistance = 0.5f;
    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -40.0f;
    public float yMaxLimit = 80.0f;
    public float zoomRate = 20.0f;
    public float rotationDampening = 3.0f;
    private float x = 0.0f;
    private float y = 0.0f;
    public bool isTalking = false;
 
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
 
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;
    }
 
    void LateUpdate()
    {
        if (!target) return;
 
        // If either mouse buttons are down, let them govern camera position
        if (Input.GetMouseButton(0) || (Input.GetMouseButton(1)))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }
        // otherwise, ease behind the target if any of the directional keys are pressed
        else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            float targetRotationAngle = target.eulerAngles.y;
            float currentRotationAngle = transform.eulerAngles.y;
            x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
        }
 
        distance -= (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
 
        y = ClampAngle(y, yMinLimit, yMaxLimit);
 
        // Rotate Camera:
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;
 
        // Camera Position:
        Vector3 position = target.position - (rotation * Vector3.forward * distance + new Vector3(0, -targetHeight, 0));
        transform.position = position;
 
        // Is the view being blocked??
        RaycastHit hit = new RaycastHit();
        Vector3 trueTargetPosition = target.transform.position - new Vector3(0, -targetHeight, 0);

        // Cast the line to check:
        if (Physics.Linecast(trueTargetPosition, transform.position, out hit))
        {
            // If so, shorten distance so camera is in front of object:
            float tempDistance = Vector3.Distance(trueTargetPosition, hit.point) - 0.28f;

            // reposition camera:
            position = target.position - (rotation * Vector3.forward * tempDistance + new Vector3(0, -targetHeight, 0));
            transform.position = position;
        }
    }
 
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
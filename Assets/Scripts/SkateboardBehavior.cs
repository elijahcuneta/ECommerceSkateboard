using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateboardBehavior : MonoBehaviour {

    [SerializeField]
    float rotationSpeed = 10f, weight = 5f, zoomSpeed = 5f, adjustSpeed = 1;

    [SerializeField]
    float zoomUpwardLimit, zoomDownwardLimit;
    int currentPart;
    Vector3 rotationVelocity, origPos;
    bool adjust;
    int index;
    [SerializeField]
    Vector3[] rotation;

    private void Start() {
        origPos = transform.position;
        transform.rotation = Quaternion.Euler(rotation[0]);
    }

    public void printMode() {
        transform.rotation = Quaternion.Euler(rotation[0]);
        transform.position = origPos;
    }

    void Update() {
        if (adjust) {
            Quaternion rot = Quaternion.Euler(rotation[index]);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime * adjustSpeed);
            transform.position = Vector3.MoveTowards(transform.position, origPos, Time.deltaTime * adjustSpeed);
            if(transform.rotation == rot && transform.position == origPos) {
                adjust = false;
            }
        } else {
            if (Input.GetMouseButton(0)) {
                rotationVelocity = new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0.0F);
            } else {
                float rotationWeight = weight * Time.deltaTime;
                rotationVelocity = Vector3.Lerp(rotationVelocity, Vector3.zero, rotationWeight);
            }
            /*
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (zoomSpeed * Time.deltaTime));
                if (transform.position.z >= zoomUpwardLimit) {
                    transform.position = new Vector3(transform.position.x, transform.position.y, zoomUpwardLimit);
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") <= -1f) {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (zoomSpeed * Time.deltaTime));
                if (transform.position.z < zoomDownwardLimit) {
                    transform.position = new Vector3(transform.position.x, transform.position.y, zoomDownwardLimit);
                }
            }
            */
            transform.Rotate(Camera.main.transform.up * rotationVelocity.x * rotationSpeed, Space.World);
            transform.Rotate(Camera.main.transform.right * rotationVelocity.y * rotationSpeed, Space.World);
        }
    }

    public void changeSkateboard(int part) {
        index = part;
        adjust = true;
    }

    public void ResetRotation() {
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}


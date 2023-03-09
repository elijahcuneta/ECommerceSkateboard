using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour {

    [SerializeField]
    float speed = 10;
    [SerializeField]
    public SkateboardParts skateboardParts;

    Vector3 targetPosition;

    void Update() {
        if (transform.localPosition != targetPosition) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
        }
    }

    public void GoToPosition(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
    }
}

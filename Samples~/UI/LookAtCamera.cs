using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCamera;

    private void Start()
    {
        mainCamera = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 targetPosition = new Vector3(mainCamera.position.x, transform.position.y, mainCamera.position.z);
        transform.LookAt(targetPosition);
    }
}

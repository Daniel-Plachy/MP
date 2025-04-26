using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (cam == null) return;
        // otočí tento objekt čelem ke kameře, zachová svislou osu
        Vector3 forward = transform.position - cam.transform.position;
        forward.y = 0;
        transform.rotation = Quaternion.LookRotation(forward);
    }
}

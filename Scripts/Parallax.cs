using UnityEngine;

namespace Kok
{
    public class Parallax : MonoBehaviour
    {
        Transform cam;
        Vector3 previousCamPos;

        [Range(-1, 1)] public float parallaxX = 0f;
        [Range(-1, 1)] public float parallaxY = 0f;

        void Start()
        {
            cam = Camera.main.transform;
            previousCamPos = cam.position;
        }

        void LateUpdate()
        {
            Vector3 deltaMovement = cam.position - previousCamPos;
            transform.position += new Vector3(deltaMovement.x * parallaxX, deltaMovement.y * parallaxY, deltaMovement.z);
            previousCamPos = cam.position;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;

    [Range(-1, 1)] public float parallaxX = 0f;
    [Range(-1, 1)] public float parallaxY = 0f;
    public bool repeatX = true;
    public bool repeatY = false;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position; 
        Sprite sprite = GetComponentInChildren<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        // Parralax
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxX, deltaMovement.y * parallaxY, deltaMovement.z);
        lastCameraPosition = cameraTransform.position; 

        // Repeat
        if (repeatX)
        {
            if(Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX) {
                float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }

        if (repeatY)
        {
            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
            {
                float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(cameraTransform.position.x, transform.position.y + offsetPositionY);
            }
        }
    }
}

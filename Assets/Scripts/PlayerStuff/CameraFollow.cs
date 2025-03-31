using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public Transform playerCharacter;
    public float movementSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerCharacter == null) return;

        transform.position = Vector3.Lerp(transform.position, playerCharacter.position, movementSpeed * Time.deltaTime);
    }
}

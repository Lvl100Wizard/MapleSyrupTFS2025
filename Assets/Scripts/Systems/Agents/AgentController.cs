using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    [SerializeField] private AgentTypes.Types agent = AgentTypes.Types.NpcIndividual;
    [SerializeField] private AgentPaths destinations;   
    private int currentDestinationIndex = 0; // Tracks which destination to move towards

    [SerializeField] private float moveSpeed = 3f; // Movement speed
    [SerializeField] private float rotationSpeed = 5f; // Rotation speed
    [SerializeField] private float stopDistance = 0.1f;

    [SerializeField] private float minPauseTime = 1f; // Minimum pause time at a destination
    [SerializeField] private float maxPauseTime = 3f; // Maximum pause time at a destination

    [SerializeField] private bool isWaiting = false; // Tracks whether NPC is pausing

    void FixedUpdate()
    {        
        if (destinations == null || destinations.path.Count == 0 || isWaiting) return; // No movement if empty or waiting

        Vector3 targetPosition = destinations.path[currentDestinationIndex].transform.position;
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Rotate NPC towards the target
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Move NPC towards the target
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

        // Check if NPC reached the target
        if (Vector3.Distance(transform.position, targetPosition) < stopDistance)
        {
            StartCoroutine(PauseBeforeNextDestination());
        }
    }
    
    private IEnumerator PauseBeforeNextDestination()
    {
        isWaiting = true;
        float pauseTime = Random.Range(minPauseTime, maxPauseTime);
        yield return new WaitForSeconds(pauseTime);

        // Move to the next destination
        currentDestinationIndex = (currentDestinationIndex + 1) % destinations.path.Count; // Loop to first if at last index

        isWaiting = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Globalization;

public class HunterController : MonoBehaviour
{
    private const char NEW_LINE = '\n';
    private const char EQUALS = '=';
    private Animator enemyAn;
    private Rigidbody enemyRb;
    private NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    private float radius;
    private float maxDistance;
    public bool hitPlayer = true;

    void Start()
    {
        enemyAn = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updatePosition = false;

        string filePath = "File/HunterDamageMultipliers";

        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split(NEW_LINE);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split(EQUALS);

            switch (token[0])
            {
                case "radius":
                    radius = float.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("radius: " + radius);
                    break;
                case "maxDistance":
                    maxDistance = float.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("maxDistance: " + maxDistance);
                    break;
                default:
                    break;
            }
        }
    }

    void Update()
    {
        RunToPlayer();
        FollowPlayer();
    }
    void RunToPlayer()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;
        GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
        // Update animation parameters
        enemyAn.SetBool("isMoving", shouldMove);
    }
    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }
    void FollowPlayer()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, transform.TransformDirection(Vector3.forward), out hit, maxDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                enemyAn.SetBool("isMoving", false);
                hitPlayer = true;
            }
        }
        else
        {
            hitPlayer = false;
        }
    }
}

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ChickenController : MonoBehaviour
{
    private Animator chickenAn;
    private Rigidbody chickenRb;
    private NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        chickenAn = GetComponent<Animator>();
        chickenRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
    }

    // Update is called once per frame
    void Update()
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

        // Update animation parameters
        chickenAn.SetBool("Run", shouldMove);
        
        if(shouldMove==true)
        {
            DestroyObjectDelayed();
        }
    }
    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }
    void DestroyObjectDelayed()
    {
       float timer =  Random.Range(3.0f, 5.0f);
        Destroy(gameObject, timer);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}

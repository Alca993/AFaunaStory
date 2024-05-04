using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Globalization;

public enum EnemyType {Farmer,Frog, Spider, Hunter}
public class EnemyController : MonoBehaviour
{
    private const char NEW_LINE = '\n';
    private const char EQUALS = '=';

    public NavMeshAgent enemy;
    private Transform target;
    private GameObject player;
    private Rigidbody enemyRb;
    private Animator enemyAn;
    private int velocity;
    private float range;
    private float rotationSpeed;
    private float radius;
    private float maxDistance;
    private Dictionary<string, float> multiplierDict;
    [SerializeField] private EnemyType type;
    public bool hitPlayer = true;



    public Dictionary<string, float> DamagesMultiplierDic
    {

        get
        {
            return multiplierDict;
        }


    }
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        target = player.transform;
        enemyAn = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody>();
        multiplierDict = new Dictionary<string, float>();

        string eT = type.ToString();
        string filePath = "File/" + eT + "DamageMultipliers";
        List<float> dM = new List<float>();

        TextAsset data = Resources.Load<TextAsset>(filePath);
        string[] lines = data.text.Split(NEW_LINE);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] token = line.Split(EQUALS);

            switch (token[0])
            {
                case "velocity":
                    velocity = int.Parse(token[1], CultureInfo.InvariantCulture);
                    break;
                case "range":
                    range = float.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("damageTimeout: " + range);
                    break;
                case "rotationSpeed":
                    rotationSpeed = float.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("damageTimeout: " + rotationSpeed);
                    break;
                case "radius":
                    radius = float.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("radius: " + radius);
                    break;
                case "maxDistance":
                    maxDistance = float.Parse(token[1], CultureInfo.InvariantCulture);
                    Debug.Log("maxDistance: " + maxDistance);
                    break;
                default:
                    Debug.Log(token[0]);
                    multiplierDict.Add(token[0], float.Parse(token[1], CultureInfo.InvariantCulture));
                    break;
            }
        }
    }

   
    void Update()
    {

        RunTo();
        AttackPlayer();
    }
    void RunTo()
    {
        Vector3 runTo = transform.position + ((target.position - transform.position) * velocity);
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < range)
        {
            enemyAn.SetBool("isMoving", true);
            enemy.SetDestination(runTo);
            if (distance <= enemy.stoppingDistance)
            {
                FaceTarget();
                enemyAn.SetBool("isMoving", false);
            }
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion LookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, Time.deltaTime * rotationSpeed);
    }
    void AttackPlayer()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, transform.TransformDirection(Vector3.forward), out hit, maxDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                enemyAn.SetBool("isMoving", false);
                enemyAn.SetBool("isAttacking", true);
                hitPlayer = true;
            }
        }
        else
        {
            hitPlayer = false;
            enemyAn.SetBool("isAttacking", false);
        }
    }
}

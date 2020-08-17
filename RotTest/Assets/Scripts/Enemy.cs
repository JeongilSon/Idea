using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float turnSpeed;

    private Transform target;
    private NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()   
    {
        nav.SetDestination(target.position);
    }
}

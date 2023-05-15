using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    public Rigidbody rb;
    public float RestLength;
    public float WheelRadius;
    public float SpringStiffness;
    public float DamperStiffness;
    public GameObject wheel;
    private float LastLength;
    private float CL;
    public Vector3 hitLocation = new Vector3(0,0,0);
    public Vector3 hitNormal = new Vector3(0,0,0);
    public float F;
    public Vector3 pos;
    public bool hasHit;
    public float SA;
    public Vector3 hitPoint;
    void Start(){
        LastLength = RestLength;
    }

    // Update is called once per frame
    void FixedUpdate(){
        hasHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, RestLength + WheelRadius);
        if (hasHit){
            // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance,Color.red);
            Vector3 WL = hit.point + Vector3.up * WheelRadius;
            hitLocation = hit.point;
            hitNormal = hit.normal;
            CL = (transform.position - WL).magnitude;
            wheel.transform.position = new Vector3(wheel.transform.position.x,transform.position.y - CL,wheel.transform.position.z);
            pos = wheel.transform.position;
            float SF = (RestLength - CL) * SpringStiffness;
            float DF = ((LastLength - CL) / Time.deltaTime) * DamperStiffness;
            LastLength = CL;
            F = SF + DF;
            rb.AddForceAtPosition(transform.up * F * 100,transform.position);
            hitPoint = hit.point;
        }
        else{
            CL = RestLength;
            hitPoint = Vector3.zero;
        }
        Vector3 linearWorldVelocity = rb.GetPointVelocity(hit.point);
        Vector3 linearLocalVelocity = transform.InverseTransformDirection(linearWorldVelocity); 

        // if (linearLocalVelocity.z != 0)
        // {
        //     float x = Mathf.Sqrt(linearLocalVelocity.x * linearLocalVelocity.x + linearLocalVelocity.z * linearLocalVelocity.z);
        //     SA = Mathf.Asin( (linearLocalVelocity.x / x)) * Mathf.Rad2Deg; //Calculate slip Angle
        //     // SA = Mathf.Atan(linearLocalVelocity.x / linearLocalVelocity.)
        // }
        // else
        // {
        //     SA = 0;
        // }   
         if (linearLocalVelocity.z != 0)
        {
            SA = Mathf.Atan((linearLocalVelocity.x * -1f) / Mathf.Abs(linearLocalVelocity.z)) * Mathf.Rad2Deg; //Calculate slip Angle
        }
        else
        {
            SA = 0;
        } 
        
    }
}

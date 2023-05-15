// 2023 PMR Studios. Open-source project. 2023 Kanto Legends V0.15b Dev
// The formula is open-source (will be)
using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CA_TireModel : MonoBehaviour
{
    // Tire Stuff

    // Public
    public float µ;
    public float TirePressure;
    public float force;
    public float MaxAngle;
    public float camber;

    //Private
        // private float B = 10.0f; private float C = 1.9f; private float D = 1.0f; private float E = 0.97f;
    private float AngularVelocity;
    private float Inertia;
    private float load = 0;

    // Wheel Properties
    public float WheelRadius;
    public float WheelMass;

    // Car Properties
    public Rigidbody rb;
    public Suspension s;

    private void FixedUpdate() {
        // Apply any forces only if the wheel is touching the ground
        if (s.hasHit){
            // Force application
            load = s.F * 100 / 9.81f;

            float steer = Input.GetAxis("Horizontal");

            transform.localRotation = Quaternion.Euler(0, MaxAngle * steer, 0);
            
            rb.AddForceAtPosition(CATM() + transform.forward * force * Input.GetAxis("Vertical"), s.hitPoint);

            // Debug
            Debug.DrawRay(s.hitPoint,(force * transform.forward).normalized,Color.green);
            Debug.DrawRay(s.hitPoint,(CATM()).normalized,Color.red);
        }   
    }

    Vector3 CATM(){
        
        // Velocities
        Vector3 vel = rb.GetPointVelocity(s.hitPoint);
        Vector3 localvel = transform.InverseTransformDirection(vel);

        // Slip Angle
        float SA = 0;
        if (Mathf.Round(localvel.z) != 0){
            SA = Mathf.Atan(1 * (-localvel.x/Mathf.Abs(localvel.z))) * Mathf.Rad2Deg;
        }

        // Values
        float x = SA;// assign the value of x
        float g = 2f * µ;// assign the value of g
        float f = camber;// assign the value of f
        float a = µ;// assign the value of a
        float Tp = TirePressure; // Tire Pressure
        float n = 100 * a * Tp/300; // Calculation of tire bending capability (stiffness)

        // CAF (Curve Approximation Formula) by me
        float LateralForceInNewtons = (Mathf.Atan(Mathf.Abs(x) * (Mathf.Abs(g) + Mathf.Abs(f) * 0.1f)) * ((10 * a + Mathf.Abs(f) * 0.1f) + -0.1f * Mathf.Abs(x))) * 0.07692f * load * 9.81f;

        // Other Variables idk
        float steer = Input.GetAxis("Horizontal");
        float Final = LateralForceInNewtons * steer * 0.5f;
        Vector3 ReturnVar = Vector3.zero;

        // Result
        ReturnVar = -(Final - SA * n) * transform.right;

        // Debug
        Debug.Log(Mathf.Abs(x));

        return ReturnVar;

    }

    // Things to work on

    // float LongF(float Radius, float MotorTorque, float Mass, float Load){
    //     Vector3 velocity = rb.GetPointVelocity(s.hitPoint);
    //     float LinearVelocity = transform.InverseTransformDirection(velocity).z;
    //     Inertia = Radius * Radius * Mass * 0.5f;

    //     float slipRatio = Mathf.Clamp(((AngularVelocity * Radius) - LinearVelocity) / Mathf.Max(Mathf.Abs(LinearVelocity),5f),0,100);
    //     float force = Pacejka(slipRatio) * Load;
    //     float torque = MotorTorque - (force * Radius);

    //     AngularVelocity = AngularVelocity + (torque / Inertia) * Time.fixedDeltaTime;
    //     return force;

    // }

    // private float Pacejka(float slip)
    // {
    //     return D * Mathf.Sin(C * Mathf.Atan(B * slip - E * (B * slip - Mathf.Atan(B * slip))));
    // }

}


using System;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    //coenficient dermining the RPM to thrust ratio
    [SerializeField] float thrustCoeficient;

    //minimum and maximum RPM constraints
    [SerializeField] float maxRevolutionsPerMinute = 8000;
    [SerializeField] float minRevolutionsPerMinute = 0;


    //this value dictates how quickly a rotor's rotation can be changed
    [Tooltip("RPM increase/decrese per ms.")]
    [SerializeField] float spinupRate = 1000;

    //rotor state tracking
    [SerializeField] float currentRevolutionRate;
    [SerializeField] float targetRevolutionRate;

    //determines if the debug lines should be drawn/visible
    [SerializeField] bool debugLines = true;

    Rigidbody2D rb;
        
    void Start()
    {
        //find and allocate the objects rigid body
        rb = GetComponent<Rigidbody2D>();        
    }

    // Update is called once per physics update
    void FixedUpdate()
    {
        //update revolution rate if the rotor is not moving at the requested rate
        if(targetRevolutionRate != currentRevolutionRate)        
            updateRevolutionRate();
        
        //calculate the current thrust vector
        Vector3 thrustVector = getThrustVector();

        //this code draws the debug lines to help visualize the applied force
        if (debugLines)
        {
            //fixed body frame vector transform
            Vector3 thrustVectorFBF = new Vector3(transform.position.x + thrustVector.x, transform.position.y + thrustVector.y);
            Debug.DrawLine(rb.transform.position, thrustVectorFBF, Color.red, 0.02f);
        }

        //add the thrust for this frame
        rb.AddForce(thrustVector, ForceMode2D.Force);
    }

    public Vector3 getThrustVector()
    {
        //derive a neutral vector where the length equals the rotor's thrust force
        Vector3 neutralThrustVector = Vector3.up * getThurstForce();
        //rotate the vector to align with the  orientation of the rotor
        Vector3 rotatedThrustVector = new Vector3(neutralThrustVector.magnitude * Mathf.Sin(Mathf.Deg2Rad * -rb.rotation), neutralThrustVector.magnitude * Mathf.Cos(Mathf.Deg2Rad * -rb.rotation));
        return rotatedThrustVector;
    }

    public float getThurstForce()
    {
        return currentRevolutionRate * thrustCoeficient;
    }

    public void setRevolutionTarget(float target)
    {
        //set the target revolution, ensuring that the value lies within the min and max RPM values
        targetRevolutionRate = Mathf.Clamp(target, minRevolutionsPerMinute, maxRevolutionsPerMinute);                
    }

    private void updateRevolutionRate()
    {
        float mSecondToSeconds = 1000;        
        //update the rotor revolution rate clamping around the maximum and minim values
        if (currentRevolutionRate < targetRevolutionRate)
            currentRevolutionRate = Math.Min(currentRevolutionRate + Time.fixedDeltaTime * mSecondToSeconds * spinupRate, Math.Min(maxRevolutionsPerMinute, targetRevolutionRate));
        else
            currentRevolutionRate = Math.Max(currentRevolutionRate - Time.fixedDeltaTime * mSecondToSeconds * spinupRate, Math.Max(minRevolutionsPerMinute, targetRevolutionRate));
    }

    public float getMaxRevolutionsPerMinute()
    {
        return maxRevolutionsPerMinute;
    }

    public float getMinRevolutionsPerMinute()
    {
        return 0;
    }

}

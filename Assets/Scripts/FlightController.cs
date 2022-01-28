using UnityEngine;

public class FlightController : MonoBehaviour
{     
    //desired position (only y value is considered)
    [SerializeField] Vector3 desiredPosition;
    //drone object to control
    [SerializeField] Quadcopter quadcopter;    

    //PID terms for each PID controller, altitude and attitude
    [SerializeField] 
    [Range(0, 100)]    
    float thrustPIDp, thrustPIDi, thrustPIDd;
    [SerializeField]
    [Range(0, 100)]
    float rollPIDp, rollPIDi, rollPIDd;
    PIDController altitudePIDController;
    PIDController attitudePIDController;

    //error values computed by the Error Estimator
    private float yError = 0;
    private float fiError = 0;    

    // Start is called before the first frame update
    void Start()
    {   
        //initialize the PID controllers - one for altitude and one for attitude
        altitudePIDController = new PIDController(thrustPIDp, thrustPIDi, thrustPIDd, quadcopter.GetMaxThurst(), quadcopter.GetMinThurst());
        attitudePIDController = new PIDController(rollPIDp, rollPIDi, rollPIDd, quadcopter.GetMaxThurst()/2, -quadcopter.GetMaxThurst()/2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //get the new estimated state error
        EstimateError();        
        //generate u1 and u2 values using the PID controllers 
        float u1 = altitudePIDController.GetPIDOutput(yError, Time.fixedDeltaTime);       
        float u2 = attitudePIDController.GetPIDOutput(fiError, Time.fixedDeltaTime);
        //push the updates to the Multi Motor Algorithm
        quadcopter.updateMMA(u1, u2);
    }

    public void EstimateError()
    {
        //calculate the delta between the current and target y position and orientation angle.   
        yError = desiredPosition.y - quadcopter.transform.position.y;
        fiError = 0 - quadcopter.transform.rotation.z;
    }


}

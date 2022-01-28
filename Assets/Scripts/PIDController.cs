using UnityEngine;
public class PIDController
{
    //PID coefficients 
    private float kP, kI, kD;

    //PID controller output clamps to prevent oversaturation
    private float saturationLimitMax, saturationLimitMin;
    
    //integral clamp flag
    private bool clamp = false;
    
    //error memory term
    public float error;

    //PID magnitudes, i is cumulative 
    public float p, i, d;

    public PIDController(float p, float i, float d, float saturationLimitMax, float saturationLimitMin)
    {
        this.kP = p;
        this.kI = i;
        this.kD = d;
        this.saturationLimitMax = saturationLimitMax;
        this.saturationLimitMin = saturationLimitMin;
    }

    public float GetPIDOutput(float curretError, float deltaTime)
    {               
        //calculate the proportional term directly from the error value
        p = curretError;
        
        //the integral term is cummalitive and we clamp this term to prevent over saturation
        //in the senario that the rotors have reached maximum capacity
        if(!clamp)
            i += curretError * deltaTime;
        
        //calculate the derivative term 
        d = (curretError - error) / deltaTime;

        //store the current error state to be used on the next update
        error = curretError;

        //apply the coefficnets to the pid values and sum the result
        float preSaturationFilterValue = p * kP + i * kI + kD * d;

        //clamp the output value based on the Min/Max saturation values
        float output = Mathf.Clamp(preSaturationFilterValue, saturationLimitMin, saturationLimitMax);

        //clamp the integral increment if output match is reached
        if (preSaturationFilterValue != output)
        {
            if(Mathf.Sign(curretError) == Mathf.Sign(preSaturationFilterValue))
                clamp = true;            
            else            
                clamp = false;            
        }
        else        
            clamp = false;
        
        return output;
    }  
}

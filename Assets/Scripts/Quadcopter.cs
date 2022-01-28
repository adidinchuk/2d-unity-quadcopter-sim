using UnityEngine;

public class Quadcopter : MonoBehaviour
{
    //Quadcopter componenets
    [SerializeField] Thruster leftRotor;
    [SerializeField] Thruster rightRotor;    

    [SerializeField] float rotorCount = 2;
    //total mass attribute to help derive required forces    
        

    public void updateMMA(float u1, float u2)
    {
        //calculate the required thrust given the angular orientation
        float adjustedU1 = (Mathf.Cos(Mathf.Deg2Rad * transform.rotation.z) * u1) / rotorCount; 
        //set rotor speed targets
        leftRotor.setRevolutionTarget(adjustedU1 - u2);
        rightRotor.setRevolutionTarget(adjustedU1 + u2);        
    }

    public float GetMaxThurst()
    {
        return leftRotor.getMaxRevolutionsPerMinute();
    }
    public float GetMinThurst()
    {
        return leftRotor.getMinRevolutionsPerMinute();
    }
}

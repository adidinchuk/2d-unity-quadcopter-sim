## 2d-unity-quadcopter-sim

## Table of contents

- [General info](#general-info)
- [Technologies](#technologies)
- [Running the Code](#running-the-code)

## General info

The code in the repo should run as-is out of the box but how the parameters of the simulation can be manipulated are covered below.

# Thrusters

The drone has left and right thrusters, these can be manipulated separately, however for best results their parameters should be identical. Each thruster also has a rigid body with a defined mass (0.3) and this rigid body is attached to the parent drone body using a 2D Fixed Joint.

![alt text](https://cdn-images-1.medium.com/max/800/1*9xIpEFJhUD_xvHlHsjfBVg.png)

The main Thruster.cs parameters to configure would be the thrust coefficient, maximum RPM, and the spinup rate.

# Quadcopter

The quadcopter also had a rigid body with a defined mass (1) along with a simple collider to prevent it from passing through the surface platform.

![alt text](https://cdn-images-1.medium.com/max/800/1*oW0ooqAlULV2XLiezIKKQg.png)

The rotors and rotor count are set in the Quadcopter.cs script and the target position and PID control coefficients in the FlightControl.cs script.

## Technologies

Project is created with:

- Unity 2020.3.21f1

## Running the Code

When we run the scene the quadcopter lifts off the ground, moves towards the specified altitude, and settles there.

The P thrust values can be modified to change how quickly the drone reaches the desired high.

The D thrust value dictates how smooth our trajectory is and can be used to minimize overshooting at a cost of slower convergence.

The I thrust value allows the system to overcome unforeseen disturbances in the environment and have a smoother recovery.

The roll PID coefficients provide the same role but for pitch.

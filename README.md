# GlobalWorming

# Structure:
- A Player object must have:
  - a CharacterController component;
  - a PlayerInput input system component;
  - a PlayerInputs script;
  - a PlayerCharacterController script;
  - a WallRun script;
  - a MyWeaponManagerScript;

- Every weapon must have:
  - a Sway Script;
  - a Weapon Controller Script;
  - a parent-children structure identical to CubePistol prefab. Please use it as an example.

- Every projectile must have:
  - a collider;
  - a ProjectileBase script;
  - a ProjectileStandard script.

# Variables
### PlayerController
The player controller is really well documented. If you're a coder, I left a lot of extensive comments throughout the script. If you're a designer, hover over the variables to reveal the tooltip. If you still have any questions, message me on discord.

### Wall Run
- ***Max Wall Distance:*** the maximum distance from the wall the wall detection works at. It shoots out rays of this length from the capsule into 4 directions: left, left+45, right, right-45.

- ***Wall Speed Multiplier:*** the number by which the speed multiplied when jumping off a wall.

- ***Minimum Height:*** the minimum height the player has to be at after the jump for the wall run to kick in.

- ***Max Angle Roll:*** the maximum angle (in degrees) the camera will rotate around the Z-axis when the wall run starts.

- ***Normalized Angle Threshold:*** value the dot product of the Up vector and the normal of the wall is compared to. 

- ***Jump Duration:*** the amount of time since the player jumped before the wall run can begin. E.g. if jump duration is set to 0.25, then the wall run will begin 0.25 seconds after the jump.

- ***Wall bouncing:*** the strength of the jump. The higher the value, the further to the side the player will go when jumping of a wall.

- ***Camera Transition Duration:*** the lerp speed of the camera roll will reach Max Angle Roll. The higher the value, the fast it transitions.

- ***Wall Gravity Down Force:*** The "gravity" applied on the wall during the wall run. I.e. the reason the player goes downward slightly as the wall run progresses.

- ***Use Sprint:*** if checked, the player will only be able to enter a wall run while sprinting.

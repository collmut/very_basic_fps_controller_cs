using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// Game Objects : 
	Node3D cam;
	
	
	// Variables : 
	float MouseSens = 0.2f;
	float MaxLookY = 85f;
	
	float TargetRotY;
	
	float Speed = 10f;
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cam = (Node3D)GetNode("cam");
		
		// set the mouse cursor at the center of the screen : 
		Input.MouseMode = Input.MouseModeEnum.Captured;
		
		TargetRotY = this.RotationDegrees.Y;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CheckInputActions();
		InterpolateRotationY();
	}
	
	void CheckInputActions()
	{
		if(Input.IsActionPressed("left"))
		{
			Move(1);
		}
		
		if(Input.IsActionPressed("right"))
		{
			Move(2);
		}
		
		if(Input.IsActionPressed("forward"))
		{
			Move(3);
		}
		
		if(Input.IsActionPressed("backward"))
		{
			Move(4);
		}
	}
	
	public override void _Input(InputEvent ev)
	{
		InputEventMouseMotion mm = new InputEventMouseMotion();
		
		if(ev.GetType() == mm.GetType())
		{
			// if its the mouse thats moved , set the mm object as the value of ev : 
			mm = (InputEventMouseMotion)ev;
			
			// call the look method of the player : 
			Look(mm.Relative);
		}
	}
	
	public void Look(Vector2 relativeMousePos)
	{
		// look upwards and downwards :
		Vector3 newCamRot = cam.RotationDegrees; 
		newCamRot.X = newCamRot.X - (MouseSens * relativeMousePos.Y / 3);
		newCamRot.X = Godot.Mathf.Clamp(value: newCamRot.X , min: -MaxLookY, max: MaxLookY);
		
		cam.RotationDegrees = newCamRot;
		
		// look sideways : 
		TargetRotY = this.RotationDegrees.Y - (relativeMousePos.X * MouseSens);					// A method called from the process() lerps the RotationDegrees.Y to the TargetRotY value 
	}
	
	public void InterpolateRotationY()
	{
		// smoothly interpolates the rotation of the player in the y axis to the targetroty variable :
		float currentRotYRad = Mathf.DegToRad(this.RotationDegrees.Y);
		float targetRotYRad = Mathf.DegToRad(TargetRotY);
		float newRotY = Mathf.LerpAngle(from: currentRotYRad, to: targetRotYRad, weight: 0.3f);
		
		
		// set rotation in the y axis : 
		Vector3 newRot = this.Rotation;
		newRot.Y = newRotY;
		
		this.Rotation = newRot;
	}
	
	public void Move(int directionIndex)
	{
		// moves player by applying speed in a certain axis , according to the direction that the player is facing : 
		Vector3 newVel = new Vector3();
		Vector3 forwardDirection = this.Transform.Basis.Z;
		Vector3 sideDirection = this.Transform.Basis.X;
		
		switch(directionIndex)
		{
			case 1: 
				newVel = sideDirection * Speed * -1;
				break;
			
			case 2: 
				newVel = sideDirection * Speed;
				break;
			
			case 3: 
				newVel = forwardDirection * Speed * -1;
				break;
			
			case 4: 
				newVel = forwardDirection * Speed;
				break;
			
			default: 
				break;
		}
		
		this.Velocity = newVel;
		this.MoveAndSlide();
	}
	
}

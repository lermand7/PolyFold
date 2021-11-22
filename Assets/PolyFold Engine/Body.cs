using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body
{
	public Vector2 Position, PrevPosition = new Vector2();
	public Vector2 Velocity = new Vector2();
	public Vector2 Force = new Vector2();
	public float AngularVelocity;
	public float Torque;
	public float Orient;
	public float Mass, InvMass, Inertia, InvInertia;
	public float StaticFriction, DynamicFriction;
	public float Restitution;
	public Shape Shape;
	public PolyFoldObject Object;
	public LineRenderer Renderer;

	public Body(Shape shape, PolyFoldObject obj, float x, float y)
	{
		Shape = shape;
		Object = obj;

		Renderer = Object.gameObject.AddComponent<LineRenderer>();

		Position.Set(x, y);
		PrevPosition.Set(x, y);
		Velocity.Set(0, 0);
		AngularVelocity = 0;
		Torque = 0;
		Orient = Random.Range(-Mathf.PI, Mathf.PI);
		Force.Set(0, 0);
		StaticFriction = 0.6f;
		DynamicFriction = 0.4f;
		Restitution = 0.05f;

		shape.Body = this;
		shape.Initialize();

        obj.AttachBody(this);
    }

	public void ApplyForce(Vector2 f)
	{
		Force += f;
	}

	public void ApplyImpulse(Vector2 impulse, Vector2 contactVector)
	{
		Velocity += impulse * InvMass;
		AngularVelocity += InvInertia * (contactVector.x * impulse.y - contactVector.y * impulse.x);
	}

	public void SetStatic()
	{
		Inertia = 0.0f;
		InvInertia = 0.0f;
		Mass = 0.0f;
		InvMass = 0.0f;
	}

	public void SetOrient(float radians)
	{
		Orient = radians;
		Shape.SetOrient(radians);
	}
}
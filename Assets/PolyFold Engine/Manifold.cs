using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manifold
{
	public Body A;
	public Body B;
	public float penetration;
	public Vector2 normal = new Vector2();
	public Vector2[] contacts = { new Vector2(), new Vector2() };
	public int contactCount;
	public float e;
	public float df;
	public float sf;

	public Manifold(Body a, Body b)
	{
		A = a;
		B = b;
	}
	public void solve()
	{
		int ia = A.Shape.GetShapeType();
		int ib = B.Shape.GetShapeType();

		Collisions.dispatch[ia][ib].handleCollision(this, A, B);
	}

	public void initialize(float dt, Vector2 Gravity)
	{
		e = Mathf.Min(A.Restitution, B.Restitution);

		sf = Mathf.Sqrt(A.StaticFriction * A.StaticFriction + B.StaticFriction * B.StaticFriction);
		df = Mathf.Sqrt(A.DynamicFriction * A.DynamicFriction + B.DynamicFriction * B.DynamicFriction);

		for (int i = 0; i < contactCount; ++i)
		{
			Vector2 ra = contacts[i] - (A.Position);
			Vector2 rb = contacts[i] - (B.Position);

			Vector2 rv = B.Velocity + new Vector2(-B.AngularVelocity * rb.y, B.AngularVelocity * rb.x) - A.Velocity - new Vector2(-A.AngularVelocity * ra.y, A.AngularVelocity * ra.x);

			if (rv.sqrMagnitude < (dt * Gravity).sqrMagnitude + Mathf.Epsilon)
			{
				e = 0.0f;
			}
		}
	}

	public void applyImpulse()
	{
		if (Mathf.Approximately(A.InvMass + B.InvMass, 0))
		{
			A.Velocity.Set(0, 0);
			B.Velocity.Set(0, 0);
			return;
		}

		for (int i = 0; i < contactCount; ++i)
		{
			Vector2 ra = contacts[i] - (A.Position);
			Vector2 rb = contacts[i] - (B.Position);

			Vector2 rv = B.Velocity + new Vector2(-B.AngularVelocity * rb.y, B.AngularVelocity * rb.x) - A.Velocity - new Vector2(-A.AngularVelocity * ra.y, A.AngularVelocity * ra.x);

			float contactVel = Vector2.Dot(rv, normal);

			if (contactVel > 0)
			{
				return;
			}

			float raCrossN = ra.x * normal.y - ra.y * normal.x;
			float rbCrossN = rb.x * normal.y - rb.y * normal.x;
			float invMassSum = A.InvMass + B.InvMass + (raCrossN * raCrossN) * A.InvInertia + (rbCrossN * rbCrossN) * B.InvInertia;

			float j = -(1.0f + e) * contactVel;
			j /= invMassSum;
			j /= contactCount;

			Vector2 impulse = normal * j;
			A.ApplyImpulse(-impulse, ra);
			B.ApplyImpulse(impulse, rb);

			rv = B.Velocity + new Vector2(-B.AngularVelocity * rb.y, B.AngularVelocity * rb.x) - A.Velocity - new Vector2(-A.AngularVelocity * ra.y, A.AngularVelocity * ra.x);

			Vector2 t = rv - (normal * Vector2.Dot(rv, normal));
			t.Normalize();

			float jt = -Vector2.Dot(rv, t);
			jt /= invMassSum;
			jt /= contactCount;

			if (Mathf.Approximately(jt, 0.0f))
			{
				return;
			}

			Vector2 tangentImpulse;
			if (Mathf.Abs(jt) < j * sf)
			{
				tangentImpulse = t * jt;
			}
			else
			{
				tangentImpulse = t * -j * df;
			}

			A.ApplyImpulse(-tangentImpulse, ra);
			B.ApplyImpulse(tangentImpulse, rb);
		}
	}

	public void positionalCorrection()
	{
		Vector2 correction = (Mathf.Max(penetration - 0.05f, 0.0f) / (A.InvMass + B.InvMass)) * normal * 0.2f;

		A.Position -= correction * A.InvMass;
		B.Position += correction * B.InvMass;
	}
}
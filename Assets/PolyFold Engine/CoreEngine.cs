using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreEngine
{
	public Vector2 Gravity = new Vector2(0, -10.0f);

	public float Dt;
	public int Iterations;
	public List<Body> Bodies = new List<Body>();
	public List<Manifold> Contacts = new List<Manifold>();

	public CoreEngine(float dt, int iterations)
	{
		this.Dt = dt;
		this.Iterations = iterations;
	}

	public void step()
	{
		Contacts.Clear();
		for (int i = 0; i < Bodies.Count; ++i)
		{
			Body A = Bodies[i];

			for (int j = i + 1; j < Bodies.Count; ++j)
			{
				Body B = Bodies[j];

				if (A.InvMass == 0 && B.InvMass == 0)
				{
					continue;
				}

				Manifold m = new Manifold(A, B);
				m.solve();

				if (m.contactCount > 0)
				{
					Contacts.Add(m);
				}
			}
		}

		for (int i = 0; i < Bodies.Count; ++i)
		{
			integrateForces(Bodies[i], Dt);
		}

		for (int i = 0; i < Contacts.Count; ++i)
		{
			Contacts[i].initialize(Dt, Gravity);
		}

		for (int j = 0; j < Iterations; ++j)
		{
			for (int i = 0; i < Contacts.Count; ++i)
			{
				Contacts[i].applyImpulse();
			}
		}

		for (int i = 0; i < Bodies.Count; ++i)
		{
			integrateVelocity(Bodies[i], Dt);
		}

		/*for (int i = 0; i < Contacts.Count; ++i)
		{
			Contacts[i].positionalCorrection();
		}*/

		for (int i = 0; i < Bodies.Count; ++i)
		{
			Body b = Bodies[i];
			b.Force.Set(0, 0);
			b.Torque = 0;
		}
	}

	public Body add(Shape shape, PolyFoldObject obj)
	{
		Body b = new Body(shape, obj, obj.transform.position.x, obj.transform.position.y);
		Bodies.Add(b);
		return b;
	}

	public void clear()
	{
		//contacts.clear();
		Bodies.Clear();
	}

	public void integrateForces(Body b, float dt)
	{
		if (b.InvMass == 0.0f)
		{
			return;
		}

		b.Velocity += (b.Force * b.InvMass + Gravity) * (dt / 2.0f);
		b.AngularVelocity += b.Torque * b.InvInertia * (dt / 2.0f);
	}

	public void integrateVelocity(Body b, float dt)
	{
		if (b.InvMass == 0.0f)
		{
			return;
		}

		b.PrevPosition = b.Position;
		b.Position += b.Velocity * dt;
		b.Orient += b.AngularVelocity * dt;
		b.SetOrient(b.Orient);

		integrateForces(b, dt);
	}

	public void Render(float alpha)
    {
		for (int i = 0; i < Bodies.Count; ++i)
		{
			Vector2 interPos = Bodies[i].PrevPosition * (1.0f - alpha) + Bodies[i].Position * alpha;
			//Debug.Log(interPos.y + " " + Bodies[i].PrevPosition.y + " " + Bodies[i].Position.y + " " + alpha.ToString() + " " + Time.time);
			//Bodies[i].Shape.Draw(interPos);
			Bodies[i].Object.transform.position = interPos;
		}
	}
}

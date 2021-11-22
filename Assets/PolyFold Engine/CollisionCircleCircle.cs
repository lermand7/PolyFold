using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCircleCircle : CollisionCallback
{

	public static CollisionCircleCircle instance = new CollisionCircleCircle();

	public void handleCollision(Manifold m, Body a, Body b)
{
	Circle A = (Circle)a.Shape;
	Circle B = (Circle)b.Shape;

	Vector2 normal = b.Position - a.Position;

	float dist_sqr = normal.SqrMagnitude();
	float radius = A.Radius + B.Radius;

	if (dist_sqr >= radius * radius)
	{
		m.contactCount = 0;
		return;
	}

	float distance = Mathf.Sqrt(dist_sqr);

	m.contactCount = 1;

	if (distance == 0.0f)
	{
		m.penetration = A.Radius;
		m.normal.Set(1.0f, 0.0f);
		m.contacts[0] = a.Position;
	}
	else
	{
		m.penetration = radius - distance;
		m.normal = normal / distance;
		m.contacts[0] = m.normal * A.Radius + a.Position;
		}
	}
}
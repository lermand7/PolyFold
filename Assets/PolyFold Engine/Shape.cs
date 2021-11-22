using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape
{
	public enum ShapeType
	{
		Circle,
		Poly,
		Count
	}

	public Shape()
	{
	}
	public abstract Shape Clone();
	public abstract void Initialize();
	public abstract void ComputeMass(float density);
	public abstract void SetOrient(float radians);
	public abstract void Draw(Vector2 interPos);
	public abstract int GetShapeType();

	public Body Body;

	public float Radius;
}

public class Circle : Shape
{
	public Circle(float r)
	{
		Radius = r;
	}

	public override Shape Clone()
	{
		return new Circle(Radius);
	}

	public override void Initialize()
	{
		ComputeMass(1.0f);
	}

	public override void ComputeMass(float density)
	{
		Body.Mass = Mathf.PI * Radius * Radius * density;
		Body.InvMass = (Body.Mass != 0.0f) ? 1.0f / Body.Mass : 0.0f;
		Body.Inertia = Body.Mass * Radius * Radius;
		Body.InvInertia = (Body.Inertia != 0.0f) ? 1.0f / Body.Inertia : 0.0f;
	}

	public override void SetOrient(float radians)
	{
	}

    public override void Draw(Vector2 interPos)
    {
		float theta_scale = 0.01f;

		float sizeValue = (2.0f * Mathf.PI) / theta_scale;
		int size = (int)sizeValue;
		size++;

		Body.Renderer.material =  new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
		Body.Renderer.startWidth = 0.1f;
		Body.Renderer.endWidth = 0.1f;
		Body.Renderer.positionCount = size;

		Vector2 pos;
		float theta = 0f;
		for (int i = 0; i < size; i++)
		{
			theta += (2.0f * Mathf.PI * theta_scale);
			float x = (Radius - 0.05f) * Mathf.Cos(theta);
			float y = (Radius - 0.05f) * Mathf.Sin(theta);
			x += interPos.x;
			y += interPos.y;
			pos = new Vector2(x, y);
			Body.Renderer.SetPosition(i, pos);
		}
	}

    public override int GetShapeType()
	{
		return 0;
	}
}

/*public class Polygon : Shape
{
	public int VertexCount;

	public Vector2[] Vertices = new Vector2[64];
	public Vector2[] Normals = new Vector2[64];

	public Polygon() { }

	public Polygon(Vector2[] verts)
	{
		Set(verts);
	}

	public Polygon(float min, float max)
	{
		SetRect(min, max);
	}

    public override Shape Clone()
    {
		Polygon p = new Polygon();

		for (int i = 0; i < VertexCount; i++)
		{
			p.Vertices[i] = Vertices[i];
			p.Normals[i] = Normals[i];
		}

		p.VertexCount = VertexCount;

		return p;
	}

    public override void Initialize()
	{
		ComputeMass(1.0f);
	}

	public override void ComputeMass(float density)
	{
		Vector2 c = new Vector2(0.0f, 0.0f);
		float area = 0.0f;
		float I = 0.0f;
		float k_inv3 = 1.0f / 3.0f;

		for (int i = 0; i < VertexCount; i++)
		{
			Vector2 p1 = Vertices[i];
			Vector2 p2 = Vertices[(i + 1) % VertexCount];

			float D = (p1.x * p2.y - p1.y * p2.x);
			float triangleArea = 0.5f * D;

			area += triangleArea;

			c += triangleArea * k_inv3 * (p1 + p2);

			float intx2 = p1.x * p1.x + p2.x * p1.x + p2.x * p2.x;
			float inty2 = p1.y * p1.y + p2.y * p1.y + p2.y * p2.y;
			I += (0.25f * k_inv3 * D) * (intx2 + inty2);
		}

		c *= 1.0f / area;

		for (int i = 0; i < VertexCount; ++i)
		{
			Vertices[i] -= c;
		}

		Body.Mass = density * area;
		Body.InvMass = (Body.Mass != 0.0f) ? 1.0f / Body.Mass : 0.0f;
		Body.Inertia = I * density;
		Body.InvInertia = (Body.Inertia != 0.0f) ? 1.0f / Body.Inertia : 0.0f;
	}
}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions
{
	public static CollisionCallback[][] dispatch =
	{
		new CollisionCallback[]{ CollisionCircleCircle.instance }//, CollisionCirclePolygon },
		//{ CollisionPolygonCircle, CollisionPolygonPolygon }
	};
}
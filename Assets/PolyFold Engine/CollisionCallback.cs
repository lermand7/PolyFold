using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CollisionCallback
{
	void handleCollision(Manifold m, Body a, Body b);
}
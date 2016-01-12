using UnityEngine;
using System.Collections.Generic;

namespace Tube
{
	public class BezierCurve
	{
		private Vector3 v1, v2, v3;
		private Vector3 a, b;
		
		public BezierCurve(Vector3 v1, Vector3 v2, Vector3 v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
			this.a = v1 - 2f * v2 + v3;
			this.b = v2 - v1;
		}

		public float Move(float l, float t)
		{
			return l / 2f / (t * a + b).magnitude;
		}
		
		
		public float BeforeEnd(float t)
		{
			return 2f * (1f - t) * (t * a + b).magnitude;
		}
		
		public Vector3 GetPoint(float t)
		{
			float t1 = 1 - t;
			return t1 * t1 * v1 + 2f * t1 * t * v2 + t * t * v3;
		}
		
		public Vector3 GetDirection(float t)
		{
			var res = a * t + b;
			res.Normalize();
			return res;
		}
		
		public Vector3 V1 { get { return v1; } }
		public Vector3 V2 { get { return v2; } }
		public Vector3 V3 { get { return v3; } }
	}
}


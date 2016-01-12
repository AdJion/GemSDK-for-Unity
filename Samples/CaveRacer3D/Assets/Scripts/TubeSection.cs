using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Tube 
{
	public class TubeSection : MonoBehaviour 
	{
		public int Slices;
		public BezierCurve Curve = null;
		public float TexEndValue = 0;
        public float ObjStartValue = 0;

		private float radius;

		public GameObject DustPrefab;
		public int dustCount;

		public GameObject StonePrefab;
		public float StoneChance;
        public float StoneOffset; //+- radians  
		public float stoneMinSize;
		public float stoneMaxSize;
		public float stoneMinRadius;
		public float stoneMaxRadius;

        public float ObjectGenerationStep = 1f;
        public Object CoinPrefab;   

        private CoinGenerator coinGenerator;

		// Use this for initialization
		void Start () {
            
		}

		public BezierCurve GetCurve() {
			return Curve;
		}

		public void initGeometry(float r, float step, float tc, float objStart) {
			radius = r;
			TexEndValue = tc;
            ObjStartValue = objStart;
            coinGenerator = GetComponentInParent<CoinGenerator>();
			
			List<Vector3> verts = new List<Vector3>();
			List<int> ids = new List<int>();
			List<Vector3> norms = new List<Vector3>();
			List<Vector2> uvs = new List<Vector2>();
			Vector3 pos = transform.position;

			//Mesh generation
			Vector3[] circle = makeCircle (r, Slices);
			float tc_factor = 2f * (float)Mathf.PI * r / (float)Slices;
			
			float tc_cur = tc;
			float t = 0f;
			
			bool complete = false;
			while (!complete)
			{
				if (t >= 1f)
				{
					tc_cur += Curve.BeforeEnd(t) * tc_factor;
					TexEndValue = tc_cur - (float)Mathf.Floor(tc_cur);
					t = 1f;
					complete = true;
				}
				
				Vector3 pos1 = Curve.GetPoint(t) - pos;
				Quaternion transform1 = Quaternion.LookRotation(Curve.GetDirection(t));
				
				for (int i = 0; i <= Slices; i++)
				{
					Vector3 n1 = transform1 * circle[i % Slices];
					Vector3 v1 = n1 + pos1;
					n1 = -n1;
					n1.Normalize();
					verts.Add(v1);
					uvs.Add(new Vector2(i, tc_cur));
					norms.Add(n1);
				}
				
				t += Curve.Move(step, t);
				tc_cur += step * tc_factor;
			}

			int cverts = Slices + 1;
			int ccount = verts.Count / cverts - 1;

			for (int i = 0; i < ccount; i++)
			{
				int startIndex = 6 * i * Slices;
				for (int j = 0; j < Slices; j++)
				{
					//int curIndex = startIndex + j * 6;
					int v0 = i * cverts + j;
					int v1 = (i + 1) * cverts + j + 1;
					int v2 = (i + 1) * cverts + j;
					int v3 = i * cverts + j + 1;
					
					ids.AddRange(new int[] { v1, v0, v2, v3, v0, v1 });
				}
			}
					
			//Finish mesh
			Mesh m = new Mesh ();
			m.vertices = verts.ToArray();
			m.triangles = ids.ToArray();
			m.normals = norms.ToArray();
			m.uv = uvs.ToArray();
			m.RecalculateBounds();
			
			GetComponent<MeshFilter> ().mesh = m;
            GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		}

		public void makeDust() {
			for (int i = 0; i < dustCount; i++) {
				float t = Random.Range(0f, 1f);
				Vector3 centerPos = Curve.GetPoint(t);
				Quaternion posTransform = Quaternion.LookRotation(Curve.GetDirection(t)); 
				
				float angle = Random.value * Mathf.PI * 2f;
				Vector3 localPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
				
				Vector3 pos = centerPos + posTransform * localPos;
				
				GameObject dust = (GameObject)Instantiate(DustPrefab, pos, 
					Quaternion.LookRotation(posTransform * (-localPos)));

				dust.transform.parent = this.transform;
			}
		}

        public void makeObjects()
        {
            float obj_cur = ObjStartValue;
            float t = Curve.Move(ObjStartValue, 0f);

            while (t <= 1f)
            {
                obj_cur += ObjectGenerationStep;
                t += Curve.Move(ObjectGenerationStep, t);

                coinGenerator.Next();

                float r = Random.Range(0, 1f);
                //Debug.Log(r);
                if (r <= StoneChance) makeStone(t);

                if (coinGenerator.isCoin)
                {
                    makeCoin(t);
                }
            }


            ObjStartValue = ObjectGenerationStep - Curve.BeforeEnd(t);
        }

        public void makeCoin(float t)
        {
            Vector3 centerPos = Curve.GetPoint(t);
            Quaternion posTransform = Quaternion.LookRotation(Curve.GetDirection(t));

            float angle = coinGenerator.Angle;
            Vector3 localPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius * 0.65f;

            Vector3 pos = centerPos + posTransform * localPos;

            GameObject coin = (GameObject)Instantiate(CoinPrefab, pos,
                Quaternion.LookRotation(posTransform * (-localPos)));

            coin.transform.parent = this.transform;
        }

        public void makeStone(float t)
        {
            float angle;
            do {
                angle = Random.Range(0,  Mathf.PI * 2f);
            }
            while(coinGenerator.isCoin && isInBounds(angle, coinGenerator.Angle, StoneOffset));

            Vector3 centerPos = Curve.GetPoint(t);
            Quaternion posTransform = Quaternion.LookRotation(Curve.GetDirection(t)); 

            float stoneScale = Random.Range(stoneMinSize, stoneMaxSize);
			
            Vector3 localPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle))
               * (radius - Random.Range(stoneMinRadius, stoneMaxRadius) * stoneScale);

            Vector3 pos = centerPos + posTransform * localPos;

            GameObject stone = (GameObject)Instantiate(StonePrefab, pos, Random.rotation);
            stone.transform.localScale = new Vector3(stoneScale, stoneScale, stoneScale);
            stone.transform.parent = this.transform;
        }
		
        private bool isInBounds(float angle, float origin, float limit) {
            float up = Mathf.Repeat(origin + limit, Mathf.PI * 2f);
            float down = Mathf.Repeat(origin - limit, Mathf.PI * 2f);

            if (up >= down) return angle >= down && angle <= up;
            return angle >= down || angle <= up;
        }

		private Vector3[] makeCircle(float r, int slices)
		{
			Vector3[] verts = new Vector3[Slices];
			for (int i = 0; i < verts.Length; i++)
			{
				float pi2slices = Mathf.PI * 2f / (float)slices;
				float ca = Mathf.Cos(pi2slices * i) * r;
				float sa = Mathf.Sin(pi2slices * i) * r;
				
				verts[i] = new Vector3(ca, sa, 0f);
			}
			return verts;
		}
    }
}
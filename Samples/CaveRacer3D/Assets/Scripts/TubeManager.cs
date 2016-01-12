using System.Collections.Generic;
using UnityEngine;

namespace Tube
{
    [RequireComponent(typeof(CoinGenerator))]    
	public class TubeManager : MonoBehaviour
	{
		public int SectionsBackward;
		public int SectionsForward;
				
		public float radius;
		public float stepLength;
		public float sectionLength;
		public float speed;

		public GameObject SectionPrefab;

		private float t;
		private List<TubeSection> sections = new List<TubeSection>();

        private CoinGenerator coinGenerator;
		
		private float LocalT
		{
			get
			{
				return t - (int)t;
			}
		}

		public void Start()
		{
            coinGenerator = GetComponent<CoinGenerator>();

			for (int i = 0; i < SectionsBackward + SectionsForward; i++) {
				if(i > SectionsBackward)
					AddSection (false); 
				else
					AddSection (true); 
			}
			t = SectionsBackward;
		}

		public void Restart() {
			foreach(var s in sections) Destroy(s.gameObject);
			sections.Clear ();
            coinGenerator.Reset();
			Start ();
		}

		public void Update() {
			Move(Time.deltaTime * speed);
		}

		private BezierCurve getCurve()
		{
			int pos = (int)t + SectionsBackward;
			if (pos < -SectionsBackward && pos >= sections.Count)
				throw new System.ArgumentOutOfRangeException("t");
			
			return sections[pos].Curve;
		}

		private Vector3 randPoint(float l, float angle) {
			Vector2 p = Random.insideUnitCircle * Mathf.Sin(angle);
			return l * (Vector3.forward + new Vector3(p.x, p.y, 0f));
		}

		private void AddSection(bool needMakeObjects) {
			Vector3 v1, v2, v3;
			v3 = randPoint (sectionLength, 3.14f / 6f);
			float tc = 0f;
            float objStart = 0f;
			
			if(sections.Count == 0) {
				v1 = Vector3.zero;
				v2 = randPoint (sectionLength * 0.5f, 3.14f/ 3f);
			}
			else
			{
				var lastCurve = sections[sections.Count - 1].Curve;
				tc = sections[sections.Count - 1].TexEndValue;
                objStart = sections[sections.Count - 1].ObjectGenerationStep; 
				
				v1 = lastCurve.V3;
				
				Vector3 t = (lastCurve.V3 - lastCurve.V2);
				t.Normalize();
				
				v2 = v1 + t * sectionLength * 0.5f;
				Vector3 dir = lastCurve.GetDirection(1f);
				
				Quaternion tr = Quaternion.LookRotation(dir);
				v3 = v1 + tr * v3;
			}

			GameObject s = (GameObject)Instantiate (SectionPrefab, Vector3.zero, Quaternion.identity);
            s.transform.parent = this.transform;			
            TubeSection section = (TubeSection)s.GetComponent<TubeSection>();
			section.Curve = new BezierCurve (v1, v2, v3);
			sections.Add (section);
            section.initGeometry(radius, stepLength, tc, objStart);

            if (needMakeObjects)
            {
                section.makeObjects();
            }
			section.makeDust ();
		}
		
		public void Move(float length)
		{
			BezierCurve cur = getCurve();
			float lt = LocalT;
			
			float dt = cur.Move(length, lt);
			
			if ((int)(t + dt) > (int)t)
			{
				float dl = cur.BeforeEnd(lt);
				t = (int)t + 1;
				Move(length - dl);
			}
			else t += dt;
			
			if (t > 1f)
			{
				t -= 1f;
				Destroy(sections[0].gameObject);
				sections.RemoveAt(0);
				AddSection(true);
			}
		}
		
		public Vector3 GetPoint()
		{
			return getCurve().GetPoint(LocalT);
		}

		public Vector3 GetDirection()
		{
			return getCurve().GetDirection(LocalT);
		}
	}
}



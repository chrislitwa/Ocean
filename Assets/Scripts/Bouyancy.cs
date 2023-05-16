using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Bouyancy : MonoBehaviour
{
	[SerializeField]
	protected WaterSurface oceanSurface;

	[SerializeField]
	protected Rigidbody hullRigidbody;

	[SerializeField]
	protected int bouyCount;

	[SerializeField]
	protected float submergeDepth;

	[SerializeField]
	protected float displacementPercent;

	[SerializeField]
	protected float waterDrag;

	[SerializeField]
	protected float waterAngularDrag;

	WaterSearchParameters Search;
	WaterSearchResult SearchResult;

	private void FixedUpdate()
	{
		hullRigidbody.AddForceAtPosition(Physics.gravity / bouyCount, transform.position, ForceMode.Acceleration);
		Search.startPosition = transform.position;
		oceanSurface.FindWaterSurfaceHeight(Search, out SearchResult);

		if (transform.position.y < SearchResult.height)
		{
			float displacementHeight = Mathf.Clamp01(SearchResult.height - transform.position.y / submergeDepth) * displacementPercent;
			hullRigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementHeight, 0f), transform.position, ForceMode.Acceleration);
			hullRigidbody.AddForce(displacementHeight * -hullRigidbody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
			hullRigidbody.AddTorque(displacementHeight * -hullRigidbody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
		}
	}
}
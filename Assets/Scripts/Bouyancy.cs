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

	[SerializeField]
	protected float smoothFactor;

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

				// Smoothly interpolate the forces
				Vector3 targetForce = new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementHeight, 0f);
				Vector3 smoothedForce = Vector3.Lerp(Vector3.zero, targetForce, Time.fixedDeltaTime * smoothFactor);
				hullRigidbody.AddForceAtPosition(smoothedForce, transform.position, ForceMode.Acceleration);

				Vector3 velocityForce = displacementHeight * -hullRigidbody.velocity * waterDrag * Time.fixedDeltaTime;
				hullRigidbody.AddForce(velocityForce, ForceMode.VelocityChange);

				Vector3 angularVelocityForce = displacementHeight * -hullRigidbody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime;
				hullRigidbody.AddTorque(angularVelocityForce, ForceMode.VelocityChange);
		}
	}
}
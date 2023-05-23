using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Bouyancy1 : MonoBehaviour
{
    [SerializeField]
    protected WaterSurface oceanSurface;

    [SerializeField]
    protected Transform rootObject;

    [SerializeField]
    protected Transform bow;

    [SerializeField]
    protected Transform stern;

    [SerializeField]
    protected Transform port;

    [SerializeField]
    protected Transform starboard;

    [SerializeField]
    protected float smoothFactor;

    WaterSearchParameters Search;
    WaterSearchResult bowHeight;
    WaterSearchResult sternHeight;
    WaterSearchResult portHeight;
    WaterSearchResult starboardHeight;

    private Vector2 targetValues;

    private void Start()
    {
        targetValues = Vector2.zero;
    }

    private void FixedUpdate()
    {
        Search.startPosition = bow.position;
        oceanSurface.FindWaterSurfaceHeight(Search, out bowHeight);

        Search.startPosition = stern.position;
        oceanSurface.FindWaterSurfaceHeight(Search, out sternHeight);

        Search.startPosition = port.position;
        oceanSurface.FindWaterSurfaceHeight(Search, out portHeight);

        Search.startPosition = starboard.position;
        oceanSurface.FindWaterSurfaceHeight(Search, out starboardHeight);

        float targetXAxis = bowHeight.height - sternHeight.height;
        float targetZAxis = portHeight.height - starboardHeight.height;

        // Slowly interpolate towards the target values
        targetValues = Vector2.Lerp(targetValues, new Vector2(targetXAxis, targetZAxis), Time.fixedDeltaTime * smoothFactor);

        // Clamp the target values to prevent reaching 1 or -1
        float clampedXAxis = Mathf.Clamp(targetValues.x, -0.99f, 0.99f);
        float clampedZAxis = Mathf.Clamp(targetValues.y, -0.99f, 0.99f);

        // Calculate the rotation angles based on the clamped values
        float rotationX = clampedXAxis * 90f;
        float rotationZ = clampedZAxis * 90f;

        // Apply the rotation to the root object
        rootObject.rotation = Quaternion.Euler(rotationX, 0f, rotationZ);
    }
}

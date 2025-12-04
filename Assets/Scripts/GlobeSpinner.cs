using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Rotates a Rigidbody based on the positional movement of an interactor (hand) 
/// relative to the object's center. Simulates a "Trackball" or "Surface Drag".
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class PhysicsGlobeSpinner : MonoBehaviour
{
    public enum RotationMode { Free, Constrained }

    [Header("Rotation Settings")]
    [Tooltip("Free: Rotate in any direction (Trackball).\nConstrained: Rotate only around the specified axis.")]
    public RotationMode rotationMode = RotationMode.Free;

    [Tooltip("The axis to rotate around. Only used if Rotation Mode is Constrained.")]
    public Vector3 rotationAxis = Vector3.up;
    
    [Tooltip("Multiplier for the rotation speed. 1.0 is 1:1 tracking.")]
    public float sensitivity = 1.0f;

    [Tooltip("Smooths out the input noise.")]
    public float smoothing = 0.1f;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private IXRSelectInteractor currentInteractor;
    private Vector3 previousDirection;
    private bool isGrabbing = false;
    private float grabRadius;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        
        // Disable standard tracking so our script can take full control
        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        currentInteractor = args.interactorObject;
        isGrabbing = true;
        
        // Kill existing rotation when grabbed to prevent fighting
        rb.angularVelocity = Vector3.zero;

        // Calculate initial grab parameters
        Vector3 center = transform.position;
        Vector3 grabPos = GetInteractorHitPoint(currentInteractor, true);
        
        previousDirection = grabPos - center;
        grabRadius = previousDirection.magnitude;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbing = false;
        currentInteractor = null;
    }

    void FixedUpdate()
    {
        if (isGrabbing && currentInteractor != null)
        {
            PerformRotation();
        }
    }

    private Vector3 GetInteractorHitPoint(IXRSelectInteractor interactor, bool isInitiating = false)
    {
        // If it's a Ray Interactor, we need to calculate the hit point on the sphere
        if (interactor is XRRayInteractor rayInteractor)
        {
            // On initiation, try to get the actual hit point
            if (isInitiating && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                return hit.point;
            }
            
            // During drag, intersect ray with sphere of grabRadius
            Transform rayOrigin = rayInteractor.transform;
            Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
            
            Vector3 center = transform.position;
            Vector3 L = ray.origin - center;
            float a = 1.0f; 
            float b = 2.0f * Vector3.Dot(L, ray.direction);
            float c = Vector3.Dot(L, L) - (grabRadius * grabRadius);
            
            float discriminant = b*b - 4*a*c;
            
            if (discriminant >= 0)
            {
                float t = (-b - Mathf.Sqrt(discriminant)) / (2.0f * a);
                if (t < 0) t = (-b + Mathf.Sqrt(discriminant)) / (2.0f * a);
                
                if (t >= 0)
                {
                    return ray.GetPoint(t);
                }
            }
            
            // Fallback: Closest point on sphere to ray
            float t_closest = -Vector3.Dot(L, ray.direction);
            Vector3 closestPointOnRay = ray.GetPoint(t_closest);
            Vector3 dirFromCenter = (closestPointOnRay - center).normalized;
            return center + dirFromCenter * grabRadius;
        }
        
        // Default (Direct Interactor)
        return interactor.transform.position;
    }

    private void PerformRotation()
    {
        Vector3 center = transform.position;
        Vector3 currentPos = GetInteractorHitPoint(currentInteractor);
        Vector3 currentDirection = currentPos - center;

        Vector3 dirA = previousDirection.normalized;
        Vector3 dirB = currentDirection.normalized;

        if (rotationMode == RotationMode.Constrained)
        {
            dirA = Vector3.ProjectOnPlane(dirA, rotationAxis).normalized;
            dirB = Vector3.ProjectOnPlane(dirB, rotationAxis).normalized;
        }

        Vector3 cross = Vector3.Cross(dirA, dirB);
        float angle = Vector3.Angle(dirA, dirB);

        if (angle > 0.001f)
        {
            Vector3 axis = cross.normalized;
            
            if (rotationMode == RotationMode.Constrained)
            {
                axis = rotationAxis;
                float sign = Mathf.Sign(Vector3.Dot(cross, rotationAxis));
                angle *= sign;
            }

            float targetAngularVelocityRad = (angle * Mathf.Deg2Rad) / Time.fixedDeltaTime;
            Vector3 targetTorque = axis * targetAngularVelocityRad * sensitivity;

            if (!float.IsNaN(targetTorque.x))
            {
                rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, targetTorque, 1f - smoothing);
            }
        }
        else
        {
             rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, 1f - smoothing);
        }

        previousDirection = currentDirection;
    }
}

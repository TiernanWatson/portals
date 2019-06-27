using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform connectingPortal;
    [SerializeField] private Camera captureCamera;

    private bool canCross = false;
    
    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += PlaceCaptureCamera;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= PlaceCaptureCamera;
    }

    private void PlaceCaptureCamera(ScriptableRenderContext context, Camera cam)
    {
        if (cam.CompareTag("CaptureCamera"))
            return;

        // Get main camera's position relative to portal then apply to other portal
        Vector3 mainLocalPosition = transform.InverseTransformPoint(cam.transform.position);
        mainLocalPosition.Scale(new Vector3(-1f, 1f, -1f));  // Camera needs to face other way
        captureCamera.transform.position = connectingPortal.TransformPoint(mainLocalPosition);

        // Get main camera's rotation relative to portal then apply to other portal
        Vector3 mainLocalRotation = transform.InverseTransformDirection(cam.transform.forward);
        mainLocalRotation.Scale(new Vector3(-1f, 1f, -1f));
        captureCamera.transform.forward = connectingPortal.TransformDirection(mainLocalRotation);

        // Keep any FOV settings
        captureCamera.projectionMatrix = cam.projectionMatrix;

        // Don't capture anything in the way of the camera
        captureCamera.nearClipPlane = Vector3.Magnitude(mainLocalPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Stops player walking backwards through portals
        canCross = !HasCrossedPortal(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (canCross && HasCrossedPortal(other))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            player.SetCharControlActive(false); // Stops CharController overriding change
            TeleportObject(other.transform, connectingPortal);
            player.SetCharControlActive(true);

            canCross = false;
        }
    }

    private bool HasCrossedPortal(Collider other)
    {
        Vector3 newDirection = (transform.position - other.transform.position).normalized;

        float newDot = Vector3.Dot(newDirection, transform.forward);

        return newDot < 0f;
    }

    public void TeleportObject(Transform obj, Transform otherPortal)
    {
        Vector3 newLocation = TransferLocation(obj.position, transform, otherPortal);

        obj.position = newLocation;
    }

    public static Vector3 TransferLocation(Vector3 point, Transform A, Transform B)
    {
        Vector3 relativePoint = A.InverseTransformPoint(point);

        return B.TransformPoint(relativePoint);
    }
}

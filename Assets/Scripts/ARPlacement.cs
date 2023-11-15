using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{
    public GameObject arObjectToSpawn;
    public GameObject placementIndicator;
    private GameObject spawnedObject;
    private Pose PlacementPose;
    private ARRaycastManager arRaycastManager;
    private bool placementPoseIsValid = false;
    
    private float initialDistance;
    private Vector3 initialScale;

    //private Shoot shoot;
    // public Image Aim;
    public GameObject joystickCanvas;
    public GameObject pointSpawner;

    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        //shoot = GetComponent<Shoot>();
        // ERROR shoot.gameObject.SetActive(false);
        //shoot.enabled = false;
        //Aim.enabled = false;
        joystickCanvas.SetActive(true);
        pointSpawner.SetActive(true);
    }

    void Update()
    {
        if (spawnedObject == null 
            && placementPoseIsValid 
            && Input.touchCount > 0 
            && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ARPlaceObject();
            joystickCanvas.SetActive(true);
            //shoot.enabled = true;
            //Aim.enabled = true;
        }

        // if (spawnedObject == null && shoot.enabled == true)
        // {
        //     shoot.enabled = false;
        //     Aim.enabled = false;
        // }

        ApplyingFingerScale();
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    void ApplyingFingerScale()
    {
        if (Input.touchCount != 2) 
        {
            // touchZero  
            // touchOne 
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // touchZero || touchOne  Canceled || Ended
            // return


            if (touchZero.phase == TouchPhase.Canceled 
                || touchZero.phase == TouchPhase.Ended
                || touchOne.phase == TouchPhase.Canceled
                || touchOne.phase == TouchPhase.Ended
             )
            {
                return;
            }

            // touchZero || touchOne   started touch
            if(touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                //Vector2 kek = touchZero.position - touchOne.position;
                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);//kek.magnitude; // distance between touchZero and touchOne;
                initialScale = spawnedObject.transform.localScale;//localScale spawned Spider;
            }
            else
            {
                float currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                if (Mathf.Approximately(initialDistance, 0))
                {
                    return;
                }

                float factor = currentDistance / initialDistance;
                spawnedObject.transform.localScale = initialScale * factor;
            }
        }
    }

    void UpdatePlacementIndicator()
    {
        if (spawnedObject == null && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);

            //placementIndicator.transform.position = PlacementPose.position;
            //placementIndicator.transform.rotation = PlacementPose.rotation;

            placementIndicator.transform.SetPositionAndRotation(
                PlacementPose.position, 
                PlacementPose.rotation
            );
        } 
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        
        var hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    void ARPlaceObject()
    {
        spawnedObject = Instantiate(arObjectToSpawn, PlacementPose.position, PlacementPose.rotation);
    }
}

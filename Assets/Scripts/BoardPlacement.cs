using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BoardPlacement : MonoBehaviour
{
    [Header("Prefab Configuration")]
    [SerializeField] private GameObject gameBoardPrefab;
    
    private ARRaycastManager raycastManager;
    private GameObject spawnedGameBoard;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public GameObject SpawnedGameBoard => spawnedGameBoard;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        if (Touch.activeTouches.Count == 0)
            return;

        Touch touch = Touch.activeTouches[0];

        if (touch.phase != UnityEngine.InputSystem.TouchPhase.Began)
            return;

        if (raycastManager.Raycast(touch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (spawnedGameBoard == null)
            {
                spawnedGameBoard = Instantiate(gameBoardPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                spawnedGameBoard.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            }
        }
    }
}
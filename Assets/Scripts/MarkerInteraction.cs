using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MarkerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private BoardPlacement boardPlacement;
    [SerializeField] private TargetManager targetManager;
    
    [Header("Interaction Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float detectionDistance = 0.5f;
    [SerializeField] private float bulletHeight = 0.08f;
    [SerializeField] private bool enableRotation = false;
    
    [Header("Color Change Settings")]
    [SerializeField] private Color defaultColor = Color.green;
    [SerializeField] private Color activeColor = Color.blue;
    
    private GameObject spawnedBullet;
    private ARTrackedImage currentTrackedImage;
    private Material gameBoardMaterial;
    private bool isMarkerNearGameBoard = false;
    private bool targetsSpawned = false;

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            currentTrackedImage = trackedImage;
            Debug.Log($"Image detected: {trackedImage.referenceImage.name}");
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            currentTrackedImage = trackedImage;
            
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                CheckMarkerProximityAndUpdate(trackedImage);
            }
            else
            {
                DeactivateInteraction();
            }
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            if (currentTrackedImage == trackedImage)
            {
                DeactivateInteraction();
                currentTrackedImage = null;
            }
        }
    }

    void CheckMarkerProximityAndUpdate(ARTrackedImage trackedImage)
    {
        GameObject gameBoard = boardPlacement.SpawnedGameBoard;
        if (gameBoard == null) return;

        float distance = Vector3.Distance(trackedImage.transform.position, gameBoard.transform.position);

        if (distance < detectionDistance)
        {
            if (!isMarkerNearGameBoard)
            {
                ActivateInteraction(gameBoard);
            }
            
            UpdateBulletPosition(trackedImage, gameBoard);
        }
        else
        {
            if (isMarkerNearGameBoard)
            {
                DeactivateInteraction();
            }
        }
    }

    void ActivateInteraction(GameObject gameBoard)
    {
        isMarkerNearGameBoard = true;
        Debug.Log("🎯 Game started! Marker entered GameBoard area!");

        ChangeGameBoardColor(gameBoard, activeColor);

        if (spawnedBullet == null && bulletPrefab != null)
        {
            Vector3 spawnPosition = gameBoard.transform.position + Vector3.up * bulletHeight;
            spawnedBullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            spawnedBullet.transform.SetParent(gameBoard.transform);
            Debug.Log("Bullet spawned!");
        }
        
        if (!targetsSpawned && targetManager != null)
        {
            targetManager.SpawnTargets(gameBoard);
            targetsSpawned = true;
        }
    }

    void DeactivateInteraction()
    {
        if (!isMarkerNearGameBoard) return;
        
        isMarkerNearGameBoard = false;
        Debug.Log("Marker left GameBoard area!");

        GameObject gameBoard = boardPlacement.SpawnedGameBoard;
        
        if (gameBoard != null)
        {
            ChangeGameBoardColor(gameBoard, defaultColor);
        }

        if (spawnedBullet != null)
        {
            Destroy(spawnedBullet);
            spawnedBullet = null;
            Debug.Log("Bullet destroyed!");
        }
        
        if (targetManager != null)
        {
            targetManager.ClearTargets();
            targetsSpawned = false;
        }
    }

    void UpdateBulletPosition(ARTrackedImage trackedImage, GameObject gameBoard)
    {
        if (spawnedBullet == null) return;

        Vector3 markerPosition = trackedImage.transform.position;
        Vector3 gameBoardPosition = gameBoard.transform.position;
        
        Vector3 targetPosition = new Vector3(
            markerPosition.x,
            gameBoardPosition.y + bulletHeight,
            markerPosition.z
        );

        spawnedBullet.transform.position = Vector3.Lerp(
            spawnedBullet.transform.position,
            targetPosition,
            Time.deltaTime * 12f
        );

        if (enableRotation)
        {
            spawnedBullet.transform.rotation = Quaternion.Lerp(
                spawnedBullet.transform.rotation,
                trackedImage.transform.rotation,
                Time.deltaTime * 10f
            );
        }
        
        if (targetManager != null)
        {
            targetManager.CheckCollision(spawnedBullet);
        }
    }

    void ChangeGameBoardColor(GameObject gameBoard, Color newColor)
    {
        MeshRenderer renderer = gameBoard.GetComponent<MeshRenderer>();
        
        if (renderer != null && renderer.material != null)
        {
            if (gameBoardMaterial == null)
            {
                gameBoardMaterial = renderer.material;
            }
            
            Color colorWithAlpha = newColor;
            colorWithAlpha.a = gameBoardMaterial.color.a;
            gameBoardMaterial.color = colorWithAlpha;
        }
    }
}
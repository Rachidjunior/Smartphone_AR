using UnityEngine;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private int numberOfTargets = 3;
    [SerializeField] private float spawnRadius = 0.15f;
    [SerializeField] private float hitDistance = 0.06f;
    
    private List<GameObject> activeTargets = new List<GameObject>();
    private int score = 0;
    private GameObject currentGameBoard;

    public void SpawnTargets(GameObject gameBoard)
    {
        if (gameBoard == null) return;
        
        currentGameBoard = gameBoard;
        ClearTargets();
        
        for (int i = 0; i < numberOfTargets; i++)
        {
            Vector3 randomPos = GetRandomPositionOnBoard(gameBoard);
            GameObject target = Instantiate(targetPrefab, randomPos, Quaternion.identity);
            target.transform.SetParent(gameBoard.transform);
            activeTargets.Add(target);
        }
        
        Debug.Log($"Spawned {numberOfTargets} targets!");
    }

    Vector3 GetRandomPositionOnBoard(GameObject gameBoard)
    {
        Vector3 boardPos = gameBoard.transform.position;
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);
        
        return new Vector3(
            boardPos.x + randomX,
            boardPos.y + 0.06f,
            boardPos.z + randomZ
        );
    }

    public void CheckCollision(GameObject bullet)
    {
        if (bullet == null) return;
        
        foreach (GameObject target in activeTargets.ToArray())
        {
            if (target == null) continue;
            
            float distance = Vector3.Distance(bullet.transform.position, target.transform.position);
            
            if (distance < hitDistance)
            {
                DestroyTarget(target);
            }
        }
    }

    void DestroyTarget(GameObject target)
    {
        activeTargets.Remove(target);
        Destroy(target);
        score++;
        
        Debug.Log($"🎯 Target hit! Score: {score}/{numberOfTargets}");
        
        if (activeTargets.Count == 0)
        {
            Debug.Log("🎉 VICTORY! All targets destroyed!");
            RespawnTargets();
        }
    }

    void RespawnTargets()
    {
        if (currentGameBoard != null)
        {
            Invoke(nameof(DelayedRespawn), 1f);
        }
    }

    void DelayedRespawn()
    {
        if (currentGameBoard != null)
        {
            SpawnTargets(currentGameBoard);
        }
    }

    public void ClearTargets()
    {
        foreach (GameObject target in activeTargets)
        {
            if (target != null) Destroy(target);
        }
        activeTargets.Clear();
        score = 0;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetRemainingTargets()
    {
        return activeTargets.Count;
    }
}
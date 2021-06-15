using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    #region Level Parameters
    [Header("Used agent prefab")]
    [SerializeField] private GameObject agentPrefab;
    [Header("Board")]
    [SerializeField] private GameObject boardOnScene;
    [Header("New agent appearence time setup")]
    [Range(2, 10)] [SerializeField] private float newAgentMinEmergeneTime; 
    [Range(2, 10)][SerializeField] private float newAgentMaxEmergenceTime; 
    [Header("Maximum number of agents on level")]
    [Range(1, 30)] [SerializeField] private int maxAmmountofAgentsOnLevel;

    //Board size
    private float boardSizeX;
    private float boardSizeZ;

    //Spawn another agent
    private float timeToSpawnNewAgent;
    #endregion

    #region LevelController Instance Setup
    public static LevelController instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region OnValidation check if the values are set correctly
    private void OnValidate()
    {
        if (newAgentMaxEmergenceTime < newAgentMinEmergeneTime)
        {
            Debug.LogException(new System.Exception($"newAgentMaxEmergenceTime variable [value: {newAgentMaxEmergenceTime}] must have lower value then newAgentMaxEmergenceTime [value: {newAgentMaxEmergenceTime}]!"));
            return;
        }
    }
    #endregion

    private void Start()
    {
        DetermineBoardSize();
        timeToSpawnNewAgent = Random.Range(newAgentMinEmergeneTime, newAgentMaxEmergenceTime);
    }

    private void DetermineBoardSize()
    {
        boardSizeX = boardOnScene.transform.position.x;
        boardSizeZ = boardOnScene.transform.position.z;
    }

    public float[] ReturnBoardSize()
    {
        float[] localList = new float[2];
        localList[0] = boardSizeX;
        localList[1] = boardSizeZ;
        return localList;
    }

    private void Update()
    {
        if (timeToSpawnNewAgent > 0)
        {
            timeToSpawnNewAgent -= Time.deltaTime;
        }
        else
        {
            SpawnNewAgent();
        }
    }

    private void SpawnNewAgent()
    {
        throw new System.NotImplementedException();
    }
}

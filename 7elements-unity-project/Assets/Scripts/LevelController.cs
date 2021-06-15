using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    #region Level Parameters
    [Header("Used agent prefab")]
    [SerializeField] private GameObject agentPrefab;
    [Header("New agent appearence time setup")]
    [Range(2, 10)] [SerializeField] private float newAgentMinEmergeneTime; 
    [Range(2, 10)][SerializeField] private float newAgentMaxEmergenceTime; 
    [Header("Maximum number of agents on level")]
    [Range(1, 30)] [SerializeField] private int maxAmmountofAgentsOnLevel;
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
}

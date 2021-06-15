using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    #region Level Parameters

    private GameObject lastClickedAgent;

    [Header("Used agent prefab")]
    [SerializeField] private GameObject agentPrefab;
    [Header("Board")]
    [SerializeField] private GameObject boardOnScene;
    [Header("UI Fields")]
    [SerializeField] private GameObject numbersGenerationTable;
    [SerializeField] private Text generationTableText;
    [SerializeField] private Text agentNameText;
    [SerializeField] private Text agentHealthText;
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

    //Current ammount of agents on scene
    private int ammountOfAgentsOnScene = 0;

    //Bool for checking if numbers were generated
    private bool numbersWereGenerated = false;
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
        boardSizeX = boardOnScene.transform.localScale.x;
        boardSizeZ = boardOnScene.transform.localScale.z;
    }

    public float[] ReturnBoardSize()
    {
        float[] localList = new float[2];
        localList[0] = boardSizeX/2;
        localList[1] = boardSizeZ/2;
        return localList;
    }

    private void Update()
    {
        if (maxAmmountofAgentsOnLevel > ammountOfAgentsOnScene)
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CheckIfAgentClicked();
        }
    }

    private void SpawnNewAgent()
    {
        GameObject clone = Instantiate(agentPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        ammountOfAgentsOnScene += 1;
        clone.name = $"Agent_{ammountOfAgentsOnScene}";
        timeToSpawnNewAgent = Random.Range(newAgentMinEmergeneTime, newAgentMaxEmergenceTime);
    }

    private void CheckIfAgentClicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float maxRaycastDistance = 1000f;
        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
        {
            if (hit.collider.tag == "LevelAgent")
            {
                if (lastClickedAgent)
                {
                    lastClickedAgent.GetComponent<AgentController>().iWasClicked = false;
                }
                agentNameText.text = $"Agent Name:\n{hit.collider.gameObject.name}";
                agentHealthText.text = $"Agent Health:\n{hit.collider.gameObject.GetComponent<AgentController>().GetAgentHealth()}";
                lastClickedAgent = hit.collider.gameObject;
                lastClickedAgent.GetComponent<AgentController>().iWasClicked = true;
            }
        }
        else
        {
            agentNameText.text = "Agent Name:";
            agentHealthText.text = "Agent Health:";
            if (lastClickedAgent && lastClickedAgent.GetComponent<AgentController>().iWasClicked)
            {
                lastClickedAgent.GetComponent<AgentController>().iWasClicked = false;
            }
        }
    }

    public void CloseTextBox()
    {
        numbersGenerationTable.SetActive(false);
    }

    public void GenerateNumbers()
    {
        numbersGenerationTable.SetActive(true);
        if (!numbersWereGenerated)
        {
            string newText = "";
            for (int i = 1; i <= 100; i++)
            {
                int[] numberArray = GetArrayOfDigits(i);
                int internalScore = 0;
                foreach (int charNumber in numberArray)
                {
                    internalScore += charNumber;
                }
                if (i == 1)
                {
                    newText += "1";
                }
                else if (internalScore % 3 == 0 && (numberArray[numberArray.Length - 1] == 0 || numberArray[numberArray.Length - 1] == 5))
                {
                    newText += "\nMarkoPolo";
                }
                else if (internalScore % 3 == 0)
                {
                    newText += "\nMarko";
                }
                else if (numberArray[numberArray.Length - 1] == 0 || numberArray[numberArray.Length - 1] == 5)
                {
                    newText += "\nPolo";
                }
                else
                {
                    newText += $"\n{i}";
                }
            }
            generationTableText.text = newText;
            numbersWereGenerated = true;
        }
    }

    private static int[] GetArrayOfDigits(int n)
    {
        if (n == 0) return new int[1] { 0 };

        var digits = new List<int>();

        for (; n != 0; n /= 10)
            digits.Add(n % 10);

        var arr = digits.ToArray();
        System.Array.Reverse(arr);
        return arr;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentController : MonoBehaviour
{
    #region Agent Parameters & Setup
    private NavMeshAgent thisNavMeshAgent;
    private Color agentColor;
    [SerializeField] private Renderer agentRenderer;
    [Header("Agent Health & Setup")]
    [SerializeField] private float destinationTolerance = 1.1f;
    [SerializeField] private int agentHealth = 3;

    private float oneSecondUpdateTimer = 1f;
    private bool changedStartingPosition = false;
    public bool iWasClicked = false;
    private bool iWasHit = false;
    #endregion

    private void Awake()
    {
        thisNavMeshAgent = GetComponent<NavMeshAgent>();
        Color[] colors = {
        Color.blue,
        Color.black,
        Color.gray,
        Color.magenta,
        Color.cyan,
        Color.yellow};
        int randomColor = Random.Range(0, colors.Length);
        agentColor = colors[randomColor];
        agentRenderer = gameObject.GetComponent<Renderer>();
        agentRenderer.material.color = agentColor;
    }

    private void Start()
    {
        this.gameObject.tag = "LevelAgent";

    }
    private void Update()
    {
        
        if (!changedStartingPosition)
        {
            thisNavMeshAgent.destination = FindNewDestination(new Vector3(0, transform.position.y, 0));
            changedStartingPosition = true;
        }
        else 
        {
            if (oneSecondUpdateTimer > 0)
                oneSecondUpdateTimer -= Time.deltaTime;
            else
            {
                OneSecondUpdate();
            }
        }
        if (iWasClicked)
        {
            agentRenderer.material.color = Color.green;
        }
        else if (iWasHit)
        {
            agentRenderer.material.color = Color.red;
        }
        else if(agentRenderer.material.color != agentColor)
        {
            agentRenderer.material.color = agentColor;
        }
    }

    /// <summary>
    /// Update method that executes every second
    /// </summary>
    private void OneSecondUpdate()
    {
        if (thisNavMeshAgent.remainingDistance <= destinationTolerance || thisNavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            Vector3 previousDestination = thisNavMeshAgent.destination;
            thisNavMeshAgent.destination = FindNewDestination(previousDestination);
        }
        oneSecondUpdateTimer = 1f;
    }

    /// <summary>
    /// A method that finds a new Vector3 position for an agent
    /// </summary>
    /// <param name="oldPosition">Previous agent position</param>
    /// <returns></returns>
    private Vector3 FindNewDestination(Vector3 oldPosition)
    {
        float[] board_size = LevelController.instance.ReturnBoardSize();
        float offsetFromBoundaries = 0.5f;
        Vector3 newPosition = new Vector3(
            Random.Range(-(board_size[0]) + offsetFromBoundaries, board_size[0] - offsetFromBoundaries),
            0,
            Random.Range(-(board_size[1]) + offsetFromBoundaries, board_size[1] - offsetFromBoundaries)
            );
        while (oldPosition == newPosition)
        {
            newPosition = new Vector3(
            Random.Range(-(board_size[0]) + offsetFromBoundaries, board_size[0] - offsetFromBoundaries),
            0,
            Random.Range(-(board_size[1]) + offsetFromBoundaries, board_size[1] - offsetFromBoundaries)
            );
        }
        //Debug.Log($"New position for {gameObject.name} is {newPosition}");
        return newPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("LevelAgent"))
        {
            LoseHealth();
            //Debug.Log($"{gameObject.name} lost one health point.\nCurrent health: {agentHealth}");
            iWasHit = true;
            if (LevelController.instance.lastClickedAgent == this.gameObject)
            {
                LevelController.instance.ReloadAgentUI(gameObject.name, GetAgentHealth().ToString());
            }
            StartCoroutine(ChangeToNormalColor());
        }
    }

    /// <summary>
    /// A method that takes 1 HP point from the agent's "agentHealth" variable
    /// </summary>
    private void LoseHealth()
    {
        agentHealth -= 1;
    }

    /// <summary>
    /// A method that returns the "agentHealth" value
    /// </summary>
    /// <returns></returns>
    public int GetAgentHealth()
    {
        return agentHealth;
    }
    /// <summary>
    /// Method to change agent "iWasHit" status back to normal (false) after 3 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeToNormalColor()
    {
        yield return new WaitForSeconds(3);
        iWasHit = false;
    }


}

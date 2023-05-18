using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class MazeGuide : MonoBehaviour
{
    [SerializeField] private List<GameObject> availableBlocks;
    [SerializeField] private GameObject guideObject;
    [SerializeField] private GameObject buyGuidePanel;
    [SerializeField] private GameObject hintLimitAlert;
    [SerializeField] private TextMeshProUGUI hintLimit;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private int hintLimitCount;
    public int hintBoughtCount;
    private Transform currentDestination;
    private NavMeshAgent agent;
    public static MazeGuide Instance;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            ShowBuyGuidePanel();
        }
    }

    private void ShowBuyGuidePanel() {
        buyGuidePanel.SetActive(true);
        hintLimit.text = hintBoughtCount + " / " + hintLimitCount;
    }

    public void BuyGuideHint() {
        if (hintBoughtCount < hintLimitCount) {
            hintBoughtCount++;
            CalculateNearestBlock();
            ShowGuide();
            buyGuidePanel.SetActive(false);

            // Add deduction of money
        }
        else {
            hintLimitAlert.SetActive(true);
        }
    }

    public void RemoveBlock(GameObject block) {
        availableBlocks.Remove(block);
    }

    // Show the guide after buying a hint
    private void ShowGuide() {
        guideObject.SetActive(true);
        guideObject.transform.position = playerPosition.position;

        agent = guideObject.GetComponent<NavMeshAgent>();
        agent.destination = currentDestination.position;

        StartCoroutine(DistanceReach());
    }

    // Execute function when the destination is reached
    private IEnumerator DistanceReach() {
        while(Vector3.Distance(guideObject.transform.position, currentDestination.position) > 1f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        
        guideObject.SetActive(false);
        guideObject.transform.position = playerPosition.position;
        yield return new WaitForSeconds(.5f);

        if (Vector3.Distance(currentDestination.position, playerPosition.position) > .8f) {
            ShowGuide();
        }
    }

    // Calculate the blocks based on the availableBlocks List
    private void CalculateNearestBlock()
    {
        float nearestDistance = float.MaxValue;
        GameObject nearestBlock = null;

        foreach (GameObject block in availableBlocks)
        {
            // Calculate the straight-line distance from player to block
            float straightDistance = Vector3.Distance(block.transform.position, playerPosition.position);

            // Calculate the distance along the path from player to block
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(playerPosition.position, block.transform.position, NavMesh.AllAreas, path);
            float pathDistance = GetPathDistance(path);

            if (pathDistance < nearestDistance)
            {
                nearestDistance = pathDistance;
                nearestBlock = block;
            }
        }

        if (nearestBlock != null)
        {
            currentDestination = nearestBlock.transform;
        }
    }

    private float GetPathDistance(NavMeshPath path)
    {
        float distance = 0f;

        if (path.corners.Length < 2)
            return distance;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return distance;
    }

}
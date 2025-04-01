using UnityEngine;

public class MapleTree : MonoBehaviour
{
    public bool canTap = true;
    public float tapCooldown = 10.0f;

    //SapTimerUI Prefab
    public GameObject timerUIPrefab;

    private SapTimerUI timerUI;
    private Canvas mainCanvas;

    private void Start()
    {
        //Find the main canvas
        mainCanvas = GameObject.FindObjectOfType<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogError("No Canvas found in the scene!");
            return;
        }

        //Instantiate UI for the MapleTree
        GameObject timerUIObject = Instantiate(timerUIPrefab, mainCanvas.transform);
        timerUI = timerUIObject.GetComponent<SapTimerUI>();

        if (timerUI == null)
        {
            Debug.LogError("SapTimerUI component not found on prefab!");
            return;
        }

        //Initializes UI at the tree's position
        timerUI.Initialize(this.transform, canTap);

        //Start cooldown if tree is not ready
        if (!canTap)
        {
            timerUI.StartCooldown(tapCooldown, () => EndCooldown());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canTap)
        {
            Debug.Log("Harvesting Sap!");
            canTap = false;

            timerUI.SetCheckmarkVisibility(false);
            timerUI.StartCooldown(tapCooldown, () => EndCooldown());
        }
        else if (!canTap)
        {
            Debug.Log("Not ready to harvest!");
        }
    }

    private void EndCooldown()
    {
        canTap = true;
        timerUI.SetCheckmarkVisibility(true);
        Debug.Log("Ready to harvest!");
    }
}
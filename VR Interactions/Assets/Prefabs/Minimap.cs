using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Minimap : MonoBehaviour
{

    public float detectionDistance = 10f;

    [SerializeField] private GameObject VRPlayerRig;
    [SerializeField] private GameObject minimapCanvas;
    [SerializeField] private GameObject trackerPrefab;
    [SerializeField] private Camera currentCamera;

    private List<GameObject> targets = new List<GameObject>();
    private List<GameObject> trackers = new List<GameObject>();

    private float minimapCanvasHalfLength;
    private int lastTargetAmount;

    void Start()
    {
        detectionDistance = 5f;

        lastTargetAmount = 0;
        minimapCanvasHalfLength = minimapCanvas.GetComponent<RectTransform>().rect.width / 2;

        VRPlayerRig = GameObject.FindGameObjectWithTag("VRPlayer");
        currentCamera = VRPlayerRig.transform.Find("TrackingSpace").transform.Find("CenterEyeAnchor").GetComponent<Camera>();
    }

    void Update()
    {
        targets = allTargetsInRange(detectionDistance);

        if (targets.Count != lastTargetAmount)
        {
            updateTrackerAmount();
        }

        lastTargetAmount = targets.Count;

        if (trackers.Count > 0)
        {
            for (int i = 0; i < trackers.Count; i++)
            {
                Vector2 worldDifference = new Vector2(targets[i].transform.position.x - currentCamera.transform.position.x, targets[i].transform.position.z - currentCamera.transform.position.z);
                Vector2 screenDifference = worldDifference.normalized * (worldDifference.magnitude / detectionDistance) * minimapCanvasHalfLength;

                print(screenDifference);

                RectTransform trackerTransform = trackers[i].GetComponent<RectTransform>();

                trackerTransform.anchoredPosition = (Vector2) (currentCamera.transform.rotation * screenDifference);
            }
        }
    }

    List<GameObject> allTargetsInRange(float dist)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (GameObject val in GameObject.FindGameObjectsWithTag("Fire"))
        {
            Vector2 worldDifference = new Vector2(val.transform.position.x - currentCamera.transform.position.x, val.transform.position.z - currentCamera.transform.position.z);

            if (worldDifference.magnitude <= dist)
            {
                result.Add(val);
            }
        }

        return result;
    }
    void updateTrackerAmount()
    {
        if (trackers.Count > 0)
        {
            foreach (GameObject val in trackers)
            {
                Destroy(val);
            }
            trackers.Clear();
        }


        foreach (GameObject enemy in targets)
        {
            GameObject marker = Instantiate(trackerPrefab, minimapCanvas.transform);
            marker.gameObject.SetActive(true);
            trackers.Add(marker);
        }
    }
}

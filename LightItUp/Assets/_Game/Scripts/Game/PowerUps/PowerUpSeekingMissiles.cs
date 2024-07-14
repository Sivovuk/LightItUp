using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LightItUp.Game;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpSeekingMissiles : MonoBehaviour
{
    public GameObject missilePrefab;
    public int numberOfMissiles = 3;
    public Button launchMissiles;

    private List<GameObject> seekingMissilesPool = new List<GameObject>();

    void Start()
    {
        SpawnMissilePool();
        PowerUpController.Instance.OnSceneCleanup += OnSceneCleanup;
    }

    private void OnEnable() 
    {
        launchMissiles.onClick.AddListener(delegate{ActivateSeekingMissiles();});
    }

    private void OnDisable() 
    {
        launchMissiles.onClick.RemoveListener(delegate{ActivateSeekingMissiles();});
    }

    private void OnDestroy() 
    {
        PowerUpController.Instance.OnSceneCleanup -= OnSceneCleanup;
    }
    
    private void SpawnMissilePool()
    {
        for (int i = 0; i < numberOfMissiles; i++)
        {
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            missile.SetActive(false);
            seekingMissilesPool.Add(missile);
            missile.transform.parent = transform;
        }
    }

    public void ActivateSeekingMissiles()
    {
        List<BlockController> blocks = GameObject.FindObjectsOfType<BlockController>().ToList();

        PlayerController player = GameObject.FindObjectOfType<PlayerController>();

         // Get the position of the current GameObject (the reference point)
        Vector3 referencePosition = transform.position;
       
        // remove every element that is already lit
        blocks.RemoveAll( (x) => x.IsLit);

        // Sort the list of GameObjects by their distance to the reference position
        blocks.Sort((a, b) =>
        {
            float distanceA = Vector3.Distance(a.transform.position, referencePosition);
            float distanceB = Vector3.Distance(b.transform.position, referencePosition);
            return distanceA.CompareTo(distanceB);
        });

        blocks.Sort((x, y) =>
        {
            if (x.shape == BlockController.ShapeType.Box && y.shape != BlockController.ShapeType.Box)
                return -1;
            if (x.shape == BlockController.ShapeType.Circle && y.shape != BlockController.ShapeType.Circle)
                return -1;
            return 0;
        });

        // Instantiate the missiles and set them targets
        for (int i = 0; i < numberOfMissiles; i++)
        {
            GameObject missile = seekingMissilesPool.Find(x => !x.activeSelf);
            if (missile == null) break;
            missile.transform.position = player.transform.position;
            missile.SetActive(true);
            missile.GetComponent<SeekingMissile>().SetupMissile(blocks[i > blocks.Count - 1 ? blocks.Count-1 : i].transform.position);
        }
    }

    private void OnSceneCleanup()
    {
        foreach (var missile in seekingMissilesPool)
        {
            missile.GetComponent<SeekingMissile>().SceneCleanUp();
            missile.transform.position = transform.position;
        }
    }
}

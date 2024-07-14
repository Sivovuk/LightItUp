using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LightItUp.Game;
using UnityEngine;

public class PowerUpSeekingMissiles : MonoBehaviour
{
    public GameObject missilePrefab;
    public int numberOfMissiles = 3;

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

        // Instantiate the missiles and set them targets
        for (int i = 0; i < numberOfMissiles; i++)
        {
            GameObject missile = Instantiate(missilePrefab, player.transform.position, Quaternion.identity);
            missile.GetComponent<SeekingMissile>().SetupMissile(blocks[i > blocks.Count - 1 ? blocks.Count-1 : i].transform.position);
        }
    }
}

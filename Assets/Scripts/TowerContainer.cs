using UnityEngine;

public class TowerContainer : MonoBehaviour
{
    [SerializeField] GameObject keyObj;
    int towerCount;
    private void Start()
    {
        towerCount = GameObject.FindGameObjectsWithTag("Tower").Length;
    }
    public void SubtractTower()
    {
        towerCount = GameObject.FindGameObjectsWithTag("Tower").Length;
        Debug.Log("Tower Count: " + towerCount);
        if (towerCount <= 1) 
            keyObj.SetActive(true);
    }
}

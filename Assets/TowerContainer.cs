using UnityEngine;

public class TowerContainer : MonoBehaviour
{
    [SerializeField] GameObject keyObj;
    public void SubtractTower()
    {
        keyObj.SetActive(GameObject.FindGameObjectsWithTag("Tower").Length <= 0);
    }
}

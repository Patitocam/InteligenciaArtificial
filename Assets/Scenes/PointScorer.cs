using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] public Dictionary<float, float> pointsAndChances = new Dictionary<float, float>();

    RoulletteWheel roulette; 
    void Start()
    {
        roulette = new RoulletteWheel();
        pointsAndChances.Add(1, 10);
        pointsAndChances.Add(5, 5);
        pointsAndChances.Add(10, 1);
    }
    private void Update()
    {
        AddPoint();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Point"))
        {
            AddPoint();
            Destroy(other.gameObject);
        }
    }

    void AddPoint()
    {

        Debug.Log(roulette.RouletteWheelSelection<float>(pointsAndChances));
    }
}

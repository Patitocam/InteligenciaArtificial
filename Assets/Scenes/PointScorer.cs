using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScorer : MonoBehaviour
{
    [SerializeField] public Dictionary<float, float> pointsAndChances = new Dictionary<float, float>();
    [SerializeField] public float points;
    [SerializeField] private Text text;

    RoulletteWheel roulette; 
    void Start()
    {
        roulette = new RoulletteWheel();
        pointsAndChances.Add(1, 10);
        pointsAndChances.Add(5, 5);
        pointsAndChances.Add(10, 1);
        points = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Point"))
        {
            AddPoint();
        }
    }
    void AddPoint()
    {
        points += roulette.RouletteWheelSelection<float>(pointsAndChances);
        text.UpdateText(points);
    }
}

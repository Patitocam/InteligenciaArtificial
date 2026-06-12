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
        pointsAndChances.Add(1, 6);
        pointsAndChances.Add(5, 5);
        pointsAndChances.Add(10, 4); //Agrego numeros que pueden salir al diccionario y sus posibilidades
        points = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Point"))
        {
            AddPoint(); //Cuando un Amongus azul entra en el corral llama a sumar punto
            Destroy(other.gameObject);
        }
    }
    void AddPoint()
    {
        points += roulette.RouletteWheelSelection<float>(pointsAndChances); //Tiro la ruleta, le establezco el tipo float para las salidas y le paso el diccionario 
        text.UpdateText(points);//Actualizo el texto
    }

    void RecalculatePoints()
    {
        pointsAndChances[1] = Mathf.Clamp(pointsAndChances[1] + 1, 6, 10);
        pointsAndChances[10] = Mathf.Clamp(pointsAndChances[10] - 1, 1, 4);
    }
}

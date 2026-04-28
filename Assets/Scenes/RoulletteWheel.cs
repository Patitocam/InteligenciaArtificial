using System.Collections.Generic;
using UnityEngine;

public class RoulletteWheel
{
    public T RouletteWheelSelection<T>(Dictionary<T, float> elements) //Selector random genérico
    {
        float totalChance = 0;
        foreach (var elem in elements.Values)
            totalChance += elem; //Agrega los valores a la "Tirada"

        float randomValue = Random.Range(0, totalChance); //Genera un numero random
        foreach (var elem in elements)
        {
            randomValue -= elem.Value;
            if (randomValue <= 0)
                return elem.Key; //Va restando los valores de las posibles salidas hasta que llegue a 0 o menos, devuelve el objeto asociado a ese numero
        }

        return default;
    }
}


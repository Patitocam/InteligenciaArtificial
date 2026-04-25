using System.Collections.Generic;
using UnityEngine;

public class RoulletteWheel
{
    public T RouletteWheelSelection<T>(Dictionary<T, float> elements)
    {
        float totalChance = 0;
        foreach (var elem in elements.Values)
            totalChance += elem;

        float randomValue = Random.Range(0, totalChance);
        foreach (var elem in elements)
        {
            randomValue -= elem.Value;
            if (randomValue <= 0)
                return elem.Key;
        }

        return default;
    }
}


using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    private static List<int> _shuffledIndices;
    private static int _currentIndex = 0;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        _shuffledIndices = new List<int>();
        
        for (int i = 0; i < 6; i++)
        {
            _shuffledIndices.Add(i);
        }

        for (int i = 0; i < _shuffledIndices.Count; i++)
        {
            int temp = _shuffledIndices[i];
            int randomIndex = Random.Range(i, _shuffledIndices.Count);
            _shuffledIndices[i] = _shuffledIndices[randomIndex];
            _shuffledIndices[randomIndex] = temp;
        }
    }

    public static int GetCurrentColor()
    {
        if (_shuffledIndices == null || _shuffledIndices.Count == 0) return 0;

        int indexToReturn = _shuffledIndices[_currentIndex % _shuffledIndices.Count];
        
        _currentIndex++;
        return indexToReturn;
    }
}
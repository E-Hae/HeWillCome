using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedRandom : MonoBehaviour
{
    public static int Pick(float[] _probabilitys)
    {
        float _total = 0;

        foreach (float _probability in _probabilitys)
        {
            _total += _probability;
        }

        float _randomPoint = Random.value * _total;

        for (var i = 0; i < _probabilitys.Length; i++)
        {
            if (_randomPoint < _probabilitys[i])
            {
                return i;
            }
            else
            {
                _randomPoint -= _probabilitys[i];
            }
        }
        return _probabilitys.Length - 1;
    }
}
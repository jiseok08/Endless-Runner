using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineCache
{
    static Dictionary<float, WaitForSeconds> dictionary = new Dictionary<float, WaitForSeconds>();
 
    public static WaitForSeconds WaitForSoconds(float time)
    {
        WaitForSeconds waitForseconds;

        if (dictionary.TryGetValue(time, out waitForseconds) == false)
        {
            dictionary.Add(time, new WaitForSeconds(time));

            waitForseconds = dictionary[time];
        }

        return waitForseconds;
    }
}

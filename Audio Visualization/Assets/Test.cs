using UnityEngine;
using System.Collections;
using FireClaw.Audio;

public class Test : MonoBehaviour
{
    public float val1;
    public float[] vals;

    [ContextMenu("Test")]
    void test()
    {
        float[] converted = new float[vals.Length];

        for (int i = 0; i < converted.Length; i++)
        {
            converted[i] = AudioUnitConversions.RelToDB(vals[i]);
        }

        Debug.Log(AudioUnitConversions.DBToRel(AudioUnitOperations.AddDBs(vals)));
    }
}

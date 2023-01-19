using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;
using Biocrowds.Core;

public class BeatDetector : MonoBehaviour
{
    //public AudioClip song;
    public AudioSource song;
    //public GameObject cube;
    private bool Beated;
    private float[] historyBuffer = new float[43];
     private float[] channelRight;
    private float[] channelLeft;
    private int SamplesSize = 1024;
    public float InstantSpec;
    public float AverageSpec;
    public float VarianceSum;
    public float Variance;
    public float Constant;
    public GameObject Stage;


    // Use this for initialization
    void Start()
    {

        song = Stage.GetComponent<AudioSource>();
        Beated = false;
    }

    // Update is called once per frame
    void Update()
    {

        //compute instant sound energy
        //channelRight = song.audio.GetSpectrumData (1024, 1, FFTWindow.BlackmanHarris);  //Normal
        //channelLeft = song.audio.GetSpectrumData (1024, 2, FFTWindow.BlackmanHarris);  //Normal
        //channelRight = song.GetSpectrumData (1024, 1, FFTWindow.Hamming);  //Rafa
        //channelLeft = song.GetSpectrumData (1024, 2, FFTWindow.Hamming);  //Rafa

        //InstantSpec = sumStereo (channelLeft, channelRight);  //Normal
        InstantSpec = sumStereo2(song.GetSpectrumData(SamplesSize, 0, FFTWindow.Hamming));  //Rafa

        //compute local average sound evergy
        //AverageSpec = sumLocalEnergy ()/historyBuffer.Length; // E being the average local sound energy  //Normal
        AverageSpec = (SamplesSize / historyBuffer.Length) *sumLocalEnergy2(historyBuffer);  //Rafa

        //calculate variance
        //VarianceSum = 0;
        //for (int i = 0; i < 43; i++)  //Normal
        //VarianceSum += (historyBuffer[i]-AverageSpec)*(historyBuffer[i]-AverageSpec);

        //Variance = VarianceSum/historyBuffer.Length;  //Normal
        Variance = VarianceAdder(historyBuffer) / historyBuffer.Length;  //Rafa
        Constant = (float)((-0.0025714 * Variance) + 1.5142857);  //Normal

        float[] shiftingHistoryBuffer = new float[historyBuffer.Length]; // make a new array and copy all the values to it

        for (int i = 0; i < (historyBuffer.Length - 1) ; i++) { // now we shift the array one slot to the right
            shiftingHistoryBuffer[i + 1] = historyBuffer[i]; // and fill the empty slot with the new instant sound energy
        }

        shiftingHistoryBuffer[0] = InstantSpec;

        for (int i = 0; i < historyBuffer.Length; i++) {
            historyBuffer[i] = shiftingHistoryBuffer[i]; //then we return the values to the original array
        }

        if (InstantSpec > (Constant * AverageSpec))
        { // now we check if we have a beat
            if (!Beated)
            {
                Debug.Log("Beat");
                Beated = true;
            }
        }
        else
        {
            if (Beated)
            {
                Beated = false;
            }
            Debug.Log("No Beat");
        }

        //Debug.Log ("Avg local: " + E);
        //Debug.Log ("Instant: " + e);
        //Debug.Log ("History Buffer: " + historybuffer());

        //Debug.Log ("sum Variance: " + sumV);
        //Debug.Log ("Variance: " + V);

        //Debug.Log ("Constant: " + constant);
        //Debug.Log ("--------");
    }

    float sumStereo(float[] channel1, float[] channel2)
    {
        float e = 0;
        for (int i = 0; i < channel1.Length; i++)
        {
            e += ((channel1[i] * channel1[i]) + (channel2[i] * channel2[i]));
        }

        return e;
    }

    float sumStereo2(float[] Channel)
    {
        float e = 0;
        for (int i = 0; i < Channel.Length; i++)
        {
            float ToSquare = Channel[i];
            e += (ToSquare * ToSquare);
        }
        return e;
    }

    float sumLocalEnergy()
    {
        float E = 0;

        for (int i = 0; i < historyBuffer.Length; i++) {
            E += historyBuffer[i] * historyBuffer[i];
        }

        return E;
    }

    float sumLocalEnergy2(float[] Buffer)
    {
        float E = 0;
        for (int i = 0; i < Buffer.Length; i++)
        {
            float ToSquare = Buffer[i];
            E += (Buffer[i] * Buffer[i]);
        }
        return E;
    }

    float VarianceAdder(float[] Buffer)
    {
        float VarSum = 0;
        for (int i = 0; i < Buffer.Length; i++)
        {  //Rafa
            float ToSquare = Buffer[i] - AverageSpec;
            VarSum += (ToSquare * ToSquare);
        }
        return VarSum;
    }

    string historybuffer()
    {
        string s = "";
        for (int i = 0; i < historyBuffer.Length; i++) {
            s += (historyBuffer[i] + ",");
        }
        return s;
    }
}
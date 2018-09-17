using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NN : MonoBehaviour {
    float mutationRate = 0.1f;

    public float[] _inputBias = new float[6];
    float[,] _inputWeights = new float[6, 5];
    float[] _hiddenBias = new float[5];
    float[,] _hiddenWeights = new float[5, 2];

    public NN()
    {
        for(int i =0; i<6; i++)
            _inputBias[i] = Random.Range(-10,10);
        for (int i = 0; i < 5; i++)
            _hiddenBias[i] = Random.Range(-10, 10);
        for(int i=0; i<6; i++)
        {
            for(int j=0; j<5; j++)
            {
                _inputWeights[i,j] = Random.Range(-10, 10);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                _hiddenWeights[i, j] = Random.Range(-10, 10);
            }
        }
    }

    public NN(NN other)
    {
        for (int i = 0; i < 6; i++)
            _inputBias[i] = other._inputBias[i];
        for (int i = 0; i < 5; i++)
            _hiddenBias[i] = other._hiddenBias[i];
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                _inputWeights[i, j] = other._inputWeights[i, j];
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                _hiddenWeights[i, j] = other._hiddenWeights[i, j];
            }
        }
    }

    public NN(string filepath)
    {
        StreamReader reader = new StreamReader(filepath, true);
        string line;
        try
        {
            line = reader.ReadLine();
            Debug.Log(line);
            string []line_arr = line.Split(' ');
            for (int i = 0; i < 6; i++)
                _inputBias[i] = float.Parse(line_arr[i]);
            line = reader.ReadLine();
            Debug.Log(line);
            line_arr = line.Split(' ');
            for (int i = 0; i < 5; i++)
                _hiddenBias[i] = float.Parse(line_arr[i]);
            line = reader.ReadLine();
            Debug.Log(line);
            line_arr = line.Split(' ');
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    _inputWeights[i, j] = float.Parse(line_arr[i * 5 + j]);
                }
            }
            line = reader.ReadLine();
            Debug.Log(line);
            line_arr = line.Split(' ');
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    _hiddenWeights[i, j] = float.Parse(line_arr[i * 2 + j]);
                }
            }
            reader.Close();
        }
        catch(System.Exception e)
        {
            Debug.Log("Error reading the file " + e.Message + e.StackTrace);
        }
        
    }

    public NN(NN parentA, NN parentB)
    {
        //crossover - all crossovers are equal
        //mutate - mutation rate is taken as 0.01
        for (int i = 0; i < 6; i++)
        {
            _inputBias[i] = i % 2 == 0 ? parentA._inputBias[i] : parentB._inputBias[i];
            if(Random.value< mutationRate)
                _inputBias[i] = Random.Range(-10, 10);
        }
        for (int i = 0; i < 5; i++)
        {
            _hiddenBias[i] = i % 2 == 0 ? parentA._hiddenBias[i] : parentB._hiddenBias[i];
            if (Random.value < mutationRate)
                _hiddenBias[i] = Random.Range(-10, 10);
        }
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                _inputWeights[i, j] = i % 2 == 0 ? parentA._inputWeights[i,j] : parentB._inputWeights[i,j];
                if (Random.value < mutationRate)
                    _inputWeights[i,j] = Random.Range(-10, 10);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                _hiddenWeights[i, j] = i % 2 == 0 ? parentA._hiddenWeights[i, j] : parentB._hiddenWeights[i, j];
                if (Random.value < mutationRate)
                    _hiddenWeights[i, j] = Random.Range(-10, 10);
            }
        }
    }

    public void predict(float[] inputs,ref double outSteering,ref double outAxel) //Hard coded in not a good practice but will do for prototype. Matrix class will be added in the next version
    {
        double[] firstLayer =
         {
            inputs[0]*_inputWeights[0,0]+ inputs[1]*_inputWeights[1,0]+ inputs[2]*_inputWeights[2,0]+ inputs[3]*_inputWeights[3,0]+ inputs[4]*_inputWeights[4,0]+ inputs[5]*_inputWeights[5,0],
            inputs[0]*_inputWeights[0,1]+ inputs[1]*_inputWeights[1,1]+ inputs[2]*_inputWeights[2,1]+ inputs[3]*_inputWeights[3,1]+ inputs[4]*_inputWeights[4,1]+ inputs[5]*_inputWeights[5,1],
            inputs[0]*_inputWeights[0,2]+ inputs[1]*_inputWeights[1,2]+ inputs[2]*_inputWeights[2,2]+ inputs[3]*_inputWeights[3,2],+ inputs[4]*_inputWeights[4,2]+ inputs[5]*_inputWeights[5,2],
            inputs[0]*_inputWeights[0,3]+ inputs[1]*_inputWeights[1,3]+ inputs[2]*_inputWeights[2,3]+ inputs[3]*_inputWeights[3,3]+ inputs[4]*_inputWeights[4,3]+ inputs[5]*_inputWeights[5,3],
            inputs[0]*_inputWeights[0,4]+ inputs[1]*_inputWeights[1,4]+ inputs[2]*_inputWeights[2,4]+ inputs[3]*_inputWeights[3,4]+ inputs[4]*_inputWeights[4,4]+ inputs[5]*_inputWeights[5,4]
        };
        for (int i=0;i<6;i++)
        {
            firstLayer[i] = tanh(firstLayer[i] + _inputBias[i]);
        }
        double[] lastLayer =
         {
            firstLayer[0]*_hiddenWeights[0,0] + firstLayer[1]*_hiddenWeights[1,0] +  firstLayer[2]*_hiddenWeights[2,0] +  firstLayer[3]*_hiddenWeights[3,0] +  firstLayer[4]*_hiddenWeights[4,0],
            firstLayer[0]*_hiddenWeights[0,1] + firstLayer[1]*_hiddenWeights[1,1] +  firstLayer[2]*_hiddenWeights[2,1] +  firstLayer[3]*_hiddenWeights[3,1] +  firstLayer[4]*_hiddenWeights[4,1]
        };
        for (int i = 0; i < 2; i++)
        {
            lastLayer[i] = tanh(lastLayer[i] + _hiddenBias[i]);
        }
        outSteering = lastLayer[0];
        outAxel = lastLayer[1];
        //Debug.Log(outSteering+" "+ outAxel);
    }
    float Sigmoid(double x)
    {
        return 1 / (1 - Mathf.Exp(-1 * (float)x));
    }
    double tanh(double x)
    {
        return System.Math.Tanh(x);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void save()
    {
        StreamWriter write = new StreamWriter("./nn.txt", true);
        for (int i = 0; i < 6; i++)
            write.Write(_inputBias[i] + " ");
        write.Write("\n");
        for (int i = 0; i < 5; i++)
            write.Write(_hiddenBias[i] + " ");
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                write.Write(_inputWeights[i, j] + " ");
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                write.Write(_hiddenWeights[i, j] + " ");
            }
        }
        write.Close();
    }
}

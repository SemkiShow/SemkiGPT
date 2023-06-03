using System;

namespace SemkiNeuralNetworks
{
	public class Perceptron
	{
		// neurons[layer][neuron]
		public double[,] neurons;
		// weights[layer][neuron][weight]
		public double[,,] weights;
		public int[] neuronsConfig;
		public string datasetPath = "./dataset";
		public int maxNeurons;
		private Random random = new Random();

		public void Randomize()
		{
			// Finding max neuron value
			maxNeurons = neuronsConfig[0];
			for (int i = 0; i < neuronsConfig.Length; i++)
			{
				if (neuronsConfig[i] > maxNeurons)
				{
					maxNeurons = neuronsConfig[i];	
				}
			}
			maxNeurons++;
			Console.WriteLine("Log: Found max neuron value");

			// Initializing neurons and weights
			neurons = new double[neuronsConfig.Length,maxNeurons];
			this.ResetNeurons();

			weights = new double[neuronsConfig.Length,maxNeurons,maxNeurons];
			for (int i = 0; i < neuronsConfig.Length; i++)
			{
				for (int j = 0; j < maxNeurons; j++)
				{
					for (int k = 0; k < 10; k++)
					{
						weights[i,j,k] = 0;	
					}	
				}	
			}
			Console.WriteLine("Log: Initialized neurons and weights");

			// Randomizing neurons
			for (int i = 0; i < neuronsConfig.Length; i++)
			{
				for (int j = 0; j < maxNeurons; j++)
				{
					if (j > neuronsConfig[i])
					{
						continue;	
					}
					neurons[i,j] = random.NextDouble();
					if (j == (maxNeurons - 1))
					{
						neurons[i,j] = 1;	
					}
				}	
			}
			Console.WriteLine("Log: Randomized neurons");

			// Randomizing weights
			for (int i = 0; i < neuronsConfig.Length; i++)
			{
				for (int j = 0; j < maxNeurons; j++)
				{
					if (j > neuronsConfig[i])
					{
						continue;	
					}
					for (int k = 0; k < neuronsConfig[i]; k++)
					{
						weights[i,j,k] = random.NextDouble();		
					}	
				}	
			}
			Console.WriteLine("Log: Randomized weights");
		}

		public void CalculateNeurons()
		{
			for (int i = 1; i < neuronsConfig.Length; i++)
			{
				for (int j = 0; j < maxNeurons; j++)
				{
					if (j > neuronsConfig[i])
					{
						continue;	
					}
					neurons[i,j] = 0;
					for (int k = 0; k < neuronsConfig[i - 1]; k++)
					{
						neurons[i,j] += weights[i - 1,k,j] * neurons[i - 1,k];
					}
					neurons[i,j] = LogSigmoid(neurons[i,j]);
				}	
			}
			Console.WriteLine("Log: Calculated neurons");
		}

		public double LogSigmoid(double x)
		{
  			if (x < -45.0) return 0.0;
  			else if (x > 45.0) return 1.0;
  			else return 1.0 / (1.0 + Math.Exp(-x));
		}

		public double GetTotalError(double[] correctAnswer)
		{
			double error = 0.0;
			for (int i = 0; i < correctAnswer.Length; i++)
			{
				error += 0.5 * Math.Pow(correctAnswer[i] - neurons[neuronsConfig.Length - 1, i], 2);
			}
			return error;
		}

		public void ResetNeurons()
		{
			for (int i = 0; i < neuronsConfig.Length; i++)
			{
				for (int j = 0; j < maxNeurons; j++)
				{
					if (j > neuronsConfig[i])
					{
						continue;
					}
					neurons[i,j] = 0;
					if (j == neuronsConfig[i] && i != 0)
					{
						neurons[i,j] = 1;
					}
				}	
			}
		}
	}
}


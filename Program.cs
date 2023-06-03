using System;
using SemkiNeuralNetworks;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace SemkiGPT
{
	public class Program
	{
		static string symbols = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890`~!@#$%^&*()-_=+[]{};:'\",.<>?/\\\n\t ";
		// static string symbols = "";
		static Random random = new Random();
		static string divider = " ";

		static void Backpropagation(Perceptron perceptron, string[] datasetFiles)
		{
			// Getting random dataset file
			string randomFile = datasetFiles[random.Next(datasetFiles.Length)];
			Console.WriteLine("Log: Got a random dataset file");
			
			// Reading randomFile
			string[] randomFileContent = File.ReadAllText(randomFile).Split(divider);
			Console.WriteLine("Log: Read a random dataset file");
			
			// Getting 2 random dataset strings
			int randomNumber = random.Next(randomFileContent.Length - 1);
			string randomStrings = randomFileContent[randomNumber] + divider + randomFileContent[randomNumber + 1];
			Console.WriteLine("Log: Got 2 random nearby dataset strings");
			Console.WriteLine("Log: The strings are:\n" + randomStrings);
			
			// Clearing neurons
			perceptron.ResetNeurons();
			Console.WriteLine("Log: Cleared neurons");
			
			// Encoding strings
			for (int i = 0; i < randomStrings.Split(divider)[0].Length; i++)
			{
				int symbolIndex = symbols.Length;
				for (int j = 0; j < symbols.Length; j++)
				{
					if (randomStrings.Split(divider)[0][i] == symbols[j])
					{
						symbolIndex = j + 1;
						continue;
					}
				}
				perceptron.neurons[0,i] = 1d / symbolIndex;
				if (symbolIndex == symbols.Length)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					string errorMessage = "Error: Symbol doesn't exist! (at layer 1)";
					Console.WriteLine(errorMessage);
					Console.ResetColor();
					File.AppendAllText("Errors.log", "\n" + DateTime.Now + " " + errorMessage + " " + randomStrings);
				}
			}
			Console.WriteLine("Log: Wrote the first encoded string to the 1st neuron layer");
			
			double[] correctAnswer = new double[perceptron.neuronsConfig[perceptron.neuronsConfig.Length - 1]];
			for (int i = 0; i < randomStrings.Split(divider)[1].Length; i++)
			{
				int symbolIndex = symbols.Length;
				for (int j = 0; j < symbols.Length; j++)
				{
					if (randomStrings.Split(divider)[1][i] == symbols[j])
					{
						symbolIndex = j + 1;
						continue;
					}	
				}
				correctAnswer[i] = 1d / symbolIndex;
				if (symbolIndex == symbols.Length)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					string errorMessage = "Error: Symbol doesn't exist! (at layer " + perceptron.neuronsConfig.Length + ")";
					Console.WriteLine(errorMessage);
					Console.ResetColor();
					File.AppendAllText("Errors.log", "\n" + DateTime.Now + " " + errorMessage + " " + randomStrings);
				}

			}
			Console.WriteLine("Log: Got the correct answer");
			
			// Calculating neurons
			perceptron.CalculateNeurons();
			
			// Backpropagation
			

			// Printing total error after backpropagation
			Console.WriteLine("Total error: " + perceptron.GetTotalError(correctAnswer));
		}

		static void CorrectDataset(Perceptron perceptron, bool isHTML = false)
		{
			// Getting dataset files
			string[] datasetFiles = Directory.GetFiles("./wrongDataset/");
			Console.WriteLine("Log: Got dataset files");

			for (int i = 0; i < datasetFiles.Length; i++)
			{
				string file = File.ReadAllText(datasetFiles[i]);
				bool isTag = false;
				StringBuilder fileContent = new StringBuilder();
				for (int j = 0; j < file.Length; j++)
				{
					if (file[j] == '<' && isHTML)
					{
						isTag = true;
					}
					if (symbols.Contains(file[j]) && !isTag)
					{
						fileContent.Append(file[j]);
					}
					if (file[j] == '>' && isHTML)
					{
						isTag = false;	
					}
					//else
					//{
					//	string errorMessage = "Error: Symbol doesn't exist!";
					//	File.AppendAllText("Errors.log", "\n" + DateTime.Now + " " + errorMessage + " " + file[j]);	
					//}
				}
				File.AppendAllText("./correctedDataset/" + datasetFiles[i].Split("/")[datasetFiles[i].Split("/").Length - 1], fileContent.ToString());
				Console.WriteLine("Log: Completed file " + datasetFiles[i].Split("/")[datasetFiles[i].Split("/").Length - 1]);
			}
		}

		static void Main()
		{
			// Making log prettier
			File.AppendAllText("Errors.log", "\n" + new string('-', 50));

			Perceptron perceptron = new Perceptron();
			Console.WriteLine("Log: Created a perceptron instance");

			perceptron.neuronsConfig = new int[10000];
			for (int i = 0; i < 10000; i++)
			{
				perceptron.neuronsConfig[i] = 50;	
			}
			Console.WriteLine("Log: Set neurons config");

			perceptron.Randomize();
		
			//Symbols initialization
			/*List<char> printableChars = new List<char>();
			for (int i = char.MinValue; i <= char.MaxValue; i++)
			{
    			char c = Convert.ToChar(i);
    			if (!char.IsControl(c))
    			{
       		 		printableChars.Add(c);
    		 	}
			}
			for (int i = 0; i < printableChars.Count; i++)
			{
				symbols += printableChars[i];	
			}
			symbols = File.ReadAllText("AllSymbols.txt");
			Console.WriteLine("Log: Initialized symbols");*/

			// Getting dataset files
			string[] datasetFiles = Directory.GetFiles(perceptron.datasetPath);
			Console.WriteLine("Log: Got dataset files");

			for (int i = 1; i <= 100; i++)
			{
				Console.WriteLine("-----Log: Iteration " + i + "-----");
				Backpropagation(perceptron, datasetFiles);
			}
			Console.WriteLine("Log: Learning ended");
			// CorrectDataset(perceptron, true);
		}
	}
}


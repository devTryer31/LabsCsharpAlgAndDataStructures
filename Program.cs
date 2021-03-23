using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Search_algorithms_AandDS {
	static class Searching {

		public static int Linear(int[] arr, int key) {
			for (int i = 0; i < arr.Length; ++i) {
				if (arr[i].Equals(key))
					return i;
			}
			return -1;
		}

		public static int Interpolation(int[] arr, int key) {
			int leftIndex = 0;
			int rightIndex = arr.Length - 1;

			int ofset = 1;
			while (arr[leftIndex] < key && arr[rightIndex] > key) {
				if (arr[leftIndex] == arr[rightIndex])
					break;
				ofset = ((rightIndex - leftIndex) * (key - arr[leftIndex]))
					/ (arr[rightIndex] - arr[leftIndex]);
				int idx = leftIndex + ofset;
				if (idx < 0 || idx > arr.Length)
					return -1;
				if (arr[idx] > key)
					rightIndex = idx - 1;
				else if (arr[idx] < key)
					leftIndex = idx + 1;
				else
					return idx;
			}
			if (arr[leftIndex] == key)
				return leftIndex;
			if (arr[rightIndex] == key)
				return rightIndex;
			return -1;
		}

		public static int Binary(int[] arr, int key) {
			int last = arr.Length - 1, first = 0;
			while (true) {

				if (first > last) {
					return -1;
				}

				int middle = (first + last) / 2;
				int middleValue = arr[middle];

				if (middleValue == key) {
					return middle;
				}
				else {
					if (middleValue > key) {
						last = middle - 1;
					}
					else {
						first = middle + 1;
					}
				}
			}
		}

		public static int Fibonachi(int[] arr, int key) {
			int n = arr.Length - 1;
			int fibNum2 = 0;
			int fibNum1 = 1;
			int fibNumNext = fibNum2 + fibNum1;
			while (fibNumNext < n) {
				fibNum2 = fibNum1;
				fibNum1 = fibNumNext;
				fibNumNext = fibNum2 + fibNum1;
			}
			int offset = -1;
			while (fibNumNext > 1) {
				int i = Math.Min(offset + fibNum2, n);
				if (arr[i] < key) {
					fibNumNext = fibNum1;
					fibNum1 = fibNum2;
					fibNum2 = fibNumNext - fibNum1;
					offset = i;
				}
				else if (arr[i] > key) {
					fibNumNext = fibNum2;
					fibNum1 = fibNum1 - fibNum2;
					fibNum2 = fibNumNext - fibNum1;
				}
				else
					return i;
			}

			if (offset + 1 <= n && fibNum1 == 1 && arr[offset + 1] == key)
				return offset + 1;

			return -1;

		}

	}



	class Program {
		private static string dataFilePath = @"D:\programmes\visual studio 2k19\C# projects\LabsCsharpAlgAndDataStructures\data.csv";
		public static void GetTime() {
			//Console.WriteLine("Enter array size (-1 to exit): ");
			//int n = int.Parse(Console.ReadLine());
			Random gen = new Random();
			Stopwatch sw = new Stopwatch();
			Dictionary<string, long> results = new Dictionary<string, long> {
				{"Linear",0 },
				{"Interpolation",0 },
				{"Binary",0 },
				{"Binary tree",0 },
				{"Bor tree",0 },
				{"Fibonachi",0 }
			};
			StringBuilder titles = new StringBuilder();
			titles.Append("Names;").Append(string.Join(';', results.Keys.ToList())).Append('\n');
			File.WriteAllText(dataFilePath, titles.ToString());//Rewrite to file.

			int timesGens = 1;
			foreach (int n in new int[] { 300, 1000, 3000, 10000, 30000, 50000 }) {
				int[] arr = new int[n];
				for (int i = 0; i < n; i++) {
					arr[i] = gen.Next(-(n/2), n/2);
				}
				int[] noSortArr = (int[])arr.Clone();
				Array.Sort(arr);
				BinaryTree<int> binaryTree = new BinaryTree<int>(arr[0]);
				for (int i = 1; i < n; i++) {
					binaryTree.Add(noSortArr[i]);
				}
				BorTree<int> borTree = new BorTree<int>();
				for (int i = 0; i < n; i++) {
					//int RandNum = gen.Next(n) * 1000;
					//borTree.Add(RandNum.ToString(), RandNum);
					borTree.Add(noSortArr[i].ToString(), noSortArr[i]);
				}
				int cnt = timesGens;
				while (cnt-- != 0) {
					foreach (int searchValue in
						arr.Append(-n + 1).Append(n - 1).Append(0)) {

						{
							sw.Restart();
							Searching.Linear(noSortArr, searchValue);
							sw.Stop();
							results["Linear"] += sw.ElapsedTicks;
						}

						{
							sw.Restart();
							Searching.Interpolation(arr, searchValue);
							sw.Stop();
							results["Interpolation"] += sw.ElapsedTicks;
						}

						{
							sw.Restart();
							Searching.Binary(arr, searchValue);
							sw.Stop();
							results["Binary"] += sw.ElapsedTicks;
						}

						{
							sw.Restart();
							Searching.Fibonachi(arr, searchValue);
							sw.Stop();
							results["Fibonachi"] += sw.ElapsedTicks;
						}

						{
							sw.Restart();
							binaryTree.Search(searchValue);
							sw.Stop();
							results["Binary tree"] += sw.ElapsedTicks;
						}

						{
							sw.Restart();
							borTree.TrySearch(searchValue.ToString(), out int val);
							sw.Stop();
							results["Bor tree"] += sw.ElapsedTicks;
						}

					}

				}

				StringBuilder forCSV = new StringBuilder();
				forCSV.Append($"n = {n};");
				foreach (var key in results.Keys.ToList()) {
					results[key] /= (n * timesGens);
					forCSV.Append($"{results[key]};");
					results[key] = 0;
				}
				File.AppendAllText(dataFilePath, forCSV.Append('\n').ToString());
				//Console.WriteLine($"n = {n}");
				//foreach (var key in results.Keys.ToList()) {
				//	results[key] /= timesGens;
				//	Console.WriteLine($"{key}: {results[key]}tks.");
				//}

				//Console.WriteLine("\n\nEnter array size (-1 to exit): ");
				//n = int.Parse(Console.ReadLine());
				Console.WriteLine($"n = {n} done.");
			}
			Console.WriteLine("Program end.");
		}

		static void Main() {
			#region tests
			//int seed = 123;
			//int n = 20; Random rd = new Random(seed);
			//int[] iarr = new int[n];
			//for (int i = 0; i < 20; i++) {
			//	iarr[i] = rd.Next() % n;
			//}
			//Array.Sort(iarr);
			//for (int i = 0; i < 20; i++) {
			//	Console.Write(iarr[i].ToString() + ' ');
			//}
			//Console.WriteLine();
			//foreach (int num in iarr) {
			//	Console.Write(num.ToString() + ": ");
			//	Console.Write(Searching.Linear(iarr, num).ToString() + ' ');
			//	Console.Write(Searching.Binary(iarr, num).ToString() + ' ');
			//	Console.Write(Searching.Interpolation(iarr, num).ToString() + ' ');
			//	Console.WriteLine(Searching.Fibonachi(iarr, num).ToString() + ' ');
			//}


			//BinaryTree<int> binaryTree = new BinaryTree<int>(0);
			//foreach (int num in iarr) {
			//	binaryTree.Add(num);
			//}
			//Console.WriteLine(binaryTree.Search(100));
			//Console.WriteLine(binaryTree.Search(2));
			////binaryTree.Print();

			//foreach (int num in binaryTree.BuildList()) {
			//	Console.WriteLine(num);
			//}
			#endregion
			GetTime();
		}
	}
}

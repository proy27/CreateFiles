using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CreateFiles
{
	class Program
	{
		private static string tar;
		private static int AllCount;
		private static int Count;
		private static bool ReadCountEnd = false;

		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			var basePath = AppDomain.CurrentDomain.BaseDirectory;
			var count = 0;
			if (args.Length == 1)
			{
				if (int.TryParse(args[0], out var ttt))
				{
					count = ttt;
				}
				else
				{
					Console.WriteLine("Enter a Int");
					count = Convert.ToInt32(Console.ReadLine());
				}
			}
			else
			{
				Console.WriteLine("Enter a Int");
				count = Convert.ToInt32(Console.ReadLine());
			}
			Console.WriteLine($"Start {count} !!!");


			var sp = Stopwatch.StartNew();
			Console.WriteLine($"basePath => {basePath}");

			var RC = Task.Run(() => ReadCount(sp));


			CreateFiles(System.Convert.ToInt32(count));
			ReadCountEnd = true;

			Console.WriteLine("===============================================================");
			Console.WriteLine("AllCount => " + AllCount.ToString("##,###"));
			Console.WriteLine("Count => " + Count.ToString("##,###"));

			sp.Stop();
			Console.WriteLine("Speed {0}", (AllCount * 1000f / sp.ElapsedMilliseconds).ToString("N"));
			Console.WriteLine("Total: {0}", sp.Elapsed);

			Console.ReadLine();
		}

		static void CreateFiles(int count)
		{
			AllCount = count;
			string path = DateTime.Now.ToString("yyyyMMdd_HHmmss");
			System.IO.Directory.CreateDirectory(path);
			Parallel.For(0, count, (item) =>
			{
				File.WriteAllText(path + "/" + Guid.NewGuid().ToString("N"), "");
				Interlocked.Increment(ref Count);
			});
		}
		static void ReadCount(Stopwatch sp)
		{
			var CursorTop = Console.CursorTop;
			while (true)
			{
				if (ReadCountEnd)
				{
					return;
				}

				Console.SetCursorPosition(0, CursorTop);

				Console.WriteLine("AllCount => " + AllCount.ToString("##,###"));
				Console.WriteLine("Count => " + Count.ToString("##,###"));
				Console.WriteLine("Speed {0}", (Count * 1000f / sp.ElapsedMilliseconds).ToString("N"));
				Console.WriteLine("{0}", sp.Elapsed);
				Console.WriteLine("                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    ");
				Console.SetCursorPosition(0, CursorTop + 3);
				Console.WriteLine("{0}", tar);

				System.Threading.Thread.Sleep(500);
			}
		}
	}
}

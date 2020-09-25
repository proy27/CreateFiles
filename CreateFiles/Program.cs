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

		private static int StringLength = 0;
		private static string RandomString = "";

		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			//ThreadPool.SetMaxThreads(10000, 10000);
			//ThreadPool.SetMinThreads(1000, 1000);

			var basePath = AppDomain.CurrentDomain.BaseDirectory;
			var count = 0;
			if (args.Length > 0)
			{
				int.TryParse(args[0], out count);
				if (args.Length > 1)
					int.TryParse(args[1], out StringLength);
			}
			if (count == 0)
			{
				count = 100000;
			}
			if (StringLength == 0 || StringLength > 5000)
			{
				StringLength = 500;
			}
			Console.WriteLine($"Start count {count,-8} StringLength {StringLength,-5} !!!");
			RandomString = GetRandomString(StringLength);

			var sp = Stopwatch.StartNew();
			Console.WriteLine($"basePath => {basePath}");

			var RC = Task.Run(() => ReadCount(sp));


			CreateFiles(System.Convert.ToInt32(count));
			ReadCountEnd = true;

			Console.WriteLine("===============================================================");
			Console.WriteLine("AllCount => " + AllCount.ToString("##,###"));
			Console.WriteLine("Count => " + Count.ToString("##,###"));

			sp.Stop();
			Console.WriteLine($"Speed { (AllCount * 1000f / sp.ElapsedMilliseconds).ToString("N")}  files/sec");

			Console.WriteLine("Speed {0}", (AllCount * 1000f / sp.ElapsedMilliseconds).ToString("N"));
			Console.WriteLine("Total: {0}", sp.Elapsed);

			Console.ReadLine();
		}

		static void CreateFiles(int count)
		{
			AllCount = count;
			string path = DateTime.Now.ToString("yyyyMMdd_HHmmss_fffff");
			System.IO.Directory.CreateDirectory(path);
			Parallel.For(0, count, (item) =>
			{
				var text = Guid.NewGuid().ToString("N") + RandomString;
				File.WriteAllText(path + "/" + Guid.NewGuid().ToString("N"), text);
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
				Console.WriteLine($"Count => {Count.ToString("##,###")}");
				Console.WriteLine($"Speed { (Count * 1000f / sp.ElapsedMilliseconds).ToString("N")}  files/sec");
				Console.WriteLine("{0}", sp.Elapsed);
				Console.WriteLine("                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    ");
				Console.SetCursorPosition(0, CursorTop + 3);
				Console.WriteLine("{0}", tar);

				System.Threading.Thread.Sleep(500);
			}
		}
		private static Random random = new Random();
		public static string GetRandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}

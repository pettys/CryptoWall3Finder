using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cw3find {

	public class Program {

		private static List<string> _visitedDirs;
		private static List<string> _evilFiles;
		private static List<string> _errors;
		private static int _filesProcessed;
		private static string _outputFile;

		static void Main(string[] args) {

			_visitedDirs = new List<string>();
			_evilFiles = new List<string>();
			_errors = new List<string>();
			_filesProcessed = 0;
			_outputFile = Path.Combine(Environment.CurrentDirectory, "evil.txt");

			string rootDir;
			if (args.Length == 0) {
				rootDir = Environment.CurrentDirectory;
			} else {
				rootDir = args[0];
			}

			Search(rootDir);

			var output = WriteResults(true);
			Console.WriteLine(output);
			Console.WriteLine();
			Console.WriteLine("Done. Results written to " + _outputFile);
		}

		private static byte[] _tempBuffer = new byte[16];

		// Need to replace the contents of _evilBytes with the first 16 bytes
		// of one of your known-encrypted files. Every file encrypted by CW3
		// starts with the same MD5 hash, although the hash is unique to your PC.
		private static byte[] _evilBytes = new byte[] {
			0x7b, 0x2f, 0xb5, 0x23,
			0xe0, 0xd2, 0x76, 0xdd,
			0x7a, 0x70, 0xe0, 0x4a,
			0x96, 0x92, 0x4b, 0xd4
		};

		private static void Search(string dirName) {
			var dir = new DirectoryInfo(dirName);
			if (_visitedDirs.Contains(dir.FullName)) {
				return;
			}
			_visitedDirs.Add(dir.FullName);

			Console.WriteLine("Folder " + dir.FullName);
			var files = dir.GetFiles();
			foreach (var file in files) {
				_filesProcessed++;
				Console.Write(string.Format("{0,-60} ", file.Name));
				if (IsEvil(file)) {
					Console.WriteLine("EVIL!!!!");
					_evilFiles.Add(file.FullName);
				} else {
					Console.WriteLine("ok");
				}
				if (_filesProcessed % 200 == 0) WriteResults(false);
			}
			var childDirs = Directory.GetDirectories(dirName);
			foreach (var cd in childDirs) {
				Search(cd);
			}
		}
		private static bool IsEvil(FileInfo file) {
			try {
				using (var stream = file.OpenRead()) {
					int readBytes = stream.Read(_tempBuffer, 0, 16);
					if (readBytes != 16) return false;
				}
				for (var i = 0; i < 16; i++) {
					if (_tempBuffer[i] != _evilBytes[i]) return false;
				}
				return true;
			} catch (Exception ex) {
				_errors.Add(string.Format("Error on file \"{0}\": {1}", file.FullName, ex.Message));
				return false;
			}
		}

		private static string WriteResults(bool isFinal) {
			var finalOutput = new StringBuilder();

			finalOutput.AppendFormat("{0} total files processed{1} at {2}.", _filesProcessed, isFinal ? "" : " so far", DateTime.Now)
				.AppendLine();

			finalOutput.AppendLine().AppendFormat("{0} evil files found{1}:", _evilFiles.Count, isFinal ? "" : " so far")
				.AppendLine();
			foreach (var item in _evilFiles) {
				finalOutput.AppendLine(item);
			}

			finalOutput.AppendLine().AppendFormat("{0} errors{1}:", _errors.Count, isFinal ? "" : " so far")
				.AppendLine();
			foreach (var item in _errors) {
				finalOutput.AppendLine(item);
			}

			File.WriteAllText(_outputFile + (isFinal ? "" : ".temp"), finalOutput.ToString());
			return finalOutput.ToString();
		}
	}
}

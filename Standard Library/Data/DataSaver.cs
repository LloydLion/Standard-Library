using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StandardLibrary.Interfaces;
using StandardLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FileC = System.IO.File;

namespace StandardLibrary.Data
{
	public class DataSaver : INumberIndicatedObject, IDisposable, INewDisposedInformator
	{
		public static DataSaver Global { get; private set; }
		public int Id { get; private set; }
		public bool IsDisposed { get; set; }
		private FileStream File 
		{
			get => file; 
			set 
			{
				file = value;
				fileReader = new StreamReader(value);
				fileWriter = new StreamWriter(value) { AutoFlush = true };
			}
		}
		private string FileLocation => $"{filesPath}{Path.DirectorySeparatorChar}{appName}{Path.DirectorySeparatorChar}__{Id}.dat";


		private static int nextId;
		private string filesPath;
		private FileStream file;
		private StreamReader fileReader;
		private StreamWriter fileWriter;
		private static string appName;


		public event DisposingEventHandler NewDisposing;
		public event DisposedEventHandler NewDisposed;


		static DataSaver()
		{
			appName = Assembly.LoadFile(Process.GetCurrentProcess().MainModule.FileName).GetName().Name;
			Global = new DataSaver(DataLocation.UserData);
		}

		public DataSaver(DataLocation location)
		{
			switch (location)
			{
				case DataLocation.ProgramDirectory: filesPath = "Components Data";
					break;
				case DataLocation.UserData: filesPath = Environment.GetEnvironmentVariable("appdata", EnvironmentVariableTarget.User);
					break;
				case DataLocation.HostComputerData: filesPath = Environment.GetEnvironmentVariable("windir", EnvironmentVariableTarget.Machine) + $"..{Path.DirectorySeparatorChar}ProgramData";
					break;
			}

			Init(filesPath);
		}

		public DataSaver(string filesPath)
		{
			Init(filesPath);
		}


		public void Dispose()
		{
			NewDisposing?.Invoke(this, EventArgs.Empty);
			File.Close();
			NewDisposed?.Invoke(this, EventArgs.Empty);
		}

		public void Save(string key, int number) => Save(key, (object)number);

		public void Save(string key, string line) => Save(key, (object)line);

		public void Save(string key, Array array) => Save(key, (object)array);

		public void Save(string key, object obj)
		{
			var data = ReadDataFile();

			var dataUnit = new DataUnit() { Key = key, Data = obj };
			dataUnit.FinalizeObject();

			data.Add(dataUnit.Key, dataUnit.Data);

			try
			{
				var toWrite = JsonConvert.SerializeObject(data);
				ReCreateFile();
				fileWriter.Write(toWrite);
			}
			catch(Exception ex)
			{
				throw new InvalidDataException($"Data file at {FileLocation} is contains invalid data", ex);
			}
		}

		public bool HasKey(string key)
		{
			var data = ReadDataFile();
			return data.ContainsKey(key);
		}

		public int GetSavedNumber(string key) => GetSavedObject<int>(key);

		public string GetSavedString(string key) => GetSavedObject<string>(key);

		public T[] GetSavedArray<T>(string key) => GetSavedObject<T[]>(key);

		public T GetSavedObject<T>(string key) => (T)GetSavedObject(key, typeof(T));

		public object[] GetSavedArray(string key, Type info) => (object[])GetSavedObject(key, info.MakeArrayType());

		public object GetSavedObject(string key, Type info)
		{
			var data = ReadDataFile();
			var rawObject = data[key];

			return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rawObject), info);
		}

		public void SetDataFilesPath(string path)
		{
			filesPath = path;
		}

		public void SetApplicationName(string name)
		{
			appName = name;
		}

		private void ReCreateFile()
		{
			File.Close();
			FileC.Delete(FileLocation);
			File = OpenDataFile();
		}

		private FileStream OpenDataFile() => FileC.Open($"{filesPath}{Path.DirectorySeparatorChar}{appName}{Path.DirectorySeparatorChar}__{Id}.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);

		private Dictionary<string, object> ReadDataFile()
		{
			var data = ReadDataFileRaw();
			return JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
		}

		private string ReadDataFileRaw()
		{
			var data = fileReader.ReadToEnd();
			if (string.IsNullOrWhiteSpace(data)) data = "{}";
			return data;
		}

		private void Init(string path)
		{
			filesPath = path;

			Id = ++nextId;

			File = OpenDataFile();

			fileReader = new StreamReader(File);
			fileWriter = new StreamWriter(File) { AutoFlush = true };
		}


		public enum DataLocation
		{
			ProgramDirectory,
			UserData,
			HostComputerData
		}

		[Serializable]
		private class DataUnit : Model
		{
			private string key;
			private object data;

			public string Key { get => key; set { OnPropertyChanging(); key = value; OnPropertyChanged(); } }
			public object Data { get => data; set { OnPropertyChanging(); data = value; OnPropertyChanged(); } }

			public override string ToString()
			{
				return $"{Key}: {Data}";
			}
		}
	}
}

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
		/// <summary>
		/// 
		///		Global DataSaver Object
		///		Recomended use for each compoment in program individual saver
		/// 
		/// </summary>
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

		/// <summary>
		/// 
		///		Create saver with given location of data files
		/// 
		/// </summary>
		/// <param name="location">Files location</param>
		public DataSaver(DataLocation location)
		{
			switch (location)
			{
				case DataLocation.ProgramDirectory: SetDataFilesPath("Components Data");
					break;
				case DataLocation.UserData: SetDataFilesPath(Environment.GetEnvironmentVariable("appdata", EnvironmentVariableTarget.User));
					break;
				case DataLocation.HostComputerData: SetDataFilesPath(Environment.GetEnvironmentVariable("windir", EnvironmentVariableTarget.Machine) + $"..{Path.DirectorySeparatorChar}ProgramData");
					break;
			}

			Init(filesPath);
		}

		/// <summary>
		/// 
		///		Create saver with given location of data files
		/// 
		/// </summary>
		/// <param name="filesPath">Files location</param>
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

		/// <summary>
		/// 
		///		Saves object to storage with given key
		/// 
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="number">Object to save</param>
		public void Save(string key, int number) => Save(key, (object)number);

		/// <summary>
		/// 
		///		Saves object to storage with given key
		/// 
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="line">Object to save</param>
		public void Save(string key, string line) => Save(key, (object)line);

		/// <summary>
		/// 
		///		Saves object to storage with given key
		/// 
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="array">Object to save</param>
		public void Save(string key, Array array) => Save(key, (object)array);

		/// <summary>
		/// 
		///		Saves object to storage with given key
		/// 
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="obj">Object to save</param>
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

		/// <summary>
		/// 
		///		Check storage for contains element with equals key
		/// 
		/// </summary>
		/// <param name="key">Key for search</param>
		/// <returns>Search result</returns>
		public bool HasKey(string key)
		{
			var data = ReadDataFile();
			return data.ContainsKey(key);
		}

		/// <summary>
		/// 
		///		Gets saved in storage object by key
		/// 
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns>Saved object</returns>
		public int GetSavedNumber(string key) => GetSavedObject<int>(key);

		/// <summary>
		/// 
		///		Gets saved in storage object by key
		/// 
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns>Saved object</returns>
		public string GetSavedString(string key) => GetSavedObject<string>(key);

		/// <summary>
		/// 
		///		Gets saved in storage array by key
		/// 
		/// </summary>
		/// <typeparam name="T">Type of array</typeparam>
		/// <param name="key">Key</param>
		/// <returns>Saved object</returns>
		public T[] GetSavedArray<T>(string key) => GetSavedObject<T[]>(key);

		/// <summary>
		/// 
		///		Gets saved in storage object by key
		/// 
		/// </summary>
		/// <typeparam name="T">Type of object</typeparam>
		/// <param name="key">Key</param>
		/// <returns>Saved object</returns>
		public T GetSavedObject<T>(string key) => (T)GetSavedObject(key, typeof(T));

		/// <summary>
		/// 
		///		Gets saved in storage array by key and element type info
		/// 
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="info">Element type info</param>
		/// <returns>Saved object</returns>
		public object[] GetSavedArray(string key, Type info) => (object[])GetSavedObject(key, info.MakeArrayType());

		/// <summary>
		/// 
		///		Gets saved in storage object by key and object type info
		/// 
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="info">Element type info</param>
		/// <returns>Saved object</returns>
		public object GetSavedObject(string key, Type info)
		{
			var data = ReadDataFile();
			var rawObject = data[key];

			return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rawObject), info);
		}

		/// <summary>
		/// 
		///		Sets data files path for this saver
		///		*The final data file path: {files path}/{application name}/__{Saver Id}.dat
		/// 
		/// </summary>
		/// <param name="path">Target path</param>
		public void SetDataFilesPath(string path)
		{
			filesPath = path;
		}

		/// <summary>
		/// 
		///		Set application name for all savers
		///		*Default app name is name of assembly
		///		*The final data file path: {files path}/{application name}/__{Saver Id}.dat
		/// 
		/// </summary>
		/// <param name="name">Name of App</param>
		public static void SetApplicationName(string name)
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
			SetDataFilesPath(path);

			Id = ++nextId;

			File = OpenDataFile();
		}


		/// <summary>
		/// 
		///		Data file location
		/// 
		/// </summary>
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

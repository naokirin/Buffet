using System;
using System.IO;
using System.Runtime.Serialization;

namespace RoughlySQLite
{
	static class ValueTranslation
	{
		public static object ToSQLiteValue(this object value, Type t)
		{
			var type = t.ToSQLiteType();

			if (type == SQLiteType.Text && t.IsSerializable)
			{
				return Serialize(value);
			}

			return value;
		}

		static string Serialize(object value)
		{
			using(MemoryStream memoryStream = new MemoryStream())
			using(StreamReader reader = new StreamReader(memoryStream)) {
				DataContractSerializer serializer = new DataContractSerializer(value.GetType());
				serializer.WriteObject(memoryStream, value);
				memoryStream.Position = 0;
				return reader.ReadToEnd();
			}
		}

		static object Deserialize(string xml, Type t)
		{
			using(Stream stream = new MemoryStream())
			{
				byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
				stream.Write(data, 0, data.Length);
				stream.Position = 0;
				DataContractSerializer deserializer = new DataContractSerializer(t);
				return deserializer.ReadObject(stream);
			}
		}
	}
}


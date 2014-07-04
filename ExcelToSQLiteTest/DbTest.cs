using System;
using System.IO;

public class DbTest
{
	public static string GetTempFileName()
	{
		#if NETFX_CORE
		var name = Guid.NewGuid () + ".sqlite";
		return Path.Combine (Windows.Storage.ApplicationData.Current.LocalFolder.Path, name);
		#else
		return Path.GetTempFileName();
		#endif
	}
}


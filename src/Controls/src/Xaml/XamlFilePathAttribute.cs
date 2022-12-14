using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.Maui.Controls.Xaml
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class XamlFilePathAttribute : Attribute
	{
		public XamlFilePathAttribute([CallerFilePath] string filePath = "") => FilePath = filePath;

		public string FilePath { get; }

		internal static string GetFilePathForObject(object view) => (view?.GetType().GetCustomAttributes(typeof(XamlFilePathAttribute), false).FirstOrDefault() as XamlFilePathAttribute)?.FilePath;
	}
}
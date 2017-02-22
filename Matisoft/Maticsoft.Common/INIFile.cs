using System;
using System.Runtime.InteropServices;
using System.Text;
namespace Maticsoft.Common
{
	public class INIFile
	{
		public string path;
		public INIFile(string INIPath)
		{
			this.path = INIPath;
		}
		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string defVal, byte[] retVal, int size, string filePath);
		public void IniWriteValue(string Section, string Key, string Value)
		{
			INIFile.WritePrivateProfileString(Section, Key, Value, this.path);
		}
		public string IniReadValue(string Section, string Key)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			INIFile.GetPrivateProfileString(Section, Key, "", stringBuilder, 255, this.path);
			return stringBuilder.ToString();
		}
		public byte[] IniReadValues(string section, string key)
		{
			byte[] array = new byte[255];
			INIFile.GetPrivateProfileString(section, key, "", array, 255, this.path);
			return array;
		}
		public void ClearAllSection()
		{
			this.IniWriteValue(null, null, null);
		}
		public void ClearSection(string Section)
		{
			this.IniWriteValue(Section, null, null);
		}
	}
}

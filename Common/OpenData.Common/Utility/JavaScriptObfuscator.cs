using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace OpenData.Utility
{
	///<exclude/>
	public class JavaScriptObfuscator
	{
		XmlDocument _obfuscatorFile;

		public JavaScriptObfuscator( XmlDocument obfuscatorFile )
		{
			this._obfuscatorFile = obfuscatorFile;
		}

		public string Execute( string source )
		{
			return ExecuteReplacement( source );
		}

		public string ExecuteReplacement( string source )
		{
			if( this._obfuscatorFile == null )
				return source;

			StringBuilder sbSource = new StringBuilder( source );
			XmlNodeList replaceNodes = this._obfuscatorFile.SelectNodes( "//replacement" );
			foreach( XmlNode node in replaceNodes )
			{
				string target = node.Attributes["target"].Value.ToString();
				string replaceWith = node.Attributes["replaceWith"].Value.ToString();

				sbSource.Replace( target, replaceWith );
			}

			return sbSource.ToString();
		}
	}
}

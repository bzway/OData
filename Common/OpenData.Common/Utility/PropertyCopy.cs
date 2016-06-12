using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace OpenData.Utility
{
    public class PropertyCopy
	{
		#region Set Properties
		public static void SetProperties( PropertyInfo[] fromFields,
										 PropertyInfo[] toFields,
										 object fromRecord,
										 object toRecord )
		{
			PropertyInfo fromField = null;
			PropertyInfo toField = null;

			try
			{
				if( fromFields == null )
				{
					return;
				}
				if( toFields == null )
				{
					return;
				}

				for( int f = 0; f < fromFields.Length; f++ )
				{

					fromField = (PropertyInfo)fromFields[f];

					for( int t = 0; t < toFields.Length; t++ )
					{

						toField = (PropertyInfo)toFields[t];

						if( fromField.Name != toField.Name )
						{
							continue;
						}

						toField.SetValue( toRecord,
										 fromField.GetValue( fromRecord, null ),
										 null );
						break;

					}

				}

			}
			catch( Exception )
			{
				throw;
			}
		}
		#endregion

		#region Set Properties
		public static void SetProperties( PropertyInfo[] fromFields,
										 object fromRecord,
										 object toRecord )
		{
			PropertyInfo fromField = null;

			try
			{

				if( fromFields == null )
				{
					return;
				}

				for( int f = 0; f < fromFields.Length; f++ )
				{

					fromField = (PropertyInfo)fromFields[f];

					fromField.SetValue( toRecord,
									   fromField.GetValue( fromRecord, null ),
									   null );
				}

			}
			catch( Exception )
			{
				throw;
			}
		}
		#endregion


	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace OpenData.Utility
{
	public class ImageUtility
	{

		/// <summary>
		/// ��ȡָ��mimeType��ImageCodecInfo
		/// </summary>
		private static ImageCodecInfo GetImageCodecInfo( string mimeType )
		{
			ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
			foreach( ImageCodecInfo ici in CodecInfo )
			{
				if( ici.MimeType == mimeType ) return ici;
			}
			return null;
		}

		/// <summary>
		///  ��ȡinputStream�е�Bitmap����
		/// </summary>
		public static Bitmap GetBitmapFromStream( Stream inputStream )
		{
			Bitmap bitmap = new Bitmap( inputStream );
			return bitmap;
		}

		/// <summary>
		/// ��Bitmap����ѹ��ΪJPGͼƬ����
		/// </summary>
		/// </summary>
		/// <param name="bmp">Դbitmap����</param>
		/// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
		/// <param name="quality">ѹ��������Խ����ƬԽ�������Ƽ�80</param>
		public static void CompressAsJPG( Bitmap bmp, string saveFilePath, int quality )
		{
			EncoderParameter p = new EncoderParameter( System.Drawing.Imaging.Encoder.Quality, quality ); ;
			EncoderParameters ps = new EncoderParameters( 1 );
			ps.Param[0] = p;
			bmp.Save( saveFilePath, GetImageCodecInfo( "image/jpeg" ), ps );
			bmp.Dispose();
		}

		/// <summary>
		/// ��inputStream�еĶ���ѹ��ΪJPGͼƬ����
		/// </summary>
		/// <param name="inputStream">ԴStream����</param>
		/// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
		/// <param name="quality">ѹ��������Խ����ƬԽ�������Ƽ�80</param>
		public static void CompressAsJPG( Stream inputStream, string saveFilePath, int quality )
		{
			Bitmap bmp = GetBitmapFromStream( inputStream );
			CompressAsJPG( bmp, saveFilePath, quality );
		}


		/// <summary>
		/// ��������ͼ��JPG ��ʽ��
		/// </summary>
		/// <param name="inputStream">����ͼƬ��Stream</param>
		/// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
		/// <param name="width">����ͼ�Ŀ�</param>
		/// <param name="height">����ͼ�ĸ�</param>
		public static void ThumbAsJPG( Stream inputStream, string saveFilePath, int width, int height )
		{
			Image image = Image.FromStream( inputStream );
			if( image.Width == width && image.Height == height )
			{
				CompressAsJPG( inputStream, saveFilePath, 80 );
			}
			int tWidth, tHeight, tLeft, tTop;
			double fScale = (double)height / (double)width;
			if( ( (double)image.Width * fScale ) > (double)image.Height )
			{
				tWidth = width;
				tHeight = (int)( (double)image.Height * (double)tWidth / (double)image.Width );
				tLeft = 0;
				tTop = ( height - tHeight ) / 2;
			}
			else
			{
				tHeight = height;
				tWidth = (int)( (double)image.Width * (double)tHeight / (double)image.Height );
				tLeft = ( width - tWidth ) / 2;
				tTop = 0;
			}
			if( tLeft < 0 ) tLeft = 0;
			if( tTop < 0 ) tTop = 0;

			Bitmap bitmap = new Bitmap( width, height, PixelFormat.Format32bppArgb );
			Graphics graphics = Graphics.FromImage( bitmap );

			//����������������䱳����ɫ
			graphics.Clear( Color.White );
			graphics.DrawImage( image, new Rectangle( tLeft, tTop, tWidth, tHeight ) );
			image.Dispose();
			try
			{
				CompressAsJPG( bitmap, saveFilePath, 80 );
			}
			catch
			{
				;
			}
			finally
			{
				bitmap.Dispose();
				graphics.Dispose();
			}
		}

		/// <summary>
		/// ��Bitmap����ü�Ϊָ��JPG�ļ�
		/// </summary>
		/// <param name="bmp">Դbmp����</param>
		/// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
		/// <param name="x">��ʼ����x����λ������</param>
		/// <param name="y">��ʼ����y����λ������</param>
		/// <param name="width">��ȣ�����</param>
		/// <param name="height">�߶ȣ�����</param>
		public static void CutAsJPG( Bitmap bmp, string saveFilePath, int x, int y, int width, int height )
		{
			int bmpW = bmp.Width;
			int bmpH = bmp.Height;

			if( x >= bmpW || y >= bmpH )
			{
				CompressAsJPG( bmp, saveFilePath, 80 );
				return;
			}

			if( x + width > bmpW )
			{
				width = bmpW - x;
			}

			if( y + height > bmpH )
			{
				height = bmpH - y;
			}

			Bitmap bmpOut = new Bitmap( width, height, PixelFormat.Format24bppRgb );
			Graphics g = Graphics.FromImage( bmpOut );
			g.DrawImage( bmp, new Rectangle( 0, 0, width, height ), new Rectangle( x, y, width, height ), GraphicsUnit.Pixel );
			g.Dispose();
			bmp.Dispose();
			CompressAsJPG( bmpOut, saveFilePath, 80 );
		}

		/// <summary>
		/// ��Stream�еĶ���ü�Ϊָ��JPG�ļ�
		/// </summary>
		/// <param name="inputStream">Դbmp����</param>
		/// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
		/// <param name="x">��ʼ����x����λ������</param>
		/// <param name="y">��ʼ����y����λ������</param>
		/// <param name="width">��ȣ�����</param>
		/// <param name="height">�߶ȣ�����</param>
		public static void CutAsJPG( Stream inputStream, string saveFilePath, int x, int y, int width, int height )
		{
			Bitmap bmp = GetBitmapFromStream( inputStream );
			CutAsJPG( bmp, saveFilePath, x, y, width, height );
		}


		#region ͼƬˮӡ����

		/// <summary>
		/// ��ͼƬ���ͼƬˮӡ
		/// </summary>
		/// <param name="inputStream">����ҪԴͼƬ����</param>
		/// <param name="watermarkPath">ˮӡͼƬ�������ַ</param>
		/// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
		/// <param name="mp">ˮӡλ��</param>
		public static void AddPicWatermarkAsJPG( Stream inputStream, string watermarkPath, string saveFilePath, MarkPosition mp )
		{

			Image image = Image.FromStream( inputStream );
			Bitmap b = new Bitmap( image.Width, image.Height, PixelFormat.Format24bppRgb );
			Graphics g = Graphics.FromImage( b );
			g.Clear( Color.White );
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.High;
			g.DrawImage( image, 0, 0, image.Width, image.Height );

			AddWatermarkImage( g, watermarkPath, mp, image.Width, image.Height );

			try
			{
				CompressAsJPG( b, saveFilePath, 80 );
			}
			catch { ;}
			finally
			{
				b.Dispose();
				image.Dispose();
			}
		}


		/// <summary>
		/// ��ͼƬ���ͼƬˮӡ
		/// </summary>
		/// <param name="sourcePath">ԴͼƬ�Ĵ洢��ַ</param>
		/// <param name="watermarkPath">ˮӡͼƬ�������ַ</param>
		/// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
		/// <param name="mp">ˮӡλ��</param>
		public static void AddPicWatermarkAsJPG( string sourcePath, string watermarkPath, string saveFilePath, MarkPosition mp )
		{
			if( File.Exists( sourcePath ) )
			{
				using( StreamReader sr = new StreamReader( sourcePath ) )
				{
					AddPicWatermarkAsJPG( sr.BaseStream, watermarkPath, saveFilePath, mp );
				}
			}
		}
		/// <summary>
		/// ��ͼƬ�������ˮӡ
		/// </summary>
		/// <param name="inputStream">����ҪԴͼƬ����</param>
		/// <param name="text">ˮӡ����</param>
		/// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
		/// <param name="mp">ˮӡλ��</param>
		public static void AddTextWatermarkAsJPG( Stream inputStream, string text, string saveFilePath, MarkPosition mp )
		{

			Image image = Image.FromStream( inputStream );
			Bitmap b = new Bitmap( image.Width, image.Height, PixelFormat.Format24bppRgb );
			Graphics g = Graphics.FromImage( b );
			g.Clear( Color.White );
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.High;
			g.DrawImage( image, 0, 0, image.Width, image.Height );

			AddWatermarkText( g, text, mp, image.Width, image.Height );

			try
			{
				CompressAsJPG( b, saveFilePath, 80 );
			}
			catch { ;}
			finally
			{
				b.Dispose();
				image.Dispose();
			}
		}

		/// <summary>
		/// ��ͼƬ�������ˮӡ
		/// </summary>
		/// <param name="sourcePath">ԴͼƬ�Ĵ洢��ַ</param>
		/// <param name="text">ˮӡ����</param>
		/// <param name="saveFilePath">Ŀ��ͼƬ�Ĵ洢��ַ</param>
		/// <param name="mp">ˮӡλ��</param>
		public static void AddTextWatermarkAsJPG( string sourcePath, string text, string saveFilePath, MarkPosition mp )
		{
			if( File.Exists( sourcePath ) )
			{
				using( StreamReader sr = new StreamReader( sourcePath ) )
				{
					AddTextWatermarkAsJPG( sr.BaseStream, text, saveFilePath, mp );
				}
			}
		}

		/// <summary>
		/// �������ˮӡ
		/// </summary>
		/// <param name="picture">Ҫ��ˮӡ��ԭͼ��</param>
		/// <param name="text">ˮӡ����</param>
		/// <param name="mp">��ӵ�λ��</param>
		/// <param name="width">ԭͼ��Ŀ��</param>
		/// <param name="height">ԭͼ��ĸ߶�</param>
		private static void AddWatermarkText( Graphics picture, string text, MarkPosition mp, int width, int height )
		{
			int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
			Font crFont = null;
			SizeF crSize = new SizeF();
			for( int i = 0; i < 7; i++ )
			{
				crFont = new Font( "Arial", sizes[i], FontStyle.Bold );
				crSize = picture.MeasureString( text, crFont );

				if( (ushort)crSize.Width < (ushort)width )
					break;
			}

			float xpos = 0;
			float ypos = 0;

			switch( mp )
			{
				case MarkPosition.MP_Left_Top:
					xpos = ( (float)width * (float).01 ) + ( crSize.Width / 2 );
					ypos = (float)height * (float).01;
					break;
				case MarkPosition.MP_Right_Top:
					xpos = ( (float)width * (float).99 ) - ( crSize.Width / 2 );
					ypos = (float)height * (float).01;
					break;
				case MarkPosition.MP_Right_Bottom:
					xpos = ( (float)width * (float).99 ) - ( crSize.Width / 2 );
					ypos = ( (float)height * (float).99 ) - crSize.Height;
					break;
				case MarkPosition.MP_Left_Bottom:
					xpos = ( (float)width * (float).01 ) + ( crSize.Width / 2 );
					ypos = ( (float)height * (float).99 ) - crSize.Height;
					break;
			}

			StringFormat StrFormat = new StringFormat();
			StrFormat.Alignment = StringAlignment.Center;

			SolidBrush semiTransBrush2 = new SolidBrush( Color.FromArgb( 153, 0, 0, 0 ) );
			picture.DrawString( text, crFont, semiTransBrush2, xpos + 1, ypos + 1, StrFormat );

			SolidBrush semiTransBrush = new SolidBrush( Color.FromArgb( 153, 255, 255, 255 ) );
			picture.DrawString( text, crFont, semiTransBrush, xpos, ypos, StrFormat );

			semiTransBrush2.Dispose();
			semiTransBrush.Dispose();

		}

		/// <summary>
		/// ���ͼƬˮӡ
		/// </summary>
		/// <param name="picture">Ҫ��ˮӡ��ԭͼ��</param>
		/// <param name="waterMarkPath">ˮӡ�ļ��������ַ</param>
		/// <param name="mp">��ӵ�λ��</param>
		/// <param name="width">ԭͼ��Ŀ��</param>
		/// <param name="height">ԭͼ��ĸ߶�</param>
		private static void AddWatermarkImage( Graphics picture, string waterMarkPath, MarkPosition mp, int width, int height )
		{
			Image watermark = new Bitmap( waterMarkPath );

			ImageAttributes imageAttributes = new ImageAttributes();
			ColorMap colorMap = new ColorMap();

			colorMap.OldColor = Color.FromArgb( 255, 0, 255, 0 );
			colorMap.NewColor = Color.FromArgb( 0, 0, 0, 0 );
			ColorMap[] remapTable = { colorMap };

			imageAttributes.SetRemapTable( remapTable, ColorAdjustType.Bitmap );

			float[][] colorMatrixElements = {
                                                 new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                             };

			ColorMatrix colorMatrix = new ColorMatrix( colorMatrixElements );

			imageAttributes.SetColorMatrix( colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap );

			int xpos = 0;
			int ypos = 0;
			int WatermarkWidth = 0;
			int WatermarkHeight = 0;
			double bl = 1d;
			if( ( width > watermark.Width * 4 ) && ( height > watermark.Height * 4 ) )
			{
				bl = 1;
			}
			else if( ( width > watermark.Width * 4 ) && ( height < watermark.Height * 4 ) )
			{
				bl = Convert.ToDouble( height / 4 ) / Convert.ToDouble( watermark.Height );

			}
			else

				if( ( width < watermark.Width * 4 ) && ( height > watermark.Height * 4 ) )
				{
					bl = Convert.ToDouble( width / 4 ) / Convert.ToDouble( watermark.Width );
				}
				else
				{
					if( ( width * watermark.Height ) > ( height * watermark.Width ) )
					{
						bl = Convert.ToDouble( height / 4 ) / Convert.ToDouble( watermark.Height );

					}
					else
					{
						bl = Convert.ToDouble( width / 4 ) / Convert.ToDouble( watermark.Width );

					}

				}

			WatermarkWidth = Convert.ToInt32( watermark.Width * bl );
			WatermarkHeight = Convert.ToInt32( watermark.Height * bl );


			switch( mp )
			{
				case MarkPosition.MP_Left_Top:
					xpos = 10;
					ypos = 10;
					break;
				case MarkPosition.MP_Right_Top:
					xpos = width - WatermarkWidth - 10;
					ypos = 10;
					break;
				case MarkPosition.MP_Right_Bottom:
					xpos = width - WatermarkWidth - 10;
					ypos = height - WatermarkHeight - 10;
					break;
				case MarkPosition.MP_Left_Bottom:
					xpos = 10;
					ypos = height - WatermarkHeight - 10;
					break;
			}

			picture.DrawImage( watermark, new Rectangle( xpos, ypos, WatermarkWidth, WatermarkHeight ), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes );


			watermark.Dispose();
			imageAttributes.Dispose();
		}

		/// <summary>
		/// ˮӡ��λ��
		/// </summary>
		public enum MarkPosition
		{
			/// <summary>
			/// ���Ͻ�
			/// </summary>
			MP_Left_Top,

			/// <summary>
			/// ���½�
			/// </summary>
			MP_Left_Bottom,

			/// <summary>
			/// ���Ͻ�
			/// </summary>
			MP_Right_Top,

			/// <summary>
			/// ���½�
			/// </summary>
			MP_Right_Bottom
		}


		#endregion

	}
}


using OpenData.Collections;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Xml;

namespace OpenData.Utility
{
    public class ExcelHelper
    {
        #region
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public static void Export(Stream stream, DataSet dataSet)
        {

            DataTableCollection tables = dataSet.Tables;
            using (XmlTextWriter x = new XmlTextWriter(stream, Encoding.UTF8))
            {
                int sheetNumber = 0;
                x.WriteRaw("<?xml version=\"1.0\"?><?mso-application progid=\"Excel.Sheet\"?>");
                x.WriteRaw("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" ");
                x.WriteRaw("xmlns:o=\"urn:schemas-microsoft-com:office:office\" ");
                x.WriteRaw("xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                x.WriteRaw("<Styles><Style ss:ID='sText'>" +
                           "<NumberFormat ss:Format='@'/></Style>");
                x.WriteRaw("<Style ss:ID='sDate'><NumberFormat" +
                           " ss:Format='[$-409]m/d/yy\\ h:mm\\ AM/PM;@'/>");
                x.WriteRaw("</Style></Styles>");
                foreach (DataTable dt in tables)
                {
                    sheetNumber++;
                    string sheetName = !string.IsNullOrEmpty(dt.TableName) ?
                           dt.TableName : "Sheet" + sheetNumber.ToString();
                    x.WriteRaw("<Worksheet ss:Name='" + sheetName + "'>");
                    x.WriteRaw("<Table>");
                    string[] columnTypes = new string[dt.Columns.Count];

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string colType = dt.Columns[i].DataType.ToString().ToLower();

                        if (colType.Contains("datetime"))
                        {
                            columnTypes[i] = "DateTime";
                            x.WriteRaw("<Column ss:StyleID='sDate'/>");

                        }
                        else if (colType.Contains("string"))
                        {
                            columnTypes[i] = "String";
                            x.WriteRaw("<Column ss:StyleID='sText'/>");

                        }
                        else
                        {
                            x.WriteRaw("<Column />");

                            if (colType.Contains("boolean"))
                            {
                                columnTypes[i] = "Boolean";
                            }
                            else
                            {
                                //default is some kind of number.
                                columnTypes[i] = "Number";
                            }

                        }
                    }
                    //column headers
                    x.WriteRaw("<Row>");
                    foreach (DataColumn col in dt.Columns)
                    {
                        x.WriteRaw("<Cell ss:StyleID='sText'><Data ss:Type='String'>");
                        x.WriteRaw(col.ColumnName);
                        x.WriteRaw("</Data></Cell>");
                    }
                    x.WriteRaw("</Row>");
                    //data
                    bool missedNullColumn = false;
                    foreach (DataRow row in dt.Rows)
                    {
                        x.WriteRaw("<Row>");
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (!row.IsNull(i))
                            {
                                if (missedNullColumn)
                                {
                                    int displayIndex = i + 1;
                                    x.WriteRaw("<Cell ss:Index='" + displayIndex.ToString() +
                                               "'><Data ss:Type='" +
                                               columnTypes[i] + "'>");
                                    missedNullColumn = false;
                                }
                                else
                                {
                                    x.WriteRaw("<Cell><Data ss:Type='" +
                                               columnTypes[i] + "'>");
                                }

                                switch (columnTypes[i])
                                {
                                    case "DateTime":
                                        x.WriteRaw(((DateTime)row[i]).ToString("s"));
                                        break;
                                    case "Boolean":
                                        x.WriteRaw(((bool)row[i]) ? "1" : "0");
                                        break;
                                    case "String":
                                        x.WriteString(row[i].ToString());
                                        break;
                                    default:
                                        x.WriteString(row[i].ToString());
                                        break;
                                }

                                x.WriteRaw("</Data></Cell>");
                            }
                            else
                            {
                                missedNullColumn = true;
                            }
                        }
                        x.WriteRaw("</Row>");
                    }
                    x.WriteRaw("</Table></Worksheet>");
                }
                x.WriteRaw("</Workbook>");
            }
            //Response.End();
        }

        public static void Export(string fileName, DataSet dataSet)
        {
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.Buffer = true;
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("content-disposition",
            //         "attachment; filename=" + fileName + ".xls");
            DataTableCollection tables = dataSet.Tables;
            //using (XmlTextWriter x = new XmlTextWriter(Response.OutputStream, Encoding.UTF8))
            using (XmlTextWriter x = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                int sheetNumber = 0;
                x.WriteRaw("<?xml version=\"1.0\"?><?mso-application progid=\"Excel.Sheet\"?>");
                x.WriteRaw("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" ");
                x.WriteRaw("xmlns:o=\"urn:schemas-microsoft-com:office:office\" ");
                x.WriteRaw("xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                x.WriteRaw("<Styles><Style ss:ID='sText'>" +
                           "<NumberFormat ss:Format='@'/></Style>");
                x.WriteRaw("<Style ss:ID='sDate'><NumberFormat" +
                           " ss:Format='[$-409]m/d/yy\\ h:mm\\ AM/PM;@'/>");
                x.WriteRaw("</Style></Styles>");
                foreach (DataTable dt in tables)
                {
                    sheetNumber++;
                    string sheetName = !string.IsNullOrEmpty(dt.TableName) ?
                           dt.TableName : "Sheet" + sheetNumber.ToString();
                    x.WriteRaw("<Worksheet ss:Name='" + sheetName + "'>");
                    x.WriteRaw("<Table>");
                    string[] columnTypes = new string[dt.Columns.Count];

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string colType = dt.Columns[i].DataType.ToString().ToLower();

                        if (colType.Contains("datetime"))
                        {
                            columnTypes[i] = "DateTime";
                            x.WriteRaw("<Column ss:StyleID='sDate'/>");

                        }
                        else if (colType.Contains("string"))
                        {
                            columnTypes[i] = "String";
                            x.WriteRaw("<Column ss:StyleID='sText'/>");

                        }
                        else
                        {
                            x.WriteRaw("<Column />");

                            if (colType.Contains("boolean"))
                            {
                                columnTypes[i] = "Boolean";
                            }
                            else
                            {
                                //default is some kind of number.
                                columnTypes[i] = "Number";
                            }

                        }
                    }
                    //column headers
                    x.WriteRaw("<Row>");
                    foreach (DataColumn col in dt.Columns)
                    {
                        x.WriteRaw("<Cell ss:StyleID='sText'><Data ss:Type='String'>");
                        x.WriteRaw(col.ColumnName);
                        x.WriteRaw("</Data></Cell>");
                    }
                    x.WriteRaw("</Row>");
                    //data
                    bool missedNullColumn = false;
                    foreach (DataRow row in dt.Rows)
                    {
                        x.WriteRaw("<Row>");
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (!row.IsNull(i))
                            {
                                if (missedNullColumn)
                                {
                                    int displayIndex = i + 1;
                                    x.WriteRaw("<Cell ss:Index='" + displayIndex.ToString() +
                                               "'><Data ss:Type='" +
                                               columnTypes[i] + "'>");
                                    missedNullColumn = false;
                                }
                                else
                                {
                                    x.WriteRaw("<Cell><Data ss:Type='" +
                                               columnTypes[i] + "'>");
                                }

                                switch (columnTypes[i])
                                {
                                    case "DateTime":
                                        x.WriteRaw(((DateTime)row[i]).ToString("s"));
                                        break;
                                    case "Boolean":
                                        x.WriteRaw(((bool)row[i]) ? "1" : "0");
                                        break;
                                    case "String":
                                        x.WriteString(row[i].ToString());
                                        break;
                                    default:
                                        x.WriteString(row[i].ToString());
                                        break;
                                }

                                x.WriteRaw("</Data></Cell>");
                            }
                            else
                            {
                                missedNullColumn = true;
                            }
                        }
                        x.WriteRaw("</Row>");
                    }
                    x.WriteRaw("</Table></Worksheet>");
                }
                x.WriteRaw("</Workbook>");
            }
            //Response.End();
        }

        public static DataSet ParseExcel(string path, string sheetname = "")
        {
            DataSet ds = new DataSet();
            OleDbConnection objConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0 Xml;imex=1;hdr=YES\";");
            objConn.Open();
            if (string.IsNullOrEmpty(sheetname))
            {
                sheetname = GetAllSheetName(path)[0];
            }
            string sql = "select * from  [" + sheetname + "]";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, objConn);
            try
            {
                adapter.Fill(ds);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                objConn.Close();
            }
        }

        #region 获取所有Excel sheet 名称
        public static List<string> GetAllSheetName(string path)
        {
            List<string> list = new List<string>();
            OleDbConnection objConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0 Xml;imex=1;hdr=YES\";");
            objConn.Open();
            try
            {
                DataTable sheetNames = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                foreach (DataRow dr in sheetNames.Rows)
                {
                    list.Add(dr[2].ToString());
                }
                return list;
            }
            finally
            {
                objConn.Close();
            }
        }
        #endregion


        public static DataSet GetDataFromExcel(string filePath)
        {
            DataSet ds = new DataSet();
            try
            {
                string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";

                using (OleDbConnection oleDB = new OleDbConnection(strConn))
                {
                    oleDB.Open();
                    var dt = oleDB.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return ds;
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        var excelSheetName = row["TABLE_NAME"].ToString().Replace("'", "");
                        var query = String.Format("select * from [{0}]", excelSheetName);
                        OleDbDataAdapter da = new OleDbDataAdapter(query, strConn);
                        da.Fill(ds, excelSheetName);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("GetDataFromExcel", ex);
            }
            return ds;
        }


        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <returns>DataSet</returns>
        public static DataSet ImportDataSetFromExcel(Stream excelFileStream, int headerRowIndex)
        {
            DataSet ds = new DataSet();
            HSSFWorkbook workbook = new HSSFWorkbook(excelFileStream);
            for (int a = 0, b = workbook.NumberOfSheets; a < b; a++)
            {
                var sheet = workbook.GetSheetAt(a);
                DataTable table = new DataTable(sheet.SheetName);
                var headerRow = sheet.GetRow(headerRowIndex);
                int cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    if (headerRow.GetCell(i) == null || headerRow.GetCell(i).StringCellValue.Trim() == "")
                    {
                        // 如果遇到第一个空列，则不再继续向后读取
                        cellCount = i + 1;
                        break;
                    }
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null || row.GetCell(0) == null || row.GetCell(0).ToString().Trim() == "")
                    {
                        // 如果遇到第一个空行，则不再继续向后读取
                        break;
                    }
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null)
                        {
                            continue;
                        }
                        dataRow[j] = row.GetCell(j).ToString();
                    }
                    table.Rows.Add(dataRow);
                }
                ds.Tables.Add(table);
            }
            excelFileStream.Close();
            workbook = null;
            return ds;
        }
        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <returns>DataSet</returns>
        public static DataSet ImportDataSetFromExcel(string excelFilePath, int headerRowIndex)
        {
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                return ImportDataSetFromExcel(stream, headerRowIndex);
            }
        }


    }
}
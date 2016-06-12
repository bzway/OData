
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
public class HttpRequestClient
{
    #region //�ֶ�
    private ArrayList bytesArray;
    private Encoding encoding = Encoding.UTF8;
    private string boundary = String.Empty;
    #endregion

    #region //���췽��
    public HttpRequestClient()
    {
        bytesArray = new ArrayList();
        string flag = DateTime.Now.Ticks.ToString("x");
        boundary = "---------------------------" + flag;
    }
    #endregion

    #region //����
    /// <summary>
    /// �ϲ���������
    /// </summary>
    /// <returns></returns>
    private byte[] MergeContent()
    {
        int length = 0;
        int readLength = 0;
        string endBoundary = "--" + boundary + "--\r\n";
        byte[] endBoundaryBytes = encoding.GetBytes(endBoundary);

        bytesArray.Add(endBoundaryBytes);

        foreach (byte[] b in bytesArray)
        {
            length += b.Length;
        }

        byte[] bytes = new byte[length];

        foreach (byte[] b in bytesArray)
        {
            b.CopyTo(bytes, readLength);
            readLength += b.Length;
        }

        return bytes;
    }

    /// <summary>
    /// �ϴ�
    /// </summary>
    /// <param name="requestUrl">����url</param>
    /// <param name="responseText">��Ӧ</param>
    /// <returns></returns>
    public bool Upload(String requestUrl, out String responseText)
    {
        WebClient webClient = new WebClient();
        webClient.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);

        byte[] responseBytes;
        byte[] bytes = MergeContent();

        try
        {
            responseBytes = webClient.UploadData(requestUrl, bytes);
            responseText = System.Text.Encoding.UTF8.GetString(responseBytes);
            return true;
        }
        catch (WebException ex)
        {
            Stream responseStream = ex.Response.GetResponseStream();
            responseBytes = new byte[ex.Response.ContentLength];
            responseStream.Read(responseBytes, 0, responseBytes.Length);
        }
        responseText = System.Text.Encoding.UTF8.GetString(responseBytes);
        return false;
    }

    /// <summary>
    /// ���ñ������ֶ�
    /// </summary>
    /// <param name="fieldName">�ֶ���</param>
    /// <param name="fieldValue">�ֶ�ֵ</param>
    /// <returns></returns>
    public void SetFieldValue(String fieldName, String fieldValue)
    {
        string httpRow = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
        string httpRowData = String.Format(httpRow, fieldName, fieldValue);

        bytesArray.Add(encoding.GetBytes(httpRowData));
    }

    /// <summary>
    /// ���ñ��ļ�����
    /// </summary>
    /// <param name="fieldName">�ֶ���</param>
    /// <param name="filename">�ֶ�ֵ</param>
    /// <param name="contentType">��������</param>
    /// <param name="fileBytes">�ļ��ֽ���</param>
    /// <returns></returns>
    public void SetFieldValue(String fieldName, String filename, String contentType, Byte[] fileBytes)
    {
        string end = "\r\n";
        string httpRow = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
        string httpRowData = String.Format(httpRow, fieldName, filename, contentType);

        byte[] headerBytes = encoding.GetBytes(httpRowData);
        byte[] endBytes = encoding.GetBytes(end);
        byte[] fileDataBytes = new byte[headerBytes.Length + fileBytes.Length + endBytes.Length];

        headerBytes.CopyTo(fileDataBytes, 0);
        fileBytes.CopyTo(fileDataBytes, headerBytes.Length);
        endBytes.CopyTo(fileDataBytes, headerBytes.Length + fileBytes.Length);

        bytesArray.Add(fileDataBytes);
    }
    #endregion
}


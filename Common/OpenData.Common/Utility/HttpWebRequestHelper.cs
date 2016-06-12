/*
 *	Author: Weijie.JIN 
 *  Date:	2009-7-1
 *	Usage:	protected void btnTestGet_Click( object sender, EventArgs e )
			{
				HttpHelper hh = new HttpHelper( new Uri( "http://www.google.cn" ) );
				WebProxy wp = new WebProxy( "proxy-server", 8080 );
				wp.Credentials = new NetworkCredential( "weijie", "pass" );
				hh.Proxy = wp;
				Response.Write( hh.Execute() );
			}

			protected void btnTestPost_Click( object sender, EventArgs e )
			{
				HttpHelper hh = new HttpHelper( new Uri( "http://localhost:10906/TestPostHandler.aspx" ) );
				hh.Method = "POST";
				hh.PostValues.Add( "hello", "world" );
				Response.Write( hh.Execute() );
			}
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace OpenData.Utility
{
    public class HttpWebRequestHelper
    {
        #region Event (for async mode use)

        public event HttpResponseCompleteEventHandler ResponseComplete;
        private void OnResponseComplete(HttpResponseCompleteEventArgs e)
        {
            if (this.ResponseComplete != null)
            {
                this.ResponseComplete(e);
            }
        }


        #endregion

        #region properties

        #region Request

        private HttpWebRequest _request;

        public HttpWebRequest Request
        {
            get { return _request; }
            set { _request = value; }
        }

        #endregion

        #region PostValues

        private Dictionary<string, string> _postValues = new Dictionary<string, string>();
        public Dictionary<string, string> PostValues
        {
            get
            {
                return this._postValues;
            }
            set
            {
                this._postValues = value;
            }
        }

        #endregion

        #region PostDataString (do not use this properties, for async mode)

        private string _postDataString = string.Empty;

        public string PostDataString
        {
            get { return _postDataString; }
            set { _postDataString = value; }
        }


        #endregion

        #region Proxy

        private IWebProxy _proxy;
        public IWebProxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        #endregion

        #region Method (string)

        private string _method = "GET";
        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }

        #endregion

        #region Encoding

        private Encoding _encoding = Encoding.UTF8;
        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        #endregion

        #region EnableAdditionalParam

        private bool _enableAdditionalParam = true;

        public bool EnableAdditionalParam
        {
            get { return _enableAdditionalParam; }
            set { _enableAdditionalParam = value; }
        }


        #endregion

        #region Credentials

        private ICredentials _credentials;
        public ICredentials Credentials
        {
            get { return _credentials; }
            set { _credentials = value; }
        }

        #endregion

        private bool _enableAsyncBehavior = false;

        public bool EnableAsyncBehavior
        {
            get { return _enableAsyncBehavior; }
            set { _enableAsyncBehavior = value; }
        }


        #endregion

        #region Constructors

        public HttpWebRequestHelper(Uri requestUri)
        {
            this._request = (HttpWebRequest)WebRequest.Create(requestUri);
        }

        public HttpWebRequestHelper(Uri requestUri, string method)
            : this(requestUri)
        {
            this._method = method;
        }

        public HttpWebRequestHelper(Uri requestUri, string method, Encoding encoding)
            : this(requestUri, method)
        {
            this._encoding = encoding;
        }

        #endregion

        #region Execute

        public string Execute()
        {
            #region prepare request

            //set method
            this._request.Method = this.Method;

            //set proxy
            if (this._proxy != null)
                this._request.Proxy = this._proxy;

            //set credentials
            if (this._credentials != null)
                this._request.Credentials = this._credentials;

            //additional params
            if (this._enableAdditionalParam)
            {
                this._request.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                this._request.Accept = "*/*";
                this._request.KeepAlive = true;
                this._request.Headers.Add("Accept-Language", "en-us,zh-cn;q=0.5");
            }

            if (this.Method == "POST")
            {
                this._request.ContentType = "application/x-www-form-urlencoded";

                #region handle post data
                StringBuilder sbPostData = new StringBuilder();

                foreach (KeyValuePair<string, string> item in this.PostValues)
                {
                    sbPostData.Append(string.Format("{0}={1}&", item.Key, HttpUtility.UrlEncode(item.Value)));
                }

                if (sbPostData.Length > 0)
                    sbPostData.Remove(sbPostData.Length - 1, 1);

                #endregion

                byte[] bytes = Encoding.ASCII.GetBytes(sbPostData.ToString());
                this._request.ContentLength = bytes.Length;

                using (StreamWriter writer = new StreamWriter(this._request.GetRequestStream()))
                {
                    writer.Write(sbPostData.ToString());
                }

            }



            #endregion

            #region get response
            if (this._enableAsyncBehavior)
            {
                this._request.BeginGetResponse(new AsyncCallback(HttpWebRequestHelper.BeginResponse), this);
            }
            else
            {
                HttpWebResponse response = null;

                try
                {
                    response = (HttpWebResponse)this._request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            if (stream != null)
                            {
                                using (StreamReader reader = new StreamReader(stream, this._encoding))
                                {
                                    return reader.ReadToEnd();
                                }
                            }
                        }
                    }
                    else
                        return response.StatusCode.ToString();

                }
                catch { throw; }
                finally { if (null != response) { response.Close(); } }
            }

            return null;

            #endregion
        }

        #endregion

        #region BeginResponse

        /*
		private static void BeginRequest( IAsyncResult ar )
		{
			HttpHelper helper = ar.AsyncState as HttpHelper;
			if( helper != null )
			{
				if( helper.Method == "POST" )
				{
					using( StreamWriter writer = new StreamWriter( helper.Request.EndGetRequestStream( ar ) ) )
					{

						writer.Write( helper.PostDataString );
					}
				}

				helper.Request.BeginGetResponse( new AsyncCallback( HttpHelper.BeginResponse ), helper );
			}
		}
		*/

        private static void BeginResponse(IAsyncResult ar)
        {
            HttpWebRequestHelper helper = ar.AsyncState as HttpWebRequestHelper;
            if (helper != null)
            {
                HttpWebResponse response = (HttpWebResponse)helper.Request.EndGetResponse(ar);
                if (response != null)
                {
                    Stream stream = response.GetResponseStream();
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            helper.OnResponseComplete(new HttpResponseCompleteEventArgs(reader.ReadToEnd()));
                        }
                    }
                }
            }
        }


        #endregion

    }

    public delegate void HttpResponseCompleteEventHandler(HttpResponseCompleteEventArgs e);
    public class HttpResponseCompleteEventArgs : EventArgs
    {
        private string _response = string.Empty;

        public string Response
        {
            get { return _response; }
            set { _response = value; }
        }


        public HttpResponseCompleteEventArgs(string response)
        {
            this.Response = response;
        }
    }
}

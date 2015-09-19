using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Http
{
	public enum HttpRestMethods
	{
		GET,
		POST,
		DELETE,
		PATCH,
		PUT
	}
	
	public class Parameter
	{
		public string FieldName;
	}
	
	class IntParameter : Parameter
	{
		public int Value;
	}
	
	class StringParameter : Parameter
	{
		public string Value;
		public System.Text.Encoding Encoding;
	}
	

	public class HttpForm
	{
		private HttpRestMethods _method = HttpRestMethods.GET;
		private Dictionary<Parameter, string> _parameters = new Dictionary<Parameter, string>();
		private Dictionary<string,string> _headers = new Dictionary<string,string>();
		private byte[] _binaryData;
		private string mimeType = "application/json";
		private string binaryFileName = "bin";
		public HttpRestMethods Method
		{
			get { return _method; }
			set { _method = value; checkMethod(); }
		}

		public string BinaryFileName
		{
			get { return binaryFileName; }
			set { binaryFileName = value; }
		}

		public string MimeType
		{
			get { return mimeType; }
			set { mimeType = value; }
		}
		
		public byte[] Data
		{
			get { return _binaryData; }
		}
		
		public Dictionary<string,string> Headers
		{
			get { return _headers; }
		}
		
		public Dictionary<Parameter, string> Parameters
		{
			get { return _parameters; }
		}
		
		public void AddBinaryData(byte[] data)
		{
			_binaryData = data;
			if(_method == HttpRestMethods.GET)
				_method = HttpRestMethods.POST;
			
			checkMethod();
		}
		
		public void AddParameter(string fieldName, int val)
		{
			IntParameter param = new IntParameter();
			param.FieldName = fieldName;
			param.Value = val;
			_parameters[param] = "int";
			checkMethod();
		}
		
		public void AddParameter(string fieldName, string val)
		{
			AddParameter(fieldName, val, new System.Text.ASCIIEncoding());
			checkMethod();
		}
		
		public void AddParameter(string fieldName, string val, System.Text.Encoding encoding)
		{
			StringParameter param = new StringParameter();
			param.FieldName = fieldName;
			param.Value = val;
			param.Encoding = encoding;
			_parameters[param] = "string";
			checkMethod();
		}
		
		void checkMethod()
		{
			if( 
				((_parameters != null && _parameters.Count > 0)
				|| _binaryData != null 
				|| (_headers != null && _headers.Count > 0)) 
				)
			{
				if(_method == HttpRestMethods.GET)
					_method = HttpRestMethods.POST;
			}
			else
			{
				if(_method != HttpRestMethods.GET)
				{
					_method = HttpRestMethods.GET;
				}
			}
		}
	}
	
	public class HttpRequest
	{
		private string _url;
		private byte[] _postData;
		private HttpRestMethods _method;
		private Dictionary<string,string> _customHeaders;
		
		private HttpForm _form;

		private static UpdateHelper _updateHelper = null;
	
		//@PLATFORM_SPECIFIC
		WWW _www;

		public string ErrorText
		{
			get { return string.IsNullOrEmpty(_www.error) ? "" : _www.error; }
		}

		public int StatusCode
		{
			get 
			{ 
				if(isDone == false)
					return -1;
				else
				{
					if(string.IsNullOrEmpty(_www.error))
						return 200;
					else
					{
						string[] splitted = _www.error.Split(' ');

						int statCode = -1;
						int.TryParse(splitted[0], out statCode);
						return statCode;
					}
				}
			}
		}

		public bool Success
		{
			get 
			{
				if(isDone)
				{
					int sc = StatusCode;
					if(sc >= 200 && sc < 300)
						return true;
					else
						return false;
				}
				else
					return false;
			}
		}

		public bool isDone
		{
			get
			{
				return _www.isDone;
			}
		}
		
		public string Text
		{
			get
			{
				return _www.text;
			}
		}

		public byte[] Data
		{
			get { return _www.bytes; }
		}

		public Texture2D Image
		{
			get { return _www.texture; }
		}

		public delegate void requestDlg(HttpRequest request);
		public event requestDlg OnRequestDone;
		
		public HttpRequest(string url, byte[] postData, Dictionary<string,string> customHeaders)
		{
			_url = url;
			_postData = postData;
			_customHeaders = customHeaders;
			initUpdater();
			createRequest(_url, _postData, _customHeaders);
		}
		
		public HttpRequest(string url)
		{
			initUpdater();
			createRequest(url);
		}
		
		public HttpRequest(string url, HttpForm form)
		{
			_url = url;
			_form = form;
			initUpdater();
			createRequest(_url, _form);
		}
		
		void createRequest(string url)
		{
			_www = new WWW(url);
		}
		
		void createRequest(string url, byte[] postData, Dictionary<string,string> headers)
		{
			_www = new WWW(url, postData, headers);
		}
		
		void createRequest(string url, HttpForm form)
		{
			string methodStringValue = "";
			
			switch(form.Method)
			{
			case HttpRestMethods.GET:
				methodStringValue = "GET";
				break;
				
			case HttpRestMethods.POST:
				methodStringValue = "POST";
				break;
				
			case HttpRestMethods.DELETE:
				methodStringValue = "DELETE";
				break;
				
			case HttpRestMethods.PUT:
				methodStringValue = "PUT";
				break;
				
			case HttpRestMethods.PATCH:
				methodStringValue = "PATCH";
				break;
				
			default:
				methodStringValue = "GET";
				break;
			}
			
			// @PLATFORM_SPECIFIC
			if(form.Method == HttpRestMethods.GET)
			{
				_www = new WWW(url);
			}
			else
			{
				WWWForm wwwform = new WWWForm();
				
				if(form.Data != null)
					wwwform.AddBinaryData("binary", form.Data, form.BinaryFileName, form.MimeType);
				
				wwwform.AddField("_method", methodStringValue);
				
				foreach (KeyValuePair<Parameter, string> kv in form.Parameters)
				{
					switch(kv.Value)
					{
					case "string":
						wwwform.AddField(kv.Key.FieldName, (kv.Key as StringParameter).Value, (kv.Key as StringParameter).Encoding);
						break;
						
					case "int":
						wwwform.AddField(kv.Key.FieldName, (kv.Key as IntParameter).Value);
						break;
						
					default:
						break;
					}
				}
				
				foreach (KeyValuePair<string, string> kv in form.Headers)
				{
					wwwform.headers.Add(kv.Key, kv.Value);
				}
				
				_www = new WWW(url, wwwform);
			}
		}
		
		private void updateFunc()
		{
			if(isDone)
			{
				if(OnRequestDone != null) OnRequestDone(this);

				_updateHelper.OnUpdate -= updateFunc;
			}
		}

		private void initUpdater()
		{
			if(_updateHelper == null)
			{
				GameObject go = new GameObject();
				go.hideFlags = HideFlags.HideAndDontSave; 
				_updateHelper = go.AddComponent<UpdateHelper>();
			}

			_updateHelper.OnUpdate += updateFunc;
		}
		
	}
	
	public class HttpApi
	{
		public static HttpRequest Request(string url)
		{
			return new HttpRequest(url);
		}
		
		public static HttpRequest Request(string url, byte[] postData)
		{
			return new HttpRequest(url, postData, null);
		}
		
		public static HttpRequest Request(string url, HttpForm form)
		{
			return new HttpRequest(url, form);
		}
		
		public static HttpRequest Request(string url, byte[] postData, Dictionary<string,string> customHeaders)
		{
			return new HttpRequest(url, postData, customHeaders);
		}
	}
}
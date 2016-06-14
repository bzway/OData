using OpenData.Data.Core;
using OpenData.Utility;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using OpenData.Framework.Core.Wechat.Models;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Drawing;
using System.Security.Cryptography;
using OpenData.Framework.Core.Entity;
using OpenData.Caching;
using Newtonsoft.Json;

namespace OpenData.Framework.Core
{

    public class WechatManager
    {

        #region ctor
        static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        readonly ICacheManager cache = ApplicationEngine.Current.Resolve<ICacheManager>();

        readonly IDatabase db;
        public WechatManager(IDatabase db, string uuid)
        {
            this.db = db;
            if (!cache.IsSet(uuid))
            {
                var wechat = db.Entity<WechatOfficialAccount>().Query()
                    .Where(m => m.Id, uuid, CompareType.Equal)
                    .First();
                this.CurrentWechat = wechat;
                cache.Set(uuid, this.CurrentWechat, 20);
            }
            else
            {
                this.CurrentWechat = cache.Get<WechatOfficialAccount>(uuid);
            }
        }
        #endregion

        #region 基础信息

        public WechatOfficialAccount CurrentWechat { get; private set; }


        public string TryGetAccessToken(bool refresh = true)
        {
            var key = "AccessToken_" + this.CurrentWechat.Id;

            if (refresh || !cache.IsSet(key))
            {
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + this.CurrentWechat.AppID + "&secret=" + this.CurrentWechat.AppSecret;
                string response = HttpGet(url);
                var result = JsonConvert.DeserializeObject<AccessTokenResultModel>(response);
                if (string.IsNullOrEmpty(result.access_token))
                {
                    return string.Empty;
                }
                cache.Set(key, result.access_token, result.expires_in);

            }
            return cache.Get<string>(key);
        }
        public string TryGetJsApiTicket(bool refresh = true)
        {
            var key = "JsApiTicket_" + this.CurrentWechat.Id;

            if (refresh || !cache.IsSet(key))
            {
                var url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + this.TryGetAccessToken(false) + "&type=jsapi";
                var response = HttpGet(url);
                var jsonObject = JsonConvert.DeserializeObject<JSAPITicketResultModel>(response);
                if (string.IsNullOrEmpty(jsonObject.ticket))
                {
                    return string.Empty;
                }
                cache.Set(key, jsonObject.ticket, jsonObject.expires_in);
            }
            return cache.Get<string>(key);
        }

        #region Auth

        public string TryGetAuthorizeUrl(string redirect_uri, string state, string scope = "snsapi_base")
        {
            redirect_uri = System.Web.HttpUtility.UrlEncode(redirect_uri);
            state = System.Web.HttpUtility.UrlEncode(state);
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect", this.CurrentWechat.AppID, redirect_uri, scope, state);
        }

        public UserAccessTokenResultModel GetOpenID(string code)
        {
            var url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + this.CurrentWechat.AppID + "&secret=SECRET&code=" + code + "&grant_type=authorization_code";
            var response = HttpGet(url);
            var jsonObject = JsonConvert.DeserializeObject<UserAccessTokenResultModel>(response);
            jsonObject.errmsg = response;
            return jsonObject;
        }
        #endregion
        public string GetJsApiSignature(string noncestr, string timestamp, string url)
        {
            var jsapi_ticket = TryGetJsApiTicket();

            var arrString = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&relatedUrl={3}", jsapi_ticket, noncestr, timestamp, url);
            var sha1 = SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            return enText.ToString();
        }

        public List<string> GetWechatIpList()
        {
            var url = "https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var response = this.HttpGet(url);
            var jsonObject = JsonConvert.DeserializeObject<WechatGetWechatIpListJsonResultModel>(response);

            return jsonObject.ip_list;
        }

        #region Get


        /// <summary>  
        /// HTTP GET方式请求数据.  
        /// </summary>  
        /// <param name="url">URL.</param>  
        /// <returns></returns>  
        private string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStr;
        }
        #endregion

        #region POST
        /// <summary>  
        /// HTTP POST方式请求数据  
        /// </summary>  
        /// <param name="url">URL.</param>  
        /// <param name="param">POST的数据</param>  
        /// <returns></returns>  
        public string HttpPost(string url, string param)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Proxy = WebProxyHelper.CreateWebProxy();
                wc.Encoding = Encoding.UTF8;
                //WebProxyHelper.CancelCertificateValidate();

                var respones = wc.UploadString(url, "POST", param);
                return respones;
            }

            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.Accept = "*/*";
            ////request.Timeout = 15000;
            ////request.AllowAutoRedirect = false;
            //WebResponse response = null;
            //string responseStr = null;

            //try
            //{
            //    response = request.GetResponse();

            //    if (response != null)
            //    {
            //        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            //        responseStr = reader.ReadToEnd();
            //        reader.Close();
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //finally
            //{
            //    request = null;
            //    response = null;
            //}

            //return responseStr;

        }
        /// <summary>
        /// HTTP POST方式文件数据  
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileForm"></param>
        /// <param name="filePath"></param>
        /// <param name="forms"></param>
        /// <returns></returns>
        public string HttpPost(string url, string fileForm, string filePath, params  WechatMaterialRequestModel[] forms)
        {

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            byte[] fileBytes = new byte[fs.Length];
            fs.Read(fileBytes, 0, fileBytes.Length);
            fs.Close();
            fs.Dispose();

            HttpRequestClient httpRequestClient = new HttpRequestClient();
            foreach (var item in forms)
            {
                httpRequestClient.SetFieldValue(item.Field, item.Value);
            }

            httpRequestClient.SetFieldValue(fileForm, Path.GetFileName(filePath), "application/octet-stream", fileBytes);
            string responseText;
            bool uploaded = httpRequestClient.Upload(url, out responseText);
            return responseText;
        }
        #endregion

        #endregion

        #region 菜单
        public string CreateMenu(List<WechatMenu> menuList)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new Menu() { button = new List<Menu.BaseButton>() };
            foreach (var item in menuList.Where(m => string.IsNullOrEmpty(m.ParentId)).Take(3))
            {
                var button = new Menu.BaseButton() { type = item.Type, name = item.Name, sub_button = new List<Menu.BaseButton>() };
                foreach (var subItem in menuList.Where(m => m.ParentId == item.Id).Take(5))
                {
                    switch (subItem.Type)
                    {
                        case "click":
                            button.sub_button.Add(new Menu.BaseButton { type = subItem.Type, name = subItem.Name, key = subItem.Key });
                            break;
                        case "view":
                            button.sub_button.Add(new Menu.BaseButton { type = subItem.Type, name = subItem.Name, url = subItem.Url });
                            break;
                        case "scancode_push":
                            button.sub_button.Add(new Menu.BaseButton { type = subItem.Type, name = subItem.Name, url = subItem.Url });
                            break;
                        case "scancode_waitmsg":
                            button.sub_button.Add(new Menu.BaseButton { type = subItem.Type, name = subItem.Name });
                            break;
                        case "pic_sysphoto":
                            button.sub_button.Add(new Menu.BaseButton { type = subItem.Type, name = subItem.Name });
                            break;
                        case "pic_photo_or_album":
                            button.sub_button.Add(new Menu.BaseButton { type = subItem.Type, name = subItem.Name });
                            break;
                        case "pic_weixin":
                            button.sub_button.Add(new Menu.BaseButton { type = subItem.Type, name = subItem.Name });

                            break;
                        case "location_select":
                            button.sub_button.Add(new Menu.BaseButton { type = subItem.Type, name = subItem.Name });
                            break;
                        case "media_id":
                            button.sub_button.Add(new Menu.BaseButton { type = subItem.Type, name = subItem.Name, media_id = subItem.MediaId });
                            break;
                        case "view_limited":
                            button.sub_button.Add(new Menu.BaseButton { type = subItem.Type, name = subItem.Name, media_id = subItem.MediaId });
                            break;
                        default:
                            button.sub_button.Add(new Menu.BaseButton { type = subItem.Type, name = subItem.Name, key = subItem.Key });
                            break;
                    }

                }
                switch (item.Type)
                {
                    case "click":
                        button.key = item.Key;
                        break;
                    case "view":
                        button.url = item.Url;
                        break;
                    case "scancode_push":
                        button.url = item.Url;
                        break;
                    case "scancode_waitmsg":
                        break;
                    case "pic_sysphoto":
                        break;
                    case "pic_photo_or_album":
                        break;
                    case "pic_weixin":
                        break;
                    case "location_select":
                        break;
                    case "media_id":
                        button.media_id = item.MediaId;
                        break;
                    case "view_limited":
                        button.media_id = item.MediaId;
                        break;
                    default:
                        break;

                }
                model.button.Add(button);
            }

            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return result.errmsg;
        }

        public bool GetMenu()
        {

            var url = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var response = HttpGet(url);
            var jsonObject = JsonConvert.DeserializeObject<WechatGetWechatMenuJsonResultModel>(response);

            if (jsonObject.menu == null)
            {
                return false;
            }
            foreach (var item in jsonObject.menu.button)
            {
                var menu = this.db.Entity<WechatMenu>().Query()
                     .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                     .Where(m => m.ParentId, string.Empty, CompareType.Equal)
                     .Where(m => m.Name, item.name, CompareType.Equal)
                     .First();
                if (menu == null)
                {

                    menu = new WechatMenu()
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Name = item.name,
                        OfficialAccount = this.CurrentWechat.Id,
                        ParentId = string.Empty,
                        MediaId = item.media_id,
                        Url = item.url,
                        Key = item.key,
                        Type = item.type,
                    };
                    this.db.Entity<WechatMenu>().Insert(menu);
                }
                else
                {
                    menu.MediaId = item.media_id;
                    menu.Url = item.url;
                    menu.Key = item.key;
                    menu.Type = item.type;
                    this.db.Entity<WechatMenu>().Update(menu);
                }
                foreach (var subItem in item.sub_button)
                {
                    var subMenu = this.db.Entity<WechatMenu>().Query()
                                    .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                                    .Where(m => m.ParentId, menu.Id, CompareType.Equal)
                                    .Where(m => m.Name, subItem.name, CompareType.Equal)
                                    .First();
                    if (subMenu == null)
                    {
                        this.db.Entity<WechatMenu>().Insert(new WechatMenu()
                        {
                            Name = subItem.name,
                            OfficialAccount = this.CurrentWechat.Id,
                            ParentId = menu.Id,
                            MediaId = subItem.media_id,
                            Url = subItem.url,
                            Key = subItem.key,
                            Type = subItem.type,
                        });
                    }
                    else
                    {
                        subMenu.MediaId = subItem.media_id;
                        subMenu.Url = subItem.url;
                        subMenu.Key = subItem.key;
                        subMenu.Type = subItem.type;
                        this.db.Entity<WechatMenu>().Update(subMenu);
                    }
                }
            }
            return true;
        }
        #endregion

        #region 粉丝管理
        /// <summary>
        /// 创建分组 - 一个公众账号，最多支持创建100个分组
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public bool CreateUserGroup(string groupName)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));
            var model = new { group = new { name = groupName } };

            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var jsonObject = JsonConvert.DeserializeObject<WechatCreateUserGroupJsonResultModel>(response);
            if (jsonObject.errcode == "40001")
            {
                TryGetAccessToken();
                return CreateUserGroup(groupName);
            }
            if (string.IsNullOrEmpty(jsonObject.errcode))
            {
                this.db.Entity<WechatUserGroup>().Insert(new WechatUserGroup());
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 查询所有分组
        /// </summary>
        /// <returns></returns>
        public List<WechatUserGroup> GetGroupList()
        {
            List<WechatUserGroup> list = new List<WechatUserGroup>();
            var url = "https://api.weixin.qq.com/cgi-bin/groups/get?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));
            var response = HttpGet(url);

            var result = JsonConvert.DeserializeObject<WechatGetGroupModel>(response);
            if (result.errcode == "40001")
            {
                TryGetAccessToken();
                return GetGroupList();
            }
            foreach (var item in result.groups)
            {
                list.Add(new WechatUserGroup()
                {
                    GroupId = item.id,
                    Name = item.name,
                    OfficialAccount = this.CurrentWechat.Id,
                });
            }

            return list;
        }
        /// <summary>
        /// 查询用户所在分组
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public int GetUserGroup(string openId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/groups/getid?access_token=" + this.TryGetAccessToken(false);
            var model = new { openid = openId };
            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var jsonObject = JsonConvert.DeserializeObject<WechatGetUserGroupResultModel>(response);
            if (jsonObject.errcode == "40001")
            {
                return GetUserGroup(openId);
            }
            return jsonObject.groupid;
        }

        /// <summary>
        /// 修改分组名
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool ChangeGroupName(string groupId, string groupName)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/groups/update?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { group = new { id = groupId, name = groupName } };
            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            if (result.errcode == "40001")
            {
                return ChangeGroupName(groupId, groupName);
            }
            return true;
        }

        /// <summary>
        /// 移动用户分组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public bool ChangeUserGroup(string groupId, string openId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { to_groupid = groupId, openid = openId };
            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            if (result.errcode == "40001")
            {
                return ChangeUserGroup(groupId, openId);
            }
            if (result.errcode == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 批量移动用户分组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public bool ChangeUserGroup(string groupId, List<string> openIds)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/groups/members/batchupdate?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));
            if (openIds.Count > 50)
            {
                return false;
            }
            var model = new { to_groupid = groupId, openid_list = openIds };
            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            if (result.errcode == "40001")
            {
                return ChangeUserGroup(groupId, openIds);
            }
            if (result.errcode == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool DeleteGroup(string groupId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/groups/delete?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { group = new { id = groupId } };
            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            if (result.errcode == "40001")
            {
                return DeleteGroup(groupId);
            }
            if (result.errcode == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 设置备注名
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool SetUserRemark(string openId, string remark)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));
            var model = new { openid = openId, remark = remark };
            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            if (result.errcode == "40001")
            {
                return SetUserRemark(openId, remark);
            }
            if (result.errcode == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public WechatUser GetUserInfo(string openId)
        {
            try
            {

                var url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + this.TryGetAccessToken(false) + "&openid=" + openId + "&lang=zh_CN";
                var response = HttpGet(url);

                var jsonObject = JsonConvert.DeserializeObject<WechatGetUserInfoResultModel>(response);
                if (jsonObject.errcode == "40001")
                {
                    return GetUserInfo(openId);
                }
                WechatUser user = new WechatUser()
                {
                    //Status = 0,
                    //CreatedBy = "",
                    //CreatedOn = DateTime.Now,
                    //UpdatedBy = "",
                    //UpdatedOn = "",
                    City = jsonObject.city,
                    Country = jsonObject.country,
                    GroupId = jsonObject.groupid,
                    HeadImageUrl = jsonObject.headimgurl,
                    NickName = jsonObject.nickname,
                    OfficialAccount = this.CurrentWechat.Id,
                    OpenId = jsonObject.openid,
                    Privilege = "",
                    Province = jsonObject.province,
                    Sex = jsonObject.sex,
                    UnionId = "",
                };
                return user;

            }
            catch (Exception ex)
            {
                log.Error("GetUserInfo", ex);
                return null;
            }
        }


        /// <summary>
        /// 得到微信公众号有所有粉丝OpenId
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserList()
        {
            List<string> list = new List<string>();
            var url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var response = HttpGet(url + "&next_openid=");
            var result = JsonConvert.DeserializeObject<WechatGetUserListResultModel>(response);
            if (string.IsNullOrEmpty(result.errcode))
            {
                list.AddRange(result.data.openid);
            }
            else
            {
                return list;
            }

            while (result.count > 10000 && !string.IsNullOrEmpty(result.next_openid))
            {
                response = HttpGet(url + "&next_openid=" + result.next_openid);

                result = JsonConvert.DeserializeObject<WechatGetUserListResultModel>(response);
                if (string.IsNullOrEmpty(result.errcode))
                {
                    list.AddRange(result.data.openid);
                }
                else
                {
                    return list;
                }
            }
            return list;
        }


        #endregion

        #region 客户消息

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendTextMessage(string openId, string content)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";

            var model = new TextMessageRequest(openId, content);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }
        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendImageMessage(string openId, string mediaId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";

            var model = new ImageMessageRequest(openId, mediaId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }
        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendVoiceMessage(string openId, string mediaId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";

            var model = new VoiceMessageRequest(openId, mediaId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }

        /// <summary>
        /// 发送视频消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendVideoMessage(string openId, string mediaId, string title, string description)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";

            var model = new VideoMessageRequest(openId, mediaId, title, description);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }

        /// <summary>
        /// 发送音乐消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="title">标题</param>
        /// <param name="description">描述</param>
        /// <param name="hqmusicurl">高清音乐链接</param>
        /// <param name="musicurl">音乐链接</param>
        /// <param name="cardId">封面</param>
        /// <returns></returns>
        public WechatJsonResultModel SendMusicMessage(string openId, string title, string description, string hqmusicurl, string musicurl, string mediaId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";

            var model = new MusicMessageRequest(openId, title, description, hqmusicurl, musicurl, mediaId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }


        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendNewsMessage(string openId, List<Article> articles)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";

            var model = new NewsMessageRequest(openId);
            foreach (var item in articles)
            {
                model.AddArticles(item.Title, item.Description, item.Url, item.PicUrl);
            }
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }

        #endregion

        #region 群发消息

        /// <summary>
        /// 群发图文消息到组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendBatchNewsMessageToGroup(string groupId, string mediaId)
        {

            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token=ACCESS_TOKEN";
            var model = new GroupBatchRequestNewsMessage(groupId, mediaId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }
        /// <summary>
        /// 群发图文消息到用户列表
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendBatchNewsMessageToOpenList(string openIds, string mediaId)
        {
            if (!this.CurrentWechat.IsServiceAccount)
            {
                return null;
            }
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=ACCESS_TOKEN";
            var model = new BatchRequestNewsMessage(openIds, mediaId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }



        /// <summary>
        /// 群发文本消息到组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendBatchTextMessageToGroup(string groupId, string content)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token=ACCESS_TOKEN";
            var model = new GroupBatchRequestTextMessage(groupId, content);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }
        /// <summary>
        /// 群发文本消息到用户列表
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendBatchTextMessageToOpenList(string openIds, string content)
        {
            if (!this.CurrentWechat.IsServiceAccount)
            {
                return null;
            }
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=ACCESS_TOKEN";
            var model = new BatchRequestTextMessage(openIds, content);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }
        /// <summary>
        /// 群发语音消息到组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendBatchVoiceMessageToGroup(string groupId, string mediaId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token=ACCESS_TOKEN";
            var model = new GroupBatchRequestVoiceMessage(groupId, mediaId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }
        /// <summary>
        /// 群发语音消息到用户列表
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendBatchVoiceMessageToOpenList(string openIds, string mediaId)
        {
            if (!this.CurrentWechat.IsServiceAccount)
            {
                return null;
            }
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=ACCESS_TOKEN";
            var model = new BatchRequestVoiceMessage(openIds, mediaId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }

        /// <summary>
        /// 群发图片消息到组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendBatchImageMessageToGroup(string groupId, string mediaId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token=ACCESS_TOKEN";
            var model = new GroupBatchRequestImageMessage(groupId, mediaId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }
        /// <summary>
        /// 群发图片消息到用户列表
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendBatchImageMessageToOpenList(string openIds, string mediaId)
        {
            if (!this.CurrentWechat.IsServiceAccount)
            {
                return null;
            }
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=ACCESS_TOKEN";
            var model = new BatchRequestImageMessage(openIds, mediaId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }
        /// <summary>
        /// 群发视频消息到组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendBatchVideoMessageToGroup(string groupId, string mediaId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token=ACCESS_TOKEN";
            var model = new GroupBatchRequestVideoMessage(groupId, mediaId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }
        /// <summary>
        /// 群发视频消息到用户列表
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendBatchVideoMessageToOpenList(string openIds, string mediaId)
        {
            if (!this.CurrentWechat.IsServiceAccount)
            {
                return null;
            }
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=ACCESS_TOKEN";
            var model = new BatchRequestVideoMessage(openIds, mediaId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }

        /// <summary>
        /// 群发卡券消息到组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendBatchCardMessageToGroup(string groupId, string cardId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token=ACCESS_TOKEN";
            var model = new GroupBatchRequestCardMessage(groupId, cardId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }
        /// <summary>
        /// 群发卡券消息到用户列表
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WechatJsonResultModel SendBatchCardMessageToOpenList(string openIds, string cardId)
        {
            if (!this.CurrentWechat.IsServiceAccount)
            {
                return null;
            }
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=ACCESS_TOKEN";
            var model = new BatchRequestCardMessage(openIds, cardId);
            var response = HttpPost(url, model.ToString());
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }

        #endregion

        #region 模板消息
        public Task<WechatJsonResultModel> SendTemplateMessageAsync(string to, string templateID, string relatedUrl, object data)
        {
            Task<WechatJsonResultModel> task = new Task<WechatJsonResultModel>(() => { return this.SendTemplateMessage(to, templateID, relatedUrl, data); });
            task.Start();
            return task;
        }
        public WechatJsonResultModel SendTemplateMessage(string to, string templateID, string relatedUrl, object data)
        {
            TemplateMessageRequest model = new TemplateMessageRequest()
            {
                data = new
                {
                    first = new
                    {
                        value = "恭喜你购买成功！",
                        color = "#173177"
                    },
                    schedule = new
                    {
                        value = "恭喜你购买成功！",
                        color = "#173177"
                    },
                    time = new
                    {
                        value = "恭喜你购买成功！",
                        color = "#173177"
                    },
                    remark = new
                    {
                        value = "恭喜你购买成功！",
                        color = "#173177"
                    },
                },
                template_id = templateID,
                url = relatedUrl,
                touser = to
            };


            var requestString = JsonConvert.SerializeObject(model);
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + this.TryGetAccessToken();
            var response = HttpPost(url, requestString);
            var jsonObject = JsonConvert.DeserializeObject<WechatJsonResultModel>(response);
            return jsonObject;
        }

        #endregion

        #region 素材管理

        #region QR Code
        /// <summary>
        /// 临时二维码
        /// </summary>
        /// <param name="scene_id">场景值ID，32位非0整型</param>
        /// <param name="expireedSeconds"></param>
        /// <returns></returns>
        public WechatQRCode CreateSceneQRCode(int scene_id, int expireedSeconds = 2592000)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));
            var model = new { expire_seconds = 2592000, action_name = "QR_SCENE", action_info = new { scene = new { scene_id = scene_id } } };
            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatCreateSceneQRCodeResultModel>(response);
            if (result.errcode == "40001")
            {
                return CreateSceneQRCode(scene_id, expireedSeconds);
            }
            if (!string.IsNullOrEmpty(result.ticket))
            {
                var entity = new WechatQRCode()
                {
                    ExpiredTime = DateTimeHelper.GetDateTimeFromXml(result.expire_seconds),
                    OfficialAccount = this.CurrentWechat.Id,
                    Ticket = result.ticket,
                    Url = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + result.ticket,
                    Content = result.url,
                    Scene = scene_id.ToString(),
                    Status = 0,
                    IsUsed = false,
                    Id = Guid.NewGuid().ToString("N"),
                    Type = QRCodeType.TemporalScene,

                };
                this.db.Entity<WechatQRCode>().Insert(entity);
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 永久二维码
        /// </summary>
        /// <param name="sceneString"></param>
        /// <param name="expireedSeconds"></param>
        /// <returns></returns>
        public bool CreateLimitSceneQRCode()
        {
            var url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var length = 100000 - this.db.Entity<WechatQRCode>().Query()
                .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                .Where(m => m.Type, QRCodeType.LimitScene, CompareType.Equal)
                .Count();

            for (int i = 0; i < length; i++)
            {
                WechatQRCode entity = new WechatQRCode()
                {

                    Type = QRCodeType.LimitScene,
                    OfficialAccount = this.CurrentWechat.Id,
                    Id = Guid.NewGuid().ToString("N"),
                    ExpiredTime = DateTime.Parse("2024-12-12"),
                    IsUsed = false,
                    Scene = Guid.NewGuid().ToString("N"),
                    Status = 0,
                    //Url = string.Empty,
                    //Content = string.Empty,
                    //Ticket = string.Empty,
                };
                this.db.Entity<WechatQRCode>().Insert(entity);

                var model = new { action_name = "QR_LIMIT_STR_SCENE", action_info = new { scene = new { scene_str = entity.Scene } } };
                var response = HttpPost(url, JsonConvert.SerializeObject(model));
                var result = JsonConvert.DeserializeObject<WechatCreateSceneQRCodeResultModel>(response);
                if (!string.IsNullOrEmpty(result.ticket))
                {
                    entity.Url = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + result.ticket;
                    entity.Ticket = result.ticket;
                    entity.Content = result.url;
                    this.db.Entity<WechatQRCode>().Update(entity);
                }
            }
            return true;
        }


        /// <summary>
        /// 通过ticket换取二维码
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="fileDirectory"></param>
        public void DownloadQrCodeImage(string ticket, string fileDirectory)
        {
            var url = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticket;
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            var filePath = Path.Combine(fileDirectory, ticket, ".jpg");

            WebRequest request = WebRequest.Create(url);

            using (WebResponse response = request.GetResponse())
            {
                Image image = Image.FromStream(response.GetResponseStream());
                image.Save(filePath);
            }
        }

        #endregion

        /// <summary>
        /// 长链接转成短链接
        /// </summary>
        /// <param name="longUrl"></param>
        /// <returns></returns>
        public string GetShortUrl(string longUrl)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/shorturl?access_token=ACCESS_TOKEN";

            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { action = "long2short", long_url = longUrl };
            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatGetShortUrlResultModel>(response);
            if (result.errcode == "40001")
            {
                return GetShortUrl(longUrl);
            }
            if (result.errcode == "0")
            {
                return result.short_url;
            }
            return string.Empty;

        }

        /// <summary>
        /// 新增永久图文素材
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        public string CreateNewsMaterial(string title, string description, params  WechatNewsMaterial[] article)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/material/add_news?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new WechatNewsMaterialRequestModel();
            foreach (var item in article)
            {
                model.articles.Add(new WechatNewsMaterialRequestModel.WechatNewsMaterialModel()
                {

                    author = item.Author,
                    content = item.Content,
                    content_source_url = item.ContentSourceUrl,
                    digest = item.Digest,
                    show_cover_pic = item.ShowCoverPicture,
                    thumb_media_id = item.ThumbMediaId,
                    title = item.Title,
                    url = item.Url,
                });
            }


            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatAddNewsMaterialResultModel>(response);
            if (result.errcode == "40001")
            {
                return CreateNewsMaterial(title, description, article);
            }
            if (result.errcode == "0")
            {
                return result.media_id;
            }

            return string.Empty;
        }

        /// <summary>
        /// 上传图文消息内的图片获取URL 
        /// </summary>
        /// <param name="ACCESS_TOKEN">
        /// <param name="Type">
        /// <returns></returns>
        public string UploadNewsImage(string filePath)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={0}", this.TryGetAccessToken(false));

            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                byte[] responseArray = myWebClient.UploadFile(url, "POST", filePath);
                var response = System.Text.Encoding.Default.GetString(responseArray, 0, responseArray.Length);
                var result = JsonConvert.DeserializeObject<WechatUploadNewsImageResultModel>(response);

                return result.url;
            }
            catch (Exception ex)
            {
                log.Error("UploadNewsImage", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 新增其他类型永久素材
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <param name="title"></param>
        /// <param name="introduction"></param>
        /// <returns></returns>
        public WechatMaterial CreateMaterial(string filePath, WechatMaterialType type = WechatMaterialType.image, string title = "", string introduction = "")
        {
            var url = "https://api.weixin.qq.com/cgi-bin/material/add_material?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));


            string response;
            if (type == WechatMaterialType.video)
            {
                var description = string.Format("{\"title\":{0},\"introduction\":{1}}", title, introduction);
                response = HttpPost(url, "media", filePath,
                     new WechatMaterialRequestModel() { Field = "type", Value = type.ToString() },
                     new WechatMaterialRequestModel() { Field = "description", Value = description }
                     );
            }
            else
            {
                response = HttpPost(url, "media", filePath, new WechatMaterialRequestModel() { Field = "type", Value = type.ToString() });

            }
            var result = JsonConvert.DeserializeObject<WechatAddNewsMaterialResultModel>(response);
            if (result.errcode == "40001")
            {
                return CreateMaterial(filePath, type, title, introduction);
            }
            if (result.errcode == "0")
            {
                return new WechatMaterial()
                {
                    MediaId = result.media_id,
                    Name = "",
                    Url = result.url,
                    Type = type,
                };
            }

            return null;
        }

        public void BatchGetMaterial(int start = 1, int end = 1000, WechatMaterialType type = WechatMaterialType.news)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));
            if (start < 1)
            {
                start = 1;
            }
            int count = (end - start + 1);
            int length = count / 20;
            int left = count % 20;
            int pageIndex = 0;
            int offset;
            object model;
            while (pageIndex < length)
            {
                offset = start + pageIndex * 20 - 1;

                model = new { type = type.ToString(), offset = offset, count = 20 };
                switch (type)
                {
                    case WechatMaterialType.news:
                        BatchProcessNewsMaterial(url, model, type);
                        break;
                    case WechatMaterialType.image:
                    case WechatMaterialType.video:
                    case WechatMaterialType.voice:
                    default:
                        BatchProcessOtherMaterial(url, model, type);
                        break;
                }

                pageIndex++;
            }
            if (left > 0)
            {
                offset = start + pageIndex * 20 - 1;
                model = new { type = type.ToString(), offset = start + pageIndex * 20 - 1, count = left };
                switch (type)
                {
                    case WechatMaterialType.news:
                        BatchProcessNewsMaterial(url, model, type);
                        break;
                    case WechatMaterialType.image:
                    case WechatMaterialType.video:
                    case WechatMaterialType.voice:
                    default:
                        BatchProcessOtherMaterial(url, model, type);
                        break;
                }
            }
        }
        /// <summary>
        /// 图片、语音、视频
        /// </summary>
        /// <param name="url"></param>
        /// <param name="model"></param>
        /// <param name="type"></param>
        private void BatchProcessOtherMaterial(string url, object model, WechatMaterialType type)
        {
            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatBatchGetMaterialResultModel>(response);
            if (result.item == null)
            {
                return;
            }
            foreach (var item in result.item)
            {
                var entity = this.db.Entity<WechatMaterial>().Query()
                    .Where(m => m.MediaId, item.media_id, CompareType.Equal)
                    .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                    .First();
                if (entity == null)
                {
                    entity = new WechatMaterial()
                    {
                        //CreatedBy = null,
                        //CreatedOn = null,
                        //UpdatedBy = null,
                        //UpdatedOn = null,
                        Id = Guid.NewGuid().ToString("N"),
                        //Status = 0,
                        OfficialAccount = this.CurrentWechat.Id,
                        LastUpdateTime = DateTimeHelper.GetDateTimeFromXml(item.update_time),
                        MediaId = item.media_id,
                        IsReleased = true,

                        Name = item.name,
                        Type = type,
                        Url = item.url,
                    };
                    if (type == WechatMaterialType.video)
                    {
                        var video = GetVideoMaterialData(item.media_id, entity.Id);
                        entity.Name = video.title;
                        entity.Url = video.down_url;
                        entity.Description = video.description;
                    }
                    this.db.Entity<WechatMaterial>().Insert(entity);
                }
                else
                {
                    entity.LastUpdateTime = DateTimeHelper.GetDateTimeFromXml(item.update_time);
                    entity.MediaId = item.media_id;
                    entity.IsReleased = true;
                    entity.Name = item.name;
                    entity.Type = type;
                    entity.Url = item.url;
                    if (type == WechatMaterialType.video)
                    {
                        var video = GetVideoMaterialData(item.media_id, entity.Id);
                        entity.Name = video.title;
                        entity.Url = video.down_url;
                        entity.Description = video.description;
                    }
                    else
                    {
                        DownloadOtherMaterial(item.media_id, "d:\\test.jpg");
                    }
                    this.db.Entity<WechatMaterial>().Update(entity);
                }


            }
        }
        private void BatchProcessNewsMaterial(string url, object model, WechatMaterialType type)
        {
            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatBatchGetNewsMaterialResultModel>(response);
            if (result.item == null)
            {
                return;
            }
            foreach (var item in result.item)
            {
                var entity = this.db.Entity<WechatMaterial>().Query()
                    .Where(m => m.MediaId, item.media_id, CompareType.Equal)
                    .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                    .First();
                if (entity == null)
                {
                    entity = new WechatMaterial()
                    {
                        //CreatedBy = null,
                        //CreatedOn = null,
                        //UpdatedBy = null,
                        //UpdatedOn = null,
                        Id = Guid.NewGuid().ToString("N"),
                        //Status = 0,
                        OfficialAccount = this.CurrentWechat.Id,
                        LastUpdateTime = DateTimeHelper.GetDateTimeFromXml(item.update_time),
                        MediaId = item.media_id,
                        IsReleased = true,
                        //Description = item.name,
                        //Name = item.name,
                        Type = type,
                        //Url = item.url,
                    };
                    this.db.Entity<WechatMaterial>().Insert(entity);
                }
                else
                {
                    entity.LastUpdateTime = DateTimeHelper.GetDateTimeFromXml(item.update_time);
                    entity.MediaId = item.media_id;
                    entity.IsReleased = true;
                    //entity.Name = item.name;
                    entity.Type = type;
                    //entity.Url = item.url;
                    this.db.Entity<WechatMaterial>().Update(entity);
                }
                int index = 0;
                foreach (var content in item.content.news_item)
                {
                    var entityContent = this.db.Entity<WechatNewsMaterial>().Query()
                        .Where(m => m.MediaId, entity.Id, CompareType.Equal)
                        .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                        .Where(m => m.SortBy, index, CompareType.Equal)
                        .First();
                    if (entityContent == null)
                    {
                        entityContent = new WechatNewsMaterial()
                        {
                            //CreatedBy = null,
                            //CreatedOn = null,
                            //UpdatedBy = null,
                            //UpdatedOn = null,
                            //Id = null,
                            MaterialID = entity.Id,
                            MediaId = item.media_id,
                            Status = 0,
                            OfficialAccount = this.CurrentWechat.Id,
                            LastUpdateTime = DateTime.UtcNow,
                            IsReleased = true,
                            Digest = content.digest,
                            Content = content.content,
                            Author = content.author,
                            Url = content.url,
                            ContentSourceUrl = content.content_source_url,
                            ShowCoverPicture = !string.Equals("0", content.show_cover_pic),
                            SortBy = index,
                            ThumbMediaId = content.thumb_media_id,
                            Title = content.title,
                        };
                        this.db.Entity<WechatNewsMaterial>().Insert(entityContent);
                    }
                    else
                    {
                        entityContent.Digest = content.digest;
                        entityContent.Content = content.content;
                        entityContent.Author = content.author;
                        entityContent.Url = content.url;
                        entityContent.ContentSourceUrl = content.content_source_url;
                        entityContent.ShowCoverPicture = !string.Equals("0", content.show_cover_pic);
                        entityContent.SortBy = index;
                        entityContent.ThumbMediaId = content.thumb_media_id;
                        entityContent.Title = content.title;
                        this.db.Entity<WechatNewsMaterial>().Update(entityContent);
                    }
                    index++;

                }
            }
        }
        /// <summary>
        /// 获取永久素材
        /// </summary>
        /// <param name="MediaId"></param>
        /// <returns></returns>
        public void GetNewsMaterialDetail(string mediaId, string materialId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));
            var model = new { media_id = mediaId };
            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatGetNewsMaterialResultModel>(response);
            if (result.news_item == null)
            {
                return;
            }
            int index = 0;
            foreach (var item in result.news_item)
            {
                var entity = this.db.Entity<WechatNewsMaterial>().Query()
                    .Where(m => m.MediaId, mediaId, CompareType.Equal)
                    .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                    .Where(m => m.SortBy, index, CompareType.Equal)
                    .First();
                if (entity == null)
                {
                    entity = new WechatNewsMaterial()
                    {
                        //CreatedBy = null,
                        //CreatedOn = null,
                        //UpdatedBy = null,
                        //UpdatedOn = null,
                        //Id = null,
                        MaterialID = materialId,
                        MediaId = mediaId.ToString(),
                        Status = 0,
                        OfficialAccount = this.CurrentWechat.Id,
                        LastUpdateTime = DateTime.UtcNow,
                        Digest = item.digest,
                        Content = item.content,
                        Author = item.author,
                        Url = item.url,
                        ContentSourceUrl = item.content_source_url,
                        ShowCoverPicture = !string.Equals("0", item.show_cover_pic),
                        SortBy = index,
                        ThumbMediaId = item.thumb_media_id,
                        Title = item.title,
                    };
                    this.db.Entity<WechatNewsMaterial>().Insert(entity);
                }
                else
                {
                    entity.Digest = item.digest;
                    entity.Content = item.content;
                    entity.Author = item.author;
                    entity.Url = item.url;
                    entity.ContentSourceUrl = item.content_source_url;
                    entity.ShowCoverPicture = !string.Equals("0", item.show_cover_pic);
                    entity.SortBy = index;
                    entity.ThumbMediaId = item.thumb_media_id;
                    entity.Title = item.title;
                    this.db.Entity<WechatNewsMaterial>().Update(entity);
                }
                index++;

            }
        }

        /// <summary>
        /// 获取永久素材
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="materialId"></param>
        private WechatGetVideoMaterialResultModel GetVideoMaterialData(string mediaId, string materialId)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));
            var model = new { media_id = mediaId };
            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatGetVideoMaterialResultModel>(response);
            return result;
        }




        //删除永久素材
        public bool DeleteMaterial(int media_id)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/material/del_material?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { media_id = media_id };


            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatGetNewsMaterialResultModel>(response);
            if (result.errcode == "40001")
            {
                return DeleteMaterial(media_id);
            }
            if (result.errcode == "0")
            {
                return true;
            }
            return false;
        }

        public void DownloadOtherMaterial(string media_id, string filePath)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));
            var model = new { media_id = media_id };
            var data = JsonConvert.SerializeObject(model);
            var stream = new FileStream(filePath, FileMode.OpenOrCreate);
            WebClient wc = new WebClient();
            var file = wc.UploadData(url, "POST", Encoding.UTF8.GetBytes(string.IsNullOrEmpty(data) ? "" : data));
            foreach (var b in file)
            {
                stream.WriteByte(b);
            }
            stream.Close();
        }
        /// <summary>
        /// 下载用户上传图片
        /// </summary>
        /// <param name="cardId">存在微信服务器文件</param>
        /// <param name="openId">用户openid</param>
        /// <param name="createTime">用户报料时间</param>
        /// <param name="filePath">存放保存后文件位置</param>
        public void DownloadUserImage(string mediaId, string openId, out string filePath)
        {
            filePath = string.Empty;
            try
            {
                string rootPath = ConfigSetting.Get("ImagePath", "/ImagePath");
                string url = string.Format("", this.TryGetAccessToken(false), mediaId);

                WebRequest request = WebRequest.Create(url);

                using (WebResponse response = request.GetResponse())
                {
                    Image image = Image.FromStream(response.GetResponseStream());
                    if (!Directory.Exists(rootPath + openId))
                    {
                        Directory.CreateDirectory(rootPath + openId);
                    }
                    string fileName = rootPath + openId + "\\" + DateTime.Now.ToString("yyyyMMdd HHmmss") + ".jpg";
                    filePath = fileName;
                    image.Save(fileName);
                }
            }
            catch (Exception ex)
            {
                log.Error("DownloadWechatFileHelper.DownloadImage", ex);
            }
        }


        /// <summary>
        /// 下载用户语音信息
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="openId"></param>
        /// <param name="createTime"></param>
        /// <param name="format"></param>
        /// <param name="filePath"></param>
        public void DownloadUserVoice(string mediaId, string openId, string format, out string filePath)
        {
            filePath = string.Empty;
            try
            {
                string rootPath = ConfigSetting.Get("VoicePath");
                string url = string.Format("", this.TryGetAccessToken(false), mediaId);
                WebClient client = new WebClient();

                if (!Directory.Exists(rootPath + openId))
                {
                    Directory.CreateDirectory(rootPath + openId);
                }
                string fileName = rootPath + openId + "\\" + DateTime.Now.ToString("yyyyMMdd HHmmss") + "." + format;
                filePath = fileName;
                client.DownloadFile(url, filePath);
            }
            catch (Exception ex)
            {
                log.Error("DownloadWechatFileHelper.DownloadVoice", ex);
            }
        }

        #endregion

        #region 数据统计

        /// <summary>
        /// 获取用户增减数据
        /// </summary>
        public void GetUserSummary(DateTime fromDate, DateTime toDate)
        {
            var url = "https://api.weixin.qq.com/datacube/getusersummary?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { begin_date = fromDate.ToString("yyyy-MM-dd"), end_date = toDate.ToString("yyyy-MM-dd") };

            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatGetUserSummaryResultModel>(response);

            foreach (var item in result.list)
            {
                var entity = this.db.Entity<WechatUserSummary>().Query()
                    .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                    .Where(m => m.RefDateTime, item.ref_date, CompareType.Equal)
                    .Where(m => m.Source, item.user_source, CompareType.Equal)
                    .First();
                if (entity == null)
                {
                    this.db.Entity<WechatUserSummary>().Insert(new WechatUserSummary()
                    {
                        RefDateTime = item.ref_date,
                        OfficialAccount = this.CurrentWechat.Id,
                        CountOfFollower = item.new_user,
                        CountOfUnfollower = item.cancel_user,
                        Source = item.user_source,
                        Status = 0,
                    });
                }
                else
                {
                    entity.CountOfUnfollower = item.cancel_user;
                    entity.CountOfFollower = item.new_user;

                    this.db.Entity<WechatUserSummary>().Update(entity);
                }
            }
        }



        /// <summary>
        ///获取累计用户数据
        /// </summary>
        public void GetUserCumulate(DateTime fromDate, DateTime toDate)
        {
            var url = "https://api.weixin.qq.com/datacube/getusercumulate?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { begin_date = fromDate.ToString("yyyy-MM-dd"), end_date = toDate.ToString("yyyy-MM-dd") };

            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatGetUserCumulateResultModel>(response);

            foreach (var item in result.list)
            {
                var entity = this.db.Entity<WechatUserCumulate>().Query()
                    .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                    .Where(m => m.RefDateTime, item.ref_date, CompareType.Equal)
                    .First();
                if (entity == null)
                {
                    this.db.Entity<WechatUserCumulate>().Insert(new WechatUserCumulate()
                    {
                        RefDateTime = item.ref_date,
                        OfficialAccount = this.CurrentWechat.Id,
                        CountOfFollower = item.cumulate_user,
                        Status = 0,
                    });
                }
                else
                {
                    entity.CountOfFollower = item.cumulate_user;
                    this.db.Entity<WechatUserCumulate>().Update(entity);
                }
            }
        }



        /// <summary>
        /// 获取图文群发每日数据
        /// </summary>
        public void GetArticleSummary(DateTime fromDate, DateTime toDate)
        {
            var url = "https://api.weixin.qq.com/datacube/getarticlesummary?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { begin_date = fromDate.ToString("yyyy-MM-dd"), end_date = toDate.ToString("yyyy-MM-dd") };

            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatGetArticleSummaryResultModel>(response);

            foreach (var item in result.list)
            {
                var entity = this.db.Entity<WechatArticleSummary>().Query()
                    .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                    .Where(m => m.MessageID, item.msgid, CompareType.Equal)
                    .Where(m => m.RefDateTime, item.ref_date, CompareType.Equal)
                    .First();
                if (entity == null)
                {
                    this.db.Entity<WechatArticleSummary>().Insert(new WechatArticleSummary()
                    {
                        RefDateTime = item.ref_date,
                        OfficialAccount = this.CurrentWechat.Id,
                        MaterialID = null,
                        //CreatedBy = null,
                        //CreatedOn = null,
                        //Id = null,
                        //UpdatedBy = null,
                        //UpdatedOn = null,
                        //Status = 0,
                        MessageID = item.msgid,
                        OriginalPageReadCount = item.ori_page_read_count,
                        OriginalPageReadUser = item.ori_page_read_user,
                        PageFavriateCount = item.add_to_fav_count,
                        PageFavriateUser = item.add_to_fav_user,
                        PageReadCount = item.int_page_read_count,
                        PageReadUser = item.int_page_read_user,
                        PageShareUser = item.share_user,
                        PageShareCount = item.share_count,
                        Title = item.title,

                    });
                }
                else
                {
                    entity.OriginalPageReadCount = item.ori_page_read_count;
                    entity.OriginalPageReadUser = item.ori_page_read_user;
                    entity.PageFavriateCount = item.add_to_fav_count;
                    entity.PageFavriateUser = item.add_to_fav_user;
                    entity.PageReadCount = item.int_page_read_count;
                    entity.PageReadUser = item.int_page_read_user;
                    entity.PageShareUser = item.share_user;
                    entity.PageShareCount = item.share_count;
                    entity.Title = item.title;
                    this.db.Entity<WechatArticleSummary>().Update(entity);
                }
            }
        }

        /// <summary>
        /// 获取图文群发每日数据
        /// </summary>
        public void GetArticleTotal(DateTime fromDate, DateTime toDate)
        {
            var url = "https://api.weixin.qq.com/datacube/getarticletotal?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { begin_date = fromDate.ToString("yyyy-MM-dd"), end_date = toDate.ToString("yyyy-MM-dd") };

            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatGetWechatArticleTotalResultModel>(response);

            foreach (var item in result.list)
            {
                foreach (var entity in this.db.Entity<WechatArticleTotal>().Query()
                    .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                    .Where(m => m.MessageID, item.msgid, CompareType.Equal)
                    .Where(m => m.RefDateTime, item.ref_date, CompareType.Equal)
                    .ToList())
                {
                    this.db.Entity<WechatArticleTotal>().Delete(entity);
                }
                foreach (var detail in item.details)
                {
                    this.db.Entity<WechatArticleTotal>().Insert(new WechatArticleTotal()
                    {
                        RefDateTime = item.ref_date,
                        OfficialAccount = this.CurrentWechat.Id,
                        //MaterialID = null,
                        //CreatedBy = null,
                        //CreatedOn = null,
                        //Id = null,
                        //UpdatedBy = null,
                        //UpdatedOn = null,
                        //Status = 0,
                        MessageID = item.msgid.Split('_')[0],
                        MessageIndex = item.msgid.Split('_')[1],
                        SendTime = detail.stat_date,
                        FeedShareFromFeedcnt = detail.feed_share_from_feed_cnt,
                        FeedShareFromFeeduser = detail.feed_share_from_feed_user,
                        FeedShareFromOtherCnt = detail.feed_share_from_other_cnt,
                        FeedShareFromOtherUser = detail.feed_share_from_other_user,
                        FeedShareFromSessionCnt = detail.feed_share_from_session_cnt,
                        FeedShareFromSessionUser = detail.feed_share_from_session_user,
                        PageFromFeedreadCount = detail.int_page_from_feed_read_count,
                        PageFromFeedreadUser = detail.int_page_from_feed_read_user,
                        PageFromFriendsReadCount = detail.int_page_from_friends_read_count,
                        PageFromFriendsReadUser = detail.int_page_from_friends_read_user,
                        PageFromHistMsgReadCount = detail.int_page_from_hist_msg_read_count,
                        PageFromHistMsgReadUser = detail.int_page_from_hist_msg_read_user,
                        PageFromOtherReadCount = detail.int_page_from_other_read_count,
                        PageFromOtherReadUser = detail.int_page_from_other_read_user,
                        PageFromSessionReadCount = detail.int_page_from_session_read_count,
                        PageFromSessionReadUser = detail.int_page_from_session_read_user,
                        OriginalPageReadCount = detail.ori_page_read_count,
                        OriginalPageReadUser = detail.ori_page_read_user,
                        PageFavriateCount = detail.add_to_fav_count,
                        PageFavriateUser = detail.add_to_fav_user,
                        PageReadCount = detail.int_page_read_count,
                        PageReadUser = detail.int_page_read_user,
                        PageShareUser = detail.share_user,
                        PageShareCount = detail.share_count,
                        Title = item.title,
                    });
                }
            }
        }

        /// <summary>
        /// 获取图文统计数据
        /// </summary>
        public void GetUserRead(DateTime fromDate, DateTime toDate)
        {
            var url = "https://api.weixin.qq.com/datacube/getuserread?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { begin_date = fromDate.ToString("yyyy-MM-dd"), end_date = toDate.ToString("yyyy-MM-dd") };

            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatGetUserReadResultModel>(response);

            foreach (var item in result.list)
            {
                var entity = this.db.Entity<WechatArticleRead>().Query()
                    .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                    .Where(m => m.RefDateTime, item.ref_date, CompareType.Equal)
                    .First();
                if (entity == null)
                {
                    this.db.Entity<WechatArticleRead>().Insert(new WechatArticleRead()
                    {
                        RefDateTime = item.ref_date,
                        OfficialAccount = this.CurrentWechat.Id,
                        //MaterialID = null,
                        //CreatedBy = null,
                        //CreatedOn = null,
                        //Id = null,
                        //UpdatedBy = null,
                        //UpdatedOn = null,
                        Status = 0,
                        OriginalPageReadCount = item.ori_page_read_count,
                        OriginalPageReadUser = item.ori_page_read_user,
                        PageFavriateCount = item.add_to_fav_count,
                        PageFavriateUser = item.add_to_fav_user,
                        PageReadCount = item.int_page_read_count,
                        PageReadUser = item.int_page_read_user,
                        PageShareUser = item.share_user,
                        PageShareCount = item.share_count,
                    });
                }
                else
                {
                    entity.OriginalPageReadCount = item.ori_page_read_count;
                    entity.OriginalPageReadUser = item.ori_page_read_user;
                    entity.PageFavriateCount = item.add_to_fav_count;
                    entity.PageFavriateUser = item.add_to_fav_user;
                    entity.PageReadCount = item.int_page_read_count;
                    entity.PageReadUser = item.int_page_read_user;
                    entity.PageShareUser = item.share_user;
                    entity.PageShareCount = item.share_count;
                    this.db.Entity<WechatArticleRead>().Update(entity);
                }
            }
        }

        /// <summary>
        /// 获取图文统计数据
        /// </summary>
        public void GetUserReadByHour(DateTime fromDate, DateTime toDate)
        {
            var url = "https://api.weixin.qq.com/datacube/getuserread?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { begin_date = fromDate.ToString("yyyy-MM-dd"), end_date = toDate.ToString("yyyy-MM-dd") };

            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechatGetUserReadByHourResultModel>(response);

            foreach (var item in result.list)
            {
                var entity = this.db.Entity<WechatArticleReadByHour>().Query()
                    .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                    .Where(m => m.RefDateTime, item.ref_date.AddHours(item.ref_hour), CompareType.Equal)
                    .Where(m => m.Source, item.user_source, CompareType.Equal)
                    .First();
                if (entity == null)
                {
                    this.db.Entity<WechatArticleReadByHour>().Insert(new WechatArticleReadByHour()
                    {
                        RefDateTime = item.ref_date.AddHours(item.ref_hour),
                        OfficialAccount = this.CurrentWechat.Id,
                        //MaterialID = null,
                        //CreatedBy = null,
                        //CreatedOn = null,
                        //Id = null,
                        //UpdatedBy = null,
                        //UpdatedOn = null,
                        Status = 0,
                        Source = item.user_source,
                        OriginalPageReadCount = item.ori_page_read_count,
                        OriginalPageReadUser = item.ori_page_read_user,
                        PageFavriateCount = item.add_to_fav_count,
                        PageFavriateUser = item.add_to_fav_user,
                        PageReadCount = item.int_page_read_count,
                        PageReadUser = item.int_page_read_user,
                        PageShareUser = item.share_user,
                        PageShareCount = item.share_count,
                    });
                }
                else
                {
                    entity.OriginalPageReadCount = item.ori_page_read_count;
                    entity.OriginalPageReadUser = item.ori_page_read_user;
                    entity.PageFavriateCount = item.add_to_fav_count;
                    entity.PageFavriateUser = item.add_to_fav_user;
                    entity.PageReadCount = item.int_page_read_count;
                    entity.PageReadUser = item.int_page_read_user;
                    entity.PageShareUser = item.share_user;
                    entity.PageShareCount = item.share_count;
                    this.db.Entity<WechatArticleReadByHour>().Update(entity);
                }
            }
        }



        /// <summary>
        /// 获取图文分享转发数据
        /// </summary>
        public void GetUserShare(DateTime fromDate, DateTime toDate)
        {
            var url = "https://api.weixin.qq.com/datacube/getusershare?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { begin_date = fromDate.ToString("yyyy-MM-dd"), end_date = toDate.ToString("yyyy-MM-dd") };

            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechaGetUserShareResultModel>(response);

            foreach (var item in result.list)
            {
                var entity = this.db.Entity<WechatUserShare>().Query()
                    .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                    .Where(m => m.RefDateTime, item.ref_date, CompareType.Equal)
                    .Where(m => m.Scene, item.share_scene, CompareType.Equal)
                    .First();
                if (entity == null)
                {
                    this.db.Entity<WechatUserShare>().Insert(new WechatUserShare()
                    {
                        RefDateTime = item.ref_date,
                        OfficialAccount = this.CurrentWechat.Id,
                        //MaterialID = null,
                        //CreatedBy = null,
                        //CreatedOn = null,
                        //Id = null,
                        //UpdatedBy = null,
                        //UpdatedOn = null,
                        Status = 0,
                        Scene = item.share_scene,
                        PageShareUser = item.share_user,
                        PageShareCount = item.share_count,
                    });
                }
                else
                {
                    entity.Scene = item.share_scene;
                    entity.PageShareUser = item.share_user;
                    entity.PageShareCount = item.share_count;
                    this.db.Entity<WechatUserShare>().Update(entity);
                }
            }
        }

        /// <summary>
        /// 获取图文群发每日数据
        /// </summary>
        public void GetUserShareByHour(DateTime fromDate, DateTime toDate)
        {
            var url = "https://api.weixin.qq.com/datacube/getarticletotal?access_token=ACCESS_TOKEN";
            url = url.Replace("ACCESS_TOKEN", this.TryGetAccessToken(false));

            var model = new { begin_date = fromDate.ToString("yyyy-MM-dd"), end_date = toDate.ToString("yyyy-MM-dd") };

            var response = HttpPost(url, JsonConvert.SerializeObject(model));
            var result = JsonConvert.DeserializeObject<WechaGetUserShareByHourResultModel>(response);

            foreach (var item in result.list)
            {
                var entity = this.db.Entity<WechatUserShareByHour>().Query()
                    .Where(m => m.OfficialAccount, this.CurrentWechat.Id, CompareType.Equal)
                    .Where(m => m.RefDateTime, item.ref_date.AddHours(item.ref_hour), CompareType.Equal)
                    .Where(m => m.Scene, item.share_scene, CompareType.Equal)
                    .First();
                if (entity == null)
                {
                    this.db.Entity<WechatUserShareByHour>().Insert(new WechatUserShareByHour()
                    {
                        RefDateTime = item.ref_date.AddHours(item.ref_hour),
                        OfficialAccount = this.CurrentWechat.Id,
                        //MaterialID = null,
                        //CreatedBy = null,
                        //CreatedOn = null,
                        //Id = null,
                        //UpdatedBy = null,
                        //UpdatedOn = null,
                        Status = 0,
                        Scene = item.share_scene,
                        PageShareUser = item.share_user,
                        PageShareCount = item.share_count,
                    });
                }
                else
                {
                    entity.Scene = item.share_scene;
                    entity.PageShareUser = item.share_user;
                    entity.PageShareCount = item.share_count;
                    this.db.Entity<WechatUserShareByHour>().Update(entity);
                }
            }
        }
        #endregion

    }
}
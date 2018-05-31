using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using wx_site_micc.GetUserByCodeService;
using wx_site_micc.OAuthTwoService;
using wx_site_micc.GetTokenService;
using wx_site_micc.FangkeService;
using wx_site_micc.Models;
using wx_site_micc.SendMessageService;
using System.Configuration;

namespace wx_site_micc.Controllers
{
    public class FangkeController : Controller
    {
        // GET: Fangke
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("query")]
        public ActionResult Query()
        {
            string url_id = Request.QueryString["id"];
            FangkeService.HVS010TB hvs010tb = new FangkeService.HVS010TB();
            hvs010tb.url_id = url_id;
            hvs010tb.vs_count = 0;
            FangkeServiceSoapClient fangkeService = new FangkeServiceSoapClient();
            DataTable dt = fangkeService.SearchHVS010TB(hvs010tb);
            Models.HVS010TB vs = new Models.HVS010TB();
            if (dt.Rows.Count > 0)
            {
                vs.vs_company = dt.Rows[0]["vs_company"].ToString();
                vs.vs_count = dt.Rows[0]["vs_count"].ToString();
                vs.vs_date = dt.Rows[0]["vs_date"].ToString();
                vs.vs_nm = dt.Rows[0]["vs_nm"].ToString();
                vs.vs_note = dt.Rows[0]["vs_note"].ToString();
                vs.vs_time = dt.Rows[0]["vs_time"].ToString();
                vs.ck_state = dt.Rows[0]["ck_state"].ToString();
                vs.dept_nm = dt.Rows[0]["dept_nm"].ToString();
                vs.emp_nm = dt.Rows[0]["emp_nm"].ToString();
                vs.emp_zw = dt.Rows[0]["emp_zw"].ToString();
                vs.car_no = dt.Rows[0]["car_no"].ToString();
            }

            return View(vs);
        }

        [HttpGet]
        [ActionName("check")]
        public ActionResult Check()
        {
            string url_id = Request.QueryString["id"];
            FangkeService.HVS010TB hvs010tb = new FangkeService.HVS010TB();
            hvs010tb.url_id = url_id;
            hvs010tb.vs_count = 0;
            FangkeServiceSoapClient fangkeService = new FangkeServiceSoapClient();
            DataTable dt = fangkeService.SearchHVS010TB(hvs010tb);
            Models.HVS010TB vs = new Models.HVS010TB();
            if (dt.Rows.Count > 0)
            {
                vs.vs_company = dt.Rows[0]["vs_company"].ToString();
                vs.vs_count = dt.Rows[0]["vs_count"].ToString();
                vs.vs_date = dt.Rows[0]["vs_date"].ToString();
                vs.vs_nm = dt.Rows[0]["vs_nm"].ToString();
                vs.vs_note = dt.Rows[0]["vs_note"].ToString();
                vs.vs_time = dt.Rows[0]["vs_time"].ToString();
                vs.ck_state = dt.Rows[0]["ck_state"].ToString();
                vs.dept_nm = dt.Rows[0]["dept_nm"].ToString();
                vs.emp_nm = dt.Rows[0]["emp_nm"].ToString();
                vs.emp_zw = dt.Rows[0]["emp_zw"].ToString();
                vs.car_no = dt.Rows[0]["car_no"].ToString();
            }

            return View(vs);
        }

        [HttpPost]
        [ActionName("check")]
        public ActionResult Check(FormCollection collection)
        {
            string url_id = Request.QueryString["id"];
            FangkeService.HVS010TB hvs010tb = new FangkeService.HVS010TB();
            hvs010tb.url_id = url_id;
            hvs010tb.vs_count = 0;
            FangkeServiceSoapClient fangkeService = new FangkeServiceSoapClient();
            //审核
            DataTable dt = fangkeService.CheckHVS010TB(hvs010tb);
            //推送信息
            if (dt.Rows.Count > 0)
            {
                string agentid = ConfigurationManager.AppSettings["AgentIdForFangke"];
                SendMessageServiceSoapClient sendMessageService = new SendMessageServiceSoapClient();
                sendMessageService.FangKePushMessage(url_id, agentid, "Y", "check_succeed");
            }
            //审核完成重新查询
            dt = null;
            dt = fangkeService.SearchHVS010TB(hvs010tb);
            Models.HVS010TB vs = new Models.HVS010TB();
            if (dt.Rows.Count > 0)
            {
                vs.vs_company = dt.Rows[0]["vs_company"].ToString();
                vs.vs_count = dt.Rows[0]["vs_count"].ToString();
                vs.vs_date = dt.Rows[0]["vs_date"].ToString();
                vs.vs_nm = dt.Rows[0]["vs_nm"].ToString();
                vs.vs_note = dt.Rows[0]["vs_note"].ToString();
                vs.vs_time = dt.Rows[0]["vs_time"].ToString();
                vs.ck_state = dt.Rows[0]["ck_state"].ToString();
                vs.dept_nm = dt.Rows[0]["dept_nm"].ToString();
                vs.emp_nm = dt.Rows[0]["emp_nm"].ToString();
                vs.emp_zw = dt.Rows[0]["emp_zw"].ToString();
                vs.car_no = dt.Rows[0]["car_no"].ToString();
            }
            return View(vs);
        }

        [HttpGet]
        [ActionName("add")]
        public ActionResult Add()
        {
            /*用户请求主页的时候要做的判断如下：
             1、看地址栏中是否有code参数，如果有参数的话，说明用户是从进入主页链接进入的
             2、地址栏中如果没有code参数，则有可能是打开下面的二级页面后点击手机上的返回键，退回的主页，此时就要去找用户的Cookie信息
             3、如果用户是新关注的企业号，尚未授权，则进入授权action，授权后再跳转到主页，此时地址栏中是携带code参数的*/
            string code = Request.QueryString["code"];
            Message message = new Message();
            //第一个判断
            if (!string.IsNullOrEmpty(code))
            {
                //获取token信息
                GetTokenServiceSoapClient getTokenService = new GetTokenServiceSoapClient();
                string agentid = ConfigurationManager.AppSettings["AgentIdForFangke"];
                string accessToken = getTokenService.GetAccessToken(agentid);
                //获取用户信息
                GetUserByCodeServiceSoapClient getUserByCodeService = new GetUserByCodeServiceSoapClient();
                Userinfo userinfo = new Userinfo();
                userinfo = getUserByCodeService.GetUserinfo(accessToken, code);
                //判断返回的数据是否异常
                if (userinfo.errcode == 0)
                {
                    //把用户信息保存到Cookie中
                    Response.Cookies["userName"].Value = userinfo.UserId;
                    Response.Cookies["userName"].Expires = DateTime.Now.AddDays(30);
                    message.Msg_cd = userinfo.UserId;
                    //返回视图
                    return View("Add", message);
                }
                else
                {
                    //返回错误页面
                    return RedirectToAction("Error_404", "ErrorPage");
                }
            }
            //第二个判断
            else if (Request.Cookies["userName"] != null)
            {
                Userinfo userinfo = new Userinfo();
                userinfo.UserId = Request.Cookies["userName"].Value;
                message.Msg_cd = userinfo.UserId;
                return View("Add", message);
            }
            //第三个判断
            return RedirectToAction("OAuth", "Fangke");
        }

        /// <summary>
        /// 进行OAuth2授权注册
        /// </summary>
        /// <returns></returns>
        public ActionResult OAuth()
        {
            string agentid = ConfigurationManager.AppSettings["AgentIdForFangke"];
            OAuthTwoServiceSoapClient oAuthTwoService = new OAuthTwoServiceSoapClient();
            string url = oAuthTwoService.OAuthTwo(agentid);
            return Redirect(url);
        }

        [HttpPost]
        [ActionName("add")]
        public ActionResult Add(FormCollection collection)
        {
            FangkeServiceSoapClient fangkeService = new FangkeServiceSoapClient();
            FangkeService.HVS010TB hvs010tb = new FangkeService.HVS010TB();
            hvs010tb.dept_nm = "";
            hvs010tb.emp_nm = collection["emp_nm"];
            hvs010tb.emp_no = Request.QueryString["id"];
            hvs010tb.emp_zw = "";
            string ss = System.Guid.NewGuid().ToString();
            string s2 = ss.Substring(24, 12);
            hvs010tb.url_id = s2;
            hvs010tb.use_no = Request.QueryString["id"];
            hvs010tb.vs_btime = collection["vs_btime"];
            hvs010tb.vs_company = collection["vs_company"];
            hvs010tb.vs_count = int.Parse(collection["vs_count"]);
            hvs010tb.vs_date = collection["vs_date"];
            hvs010tb.vs_etime = "17:00";
            hvs010tb.vs_nm = collection["vs_nm"];
            hvs010tb.vs_note = collection["vs_note"];
            hvs010tb.car_no = collection["car_no"];
            DataTable dt = fangkeService.AddHVS010TB(hvs010tb);

            Message message = new Message();
            message.Msg_cd = dt.Rows[0]["msg_cd"].ToString();
            message.Msg_text = dt.Rows[0]["msg_text"].ToString();
            //登记成功后推信息
            if (message.Msg_cd == "Y")
            {
                string agentid = ConfigurationManager.AppSettings["AgentIdForFangke"];
                SendMessageServiceSoapClient sendMessageService = new SendMessageServiceSoapClient();
                Error error = sendMessageService.FangKePushMessage(s2, agentid, "fangke", "insert_succeed");
                
            }

            return View(message);
        }
    }
}
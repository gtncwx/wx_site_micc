using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Configuration;
using System.Web.Mvc;
using wx_site_micc.GetUserByCodeService;
using wx_site_micc.OAuthTwoService;
using wx_site_micc.GetTokenService;
using wx_site_micc.RenshiService;
using wx_site_micc.Models;
using wx_site_micc.SendMessageService;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Web;

namespace wx_site_micc.Controllers
{
    public class RenshiController : Controller
    {
        // GET: Renshi
        public ActionResult Index()
        {
            /*用户请求主页的时候要做的判断如下：
             1、看地址栏中是否有code参数，如果有参数的话，说明用户是从进入主页链接进入的
             2、地址栏中如果没有code参数，则有可能是打开下面的二级页面后点击手机上的返回键，退回的主页，此时就要去找用户的Cookie信息
             3、如果用户是新关注的企业号，尚未授权，则进入授权action，授权后再跳转到主页，此时地址栏中是携带code参数的*/
            string code = Request.QueryString["code"];
            //第一个判断
            if (!string.IsNullOrEmpty(code))
            {
                //获取token信息
                GetTokenServiceSoapClient getTokenService = new GetTokenServiceSoapClient();
                string agentid = ConfigurationManager.AppSettings["AgentIdForRenshi"];
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
                    //返回视图
                    return View("Index", userinfo);
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
                return View("Index", userinfo);
            }
            //第三个判断
            return RedirectToAction("OAuth", "Renshi");
        }

        /// <summary>
        /// 进行OAuth2授权注册
        /// </summary>
        /// <returns></returns>
        public ActionResult OAuth()
        {
            string agentid = ConfigurationManager.AppSettings["AgentIdForRenshi"];
            OAuthTwoServiceSoapClient oAuthTwoService = new OAuthTwoServiceSoapClient();
            string url = oAuthTwoService.OAuthTwo(agentid);
            return Redirect(url);
        }

        /// <summary>
        /// 职员档案信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Dangan()
        {
            string emp_no = Request.QueryString["id"];
            
            RenshiServiceSoapClient renshiService = new RenshiServiceSoapClient();
            DataTable dt = renshiService.SearchUserinfo(emp_no);
            Dangan dangan = new Dangan();
            if (dt.Rows.Count > 0)
            {
                dangan.Emp_no = dt.Rows[0]["emp_no"].ToString();
                dangan.Emp_nm = dt.Rows[0]["emp_nm"].ToString();
                dangan.Entr_dt = dt.Rows[0]["entr_dt"].ToString();
                dangan.Dept_nm = dt.Rows[0]["dept_nm"].ToString();
            }

            return View(dangan);
        }

        /// <summary>
        /// 工资明细查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Gongzi")]
        public ActionResult Gongzi()
        {
            List<Gongzi_Zhifu> ltzhifu = new List<Gongzi_Zhifu>();
            Gongzi_Zhifu gongzi_Zhifu = new Gongzi_Zhifu();
            ltzhifu.Add(gongzi_Zhifu);
            ViewData["zhifu"] = ltzhifu;

            List<Gongzi_Kouchu> ltkouchu = new List<Gongzi_Kouchu>();
            Gongzi_Kouchu gongzi_Kouchu = new Gongzi_Kouchu();
            ltkouchu.Add(gongzi_Kouchu);
            ViewData["kouchu"] = ltkouchu;

            List<Gongzi_Heji> ltheji = new List<Gongzi_Heji>();
            Gongzi_Heji gongzi_Heji = new Gongzi_Heji();
            ltheji.Add(gongzi_Heji);
            ViewData["heji"] = ltheji;

            Message message = new Message();
            message.Msg_cd = "Y";
            return View(message);
        }

        [HttpPost]
        [ActionName("Gongzi")]
        public ActionResult Gongzi(FormCollection collection)
        {
            string month = collection["combox_year"];
            month = month + collection["combox_month"];
            string emp_no = Request.QueryString["id"];

            RenshiServiceSoapClient renshiService = new RenshiServiceSoapClient();
            DataTable dt = renshiService.SearchMonth("gongzi", month);
            Message message = new Message();
            message.Msg_cd = dt.Rows[0]["msg_cd"].ToString();
            message.Msg_text = dt.Rows[0]["msg_text"].ToString();
            if(message.Msg_cd == "N")
            {
                List<Gongzi_Zhifu> ltzhifu = new List<Gongzi_Zhifu>();
                Gongzi_Zhifu gongzi_Zhifu = new Gongzi_Zhifu();
                ltzhifu.Add(gongzi_Zhifu);
                ViewData["zhifu"] = ltzhifu;

                List<Gongzi_Kouchu> ltkouchu = new List<Gongzi_Kouchu>();
                Gongzi_Kouchu gongzi_Kouchu = new Gongzi_Kouchu();
                ltkouchu.Add(gongzi_Kouchu);
                ViewData["kouchu"] = ltkouchu;

                List<Gongzi_Heji> ltheji = new List<Gongzi_Heji>();
                Gongzi_Heji gongzi_Heji = new Gongzi_Heji();
                ltheji.Add(gongzi_Heji);
                ViewData["heji"] = ltheji;

                return View(message);
            }

            DataSet ds = renshiService.SearchGongzi(emp_no, month);

            //津贴项目
            if (ds.Tables[0].Rows.Count > 0)
            {
                List<Gongzi_Zhifu> ltzhifu = new List<Gongzi_Zhifu>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Gongzi_Zhifu gongzi_Zhifu = new Gongzi_Zhifu();
                    gongzi_Zhifu.Emp_no = ds.Tables[0].Rows[i]["emp_no"].ToString();
                    gongzi_Zhifu.Allow_cd = ds.Tables[0].Rows[i]["allow_cd"].ToString();
                    gongzi_Zhifu.Allow_nm1 = ds.Tables[0].Rows[i]["allow_nm1"].ToString();
                    gongzi_Zhifu.Allow = Convert.ToDecimal(ds.Tables[0].Rows[i]["allow"].ToString());
                    ltzhifu.Add(gongzi_Zhifu);
                    ViewData["zhifu"] = ltzhifu;
                }
            }
            else
            {
                List<Gongzi_Zhifu> ltzhifu = new List<Gongzi_Zhifu>();
                Gongzi_Zhifu gongzi_Zhifu = new Gongzi_Zhifu();
                ltzhifu.Add(gongzi_Zhifu);
                ViewData["zhifu"] = ltzhifu;
            }
            //扣除项目
            if (ds.Tables[1].Rows.Count > 0)
            {
                List<Gongzi_Kouchu> ltkouchu = new List<Gongzi_Kouchu>();
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    Gongzi_Kouchu gongzi_Kouchu = new Gongzi_Kouchu();
                    gongzi_Kouchu.Emp_no = ds.Tables[1].Rows[i]["emp_no"].ToString();
                    gongzi_Kouchu.Sub_cd = ds.Tables[1].Rows[i]["sub_cd"].ToString();
                    gongzi_Kouchu.Allow_nm1 = ds.Tables[1].Rows[i]["allow_nm1"].ToString();
                    gongzi_Kouchu.Sub_amt = Convert.ToDecimal(ds.Tables[1].Rows[i]["sub_amt"].ToString());
                    ltkouchu.Add(gongzi_Kouchu);
                    ViewData["kouchu"] = ltkouchu;
                }
            }
            else
            {
                List<Gongzi_Kouchu> ltkouchu = new List<Gongzi_Kouchu>();
                Gongzi_Kouchu gongzi_Kouchu = new Gongzi_Kouchu();
                ltkouchu.Add(gongzi_Kouchu);
                ViewData["kouchu"] = ltkouchu;
            }
            //合计金额
            if (ds.Tables[2].Rows.Count > 0)
            {
                List<Gongzi_Heji> ltheji = new List<Gongzi_Heji>();
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    Gongzi_Heji gongzi_Heji = new Gongzi_Heji();
                    gongzi_Heji.PROV_TOT_AMT = Convert.ToDecimal(ds.Tables[2].Rows[i]["PROV_TOT_AMT"].ToString());
                    gongzi_Heji.REAL_PROV_AMT = Convert.ToDecimal(ds.Tables[2].Rows[i]["REAL_PROV_AMT"].ToString());
                    gongzi_Heji.SUB_TOT_AMT = Convert.ToDecimal(ds.Tables[2].Rows[i]["SUB_TOT_AMT"].ToString());
                    ltheji.Add(gongzi_Heji);
                    ViewData["heji"] = ltheji;
                }
            }
            else
            {
                List<Gongzi_Heji> ltheji = new List<Gongzi_Heji>();
                Gongzi_Heji gongzi_Heji = new Gongzi_Heji();
                ltheji.Add(gongzi_Heji);
                ViewData["heji"] = ltheji;
            }

            return View(message);
        }


        /// <summary>
        /// 考勤明细查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Kaoqin")]
        public ActionResult Kaoqin()
        {
            List<Kaoqin_Jiaban> ltjiaban = new List<Kaoqin_Jiaban>();
            Kaoqin_Jiaban kaoqin_Jiaban = new Kaoqin_Jiaban();
            ltjiaban.Add(kaoqin_Jiaban);
            ViewData["jiaban"] = ltjiaban;

            List<Kaoqin_Qingjia> ltqingjia = new List<Kaoqin_Qingjia>();
            Kaoqin_Qingjia kaoqin_Qingjia = new Kaoqin_Qingjia();
            ltqingjia.Add(kaoqin_Qingjia);
            ViewData["qingjia"] = ltqingjia;

            List<Kaoqin_Yichang> ltyichang = new List<Kaoqin_Yichang>();
            Kaoqin_Yichang kaoqin_Yichang = new Kaoqin_Yichang();
            ltyichang.Add(kaoqin_Yichang);
            ViewData["yichang"] = ltyichang;

            Message message = new Message();
            message.Msg_cd = "Y";
            return View(message);
        }
        /// <summary>
        /// 考勤明细查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Kaoqin")]
        public ActionResult Kaoqin(FormCollection collection)
        {
            string month = collection["combox_year"];
            month = month + collection["combox_month"];
            string emp_no = Request.QueryString["id"];
            RenshiServiceSoapClient renshiService = new RenshiServiceSoapClient();
            DataTable dt = renshiService.SearchMonth("kaoqin", month);
            Message message = new Message();
            message.Msg_cd = dt.Rows[0]["msg_cd"].ToString();
            message.Msg_text = dt.Rows[0]["msg_text"].ToString();
            if (message.Msg_cd == "N")
            {
                List<Kaoqin_Jiaban> ltjiaban = new List<Kaoqin_Jiaban>();
                Kaoqin_Jiaban kaoqin_Jiaban = new Kaoqin_Jiaban();
                ltjiaban.Add(kaoqin_Jiaban);
                ViewData["jiaban"] = ltjiaban;

                List<Kaoqin_Qingjia> ltqingjia = new List<Kaoqin_Qingjia>();
                Kaoqin_Qingjia kaoqin_Qingjia = new Kaoqin_Qingjia();
                ltqingjia.Add(kaoqin_Qingjia);
                ViewData["qingjia"] = ltqingjia;

                List<Kaoqin_Yichang> ltyichang = new List<Kaoqin_Yichang>();
                Kaoqin_Yichang kaoqin_Yichang = new Kaoqin_Yichang();
                ltyichang.Add(kaoqin_Yichang);
                ViewData["yichang"] = ltyichang;

                return View(message);
            }
            DataSet ds = renshiService.SearchKaoqin(emp_no, month);

            //加班项目
            if (ds.Tables[0].Rows.Count > 0)
            {
                List<Kaoqin_Jiaban> ltjiaban = new List<Kaoqin_Jiaban>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Kaoqin_Jiaban kaoqin_Jiaban = new Kaoqin_Jiaban();
                    kaoqin_Jiaban.Rt_date = ds.Tables[0].Rows[i]["rt_date"].ToString();
                    kaoqin_Jiaban.Wd_day = ds.Tables[0].Rows[i]["wd_day"].ToString();
                    kaoqin_Jiaban.Dilig_nm = ds.Tables[0].Rows[i]["dilig_nm"].ToString();
                    kaoqin_Jiaban.Fq_name = ds.Tables[0].Rows[i]["fq_name"].ToString();
                    kaoqin_Jiaban.Rt_btime = ds.Tables[0].Rows[i]["rt_btime"].ToString();
                    kaoqin_Jiaban.Rt_etime = ds.Tables[0].Rows[i]["rt_etime"].ToString();
                    kaoqin_Jiaban.Rt_hh = Convert.ToDecimal(ds.Tables[0].Rows[i]["rt_hh"].ToString());
                    ltjiaban.Add(kaoqin_Jiaban);
                    ViewData["jiaban"] = ltjiaban;
                }
            }
            else
            {
                List<Kaoqin_Jiaban> ltjiaban = new List<Kaoqin_Jiaban>();
                Kaoqin_Jiaban kaoqin_Jiaban = new Kaoqin_Jiaban();
                ltjiaban.Add(kaoqin_Jiaban);
                ViewData["jiaban"] = ltjiaban;
            }
            //请假项目
            if (ds.Tables[1].Rows.Count > 0)
            {
                List<Kaoqin_Qingjia> ltqingjia = new List<Kaoqin_Qingjia>();
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    Kaoqin_Qingjia kaoqin_Qingjia = new Kaoqin_Qingjia();
                    kaoqin_Qingjia.Ex_date = ds.Tables[1].Rows[i]["ex_date"].ToString();
                    kaoqin_Qingjia.Wd_day = ds.Tables[1].Rows[i]["wd_day"].ToString();
                    kaoqin_Qingjia.Dilig_nm = ds.Tables[1].Rows[i]["dilig_nm"].ToString();
                    kaoqin_Qingjia.Fq_name = ds.Tables[1].Rows[i]["fq_name"].ToString();
                    kaoqin_Qingjia.Ex_btime = ds.Tables[1].Rows[i]["ex_btime"].ToString();
                    kaoqin_Qingjia.Ex_etime = ds.Tables[1].Rows[i]["ex_etime"].ToString();
                    kaoqin_Qingjia.Ex_hh = Convert.ToDecimal(ds.Tables[1].Rows[i]["ex_hh"].ToString());
                    ltqingjia.Add(kaoqin_Qingjia);
                    ViewData["qingjia"] = ltqingjia;
                }
            }
            else
            {
                List<Kaoqin_Qingjia> ltqingjia = new List<Kaoqin_Qingjia>();
                Kaoqin_Qingjia kaoqin_Qingjia = new Kaoqin_Qingjia();
                ltqingjia.Add(kaoqin_Qingjia);
                ViewData["qingjia"] = ltqingjia;
            }
            //异常出勤
            if (ds.Tables[2].Rows.Count > 0)
            {
                List<Kaoqin_Yichang> ltyichang = new List<Kaoqin_Yichang>();
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    Kaoqin_Yichang kaoqin_Yichang = new Kaoqin_Yichang();
                    kaoqin_Yichang.Rt_date = ds.Tables[2].Rows[i]["rt_date"].ToString();
                    kaoqin_Yichang.Wd_day = ds.Tables[2].Rows[i]["wd_day"].ToString();
                    kaoqin_Yichang.Dilig_nm = ds.Tables[2].Rows[i]["dilig_nm"].ToString();
                    kaoqin_Yichang.Fq_name = ds.Tables[2].Rows[i]["fq_name"].ToString();
                    kaoqin_Yichang.Rt_btime = ds.Tables[2].Rows[i]["rt_btime"].ToString();
                    kaoqin_Yichang.Rt_etime = ds.Tables[2].Rows[i]["rt_etime"].ToString();
                    kaoqin_Yichang.Rt_hh = Convert.ToDecimal(ds.Tables[2].Rows[i]["rt_hh"].ToString());
                    ltyichang.Add(kaoqin_Yichang);
                    ViewData["yichang"] = ltyichang;
                }
            }
            else
            {
                List<Kaoqin_Yichang> ltyichang = new List<Kaoqin_Yichang>();
                Kaoqin_Yichang kaoqin_Yichang = new Kaoqin_Yichang();
                ltyichang.Add(kaoqin_Yichang);
                ViewData["yichang"] = ltyichang;
            }

            return View(message);
        }


        /// <summary>
        /// 自助请假申请
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("qingjia")]
        public ActionResult Qingjia()
        {
            //窗体加载时要先显示职员的部分档案信息
            string emp_no = Request.QueryString["id"];
            RenshiServiceSoapClient renshiService = new RenshiServiceSoapClient();
            DataTable dt = renshiService.SearchUserinfo(emp_no);
            Dangan dangan = new Dangan();
            if (dt.Rows.Count > 0)
            {
                dangan.Emp_no = dt.Rows[0]["emp_no"].ToString();
                dangan.Emp_nm = dt.Rows[0]["emp_nm"].ToString();
                dangan.Entr_dt = dt.Rows[0]["entr_dt"].ToString();
                dangan.Dept_nm = dt.Rows[0]["dept_nm"].ToString();
            }

            return View(dangan);
        }

        /// <summary>
        /// 自助请假申请 POST
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("qingjia")]
        public ActionResult Qingjia(FormCollection collection)
        {
            string emp_no = Request.QueryString["id"];//职员工号
            RenshiServiceSoapClient renshiService = new RenshiServiceSoapClient();
            DataTable dt = renshiService.SearchUserinfo(emp_no);
            Dangan dangan = new Dangan();
            if (dt.Rows.Count > 0)
            {
                dangan.Emp_no = dt.Rows[0]["emp_no"].ToString();
                dangan.Emp_nm = dt.Rows[0]["emp_nm"].ToString();
                dangan.Entr_dt = dt.Rows[0]["entr_dt"].ToString();
                dangan.Dept_nm = dt.Rows[0]["dept_nm"].ToString();
            }

            //把用户提交的请假申请insert到数据表中，然后接收返回的信息
            RenshiService.HEX001TB hex001tb = new RenshiService.HEX001TB();
            //生成一个随机串，用作主键
            string ss = System.Guid.NewGuid().ToString();
            string s2 = ss.Substring(24, 12);
            hex001tb.id = s2;
            hex001tb.emp_no = emp_no;
            hex001tb.ex_date = Convert.ToDateTime(collection["ex_date"]);
            hex001tb.dilig_cd = collection["combox_dilig"];
            hex001tb.ex_btime = collection["ex_btime"];
            hex001tb.ex_etime = collection["ex_etime"];
            hex001tb.ex_remark = collection["ex_remark"];
            hex001tb.ins_no = emp_no;
            dt = null;
            dt = renshiService.AddHEX001TB(hex001tb);

            if (dt.Rows.Count > 0)
            {
                string msg_cd = dt.Rows[0]["msg_cd"].ToString();
                string msg_text = dt.Rows[0]["msg_text"].ToString();
                //如果返回0，则说明保存成功，此时需要发送消息给管理员审核
                if (msg_cd == "0")
                {
                    //保存成功后发送信息给审核人员
                    string agentid = ConfigurationManager.AppSettings["AgentIdForRenshi"];
                    string send_class = ConfigurationManager.AppSettings["PushMsgForQingJiaRenshi"];
                    SendMessageServiceSoapClient sendMessageService = new SendMessageServiceSoapClient();
                    Error error = new Error();
                    //推送请假审核信息给审核者
                    error = sendMessageService.QingJiaPushMessage(hex001tb.id, agentid, send_class, "insert_succeed");
                    //根据返回的信息来判断是否推送信息成功
                    if (error.errcode == 0) //推送成功
                    {
                        dangan.msg_cd = msg_cd;
                        dangan.msg_text = msg_text;
                    }
                    else//推送失败，则要把登记的请假信息删除，并推送一条信息给申请的用户，告知推送失败了。
                    {
                        dangan.msg_cd = error.errcode.ToString();
                        dangan.msg_text = error.errmsg;
                        //推送失败信息给请假申请者
                        sendMessageService.QingJiaPushMessage(hex001tb.id, agentid, send_class, "sending_failure");
                        //删除请假申请信息
                        renshiService.DeleteHEX001TB(hex001tb);
                    }
                }
            }
            return View(dangan);
        }


        /// <summary>
        /// 自助请假审核
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("check")]
        public ActionResult Check()
        {
            //窗体加载时要先显示职员的请假信息
            string id = Request.QueryString["id"];//请假信息的主键
            RenshiServiceSoapClient renshiService = new RenshiServiceSoapClient();
            RenshiService.HEX001TB hex001tb = new RenshiService.HEX001TB();
            hex001tb.id = id;
            hex001tb.ex_date = DateTime.Now;
            DataTable dt = renshiService.SearchHEX001TB(hex001tb);
            Models.HEX001TB hEX001TB = new Models.HEX001TB();
            if (dt.Rows.Count > 0)
            {
                hEX001TB.emp_no = dt.Rows[0]["emp_no"].ToString();
                hEX001TB.emp_nm = dt.Rows[0]["emp_nm"].ToString();
                hEX001TB.entr_dt = dt.Rows[0]["entr_dt"].ToString();
                hEX001TB.dept_nm = dt.Rows[0]["dept_nm"].ToString();
                hEX001TB.ex_date = dt.Rows[0]["ex_date"].ToString();
                hEX001TB.dilig_nm = dt.Rows[0]["dilig_nm"].ToString();
                hEX001TB.ex_btime = dt.Rows[0]["ex_btime"].ToString();
                hEX001TB.ex_etime = dt.Rows[0]["ex_etime"].ToString();
                hEX001TB.ex_hh = dt.Rows[0]["ex_hh"].ToString();
                hEX001TB.ex_remark = dt.Rows[0]["ex_remark"].ToString();
                hEX001TB.ck_flg = dt.Rows[0]["ck_flg"].ToString();
                hEX001TB.ck_flg_nm = dt.Rows[0]["ck_flg_nm"].ToString();
                hEX001TB.ck_remark = dt.Rows[0]["ck_remark"].ToString();
            }
            return View(hEX001TB);
        }

        /// <summary>
        /// 自助请假审核 POST
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("check")]
        public ActionResult Check(FormCollection collection)
        {
            string id = Request.QueryString["id"];//请假信息的主键
            //把审核后的信息发送给申请的人
            //先更新数据库的审核状态
            RenshiServiceSoapClient renshiService = new RenshiServiceSoapClient();
            RenshiService.HEX001TB hex001tb = new RenshiService.HEX001TB();
            hex001tb.id = id;
            hex001tb.ex_date = DateTime.Now;
            hex001tb.ck_remark = collection["ck_remark"];
            hex001tb.ck_flg = collection["check"];
            DataTable dt = renshiService.CheckHEX001TB(hex001tb);

            if (dt.Rows.Count > 0)
            {
                //请假审核后，调用推送信息的函数
                string agentid = ConfigurationManager.AppSettings["AgentIdForRenshi"];
                SendMessageServiceSoapClient sendMessageService = new SendMessageServiceSoapClient();
                sendMessageService.QingJiaPushMessage(id, agentid, hex001tb.ck_flg, "check_succeed");
            }
            dt = null;
            dt = renshiService.SearchHEX001TB(hex001tb);
            Models.HEX001TB hEX001TB = new Models.HEX001TB();
            if (dt.Rows.Count > 0)
            {
                hEX001TB.emp_no = dt.Rows[0]["emp_no"].ToString();
                hEX001TB.emp_nm = dt.Rows[0]["emp_nm"].ToString();
                hEX001TB.entr_dt = dt.Rows[0]["entr_dt"].ToString();
                hEX001TB.dept_nm = dt.Rows[0]["dept_nm"].ToString();
                hEX001TB.ex_date = dt.Rows[0]["ex_date"].ToString();
                hEX001TB.dilig_nm = dt.Rows[0]["dilig_nm"].ToString();
                hEX001TB.ex_btime = dt.Rows[0]["ex_btime"].ToString();
                hEX001TB.ex_etime = dt.Rows[0]["ex_etime"].ToString();
                hEX001TB.ex_hh = dt.Rows[0]["ex_hh"].ToString();
                hEX001TB.ex_remark = dt.Rows[0]["ex_remark"].ToString();
                hEX001TB.ck_flg = dt.Rows[0]["ck_flg"].ToString();
                hEX001TB.ck_flg_nm = dt.Rows[0]["ck_flg_nm"].ToString();
                hEX001TB.ck_remark = dt.Rows[0]["ck_remark"].ToString();
            }
            return View(hEX001TB);
        }

        /// <summary>
        /// 审核完成后，返回给用户的链接action
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("queryqj")]
        public ActionResult Queryqj()
        {
            //窗体加载时要先显示职员的请假信息
            string id = Request.QueryString["id"];//请假信息的主键
            RenshiServiceSoapClient renshiService = new RenshiServiceSoapClient();
            RenshiService.HEX001TB hex001tb = new RenshiService.HEX001TB();
            hex001tb.id = id;
            hex001tb.ex_date = DateTime.Now;
            DataTable dt = renshiService.SearchHEX001TB(hex001tb);
            Models.HEX001TB hEX001TB = new Models.HEX001TB();
            if (dt.Rows.Count > 0)
            {
                hEX001TB.emp_no = dt.Rows[0]["emp_no"].ToString();
                hEX001TB.emp_nm = dt.Rows[0]["emp_nm"].ToString();
                hEX001TB.entr_dt = dt.Rows[0]["entr_dt"].ToString();
                hEX001TB.dept_nm = dt.Rows[0]["dept_nm"].ToString();
                hEX001TB.ex_date = dt.Rows[0]["ex_date"].ToString();
                hEX001TB.dilig_nm = dt.Rows[0]["dilig_nm"].ToString();
                hEX001TB.ex_btime = dt.Rows[0]["ex_btime"].ToString();
                hEX001TB.ex_etime = dt.Rows[0]["ex_etime"].ToString();
                hEX001TB.ex_hh = dt.Rows[0]["ex_hh"].ToString();
                hEX001TB.ex_remark = dt.Rows[0]["ex_remark"].ToString();
                hEX001TB.ck_flg = dt.Rows[0]["ck_flg"].ToString();
                hEX001TB.ck_flg_nm = dt.Rows[0]["ck_flg_nm"].ToString();
                hEX001TB.ck_remark = dt.Rows[0]["ck_remark"].ToString();
            }
            return View(hEX001TB);
        }

        /// <summary>
        /// 查询工资前需要先登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("login")]
        public ActionResult Login()
        {
            return View(1);
        }

        [HttpPost]
        [ActionName("login")]
        public ActionResult Login(FormCollection collection)
        {
            string id = Request.QueryString["id"];
            string pwd = collection["password"];
            RenshiServiceSoapClient renshiService = new RenshiServiceSoapClient();
            bool bl = renshiService.UserCheck(id, pwd);
            if (bl == true)
            {
                return Redirect("~/renshi/gongzi?id=" + id);
            }
            else
            {
                return View(0);
            }
        }


        /// <summary>
        /// 职员确认考勤事件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("qrkq")]
        public ActionResult Qrkq()
        {
            string emp_no = Request.QueryString["id"];

            RenshiServiceSoapClient renshiService = new RenshiServiceSoapClient();
            DataTable dt = renshiService.SearchMonth("queren", "");
            string month = dt.Rows[0]["msg_text"].ToString();
            dt = null;
            dt = renshiService.Qrkq("get", emp_no, month);
            Message message = new Message();
            message.Msg_cd = dt.Rows[0]["msg_cd"].ToString();
            message.Msg_text = dt.Rows[0]["msg_text"].ToString();

            DataSet ds = renshiService.SearchKaoqin(emp_no, month);

            //加班项目
            if (ds.Tables[0].Rows.Count > 0)
            {
                List<Kaoqin_Jiaban> ltjiaban = new List<Kaoqin_Jiaban>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Kaoqin_Jiaban kaoqin_Jiaban = new Kaoqin_Jiaban();
                    kaoqin_Jiaban.Rt_date = ds.Tables[0].Rows[i]["rt_date"].ToString();
                    kaoqin_Jiaban.Wd_day = ds.Tables[0].Rows[i]["wd_day"].ToString();
                    kaoqin_Jiaban.Dilig_nm = ds.Tables[0].Rows[i]["dilig_nm"].ToString();
                    kaoqin_Jiaban.Fq_name = ds.Tables[0].Rows[i]["fq_name"].ToString();
                    kaoqin_Jiaban.Rt_btime = ds.Tables[0].Rows[i]["rt_btime"].ToString();
                    kaoqin_Jiaban.Rt_etime = ds.Tables[0].Rows[i]["rt_etime"].ToString();
                    kaoqin_Jiaban.Rt_hh = Convert.ToDecimal(ds.Tables[0].Rows[i]["rt_hh"].ToString());
                    ltjiaban.Add(kaoqin_Jiaban);
                    ViewData["jiaban"] = ltjiaban;
                }
            }
            else
            {
                List<Kaoqin_Jiaban> ltjiaban = new List<Kaoqin_Jiaban>();
                Kaoqin_Jiaban kaoqin_Jiaban = new Kaoqin_Jiaban();
                ltjiaban.Add(kaoqin_Jiaban);
                ViewData["jiaban"] = ltjiaban;
            }
            //请假项目
            if (ds.Tables[1].Rows.Count > 0)
            {
                List<Kaoqin_Qingjia> ltqingjia = new List<Kaoqin_Qingjia>();
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    Kaoqin_Qingjia kaoqin_Qingjia = new Kaoqin_Qingjia();
                    kaoqin_Qingjia.Ex_date = ds.Tables[1].Rows[i]["ex_date"].ToString();
                    kaoqin_Qingjia.Wd_day = ds.Tables[1].Rows[i]["wd_day"].ToString();
                    kaoqin_Qingjia.Dilig_nm = ds.Tables[1].Rows[i]["dilig_nm"].ToString();
                    kaoqin_Qingjia.Fq_name = ds.Tables[1].Rows[i]["fq_name"].ToString();
                    kaoqin_Qingjia.Ex_btime = ds.Tables[1].Rows[i]["ex_btime"].ToString();
                    kaoqin_Qingjia.Ex_etime = ds.Tables[1].Rows[i]["ex_etime"].ToString();
                    kaoqin_Qingjia.Ex_hh = Convert.ToDecimal(ds.Tables[1].Rows[i]["ex_hh"].ToString());
                    ltqingjia.Add(kaoqin_Qingjia);
                    ViewData["qingjia"] = ltqingjia;
                }
            }
            else
            {
                List<Kaoqin_Qingjia> ltqingjia = new List<Kaoqin_Qingjia>();
                Kaoqin_Qingjia kaoqin_Qingjia = new Kaoqin_Qingjia();
                ltqingjia.Add(kaoqin_Qingjia);
                ViewData["qingjia"] = ltqingjia;
            }
            //异常出勤
            if (ds.Tables[2].Rows.Count > 0)
            {
                List<Kaoqin_Yichang> ltyichang = new List<Kaoqin_Yichang>();
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    Kaoqin_Yichang kaoqin_Yichang = new Kaoqin_Yichang();
                    kaoqin_Yichang.Rt_date = ds.Tables[2].Rows[i]["rt_date"].ToString();
                    kaoqin_Yichang.Wd_day = ds.Tables[2].Rows[i]["wd_day"].ToString();
                    kaoqin_Yichang.Dilig_nm = ds.Tables[2].Rows[i]["dilig_nm"].ToString();
                    kaoqin_Yichang.Fq_name = ds.Tables[2].Rows[i]["fq_name"].ToString();
                    kaoqin_Yichang.Rt_btime = ds.Tables[2].Rows[i]["rt_btime"].ToString();
                    kaoqin_Yichang.Rt_etime = ds.Tables[2].Rows[i]["rt_etime"].ToString();
                    kaoqin_Yichang.Rt_hh = Convert.ToDecimal(ds.Tables[2].Rows[i]["rt_hh"].ToString());
                    ltyichang.Add(kaoqin_Yichang);
                    ViewData["yichang"] = ltyichang;
                }
            }
            else
            {
                List<Kaoqin_Yichang> ltyichang = new List<Kaoqin_Yichang>();
                Kaoqin_Yichang kaoqin_Yichang = new Kaoqin_Yichang();
                ltyichang.Add(kaoqin_Yichang);
                ViewData["yichang"] = ltyichang;
            }

            return View(message);
        }

        [HttpPost]
        [ActionName("qrkq")]
        public ActionResult Qrkq(FormCollection collection)
        {
            string emp_no = Request.QueryString["id"];

            RenshiServiceSoapClient renshiService = new RenshiServiceSoapClient();
            DataTable dt = renshiService.SearchMonth("queren", "");
            string month = dt.Rows[0]["msg_text"].ToString();
            dt = null;
            dt = renshiService.Qrkq("post", emp_no, month);
            Message message = new Message();
            message.Msg_cd = dt.Rows[0]["msg_cd"].ToString();
            message.Msg_text = dt.Rows[0]["msg_text"].ToString();

            DataSet ds = renshiService.SearchKaoqin(emp_no, month);

            //加班项目
            if (ds.Tables[0].Rows.Count > 0)
            {
                List<Kaoqin_Jiaban> ltjiaban = new List<Kaoqin_Jiaban>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Kaoqin_Jiaban kaoqin_Jiaban = new Kaoqin_Jiaban();
                    kaoqin_Jiaban.Rt_date = ds.Tables[0].Rows[i]["rt_date"].ToString();
                    kaoqin_Jiaban.Wd_day = ds.Tables[0].Rows[i]["wd_day"].ToString();
                    kaoqin_Jiaban.Dilig_nm = ds.Tables[0].Rows[i]["dilig_nm"].ToString();
                    kaoqin_Jiaban.Fq_name = ds.Tables[0].Rows[i]["fq_name"].ToString();
                    kaoqin_Jiaban.Rt_btime = ds.Tables[0].Rows[i]["rt_btime"].ToString();
                    kaoqin_Jiaban.Rt_etime = ds.Tables[0].Rows[i]["rt_etime"].ToString();
                    kaoqin_Jiaban.Rt_hh = Convert.ToDecimal(ds.Tables[0].Rows[i]["rt_hh"].ToString());
                    ltjiaban.Add(kaoqin_Jiaban);
                    ViewData["jiaban"] = ltjiaban;
                }
            }
            else
            {
                List<Kaoqin_Jiaban> ltjiaban = new List<Kaoqin_Jiaban>();
                Kaoqin_Jiaban kaoqin_Jiaban = new Kaoqin_Jiaban();
                ltjiaban.Add(kaoqin_Jiaban);
                ViewData["jiaban"] = ltjiaban;
            }
            //请假项目
            if (ds.Tables[1].Rows.Count > 0)
            {
                List<Kaoqin_Qingjia> ltqingjia = new List<Kaoqin_Qingjia>();
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    Kaoqin_Qingjia kaoqin_Qingjia = new Kaoqin_Qingjia();
                    kaoqin_Qingjia.Ex_date = ds.Tables[1].Rows[i]["ex_date"].ToString();
                    kaoqin_Qingjia.Wd_day = ds.Tables[1].Rows[i]["wd_day"].ToString();
                    kaoqin_Qingjia.Dilig_nm = ds.Tables[1].Rows[i]["dilig_nm"].ToString();
                    kaoqin_Qingjia.Fq_name = ds.Tables[1].Rows[i]["fq_name"].ToString();
                    kaoqin_Qingjia.Ex_btime = ds.Tables[1].Rows[i]["ex_btime"].ToString();
                    kaoqin_Qingjia.Ex_etime = ds.Tables[1].Rows[i]["ex_etime"].ToString();
                    kaoqin_Qingjia.Ex_hh = Convert.ToDecimal(ds.Tables[1].Rows[i]["ex_hh"].ToString());
                    ltqingjia.Add(kaoqin_Qingjia);
                    ViewData["qingjia"] = ltqingjia;
                }
            }
            else
            {
                List<Kaoqin_Qingjia> ltqingjia = new List<Kaoqin_Qingjia>();
                Kaoqin_Qingjia kaoqin_Qingjia = new Kaoqin_Qingjia();
                ltqingjia.Add(kaoqin_Qingjia);
                ViewData["qingjia"] = ltqingjia;
            }
            //异常出勤
            if (ds.Tables[2].Rows.Count > 0)
            {
                List<Kaoqin_Yichang> ltyichang = new List<Kaoqin_Yichang>();
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    Kaoqin_Yichang kaoqin_Yichang = new Kaoqin_Yichang();
                    kaoqin_Yichang.Rt_date = ds.Tables[2].Rows[i]["rt_date"].ToString();
                    kaoqin_Yichang.Wd_day = ds.Tables[2].Rows[i]["wd_day"].ToString();
                    kaoqin_Yichang.Dilig_nm = ds.Tables[2].Rows[i]["dilig_nm"].ToString();
                    kaoqin_Yichang.Fq_name = ds.Tables[2].Rows[i]["fq_name"].ToString();
                    kaoqin_Yichang.Rt_btime = ds.Tables[2].Rows[i]["rt_btime"].ToString();
                    kaoqin_Yichang.Rt_etime = ds.Tables[2].Rows[i]["rt_etime"].ToString();
                    kaoqin_Yichang.Rt_hh = Convert.ToDecimal(ds.Tables[2].Rows[i]["rt_hh"].ToString());
                    ltyichang.Add(kaoqin_Yichang);
                    ViewData["yichang"] = ltyichang;
                }
            }
            else
            {
                List<Kaoqin_Yichang> ltyichang = new List<Kaoqin_Yichang>();
                Kaoqin_Yichang kaoqin_Yichang = new Kaoqin_Yichang();
                ltyichang.Add(kaoqin_Yichang);
                ViewData["yichang"] = ltyichang;
            }

            return View(message);
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("change_pwd")]
        public ActionResult Change_pwd()
        {
            return View();
        }

        [HttpPost]
        [ActionName("change_pwd")]
        public ActionResult Change_pwd(FormCollection collection)
        {
            string id = Request.QueryString["id"];
            string old = collection["old"];
            string pwd = collection["password"];
            string pwd1 = collection["password1"];
            if (pwd != pwd1)
            {
                return View(2);
            }
            else
            {
                RenshiServiceSoapClient renshiService = new RenshiServiceSoapClient();
                bool bl = renshiService.UserCheck(id, old);
                if (bl == true)
                {
                    bl = false;
                    bl = renshiService.Change_pwd(id, pwd);
                    if (bl == true)
                    {
                        return View(1);
                    }
                    else
                    {
                        return View(0);
                    }
                }
                else
                {
                    return View(3);
                }
            }
        }

        [HttpPost]
        [ActionName("Qianming")]
        public ActionResult Qianming(FormCollection collection)
        {
            string strimage = collection["image"];
            string result = string.Empty;
            string relative_filename = "";
            try
            {
                string base64Image = strimage;
                strimage = strimage.Replace("data:image/png;base64,", "");

                Bitmap bitmap = GetImageFromBase64(strimage);
                string relative_path = HttpContext.Server.MapPath("../upload/");//设定上传的文件路径
                relative_filename = relative_path + DateTime.Now.ToString("yyyyMMddHHmmsssss") + ".png";
                //string relative_path = Server.MapPath("upload/signature/");
                if (!Directory.Exists(relative_path))
                {
                    Directory.CreateDirectory(relative_path);
                }
                string basepath = Server.MapPath(relative_filename);
                bitmap.Save(basepath, ImageFormat.Png);
                bitmap.Dispose();

                HttpPostedFileBase file =null;
               
                file.SaveAs(relative_filename);
                result = relative_filename;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            Response.Write(result);
            Response.End();
            return Content(relative_filename);
        }



        public Bitmap GetImageFromBase64(string base64string)
        {
            byte[] b = Convert.FromBase64String(base64string);
            MemoryStream ms = new MemoryStream(b);
            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }

        public string GetBase64FromImage(string imagefile)
        {
            string strbaser64 = "";
            try
            {
                Bitmap bmp = new Bitmap(imagefile);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                strbaser64 = Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                throw new Exception("Something wrong during convert!");
            }
            return strbaser64;
        }
    }
}
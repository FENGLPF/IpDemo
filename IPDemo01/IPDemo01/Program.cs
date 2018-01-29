using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace IPDemo01
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr1=new int[10];
            Console.WriteLine(arr1[0]);

            string ip = "14.221.236.24";

            string PostUrl = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?ip=" + ip;

            string res = GetDataByPost(PostUrl);//该条请求返回的数据为：res=1\t115.193.210.0\t115.194.201.255\t中国\t浙江\t杭州\t电信


            string[] arr = getAreaInfoList(res);

            Console.WriteLine(arr[0]);

            Console.ReadKey();
        }


        /// <summary>

        /// 根据IP获取省市

        /// </summary>

        public void GetAddressByIp(string IP)

        {

            string ip = IP;

            string PostUrl = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?ip=" + ip;

            string res = GetDataByPost(PostUrl);//该条请求返回的数据为：res=1\t115.193.210.0\t115.194.201.255\t中国\t浙江\t杭州\t电信


            string[] arr = getAreaInfoList(res);

        }


        /// <summary>

        /// Post请求数据

        /// </summary>

        /// <param name="url"></param>

        /// <returns></returns>

        public static string GetDataByPost(string url)

        {

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            string s = "anything";

            byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(s);

            req.Method = "POST";

            req.ContentType = "application/x-www-form-urlencoded";

            req.ContentLength = requestBytes.Length;

            Stream requestStream = req.GetRequestStream();

            requestStream.Write(requestBytes, 0, requestBytes.Length);

            requestStream.Close();


            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);

            string backstr = sr.ReadToEnd();

            sr.Close();

            res.Close();

            return backstr;

        }


        /// <summary>

        /// 处理所要的数据

        /// </summary>

        /// <param name="ip"></param>

        /// <returns></returns>

        public static string[] getAreaInfoList(string ipData)

        {

            //1\t115.193.210.0\t115.194.201.255\t中国\t浙江\t杭州\t电信

            string[] areaArr = new string[10];

            string[] newAreaArr = new string[2];

            try

            {

                //取所要的数据，这里只取省市

                areaArr = ipData.Split('\t');

                newAreaArr[0] = areaArr[4];//省

                newAreaArr[1] = areaArr[5];//市

            }

            catch (Exception e)

            {

                // TODO: handle exception

            }

            return newAreaArr;

        }























        /// <summary>         
        /// 得到真实IP以及所在地详细信息（Porschev）         
        /// </summary>         
        /// <returns></returns>         
        public static string GetIpDetails()
        {
            //设置获取IP地址和国家源码的网址           
            string url = "http://www.ip138.com/ips8.asp";
            string regStr = "(?<=<td\\s*align=\\\"center\\\">)[^<]*?(?=<br/><br/></td>)";

            //IP正则
            string ipRegStr = "((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)";

            //IP地址                 
            string ip = string.Empty;

            //国家            
            string country = string.Empty;

            //省市             
            string adr = string.Empty;

            //得到网页源码             
            string html = GetHtml(url);
            Regex reg = new Regex(regStr, RegexOptions.None);
            Match ma = reg.Match(html); html = ma.Value;
            Regex ipReg = new Regex(ipRegStr, RegexOptions.None);
            ma = ipReg.Match(html);

            //得到IP  
            //ip = ma.Value;
            ip = "14.221.237.92";
            int index = html.LastIndexOf("：") + 1;

            //得到国家
            country = html.Substring(index);
            adr = GetAdrByIp(ip);
            return "IP：" + ip + "  国家：" + country + "  省市：" + adr;
        }


        /// <summary>         
        /// 通过IP得到IP所在地省市（Porschev）         
        /// </summary>         
        /// <param name="ip"></param>         
        /// <returns></returns>         
        public static string GetAdrByIp(string ip)
        {
            string url = "http://www.cz88.net/ip/?ip=" + ip;
            string regStr = "(?<=<span\\s*id=\\\"cz_addr\\\">).*?(?=</span>)";

            //得到网页源码
            string html = GetHtml(url);
            Regex reg = new Regex(regStr, RegexOptions.None);
            Match ma = reg.Match(html);
            html = ma.Value;
            string[] arr = html.Split(' ');
            return arr[0];
        }
/// <summary>         
/// 获取HTML源码信息(Porschev)         
/// </summary>         
/// <param name="url">获取地址</param>         
/// <returns>HTML源码</returns>         
        public static string GetHtml(string url)
        {
            string str = "";
            try
            {
                Uri uri = new Uri(url);
                WebRequest wr = WebRequest.Create(uri);
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                str = sr.ReadToEnd();
            }
            catch (Exception e)
            {
            }
            return str;
        }
    }
}

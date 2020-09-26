using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace DropTheBitCoin
{

    public partial class Form1 : Form
    {
        private double[] buy = new double[10000];
        private int countBuy = 0;

        public Form1()
        {
            InitializeComponent();
            chart1.Series[0].Points.Clear();
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 100;
        }

        private double getMax()
        {
            double max = buy[countBuy];
            for (int i = 0; i < countBuy; i++)
            {
                if (max < buy[i])
                {
                    max = buy[i];
                }
            }
            return max;
        }

        private double getMin()
        {
            double min = buy[countBuy];
            for (int i = 0; i < countBuy; i++)
            {
                if (min > buy[i])
                {
                    min = buy[i];
                }
            }
            return min;
        }

        private void setChart()
        {
            double maxOfBuy = getMax();
            double minOfBuy = getMin();
            textBox3.Text = minOfBuy.ToString();
            textBox4.Text = maxOfBuy.ToString();
            chart1.ChartAreas[0].AxisY.Minimum = minOfBuy - (minOfBuy * 0.01);
            chart1.ChartAreas[0].AxisY.Maximum = maxOfBuy + (maxOfBuy * 0.01);

            chart1.Series[0].Points.AddXY(countBuy, buy[countBuy]);

            //chart1.Series[0].ChartType = SeriesChartType.Pie;
            //chart1.Series[0].ChartType = SeriesChartType.Line;
            //chart1.Series[0].ChartType = SeriesChartType.Bar;
            chart1.Series[0].ChartType = SeriesChartType.SplineArea;
        }

        private void getPublicTicker()
        {
            string sAPI_Key = "api connect key";
            string sAPI_Secret = "api secret key";

            string sParams = "order_currency=BTC&payment_currency=KRW";
            string sRespBodyData = String.Empty;
            XCoinAPI hAPI_Svr;
            JObject JObj = null;


            hAPI_Svr = new XCoinAPI(sAPI_Key, sAPI_Secret);

            //
            // public api
            //
            // /public/ticker
            // /public/recent_ticker
            // /public/orderbook
            // /public/recent_transactions

            Console.WriteLine("Bithumb Public API URI('/public/ticker') Request...");
            JObj = hAPI_Svr.xcoinApiCall("/public/ticker/XRP", sParams, ref sRespBodyData);
            if (JObj == null)
            {
                Console.WriteLine("Error occurred!");
                Console.WriteLine("HTTP Response JSON Data: {0}", sRespBodyData);
            }
            else
            {
                Console.WriteLine(JObj.ToString());

                if (String.Compare(JObj["status"].ToString(), "0000", true) == 0)
                {
                    if (textBox1.Text != JObj["data"]["date"].ToString())
                    {
                        textBox1.Text = JObj["data"]["date"].ToString();
                        //textBox2.Text = JObj["data"]["sell_price"].ToString();

                        Console.WriteLine("- Status Code: {0}", JObj["status"].ToString());
                        Console.WriteLine("- Opening Price: {0}", JObj["data"]["opening_price"].ToString());
                        Console.WriteLine("- Closing Price: {0}", JObj["data"]["closing_price"].ToString());
                        Console.WriteLine("- Min Price: {0}", JObj["data"]["min_price"].ToString());
                        Console.WriteLine("- Max Price: {0}", JObj["data"]["max_price"].ToString());
                        //Console.WriteLine("- Average Price: {0}", JObj["data"]["average_price"].ToString());
                        Console.WriteLine("- units traded: {0}", JObj["data"]["units_traded"].ToString());
                        //Console.WriteLine("- volume 1day: {0}", JObj["data"]["volume_1day"].ToString());
                        //Console.WriteLine("- volume_7day: {0}", JObj["data"]["volume_7day"].ToString());
                        //Console.WriteLine("- Buy Price: {0}", JObj["data"]["buy_price"].ToString());
                        //Console.WriteLine("- sell_price: {0}", JObj["data"]["sell_price"].ToString());
                        Console.WriteLine("- date: {0}", JObj["data"]["date"].ToString());

                        listBox1.Items.Add(JObj["data"]["date"].ToString()
                            + " " + JObj["data"]["min_price"].ToString()
                            + " " + JObj["data"]["max_price"].ToString()
                            );

                        //buy[countBuy] = Convert.ToDouble(JObj["data"]["buy_price"].ToString());
                        //buy[countBuy] = Convert.ToDouble(JObj["data"]["buy_price"].ToString());
                        setChart();
                        countBuy++;

                        
                    }
                }
            }

            //Console.Write("\n\n");


            //
            // private api
            //
            // endpoint => parameters
            // /info/current
            // /info/account
            // /info/balance
            // /info/wallet_address

            /* Console.WriteLine("Bithumb Private API URI('/info/account') Request...");
            JObj = hAPI_Svr.xcoinApiCall("/info/account", sParams, ref sRespBodyData);
            if (JObj == null)
            {
                Console.WriteLine("Error occurred!");
                Console.WriteLine("HTTP Response JSON Data: {0}", sRespBodyData);
            }
            else {
                Console.WriteLine(JObj.ToString());

                if (String.Compare(JObj["status"].ToString(), "0000", true) == 0) {
                    Console.WriteLine("- Status Code: {0}", JObj["status"].ToString());
                    Console.WriteLine("- Created: {0}", JObj["data"]["created"].ToString());
                    Console.WriteLine("- Account ID: {0}", JObj["data"]["account_id"].ToString());
                    Console.WriteLine("- Trade Fee: {0}", JObj["data"]["trade_fee"].ToString());
                    Console.WriteLine("- Balance: {0}", JObj["data"]["balance"].ToString());
                }
            } */
        }

        private void getPublicOrderbook()
        {
            string sAPI_Key = "api connect key";
            string sAPI_Secret = "api secret key";

            string sParams = "order_currency=BTC&payment_currency=KRW";
            string sRespBodyData = String.Empty;
            XCoinAPI hAPI_Svr;
            JObject JObj = null;


            hAPI_Svr = new XCoinAPI(sAPI_Key, sAPI_Secret);

            //
            // public api
            //
            // /public/ticker
            // /public/recent_ticker
            // /public/orderbook
            // /public/recent_transactions

            Console.WriteLine("Bithumb Public API URI('/public/orderbook') Request...");
            JObj = hAPI_Svr.xcoinApiCall("/public/orderbook/BTC", sParams, ref sRespBodyData);
            if (JObj == null)
            {
                Console.WriteLine("Error occurred!");
                Console.WriteLine("HTTP Response JSON Data: {0}", sRespBodyData);
            }
            else
            {
                Console.WriteLine(JObj.ToString());

                if (String.Compare(JObj["status"].ToString(), "0000", true) == 0)//정상처리 되었으면
                {
                    Console.WriteLine("- Status Code: {0}", JObj["status"].ToString());
                    Console.WriteLine("- Opening Price: {0}", JObj["data"]["timestamp"].ToString());
                    Console.WriteLine("- Closing Price: {0}", JObj["data"]["order_currency"].ToString());
                    Console.WriteLine("- Closing Price: {0}", JObj["data"]["payment_currency"].ToString());
                    Console.WriteLine("- Closing Price: {0}", JObj["data"]["bids"].ToString());
                    Console.WriteLine("- Closing Price: {0}", JObj["data"]["asks"].ToString());

                    //Console.WriteLine("- Sell Price: {0}", JObj["data"]["quantity"].ToString());
                    //Console.WriteLine("- Buy Price: {0}", JObj["data"]["price"].ToString());
                    listBox1.Items.Add(JObj["data"]["bids"][0]["quantity"].ToString());
                    listBox2.Items.Add(JObj["data"]["bids"][0]["price"].ToString());
                    //listBox2.Items.Add(JObj["data"]["asks"][0].ToString());
                    //listBox2.Items.Add(JObj["data"]["asks"][1].ToString());
                }
            }

            //
            // private api
            //
            // endpoint => parameters
            // /info/current
            // /info/account
            // /info/balance
            // /info/wallet_address

            /* Console.WriteLine("Bithumb Private API URI('/info/account') Request...");
            JObj = hAPI_Svr.xcoinApiCall("/info/account", sParams, ref sRespBodyData);
            if (JObj == null)
            {
                Console.WriteLine("Error occurred!");
                Console.WriteLine("HTTP Response JSON Data: {0}", sRespBodyData);
            }
            else {
                Console.WriteLine(JObj.ToString());

                if (String.Compare(JObj["status"].ToString(), "0000", true) == 0) {
                    Console.WriteLine("- Status Code: {0}", JObj["status"].ToString());
                    Console.WriteLine("- Created: {0}", JObj["data"]["created"].ToString());
                    Console.WriteLine("- Account ID: {0}", JObj["data"]["account_id"].ToString());
                    Console.WriteLine("- Trade Fee: {0}", JObj["data"]["trade_fee"].ToString());
                    Console.WriteLine("- Balance: {0}", JObj["data"]["balance"].ToString());
                }
            } */
        }

        private void getpublicRecentTransactions()
        {
            string sAPI_Key = "api connect key";
            string sAPI_Secret = "api secret key";

            string sParams = "order_currency=BTC&payment_currency=KRW";
            string sRespBodyData = String.Empty;
            XCoinAPI hAPI_Svr;
            JObject JObj = null;


            hAPI_Svr = new XCoinAPI(sAPI_Key, sAPI_Secret);

            //
            // public api
            //
            // /public/ticker
            // /public/recent_ticker
            // /public/orderbook
            // /public/recent_transactions

            Console.WriteLine("Bithumb Public API URI('/public/recent_transactions') Request...");
            JObj = hAPI_Svr.xcoinApiCall("/public/recent_transactions/BTC", sParams, ref sRespBodyData);
            if (JObj == null)
            {
                Console.WriteLine("Error occurred!");
                Console.WriteLine("HTTP Response JSON Data: {0}", sRespBodyData);
            }
            else
            {
                Console.WriteLine(JObj.ToString());

                if (String.Compare(JObj["status"].ToString(), "0000", true) == 0)
                {
                    if (textBox1.Text != JObj["data"][0]["transaction_date"].ToString())
                    {
                        Console.WriteLine("- Status Code: {0}", JObj["status"].ToString());
                        Console.WriteLine("- transaction_date: {0}", JObj["data"][0]["transaction_date"].ToString());
                        Console.WriteLine("- type: {0}", JObj["data"][0]["type"].ToString());
                        Console.WriteLine("- units_traded: {0}", JObj["data"][0]["units_traded"].ToString());
                        Console.WriteLine("- price: {0}", JObj["data"][0]["price"].ToString());
                        Console.WriteLine("- total: {0}", JObj["data"][0]["total"].ToString());

                        listBox1.Items.Add(JObj["data"][0]["transaction_date"].ToString() + " "
                            + JObj["data"][0]["type"].ToString() + " "
                            + JObj["data"][0]["units_traded"].ToString() + " "
                            + JObj["data"][0]["price"].ToString() + " "
                            + JObj["data"][0]["total"].ToString()
                            );
                        textBox1.Text = JObj["data"][0]["transaction_date"].ToString();
                        
                        
                    }
                }
            }

            Console.Write("\n\n");


            //
            // private api
            //
            // endpoint => parameters
            // /info/current
            // /info/account
            // /info/balance
            // /info/wallet_address

            /* Console.WriteLine("Bithumb Private API URI('/info/account') Request...");
            JObj = hAPI_Svr.xcoinApiCall("/info/account", sParams, ref sRespBodyData);
            if (JObj == null)
            {
                Console.WriteLine("Error occurred!");
                Console.WriteLine("HTTP Response JSON Data: {0}", sRespBodyData);
            }
            else {
                Console.WriteLine(JObj.ToString());

                if (String.Compare(JObj["status"].ToString(), "0000", true) == 0) {
                    Console.WriteLine("- Status Code: {0}", JObj["status"].ToString());
                    Console.WriteLine("- Created: {0}", JObj["data"]["created"].ToString());
                    Console.WriteLine("- Account ID: {0}", JObj["data"]["account_id"].ToString());
                    Console.WriteLine("- Trade Fee: {0}", JObj["data"]["trade_fee"].ToString());
                    Console.WriteLine("- Balance: {0}", JObj["data"]["balance"].ToString());
                }
            } */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            getPublicTicker();
            //getPublicOrderbook();
            //getpublicRecentTransactions();

        }
    }


    class XCoinAPI
    {
        private string m_sAPI_URL = "https://api.bithumb.com";
        private string m_sAPI_Key = "";
        private string m_sAPI_Secret = "";


        public XCoinAPI(string sAPI_Key, string sAPI_Secret)
        {
            this.m_sAPI_Key = sAPI_Key;
            this.m_sAPI_Secret = sAPI_Secret;
        }


        private string ByteToString(byte[] rgbyBuff)
        {
            string sHexStr = "";


            for (int nCnt = 0; nCnt < rgbyBuff.Length; nCnt++)
            {
                sHexStr += rgbyBuff[nCnt].ToString("x2"); // Hex format
            }

            return (sHexStr);
        }


        private byte[] StringToByte(string sStr)
        {
            byte[] rgbyBuff = Encoding.UTF8.GetBytes(sStr);

            return (rgbyBuff);
        }


        private long MicroSecTime()
        {
            long nEpochTicks = 0;
            long nUnixTimeStamp = 0;
            long nNowTicks = 0;
            long nowMiliseconds = 0;
            string sNonce = "";
            DateTime DateTimeNow;


            nEpochTicks = new DateTime(1970, 1, 1).Ticks;
            DateTimeNow = DateTime.UtcNow;
            nNowTicks = DateTimeNow.Ticks;
            nowMiliseconds = DateTimeNow.Millisecond;

            nUnixTimeStamp = ((nNowTicks - nEpochTicks) / TimeSpan.TicksPerSecond);

            sNonce = nUnixTimeStamp.ToString() + nowMiliseconds.ToString("D03");

            return (Convert.ToInt64(sNonce));
        }


        private string Hash_HMAC(string sKey, string sData)
        {
            byte[] rgbyKey = Encoding.UTF8.GetBytes(sKey);


            using (var hmacsha512 = new HMACSHA512(rgbyKey))
            {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(sData));

                return (ByteToString(hmacsha512.Hash));
            }
        }


        public JObject xcoinApiCall(string sEndPoint, string sParams, ref string sRespBodyData)
        {
            string sAPI_Sign = "";
            string sPostData = sParams;
            string sHMAC_Key = "";
            string sHMAC_Data = "";
            string sResult = "";
            long nNonce = 0;
            HttpStatusCode nCode = 0;


            sPostData += "&endpoint=" + Uri.EscapeDataString(sEndPoint);

            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(this.m_sAPI_URL + sEndPoint);
                byte[] rgbyData = Encoding.ASCII.GetBytes(sPostData);


                nNonce = MicroSecTime();

                sHMAC_Key = this.m_sAPI_Secret;
                sHMAC_Data = sEndPoint + (char)0 + sPostData + (char)0 + nNonce.ToString();
                sResult = Hash_HMAC(sHMAC_Key, sHMAC_Data);
                sAPI_Sign = Convert.ToBase64String(StringToByte(sResult));

                Request.Headers.Add("Api-Key", this.m_sAPI_Key);
                Request.Headers.Add("Api-Sign", sAPI_Sign);
                Request.Headers.Add("Api-Nonce", nNonce.ToString());

                Request.Method = "POST";
                Request.ContentType = "application/x-www-form-urlencoded";
                Request.ContentLength = rgbyData.Length;

                using (var stream = Request.GetRequestStream())
                {
                    stream.Write(rgbyData, 0, rgbyData.Length);
                }

                var Response = (HttpWebResponse)Request.GetResponse();

                sRespBodyData = new StreamReader(Response.GetResponseStream()).ReadToEnd();

                return (JObject.Parse(sRespBodyData));
            }
            catch (WebException webEx)
            {
                using (HttpWebResponse Response = (HttpWebResponse)webEx.Response)
                {
                    nCode = Response.StatusCode;

                    using (StreamReader reader = new StreamReader(Response.GetResponseStream()))
                    {
                        sRespBodyData = reader.ReadToEnd();

                        return (JObject.Parse(sRespBodyData));
                    }
                }
            }

            return (null);
        }
    }

    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDP_chat
{
    class Program
    {
        static int RemotePort;
        static int LocalPort;
        static IPAddress RemoteIPAddr;
        static UdpClient _client;

        static void Main(string[] args)
        {
            try
            {
                Console.SetWindowSize(40, 20);
                Console.Title = "Chat";
                Console.WriteLine("Enter remote IP: ");
                RemoteIPAddr = IPAddress.Parse(Console.ReadLine());
                Console.WriteLine("enter remote port: ");
                RemotePort = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("enter local port: ");
                LocalPort = Convert.ToInt32(Console.ReadLine());
                Console.ForegroundColor = ConsoleColor.Red;
                _client = new UdpClient(LocalPort);
                while (true)
                {
                    FuncReceive();
                    SendData(Console.ReadLine());
                }
            }
            catch (FormatException formExc)
            {
                Console.WriteLine("impossible :" + formExc);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
            finally
            {
                if (_client != null)
                    _client.Close();
            }
        }

        private static async void FuncReceive()
        {
            try
            {
                while (true)
                {
                    var res = await _client.ReceiveAsync();
                    string strResult = Encoding.UTF8.GetString(res.Buffer); 
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(strResult);
                    Console.ForegroundColor = ConsoleColor.Red;
                }
            }
            catch (SocketException sockEx)
            {
                Console.WriteLine("Socket error: " + sockEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private static async void SendData(string datagramm)
        {
            IPEndPoint ipEnd = new IPEndPoint(RemoteIPAddr, RemotePort);
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(datagramm);
                await _client.SendAsync(bytes, bytes.Length, ipEnd);
            }
            catch (SocketException sockEx)
            {
                Console.WriteLine("Socket error: " + sockEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}

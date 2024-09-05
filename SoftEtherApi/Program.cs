using System;
using SoftEtherApi.Api;
using SoftEtherApi.Containers;

namespace SoftEtherApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			基础测试();
			// using (var softEther = new SoftEther("192.168.1.7", 1701))
			// {
			// 	//连接和鉴权(softEther);
			//
			// 	//获取已连接客户端和包数量(softEther);
			//
			// 	//获取用户及IP及NAT及DHCP(softEther);
			// }
		}

		private static void 基础测试()
		{
			var softEther = new SoftEther("192.168.1.252", 1701);
			var connectResult = softEther.Connect();
			if (connectResult.Error != SoftEtherError.NoError)
			{
				Console.WriteLine("Failed to connect: " + connectResult.Error);
			}
			
			Console.WriteLine("Connected to " + connectResult.hello);

			var authResult = softEther.Authenticate("这里输入密码", "写了'VPN'反而登录不上去不知道为啥,不写这个参数反而可以登录");
			Console.WriteLine("Authenticated: " + authResult.Error);
			//获取已经连接的用户列表
			var softEtherServer = new SoftEtherServer(softEther);
			var connectionList = softEtherServer.GetConnectionList();
			foreach (var conn in connectionList)
			{
				Console.WriteLine("Connection: " + conn.Name);
			}
		}

		private static void 连接和鉴权(SoftEther softEther)
		{
			var connectResult = softEther.Connect();
			var authResult = softEther.Authenticate("填写正确的密码");
			Console.WriteLine("Connected to " + connectResult.hello);
			Console.WriteLine("Authenticated: " + authResult.Error);
		}

		private static void 获取用户及IP及NAT及DHCP(SoftEther softEther)
		{
			var userList = softEther.HubApi.GetUserList("VPN");
			foreach (var user in userList)
			{
				Console.WriteLine("user:" + user.Name);
			}
				
			var ipList = softEther.HubApi.GetIpAddressList("VPN");
			foreach (var ip in ipList)
			{
				Console.WriteLine("ip:" + ip.Ip.ToString());
			}
				
			foreach (var nat in softEther.HubApi.GetNatList("VPN"))
			{
				Console.WriteLine("NAT:" + nat);
			}
				
			foreach (var a in softEther.HubApi.GetDhcpList("VPN"))
			{
				Console.WriteLine("DHCP:" + a);
			}
		}

		private static void 获取已连接客户端和包数量(SoftEther softEther)
		{
			var sessionList = softEther.HubApi.GetSessionList("VPN");
			Console.WriteLine("已连接客户端:");
			foreach (var session in sessionList)
			{
				Console.WriteLine($"用户名:{session.Username}, 主机:{session.Hostname}, IP:{session.Ip}, 包数量:{session.PacketNum}"); 
			}
			//每隔1秒钟清屏然后输出包所有用户的包数量
			while (true)
			{
				System.Threading.Thread.Sleep(1000);
				Console.Clear();
				sessionList = softEther.HubApi.GetSessionList("VPN");
				Console.WriteLine("已连接客户端:");
				foreach (var session in sessionList)
				{
					Console.WriteLine($"用户名:{session.Username}, 主机:{session.Name}, IP:{session.Ip}, 包数量:{session.PacketNum}"); 
				}
			}
		}
	}

	// public class Tester
	// {
	// 	public static void Test()
	// 	{
	// 		Program.Main(null);
	// 	}
	// }
}
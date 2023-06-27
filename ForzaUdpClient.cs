using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using FrozaHorizonTelemetry;
using System.Diagnostics;

namespace ForzaHorizonOverlay
{
	internal class ForzaUdpClient
	{
		public ForzaData data;
		public bool received;

		IPEndPoint ipEndPoint;

		bool isRunning = false;
		Task? receiverTask;

		public void Run()
		{
			isRunning = true;
			receiverTask = Task.Run(async () =>
			{
				UdpClient udpClient = new UdpClient(ipEndPoint);
				while (isRunning)
				{
					await udpClient.ReceiveAsync().ContinueWith(receiveResult =>
					{
						byte[] receiveBytes = receiveResult.Result.Buffer;
						data.Update(receiveBytes);

						received = true;
					});
				}
			});
		}

		public void Stop()
		{
			isRunning = false;
		}

		public ForzaUdpClient(IPEndPoint endPoint)
		{
			data = new ForzaData();
			received = false;

			ipEndPoint = endPoint;
		}
	}
}

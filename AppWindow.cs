using FrozaHorizonTelemetry;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ForzaHorizonOverlay
{
	internal class AppWindow
	{
		Font font;
		RenderWindow renderWindow;
		
		static byte[] axisPropulsion = { 0, 0, 0, 0 };
		static sbyte steer = 0;

		static ForzaUdpClient? forzaUdpClient;

		static List<Drawable> staticDrawables = new List<Drawable>();

		void OnClose(object? sender, EventArgs e)
		{
			renderWindow.Close();
		}

		void DrawAxis()
		{
			int i = 0;
			while (i < 4)
			{
				if (Config.clutchEBreakStyle >= 2 && i == 2)
				{
					i++;
					continue;
				}

				const float scale = 2.5f;
				float posX = Config.metreStartX + ((Config.clutchEBreakStyle >= 1 && i >= 2) ? 2 : i) * (Config.metreWidth + Config.metreSpacing);
				float posY = (Config.clutchEBreakStyle >= 1 && i == 3) ? Config.metreStartY + Config.metreHeight * (1 - 1 / scale) : Config.metreStartY;
				float metreHeightMax = (Config.clutchEBreakStyle >= 1 && i >= 2) ? (float)Math.Floor(Config.metreHeight / scale) : Config.metreHeight;
				//float posY = Config.metreStartY;
				//float metreHeightMax = Config.metreHeight;

				RectangleShape border = new RectangleShape(new Vector2f(Config.metreWidth, metreHeightMax));
				border.Position = new Vector2f(posX, posY);
				border.FillColor = new Color(0, 0, 0, 0);
				border.OutlineColor = Config.borderColour;
				border.OutlineThickness = 1;

				float metreHeight = metreHeightMax * (axisPropulsion[i] / 255.0f);
				RectangleShape metre = new RectangleShape(new Vector2f(Config.metreWidth, metreHeight));
				metre.Position = new Vector2f(posX, posY + metreHeightMax - metreHeight);
				metre.FillColor = Config.axisColours[i];

				Text axisTxt = new Text(Config.axisNames[i], font, (uint)(Config.metreWidth / 2.0));
				axisTxt.Rotation = -90.0f;
				axisTxt.Position = new Vector2f(posX + Config.metreWidth / 4.0f, posY + metreHeightMax - Config.metreWidth / 4.0f);
				axisTxt.FillColor = Config.borderColour;

				renderWindow.Draw(metre);
				renderWindow.Draw(border);
				renderWindow.Draw(axisTxt);

				i++;
			}
		}

		void DrawSteer()
		{
			float centerX = Config.steerStartX + Config.steerWidth / 2.0f;
			//float centerY = Config.steerStartY + Config.steerHeight / 2.0f;
			float joystickCenterY = Config.steerStartY + Config.steerHeight * Config.steerJoystickOffset;

			Joystick.Update();
			float joystickX = centerX + Joystick.GetAxisPosition(0, Joystick.Axis.X) / 100.0f * Config.steerJoystickRadius - Config.steerJoystickRadius;
			float joystickY = joystickCenterY + Joystick.GetAxisPosition(0, Joystick.Axis.Y) / 100.0f * Config.steerJoystickRadius - Config.steerJoystickRadius;

			CircleShape joystickBg = new CircleShape(Config.steerJoystickRadius);
			joystickBg.Position = new Vector2f(centerX - Config.steerJoystickRadius, joystickCenterY - Config.steerJoystickRadius);
			joystickBg.FillColor = new Color(96, 96, 96);
			joystickBg.OutlineColor = new Color(16, 16, 16);
			joystickBg.OutlineThickness = 1;

			CircleShape joystick = new CircleShape(Config.steerJoystickRadius);
			//steer / 127.0f * 32.0f
			joystick.Position = new Vector2f(joystickX, joystickY);
			joystick.FillColor = new Color(216, 216, 216);
			joystick.OutlineColor = new Color(144, 144, 144);
			joystick.OutlineThickness = 1;

			renderWindow.Draw(joystickBg);
			renderWindow.Draw(joystick);

			float steerMetreY = Config.steerStartY + Config.steerHeight - Config.steerMetreHeight;
			RectangleShape steerMetreBorder = new RectangleShape(new Vector2f(Config.steerWidth, Config.steerMetreHeight));
			steerMetreBorder.Position = new Vector2f(Config.steerStartX, steerMetreY);
			steerMetreBorder.FillColor = new Color(0, 0, 0, 0);
			steerMetreBorder.OutlineColor = Config.borderColour;
			steerMetreBorder.OutlineThickness = 1;

			RectangleShape steerMetre = new RectangleShape(new Vector2f(Config.steerWidth / 2.0f * (steer / 127.0f), Config.steerMetreHeight));
			steerMetre.Position = new Vector2f(centerX, steerMetreY);
			steerMetre.FillColor = Config.steerMetreColour;

			RectangleShape centerLine = new RectangleShape(new Vector2f(1, Config.steerMetreHeight));
			centerLine.Position = new Vector2f(centerX, steerMetreY);
			centerLine.FillColor = Config.borderColour;

			renderWindow.Draw(steerMetre);
			renderWindow.Draw(centerLine);
			renderWindow.Draw(steerMetreBorder);
		}

		public void Run()
		{
			forzaUdpClient = new ForzaUdpClient(Config.ipEndPoint);

			forzaUdpClient.Run();

			while (renderWindow.IsOpen)
			{
				renderWindow.Clear(Config.bgColour);
				renderWindow.DispatchEvents();

				if (forzaUdpClient.received)
				{
					axisPropulsion[0] = (byte)forzaUdpClient.data.Throttle;
					axisPropulsion[1] = (byte)forzaUdpClient.data.Brake;
					axisPropulsion[2] = (byte)forzaUdpClient.data.Clutch;
					axisPropulsion[3] = (byte)forzaUdpClient.data.Handbrake;

					steer = (sbyte)forzaUdpClient.data.Steer;

					forzaUdpClient.received = false;
				}

				DrawAxis();

				DrawSteer();

				renderWindow.Display();
			}
		}

		public AppWindow()
		{
			string assemblyPath = string.Join('\\', Assembly.GetExecutingAssembly().Location.Split("\\").SkipLast(1));
			string fontFilePath = Path.Combine(assemblyPath, Config.fontFilename);
			font = new Font(fontFilePath);

			renderWindow = new RenderWindow(new VideoMode(Config.windowWidth, Config.windowHeight), "Forza Horizon Overlay", Styles.Titlebar | Styles.Close);
			renderWindow.SetFramerateLimit(60);
			renderWindow.Closed += OnClose;
		}
	}
}

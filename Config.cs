using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ForzaHorizonOverlay
{
	class Config
	{

		static ConfigFile? configFile;

		static public IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 12345);

		static public string fontFilename = "";

		static public Color bgColour;
		static public Color[] axisColours = new Color[4];
		static public Color steerMetreColour;
		static public Color borderColour;

		static public uint windowWidth;
		static public uint windowHeight;

		static public int metreStartX;
		static public int metreStartY;
		static public int metreWidth;
		static public int metreHeight;
		static public int metreSpacing;

		static public string[] axisNames = { "Throttle", "Brake", "Clutch", "E-Brake" };

		static public int steerStartX;
		static public int steerStartY;
		static public int steerWidth;
		static public int steerHeight;
		static public int steerMetreHeight;
		static public int steerJoystickRadius;
		static public float steerJoystickOffset;

		static public int clutchEBreakStyle;

		static void SetIPAddr(string addr, string portStr)
		{
			byte[] address = new byte[4];
			try
			{
				string[] addressStr = addr.Split(".");

				int i = 0;
				while (i < 4)
				{
					address[i] = byte.Parse(addressStr[i]);
					i++;
				}
			}
			catch
			{
				address[0] = 127;
				address[0] = 0;
				address[0] = 0;
				address[0] = 1;
			}

			int port = int.Parse(portStr);

			ipEndPoint = new IPEndPoint(new IPAddress(address), port);
		}

		#region Hex & Colour
		/*
		static string RmHexPrefix(string str)
		{
			str = str.ToLower();
			if (str.StartsWith("0x"))
				return str.Substring(2);
			else
				return str;
		}

		static Color HexToColor(string str)
		{
			return new Color(uint.Parse(RmHexPrefix(str), System.Globalization.NumberStyles.HexNumber));
		}
		*/
		static Color StrToColour(string str)
		{
			string[] strings = str.Split(',');
			byte r = byte.Parse(strings[0]);
			byte g = byte.Parse(strings[1]);
			byte b = byte.Parse(strings[2]);
			if (strings.Length > 3)
			{
				byte a = byte.Parse(strings[3]);
				return new Color(r, g, b, a);
			}

			return new Color(r, g, b);
		}
		#endregion

		static public void LoadConfig()
		{
			configFile = new ConfigFile("config.ini");

			configFile.SetTag("IP Address And Port");
			SetIPAddr(configFile.Read("ipAddress", "127.0.0.1"), configFile.Read("port", "12345"));

			configFile.SetTag("Style");
			clutchEBreakStyle = int.Parse(configFile.Read("style", "1"));

			configFile.SetTag("Font");
			fontFilename = configFile.Read("font", "HarmonyOS_Sans_Light.ttf");

			configFile.SetTag("Colours");
			bgColour = StrToColour(configFile.Read("background", "255,255,255,128"));	//background
			axisColours[0] = StrToColour(configFile.Read("throttle", "0,216,0"));		//throttle
			axisColours[1] = StrToColour(configFile.Read("brake", "236,0,0"));			//brake
			axisColours[2] = StrToColour(configFile.Read("clutch", "0,160,255"));		  //clutch
			axisColours[3] = StrToColour(configFile.Read("eBrake", "255,216,0"));		  //ebrake
			steerMetreColour = StrToColour(configFile.Read("steer", "255,192,0"));
			borderColour = StrToColour(configFile.Read("border", "0,0,0"));				//border

			configFile.SetTag("Position");
			windowWidth = uint.Parse(configFile.Read("windowWidth", "332"));
			windowHeight = uint.Parse(configFile.Read("windowHeight", "208"));

			metreStartX = int.Parse(configFile.Read("metreStartX", "208"));
			metreStartY = int.Parse(configFile.Read("metreStartY", "24"));
			metreWidth = int.Parse(configFile.Read("metreWidth", "24"));
			metreHeight = int.Parse(configFile.Read("metreHeight", "160"));
			metreSpacing = int.Parse(configFile.Read("metreSpacing", "16"));

			steerStartX = int.Parse(configFile.Read("steerStartX", "24"));
			steerStartY = int.Parse(configFile.Read("steerStartY", "24"));
			steerWidth = int.Parse(configFile.Read("steerWidth", "160"));
			steerHeight = int.Parse(configFile.Read("steerHeight", "160"));
			steerMetreHeight = int.Parse(configFile.Read("steerMetreHeight", "18"));
			steerJoystickRadius = int.Parse(configFile.Read("steerJoystickRadius", "32"));
			steerJoystickOffset = (float)double.Parse(configFile.Read("steerJoystickOffset", "0.4"));

			configFile.Save();
		}
	}
}

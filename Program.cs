namespace ForzaHorizonOverlay
{
	internal class Program
	{
		static AppWindow? appWindow;

		static void Main(string[] args)
		{
			Config.LoadConfig();

			appWindow = new AppWindow();
			appWindow.Run();
		}
	}
}
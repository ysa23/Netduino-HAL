using System;
using System.Threading;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace YsA.Netduino.HAL
{
	public class Program
	{
		private const int MaxOnTime = 500;
		private const int MaxOffTime = 300;
		private const int FadeInterval = 20;

		public static void Main()
		{
			Pulse(PWMChannels.PWM_ONBOARD_LED);
		}

		private static void Pulse(Cpu.PWMChannel channel)
		{
			const double initialIntensity = 5.212;
			const double topIntesity = 7.8;
			const double finalIntensity = 10.495;

			var led = new PWM(channel, 1000, .5, false);
			led.Start();

			while (true)
			{
				Thread.Sleep(RandomSleepTime(MaxOffTime));

				for (var startValue = initialIntensity; startValue < finalIntensity; startValue = startValue + 0.1)
				{
					var ledIntensity = Math.Sin(startValue) * .5 + .5;
					led.DutyCycle = ledIntensity;

					Thread.Sleep(FadeInterval);

					// While Sin is in max value, so is the led intensity. So, to create the random pulsing affect, we need to pause.
					if (startValue > topIntesity - 0.09 && startValue < topIntesity + 0.09)
						Thread.Sleep(RandomSleepTime(MaxOnTime));
				}
			}
		}

		private static int RandomSleepTime(int max)
		{
			return new Random().Next(max);
		}
	}
}

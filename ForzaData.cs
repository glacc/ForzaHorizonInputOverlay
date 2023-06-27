using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrozaHorizonTelemetry
{
	internal class ForzaData
	{
		static int bufferOfs = 12;	// 12 for FH4
		static byte[] buffer = new byte[384];

		static int precision = 2;

		public void Update(byte[] src)
		{
			buffer = new byte[384];
			Buffer.BlockCopy(src, 0, buffer, 0, src.Length > buffer.Length ? buffer.Length : src.Length);
		}

		public void SetPrecision(int prec)
		{
			precision = prec;
		}

		#region Parsing

		static byte[]? GetDWord(int offset)
		{
			byte[] parseBuffer = new byte[4];
			Buffer.BlockCopy(buffer, offset, parseBuffer, 0, 4);
			return parseBuffer;
		}
		static byte[]? GetWord(int offset)
		{
			byte[] parseBuffer = new byte[2];
			Buffer.BlockCopy(buffer, offset, parseBuffer, 0, 2);
			return parseBuffer;
		}

		static int GetInt(int offset)
		{
			return BitConverter.ToInt32(GetDWord(offset));
		}

		static uint GetUInt(int offset)
		{
			return BitConverter.ToUInt32(GetDWord(offset));
		}

		static ushort GetUInt16(int offset)
		{
			return BitConverter.ToUInt16(GetWord(offset));
		}

		static float GetFloat(int offset)
		{
			return (float)Math.Round(BitConverter.ToSingle(GetDWord(offset)), precision);
		}
		#endregion

		#region Data

		// Reference: https://github.com/austinbaccus/forza-telemetry/blob/main/ForzaCore/DataPacket.cs

		public bool IsRaceOn
		{
			get { return GetInt(0) > 0; }
		}
		public uint TimestampMs
		{
			get { return GetUInt(4); }
		}

		#region Engine
		public float EngineMaxRpm
		{
			get { return GetFloat(8); }
		}
		public float EngineIdleRpm
		{
			get { return GetFloat(12); }
		}
		public float EngineCurrentRpm
		{
			get { return GetFloat(16); }
		}
		#endregion

		#region Motion
		#region Acceleration
		public float AccelerationX
		{
			get { return GetFloat(20); }
		}
		public float AccelerationY
		{
			get { return GetFloat(24); }
		}
		public float AccelerationZ
		{
			get { return GetFloat(28); }
		}
		public float Acceleration
		{
			get { return (float)Math.Sqrt(Math.Pow(AccelerationX, 2) + Math.Pow(AccelerationY, 2) + Math.Pow(AccelerationZ, 2)); }
		}
		#endregion
		#region Velocity
		public float VelocityX
		{
			get { return GetFloat(32); }
		}
		public float VelocityY
		{
			get { return GetFloat(36); }
		}
		public float VelocityZ
		{
			get { return GetFloat(40); }
		}
		public float Velocity
		{
			get { return (float)Math.Sqrt(Math.Pow(VelocityX, 2) + Math.Pow(VelocityY, 2) + Math.Pow(VelocityZ, 2)); }
		}
		public float VelocityKph
		{
			get { return (float)Math.Round(Math.Sqrt(Math.Pow(VelocityX, 2) + Math.Pow(VelocityY, 2) + Math.Pow(VelocityZ, 2)) * 3.6, precision); }
		}
		public float VelocityMph
		{
			get { return (float)Math.Round(Math.Sqrt(Math.Pow(VelocityX, 2) + Math.Pow(VelocityY, 2) + Math.Pow(VelocityZ, 2)) * 2.23693629, precision); }
		}
		#endregion
		#region Angular Velocity
		public float AngularVelocityX
		{
			get { return GetFloat(44); }
		}
		public float AngularVelocityY
		{
			get { return GetFloat(48); }
		}
		public float AngularVelocityZ
		{
			get { return GetFloat(52); }
		}
		#endregion
		#region Angles
		public float Yaw
		{
			get { return GetFloat(56); }
		}
		public float Pitch
		{
			get { return GetFloat(60); }
		}
		public float Roll
		{
			get { return GetFloat(64); }
		}
		#endregion
		#endregion

		#region Suspension & Tires
		#region Normalized Suspension Travel
		public float NormSuspensionTravelFl
		{
			get { return GetFloat(68); }
		}
		public float NormSuspensionTravelFr
		{
			get { return GetFloat(72); }
		}
		public float NormSuspensionTravelRl
		{
			get { return GetFloat(76); }
		}
		public float NormSuspensionTravelRr
		{
			get { return GetFloat(80); }
		}
		#endregion
		#region Tire Slip Ratio
		public float TireSlipRatioFl
		{
			get { return GetFloat(84); }
		}
		public float TireSlipRatioFr
		{
			get { return GetFloat(88); }
		}
		public float TireSlipRatioRl
		{
			get { return GetFloat(92); }
		}
		public float TireSlipRatioRr
		{
			get { return GetFloat(96); }
		}
		#endregion
		#region Tire Slip Ratio
		public float WheelRotationSpeedFl
		{
			get { return GetFloat(100); }
		}
		public float WheelRotationSpeedFr
		{
			get { return GetFloat(104); }
		}
		public float WheelRotationSpeedRl
		{
			get { return GetFloat(108); }
		}
		public float WheelRotationSpeedRr
		{
			get { return GetFloat(112); }
		}
		#endregion
		#region Wheel On Rumble Strip
		public float WheelOnRumbleStripFl
		{
			get { return GetFloat(116); }
		}
		public float WheelOnRumbleStripFr
		{
			get { return GetFloat(120); }
		}
		public float WheelOnRumbleStripRl
		{
			get { return GetFloat(124); }
		}
		public float WheelOnRumbleStripRr
		{
			get { return GetFloat(128); }
		}
		#endregion
		#region Wheel On Rumble Strip
		public float WheelInPuddleFl
		{
			get { return GetFloat(132); }
		}
		public float WheelInPuddleFr
		{
			get { return GetFloat(136); }
		}
		public float WheelInPuddleRl
		{
			get { return GetFloat(140); }
		}
		public float WheelInPuddleRr
		{
			get { return GetFloat(144); }
		}
		#endregion
		#region Surface Rumble
		public float SurfaceRumbleFl
		{
			get { return GetFloat(148); }
		}
		public float SurfaceRumbleFr
		{
			get { return GetFloat(152); }
		}
		public float SurfaceRumbleRl
		{
			get { return GetFloat(156); }
		}
		public float SurfaceRumbleRr
		{
			get { return GetFloat(160); }
		}
		#endregion
		#region Tire Slip Angle
		public float TireSlipAngleFl
		{
			get { return GetFloat(164); }
		}
		public float TireSlipAngleFr
		{
			get { return GetFloat(168); }
		}
		public float TireSlipAngleRl
		{
			get { return GetFloat(172); }
		}
		public float TireSlipAngleRr
		{
			get { return GetFloat(176); }
		}
		#endregion
		#region Tire Combined Slip
		public float TireCombinedSlipFl
		{
			get { return GetFloat(180); }
		}
		public float TireCombinedSlipFr
		{
			get { return GetFloat(184); }
		}
		public float TireCombinedSlipRl
		{
			get { return GetFloat(188); }
		}
		public float TireCombinedSlipRr
		{
			get { return GetFloat(192); }
		}
		#endregion
		#region Suspension Travel In Meters
		public float SuspensionTravelMetersFl
		{
			get { return GetFloat(196); }
		}
		public float SuspensionTravelMetersFr
		{
			get { return GetFloat(200); }
		}
		public float SuspensionTravelMetersRl
		{
			get { return GetFloat(204); }
		}
		public float SuspensionTravelMetersRr
		{
			get { return GetFloat(208); }
		}
		#endregion
		#endregion

		#region Car Info
		public uint CarOrdinal
		{
			get { return (uint)buffer[212]; }
		}
		public uint CarClass
		{
			get { return (uint)buffer[216]; }
		}
		public uint CarPerformanceIndex
		{
			get { return (uint)buffer[220]; }
		}
		public uint DriveTrain
		{
			get { return (uint)buffer[224]; }
		}
		public uint NumOfCylinders
		{
			get { return (uint)buffer[228]; }
		}
		#endregion

		#region Position
		public float PositionX
		{
			get { return GetFloat(232 + bufferOfs); }
		}
		public float PositionY
		{
			get { return GetFloat(236 + bufferOfs); }
		}
		public float PositionZ
		{
			get { return GetFloat(240 + bufferOfs); }
		}
		#endregion

		public float Speed
		{
			get { return GetFloat(244 + bufferOfs); }
		}
		public float SpeedKph
		{
			get { return (float)Math.Round(GetFloat(244 + bufferOfs) * 3.6, precision); }
		}
		public float SpeedMph
		{
			get { return (float)Math.Round(GetFloat(244 + bufferOfs) * 2.23693629, precision); }
		}
		public float Power
		{
			get { return GetFloat(248 + bufferOfs); }
		}
		public float PowerKw
		{
			get { return (float)Math.Round(GetFloat(248 + bufferOfs) / 1000.0, precision); }
		}
		public float PowerHp
		{
			get { return (float)Math.Round(GetFloat(248 + bufferOfs) / 1000.0 * 1.34102209, precision); }
		}
		public float Torque
		{
			get { return GetFloat(252 + bufferOfs); }
		}

		#region Tire Temperature
		public float TireTempFl
		{
			get { return GetFloat(256 + bufferOfs); }
		}
		public float TireTempFr
		{
			get { return GetFloat(260 + bufferOfs); }
		}
		public float TireTempRl
		{
			get { return GetFloat(264 + bufferOfs); }
		}
		public float TireTempRr
		{
			get { return GetFloat(268 + bufferOfs); }
		}
		#endregion

		public float Boost
		{
			get { return GetFloat(272 + bufferOfs); }
		}
		public float Fuel
		{
			get { return GetFloat(276 + bufferOfs); }
		}
		public float Distance
		{
			get { return GetFloat(280 + bufferOfs); }
		}

		#region Race Info
		public float BestLapTime
		{
			get { return GetFloat(284 + bufferOfs); }
		}
		public float LastLapTime
		{
			get { return GetFloat(288 + bufferOfs); }
		}
		public float CurrentLapTime
		{
			get { return GetFloat(292 + bufferOfs); }
		}
		public float CurrentRaceTime
		{
			get { return GetFloat(296 + bufferOfs); }
		}
		public uint Lap
		{
			get { return (uint)GetUInt16(300 + bufferOfs); }
		}
		public uint RacePosotion
		{
			get { return (uint)buffer[302 + bufferOfs]; }
		}
		#endregion

		#region Control
		public uint Throttle
		{
			get { return (uint)buffer[303 + bufferOfs]; }
		}
		public uint Brake
		{
			get { return (uint)buffer[304 + bufferOfs]; }
		}
		public uint Clutch
		{
			get { return (uint)buffer[305 + bufferOfs]; }
		}
		public uint Handbrake
		{
			get { return (uint)buffer[306 + bufferOfs]; }
		}
		public uint Gear
		{
			get { return (uint)buffer[307 + bufferOfs]; }
		}
		public int Steer
		{
			get { return buffer[308 + bufferOfs] > 127 ? (int)buffer[308 + bufferOfs] - 256 : (int)buffer[308 + bufferOfs]; }
		}
		#endregion
		#region AI
		public uint NormalDrivingLine
		{
			get { return (uint)buffer[309 + bufferOfs]; }
		}
		public uint NormalAiBrakeDifference
		{
			get { return (uint)buffer[310 + bufferOfs]; }
		}
		#endregion

		#endregion

		public ForzaData()
		{
			buffer = new byte[384];
		}

		public ForzaData(byte[] data)
		{
			Update(data);
		}

		public override string ToString()
		{
			return
				$"IsRaceOn: {IsRaceOn}\n" +
				$"TimestampMs: {TimestampMs}\n" +
				$"EngineMaxRpm: {EngineMaxRpm}\n" +
				$"EngineIdleRpm: {EngineIdleRpm}\n" +
				$"EngineCurrentRpm: {EngineCurrentRpm}\n" +
				$"AccelerationX: {AccelerationX}\n" +
				$"AccelerationY: {AccelerationY}\n" +
				$"AccelerationZ: {AccelerationZ}\n" +
				$"Acceleration: {Acceleration}\n" +
				$"VelocityX: {VelocityX}\n" +
				$"VelocityY: {VelocityY}\n" +
				$"VelocityZ: {VelocityZ}\n" +
				$"Velocity: {Velocity}m/s\n" +
				$"AngularVelocityX: {AngularVelocityX}\n" +
				$"AngularVelocityY: {AngularVelocityY}\n" +
				$"AngularVelocityZ: {AngularVelocityZ}\n" +
				$"Yaw: {Yaw}\n" +
				$"Pitch: {Pitch}\n" +
				$"Roll: {Roll}\n" +
				$"NormSuspensionTravelFl: {NormSuspensionTravelFl}\n" +
				$"NormSuspensionTravelFr: {NormSuspensionTravelFr}\n" +
				$"NormSuspensionTravelRl: {NormSuspensionTravelRl}\n" +
				$"NormSuspensionTravelRr: {NormSuspensionTravelRr}\n" +
				$"TireSlipRatioFl: {TireSlipRatioFl}\n" +
				$"TireSlipRatioFr: {TireSlipRatioFr}\n" +
				$"TireSlipRatioRl: {TireSlipRatioRl}\n" +
				$"TireSlipRatioRr: {TireSlipRatioRr}\n" +
				$"WheelRotationSpeedFl: {WheelRotationSpeedFl}\n" +
				$"WheelRotationSpeedFr: {WheelRotationSpeedFr}\n" +
				$"WheelRotationSpeedRl: {WheelRotationSpeedRl}\n" +
				$"WheelRotationSpeedRr: {WheelRotationSpeedRr}\n" +
				$"WheelOnRumbleStripFl: {WheelOnRumbleStripFl}\n" +
				$"WheelOnRumbleStripFr: {WheelOnRumbleStripFr}\n" +
				$"WheelOnRumbleStripRl: {WheelOnRumbleStripRl}\n" +
				$"WheelOnRumbleStripRr: {WheelOnRumbleStripRr}\n" +
				$"WheelInPuddleFl: {WheelInPuddleFl}\n" +
				$"WheelInPuddleFr: {WheelInPuddleFr}\n" +
				$"WheelInPuddleRl: {WheelInPuddleRl}\n" +
				$"WheelInPuddleRr: {WheelInPuddleRr}\n" +
				$"SurfaceRumbleFl: {SurfaceRumbleFl}\n" +
				$"SurfaceRumbleFr: {SurfaceRumbleFr}\n" +
				$"SurfaceRumbleRl: {SurfaceRumbleRl}\n" +
				$"SurfaceRumbleRr: {SurfaceRumbleRr}\n" +
				$"TireSlipAngleFl: {TireSlipAngleFl}\n" +
				$"TireSlipAngleFr: {TireSlipAngleFr}\n" +
				$"TireSlipAngleRl: {TireSlipAngleRl}\n" +
				$"TireSlipAngleRr: {TireSlipAngleRr}\n" +
				$"TireCombinedSlipFl: {TireCombinedSlipFl}\n" +
				$"TireCombinedSlipFr: {TireCombinedSlipFr}\n" +
				$"TireCombinedSlipRl: {TireCombinedSlipRl}\n" +
				$"TireCombinedSlipRr: {TireCombinedSlipRr}\n" +
				$"SuspensionTravelMetersFl: {SuspensionTravelMetersFl}\n" +
				$"SuspensionTravelMetersFr: {SuspensionTravelMetersFr}\n" +
				$"SuspensionTravelMetersRl: {SuspensionTravelMetersRl}\n" +
				$"SuspensionTravelMetersRr: {SuspensionTravelMetersRr}\n" +
				$"CarOrdinal: {CarOrdinal}\n" +
				$"CarClass: {CarClass}\n" +
				$"CarPerformanceIndex: {CarPerformanceIndex}\n" +
				$"DriveTrain: {DriveTrain}\n" +
				$"NumOfCylinders: {NumOfCylinders}\n" +
				$"PositionX: {PositionX}\n" +
				$"PositionY: {PositionY}\n" +
				$"PositionZ: {PositionZ}\n" +
				$"Speed: {Speed}\n" +
				$"Power: {Power}\n" +
				$"Torque: {Torque}\n" +
				$"TireTempFl: {TireTempFl}\n" +
				$"TireTempFr: {TireTempFr}\n" +
				$"TireTempRl: {TireTempRl}\n" +
				$"TireTempRr: {TireTempRr}\n" +
				$"Boost: {Boost}\n" +
				$"Fuel: {Fuel}\n" +
				$"Distance: {Distance}\n" +
				$"BestLapTime: {BestLapTime}\n" +
				$"LastLapTime: {LastLapTime}\n" +
				$"CurrentLapTime: {CurrentLapTime}\n" +
				$"CurrentRaceTime: {CurrentRaceTime}\n" +
				$"Lap: {Lap}\n" +
				$"RacePosition: {RacePosotion}\n" +
				$"Throttle: {Throttle}\n" +
				$"Brake: {Brake}\n" +
				$"Clutch: {Clutch}\n" +
				$"Handbrake: {Handbrake}\n" +
				$"Gear: {Gear}\n" +
				$"Steer: {Steer}\n" +
				$"NormalDrivingLine: {NormalDrivingLine}\n" +
				$"NormalAiBrakeDifference: {NormalAiBrakeDifference}";
		}

		public string BasicInfo()
		{
			return
				$"EngineCurrentRpm: {EngineCurrentRpm} rpm\n" +
				$"Speed: {SpeedKph} km/h\n" +
				$"Throttle: {Math.Round(Throttle / 2.55, precision)}%\n" +
				$"Brake: {Math.Round(Brake / 2.55, precision)}%\n" +
				$"Clutch: {Math.Round(Clutch / 2.55, precision)}%\n" +
				$"Handbrake: {Math.Round(Handbrake / 2.55, precision)}%\n" +
				$"Gear: {Gear}\n" +
				$"Steer: {Math.Round(Steer / 1.27, precision)}%\n" +
				$"Power: {PowerKw} kW\n" +
				$"Torque: {Torque} N·m\n" +
				$"Boost: {Boost}\n" +
				$"Distance: {Distance} km\n";
		}
	}
}

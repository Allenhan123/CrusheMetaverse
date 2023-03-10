using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace QFrame
{
	public class MicroPhoneManager : MonoBehaviour
	{
		/// 录音频率
		public string frequency = "14700";

		public int samplerate = 14700;

		/// 录音时长
		public int micSecond = 3;

		private AudioSource _curAudioSource;
		public AudioSource CurAudioSource
		{
			get
			{
				if (_curAudioSource == null)
				{
					_curAudioSource = gameObject.AddComponent<AudioSource>();
				}

				return _curAudioSource;
			}
			set
			{
				_curAudioSource = value;
			}

		}

		public System.Action<int> handleCallBack;

		private bool _bRecording = false;

		/// <summary>
		/// 获取麦克风设备
		/// </summary>
		public bool GetMicrophoneDevice()
		{
			return Microphone.devices.Length > 0;
		}

		/// <summary>
		/// 开始录音
		/// </summary>
		public void StartRecordAudio(int recordTime = 0, System.Action<int> handleGetAudioOffset = null)
		{
			if (!GetMicrophoneDevice())
			{
				Debug.LogError("没有检测到录制设备");
				return;
			}

			if (recordTime <= 0)
			{
				Debug.LogError("录制的时长必须大于 0");
				return;
			}

			if (Microphone.IsRecording(null))
			{
				return;
			}

			Debug.LogFormat("开始录音..... {0}", recordTime);
			micSecond = recordTime;

			handleCallBack = handleGetAudioOffset;

			Microphone.End(null);
			CurAudioSource.Stop();
			CurAudioSource.loop = false;
			CurAudioSource.mute = false;
			CurAudioSource.clip = Microphone.Start(null, true, micSecond, int.Parse(frequency));
			_bRecording = false;

			//	StartCoroutine(WaitForPlay(handleGetAudioOffset));
		}

		public void StartRecording()
		{
			_bRecording = true;
		}

		IEnumerator DelayStart()
		{
			yield return new WaitForSeconds(3f);
			_bRecording = true;
		}

		private void Update()
		{
			if (_bRecording)
			{
				if (CurAudioSource.clip != null && handleCallBack != null)
				{
					handleCallBack(Microphone.GetPosition(null));
				}

			}
		}

		IEnumerator WaitForPlay(System.Action<int> handleGetAudioOffset = null)
		{
			// Let the Microphone start filling the buffer prior to activating the AudioSource.
			int offset = 0;

			while (!((offset = Microphone.GetPosition(null)) > 0))
			{
				if (handleGetAudioOffset != null)
				{
					handleGetAudioOffset(offset);
				}
				yield return null;
			}

			// If the AudioSource was successfully assigned, play(activate) the AudioSource.
			if (CurAudioSource.clip)
			{
				CurAudioSource.Play();
			}
			yield return null;
		}

		/// <summary>
		/// 停止录音
		/// </summary>
		public void StopRecordAudio()
		{
			Debug.Log("结束录音.....");

			_bRecording = false;
			Microphone.End(null);
			CurAudioSource.Stop();
		}

		public void ResetRecordAudio()
		{
			StopRecordAudio();
			CurAudioSource.clip = null;
		}

		/// <summary>s
		/// 回放录音
		/// </summary>
		public void PlayRecordAudio()
		{
			Debug.Log("播放录音.....");
			if (Microphone.IsRecording(null))
			{
				return;
			}

			if (CurAudioSource.clip == null)
			{
				return;
			}

			CurAudioSource.mute = false;
			CurAudioSource.loop = false;
			CurAudioSource.Play();
		}

		#region 录音写入文件

		/// <summary>
		///  写入录音信息
		/// </summary>
		public bool WriteRecordData(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				Debug.LogErrorFormat("WriteRecordData filePath is null");
				return false;
			}

			if (Microphone.IsRecording(null))
			{
				Debug.LogErrorFormat("WriteRecordData is Recording");
				return false;
			}

			if (CurAudioSource.clip == null)
			{
				Debug.LogErrorFormat("WriteRecordData CurAudioSource clip is null");
				return false;
			}

			// byte[] data = GetClipData();

			#region 用户自由固定录音时长

			// int position = Microphone.GetPosition(null);
			// var soundata = new float[CurAudioSource.clip.samples * CurAudioSource.clip.channels];
			// CurAudioSource.clip.GetData(soundata, 0);

			// var newdata = new float[CurAudioSource.clip.samples * CurAudioSource.clip.channels];
			// for (int i = 0; i < newdata.Length; i++)
			// {
			// 	newdata[i] = soundata[i];
			// }

			// CurAudioSource.clip = AudioClip.Create(CurAudioSource.clip.name, position, CurAudioSource.clip.channels, CurAudioSource.clip.frequency, false);
			// CurAudioSource.clip.SetData(newdata, 0);
			// Microphone.End(null);

			#endregion

			using(FileStream fs = CreateEmpty(filePath))
			{
				ConvertAndWrite(fs, CurAudioSource.clip);
				WriteHeader(fs, CurAudioSource.clip);

				Debug.LogFormat("file total length: {0} time: {1}", fs.Length, CurAudioSource.time);
				//todo 特殊处理 文件小于1kb 则录制失败
				if (fs.Length < 1024)
				{
					Debug.LogErrorFormat("file size not bigger 1024");
					return false;
				}

			}


			return true;
		}

		private void WriteHeader(FileStream stream, AudioClip clip)
		{

			int hz = clip.frequency;
			int channels = clip.channels;
			int samples = clip.samples;

			stream.Seek(0, SeekOrigin.Begin);

			Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
			stream.Write(riff, 0, 4);

			Byte[] chunkSize = BitConverter.GetBytes(stream.Length - 8);
			stream.Write(chunkSize, 0, 4);

			Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
			stream.Write(wave, 0, 4);

			Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
			stream.Write(fmt, 0, 4);

			Byte[] subChunk1 = BitConverter.GetBytes(16);
			stream.Write(subChunk1, 0, 4);

			UInt16 two = 2;
			UInt16 one = 1;
			Byte[] audioFormat = BitConverter.GetBytes(one);
			stream.Write(audioFormat, 0, 2);

			Byte[] numChannels = BitConverter.GetBytes(channels);
			stream.Write(numChannels, 0, 2);

			Byte[] samplerate = BitConverter.GetBytes(hz);
			stream.Write(samplerate, 0, 4);

			Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2  
			stream.Write(byteRate, 0, 4);

			UInt16 blockAlign = (ushort)(channels * 2);
			stream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

			UInt16 bps = 16;
			Byte[] bitsPerSample = BitConverter.GetBytes(bps);
			stream.Write(bitsPerSample, 0, 2);

			Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
			stream.Write(datastring, 0, 4);

			Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
			stream.Write(subChunk2, 0, 4);
		}

		private FileStream CreateEmpty(string filepath)
		{
			FileStream fileStream = new FileStream(filepath, FileMode.Create);
			byte emptyByte = new byte();

			for (int i = 0; i < 44; i++) //preparing the header  
			{
				fileStream.WriteByte(emptyByte);
			}
			return fileStream;
		}

		private void ConvertAndWrite(FileStream fileStream, AudioClip clip)
		{
			float[] samples = new float[clip.samples];

			clip.GetData(samples, 0);

			Int16[] intData = new Int16[samples.Length];

			Byte[] bytesData = new Byte[samples.Length * 2];

			int rescaleFactor = 32767; //to convert float to Int16  
			for (int i = 0; i < samples.Length; i++)
			{
				intData[i] = (short)(samples[i] * rescaleFactor);
				Byte[] byteArr = new Byte[2];
				byteArr = BitConverter.GetBytes(intData[i]);
				byteArr.CopyTo(bytesData, i * 2);
			}

			fileStream.Write(bytesData, 0, bytesData.Length);
		}

		/// <summary>
		/// 获取音频数据
		/// </summary>
		/// <returns>The clip data.</returns>
		public byte[] GetClipData()
		{
			if (CurAudioSource.clip == null)
			{
				Debug.Log("缺少音频资源！");
				return null;
			}

			float[] samples = new float[CurAudioSource.clip.samples];
			CurAudioSource.clip.GetData(samples, 0);
			byte[] outData = new byte[samples.Length * 2];
			int reScaleFactor = 32767;

			for (int i = 0; i < samples.Length; i++)
			{
				short tempShort = (short)(samples[i] * reScaleFactor);
				byte[] tempData = System.BitConverter.GetBytes(tempShort);
				outData[i * 2] = tempData[0];
				outData[i * 2 + 1] = tempData[1];
			}

			if (outData == null || outData.Length <= 0)
			{
				return null;
			}

			return outData;
		}

		#endregion

		#region  通过wav文件路径 加载 audioClip
		public IEnumerator LoadAudioClip(string filepath, System.Action<AudioClip> HandleLoadComplete)
		{
			if (string.IsNullOrEmpty(filepath))
			{
				yield break;
			}

			using(var uwr = UnityWebRequestMultimedia.GetAudioClip(filepath, AudioType.WAV))
			{
				yield return uwr.SendWebRequest();
				if (uwr.isNetworkError)
				{
					Debug.LogError(uwr.error);
					yield break;
				}

				if (HandleLoadComplete != null)
				{
					HandleLoadComplete(DownloadHandlerAudioClip.GetContent(uwr));
				}
			}

		}

		#endregion
	}
}
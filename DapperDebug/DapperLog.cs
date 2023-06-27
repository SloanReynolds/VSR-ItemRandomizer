using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DapperHelper {
	class DapperLog {
		#region static
		#endregion

		#region instance
		private StringBuilder _buffer = new StringBuilder();
		private string _logPath => $"ItemRandomizer{(_fileName != "" ? $"[{_fileName}]" : "")}_DEBUG.log";
		private string _fileName = "";

		public DapperLog(string fileName = "") {
			_fileName = fileName;
		}

		public bool Headers { get; set; } = true;

		~DapperLog() {
			{ }
			Flush();
		}

		enum LogLevel {
			CRITICAL,
			ERROR,
			WARNING,
			INFO,
			VERBOSE
		}





		public void Write(string[] strs) {
			foreach (string str in strs) {
				Write(str);
			}
		}

		public void Write(string str, bool newLine = true) {
			if (newLine) {
				_buffer.AppendLine(str);
			} else {
				_buffer.Append(str);
			}
		}

		public void WriteNow(string str) {
			//Bypasses the buffer
			using (StreamWriter w = File.AppendText(_logPath)) {
				_BeginWrite(w);
				w.Write(str);
				w.WriteLine();
				_EndWrite(w);
			}
		}

		public void CancelWrite() {
			_buffer.Length = 0;
		}

		public void Flush() {
			if (_buffer.Length <= 0) return;

			using (StreamWriter w = File.AppendText(_logPath)) {
				_BeginWrite(w);
				w.Write(_buffer);
				_EndWrite(w);

				_buffer.Length = 0; //Reset the buffer
			}
		}

		private void _BeginWrite(StreamWriter w) {
			if (Headers && _HeaderTimeStampExpired()) {
				w.WriteLine($"-=-=-=-=-= {DateTime.Now:MM/dd/yyyy HH:mm} =-=-=-=-=-");
				_headerTimeStamp = DateTime.Now;
			}
		}

		private void _EndWrite(StreamWriter w) {
			//Do Nothing?
		}

		private DateTime _headerTimeStamp = DateTime.MinValue;
		private bool _HeaderTimeStampExpired() {
			if (DateTime.Now.Subtract(_headerTimeStamp) > TimeSpan.FromMinutes(1))
				return true;
			return false;
		}
		#endregion
	}
}

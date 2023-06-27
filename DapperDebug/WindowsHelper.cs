using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DapperHelper {
	public static class WindowsHelper {

		/// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
		/// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
		public static Dictionary<IntPtr, string> GetOpenWindows() {
			IntPtr shellWindow = GetShellWindow();
			List<IntPtr> list = new List<IntPtr>();
			Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();

			EnumWindows(__EnumWindows, 0);

			return windows;

			bool __EnumWindows(IntPtr hWnd, int lParam) {
				if (hWnd == shellWindow) return true;
				if (!IsWindowVisible(hWnd)) return true;

				int length = GetWindowTextLength(hWnd);
				if (length == 0) return true;

				StringBuilder builder = new StringBuilder(length);
				GetWindowText(hWnd, builder, length + 1);

				windows.Add(hWnd, builder.ToString());

				list.Add(hWnd);

				return true;
			}
		}

		public static void SetFocus(IntPtr hWnd) {
			if (hWnd != default(IntPtr)) {
				SetForegroundWindow(hWnd);
			}
		}

		public static void SetWindowPosition(IntPtr hWnd, int x, int y) {
			if (hWnd != default(IntPtr)) {
				//Flags : SWP_NOSIZE = 0x0001, SWP_NOZORDER = 0x0004
				SetWindowPos(hWnd, IntPtr.Zero, x, y, 0, 0, 0x0001 | 0x0004);
			}
		}

		public delegate void SwitchedFocus(string windowTitle);
		private static SwitchedFocus _invokable;

		public static void SetHook_SwitchFocus(SwitchedFocus proc) {
			if (_switchFocusHook == default(IntPtr))
				UnsetHook_SwitchFocus();
			_switchFocusHook = SetWinEventHook(3, 3, IntPtr.Zero, _wepStatic, 0, 0, 0); //0 = EVENT_SYSTEM_FOREGROUND, 3 = EVENT_SYSTEM_FOREGROUND, 0 = WINEVENT_OUTOFCONTEXT
			_invokable = proc;
		}

		public static void UnsetHook_SwitchFocus() {
			UnhookWinEvent(_switchFocusHook);
		}

		public static string CurrentWindow { get; private set; }
		public static bool NeedsUpdate { get; private set; } = false;

		public static void UpdateWindowTitle() {
			string title = _GetActiveWindowTitle();
			_invokable?.Invoke(title);
			CurrentWindow = title;
			NeedsUpdate = false;
		}






		private static string _GetActiveWindowTitle() {
			var strTitle = string.Empty;
			var handle = GetForegroundWindow();

			var intLength = GetWindowTextLength(handle) + 1;
			var stringBuilder = new StringBuilder(intLength);
			if (GetWindowText(handle, stringBuilder, intLength) > 0) {
				strTitle = stringBuilder.ToString();
			}
			return strTitle;
		}

		private static IntPtr _switchFocusHook;


		private static void _WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime) {
			if (eventType == 3) { //EVENT_SYSTEM_FOREGROUND
				UpdateWindowTitle();
				//NeedsUpdate = true;
			}
		}

		private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);
		private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
		private static WinEventDelegate _wepStatic = _WinEventProc;

		[DllImport("USER32.DLL")]
		private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

		[DllImport("USER32.DLL")]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("USER32.DLL")]
		private static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("USER32.DLL")]
		private static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("USER32.DLL")]
		private static extern IntPtr GetShellWindow();

		[DllImport("USER32.DLL")]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("USER32.DLL")]
		private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

		[DllImport("USER32.DLL")]
		private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

		[DllImport("USER32.DLL")]
		private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		[DllImport("USER32.DLL")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);
	}
}
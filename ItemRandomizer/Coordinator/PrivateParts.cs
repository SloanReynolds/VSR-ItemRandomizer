using System;
using System.Collections;
using System.Reflection;

namespace ItemRandomizer.Coordinator {
	public static class PrivateParts {
		public static void SetPrivateField(object obj, string fieldName, object value) {
			if (obj != null) {
				Type t = obj.GetType();
				FieldInfo fi = t.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				fi.SetValue(obj, value);
			}
		}

		public static object GetPrivateField(object obj, string fieldName) {
			if (obj != null) {
				Type t = obj.GetType();
				FieldInfo fi = t.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				return fi.GetValue(obj);
			}
			return null;
		}

		internal static void DebugAllFieldsAndProperties(object randoText) {
			FieldInfo[] fis = randoText.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (FieldInfo field in fis) {
				object value = field.GetValue(randoText);
				if (value is Array) value = $"{value.GetType()} Length: {((Array)value).Length}";
				if (value is IList) value = $"{value.GetType()} Count: {((IList)value).Count}";

				Plugin.I.LogInfo($"{field.Name} -> {value}");
			}
			PropertyInfo[] pis = randoText.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (PropertyInfo prop in pis) {
				Plugin.I.LogInfo($"{prop.Name} -> {prop.GetValue(randoText)}");
			}
		}
	}
}

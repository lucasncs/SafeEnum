using System;
using System.Collections.Generic;
using UnityEngine;

namespace SafeEnum
{
	[Serializable]
	public struct SafeEnum<T> : IEquatable<SafeEnum<T>>, ISerializationCallbackReceiver
		where T : struct, Enum, IComparable, IConvertible, IFormattable
	{
		[SerializeField] string _stringValue;
		[SerializeField] T _enumValue;

		public T Value { get => _enumValue; set => _enumValue = value; }

		public void OnBeforeSerialize()
		{
			_stringValue = _enumValue.ToString();
		}

		public void OnAfterDeserialize()
		{
			if (Enum.TryParse(_stringValue, out T enumValue))
			{
				_enumValue = enumValue;
			}
			else
			{
				int index = (int)(ValueType)_enumValue;
				if (Enum.IsDefined(typeof(T), index))
				{
					_enumValue = (T)(ValueType)index;
				}
				else
				{
					Debug.LogError($"Deserialization failed: \"{typeof(T).FullName}\" enum has neither \"{_stringValue}\" value, nor \"{index}\" index");
				}
			}
		}

		public override string ToString()
		{
			return Enum.GetName(typeof(T), _enumValue);
		}

		public bool Equals(SafeEnum<T> other)
		{
			return EqualityComparer<T>.Default.Equals(_enumValue, other._enumValue);
		}

		public override int GetHashCode()
		{
			return _enumValue.GetHashCode();
		}

		public static implicit operator T(SafeEnum<T> safeEnum)
		{
			return safeEnum._enumValue;
		}

		public static T Convert(SafeEnum<T> safeEnum)
		{
			return safeEnum._enumValue;
		}
	}
}
using System;

namespace DeUrgenta.I18n.Service.Models
{
    public class LocalizableString : IEquatable<LocalizableString>
    {
        public string Key { get; }
        public object[] Params { get; }
        public LocalizableString(string key, params object[] @params)
        {
            Key = key;
            Params = @params;
        }

        public static implicit operator LocalizableString(string key) => new(key);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((LocalizableString)obj);
        }

        public bool Equals(LocalizableString other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            var sameKey = other.Key == Key;
            if (!sameKey) return false;

            if (Params == null && other.Params == null)
            {
                return true;
            }

            if (Params?.Length != other.Params?.Length)
            {
                return false;
            }

            return string.Join(",", Params) == string.Join(",", other.Params);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Params);
        }
    }
}
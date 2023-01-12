using System;

namespace Items
{
    [Serializable]
    public class Tag
    {
        public readonly string tagName;
        public readonly int intValue;
        public readonly string stringValue;

        public Tag(string tagName, int intValue, string stringValue)
        {
            this.tagName = tagName;
            this.intValue = intValue;
            this.stringValue = stringValue;
        }

        #region Equality override
        
        public static bool operator == (Tag tag1, Tag tag2)
        {
            return tag1!.Equals(tag2);
        }

        public static bool operator != (Tag tag1, Tag tag2)
        {
            return !(tag1 == tag2);
        }
        
        protected bool Equals(Tag other)
        {
            return tagName == other.tagName && intValue == other.intValue && stringValue == other.stringValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Tag)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(tagName, intValue, stringValue);
        }
        
        #endregion
    }
}
using System;

namespace Items
{
    [Serializable]
    public class Tag
    {
        public readonly string TagName;
        public readonly int IntValue;
        public readonly string StringValue;

        public Tag(string tagName, int intValue, string stringValue)
        {
            TagName = tagName;
            IntValue = intValue;
            StringValue = stringValue;
        }

        #region Equality override
        
        public static bool operator == (Tag tag1, Tag tag2)
        {
            if (ReferenceEquals(null, tag1)) return true;
            
            return tag1!.Equals(tag2);
        }

        public static bool operator != (Tag tag1, Tag tag2)
        {
            return !(tag1 == tag2);
        }
        
        protected bool Equals(Tag other)
        {
            return TagName == other.TagName && IntValue == other.IntValue && StringValue == other.StringValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Tag)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TagName, IntValue, StringValue);
        }
        
        #endregion
    }
}
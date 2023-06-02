namespace FGUFW
{
    public struct UIntId4_4
    {
        public const uint NoneValue = 0;
        public uint Value{get;private set;}

        public ushort Group{get;private set;}

        public ushort Item{get;private set;}
        public UIntId4_4(uint value)
        {
            Value = value;
            if(value>9999_9999)
            {
                Group = 0;
                Item = 0;
            }
            else
            {
                Group = (ushort)(value/1_0000);
                Item = (ushort)(value-Group*1_0000);
            }
        }

        public static implicit operator uint(UIntId4_4 exists)
        {
            return exists.Value;
        }

        public static implicit operator UIntId4_4(uint exists)
        {
            return new UIntId4_4(exists);
        }

    }
}
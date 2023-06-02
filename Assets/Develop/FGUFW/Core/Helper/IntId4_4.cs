namespace FGUFW
{
    public struct IntId4_4
    {
        public const int NoneValue = 0;
        public int Value{get;private set;}

        public ushort Group{get;private set;}

        public ushort Item{get;private set;}
        public IntId4_4(int value)
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

        public static implicit operator int(IntId4_4 exists)
        {
            return exists.Value;
        }

        public static implicit operator IntId4_4(int exists)
        {
            return new IntId4_4(exists);
        }

    }
}
using System;

namespace FGUFW
{
    public static class IntExtensions
    {
        public static unsafe string ToBitString(this Int32 self)
        {
            int length = 32+4-1;
            var chars = stackalloc char[length];
            
            int bitIndex=0;
            int count = 0;
            for (int i = length-1; i >= 0; i--)
            {
                if(count==8)
                {
                    chars[i] = ' ';
                    count=0;
                    continue;
                }

                if( ((1<<bitIndex) & self) != 0)
                {
                    chars[i] = '1';
                }
                else
                {
                    chars[i] = '0';
                }
                bitIndex++;
                count++;
            }
            return new String(chars);
        }

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Hashing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GT3_Language_Switcher
{
    internal class Save
    {
        private byte[] Data;
        private byte[] RawData;
        private byte[] Header = new byte[64];
        private byte[] Footer;
        private byte[] Checksum = new byte[4];

        public Save(byte[] OriginalSave)
        {
            this.Data = OriginalSave;
        }


        //Function that extracts the header and footer of the save
        //The structure of the save file is the following: Random values (12 bytes) + CRC32 value of the raw data (4 bytes) +
        //Bytes set at 00 in hexadecimal (48 bytes) + Raw data + Random values (no specific value, it simply starts when the
        //raw data has its last 0xFF byte, and this section may not appear at all)
        public void GetRawData()
        {
            //Mirrors the header of the save file onto a new byte array
            Header.Initialize();
            Buffer.BlockCopy(Data, 0, Header, 0, 64);

            int FooterLength = 0;

            //Function that checks if there's a footer present in the save file
            for(int i = 0; i < Data.Length; i++)
            {
                //It makes sure to check two bytes together to avoid the odd case
                //where a sole FF byte is inside the footer, which can happen
                if (Data[Data.Length - i - 1] == 0xFF)
                {
                    if (Data[Data.Length - i - 2] == 0xFF)
                    {
                        FooterLength = i;
                        break;
                    }
                }
            }

            //If the footer exists, it gets mirrored onto a new array
            if (FooterLength > 0)
            {
                Footer = new byte[FooterLength];
                Footer.Initialize();
                Buffer.BlockCopy(Data, Data.Length - FooterLength, Footer, 0, FooterLength);
            }

            //Pass the raw save onto a new array
            RawData = new byte[Data.Length - FooterLength - 64];
            RawData.Initialize();
            Buffer.BlockCopy(Data, 64, RawData, 0, Data.Length - FooterLength - 64);

        }

        //Returns the language of the save file, it is always located in byte in index 200 of the raw data
        //1: english
        //2: french
        //3: italian
        //4: german
        //5: spanish
        //-1: no language detected
        public int GetLanguage()
        {
            if (RawData[200].CompareTo(0xFD) == 0)
            {
                return 1;
            }
            else if (RawData[200].CompareTo(0xFC) == 0)
            {
                return 2;
            }
            else if (RawData[200].CompareTo(0xFA) == 0)
            {
                return 3;
            }
            else if (RawData[200].CompareTo(0xFB) == 0)
            {
                return 4;
            }
            else if (RawData[200].CompareTo(0xF9) == 0)
            {
                return 5;
            }

            return -1;
        }

        //Function that verifies the data integrity of the save file by checking the CRC32 of the header
        //and comparing it with the actual CRC32 from the raw data
        //-1: the header has an incorrect CRC32 value
        //0: the checksum coincides in both sides
        public bool Verify()
        {
            GetRawData();
            //After obtaining each array, we obtain the checksum
            Checksum.Initialize();
            Checksum = Crc32.Hash(RawData);
            string test = BitConverter.ToString(Checksum);

            for (int i = 0; i < Checksum.Length; i++)
            {
                if (Checksum[i].CompareTo(Header[i + 12]) != 0)
                {
                    return false;
                }
            }
            return true;
        }

        //Function that writes to disk the save data modified in the correct language
        //The values used for the language are the same as the function GetLanguage, so
        //check the docs for that function for reference
        public void Convert(string FilePath, int Language)
        {
            //Modify the byte that represents the language linked to the save file
            switch(Language)
            {
                case 1:
                    RawData[200] = 0xFD;
                    break;
                case 2:
                    RawData[200] = 0xFC;
                    break;
                case 3:
                    RawData[200] = 0xFA;
                    break;
                case 4:
                    RawData[200] = 0xFB;
                    break;
                case 5:
                    RawData[200] = 0xF9;
                    break;
            }

            //Calculate the new checksum since the file got modified
            Checksum.Initialize();
            Checksum = Crc32.Hash(RawData);

            //Write to file the new checksum
            for (int i = 0; i < Checksum.Length; i++)
            {
                Header[i + 12] = Checksum[i];
            }

            Data.Initialize();
            Buffer.BlockCopy(Header, 0, Data, 0, Header.Length);
            Buffer.BlockCopy(RawData, 0, Data, Header.Length, RawData.Length);
            Buffer.BlockCopy(Footer, 0, Data, Header.Length + RawData.Length, Footer.Length);

            File.WriteAllBytes(FilePath, Data);
        }
    }
}

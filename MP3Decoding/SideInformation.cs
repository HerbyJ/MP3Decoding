using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Decoding
{
    class SideInformation
    {

        //Single Channel MP3's have 17 bytes of side information.
        //Dual Channel MP3's have 32 bytes of side information.

        //Main Data Begin - It is a pointer that points to the beginning of the main data(audio data).
        //                - Always is 9 bits long, & is a negative offset in bytes from the first byte of the audio sync word. (It jumps backwards)
        //                - Total bytes it can point up to are: 2^9 - 1 = 511 bytes in front of the header.
        //                - If main data value is 0, then main data follows immediately after the side information.

        private int byteLength;
        private int mainDataBegin;
        private byte byte1;
        private byte byte2;
        private byte byte3;
        private byte byte4;
        private byte byte5;
        private byte byte6;
        private byte byte7;
        private byte byte8;
        private byte byte9;
        private byte byte10;
        private byte byte11;
        private byte byte12;
        private byte byte13;
        private byte byte14;
        private byte byte15;
        private byte byte16;
        private byte byte17;
        private byte byte18;
        private byte byte19;
        private byte byte20;
        private byte byte21;
        private byte byte22;
        private byte byte23;
        private byte byte24;
        private byte byte25;
        private byte byte26;
        private byte byte27;
        private byte byte28;
        private byte byte29;
        private byte byte30;
        private byte byte31;
        private byte byte32;

        //SCFSI portion of Side Information
        private byte scfsiBits;
        private int[] scfsiBitFlags;

        //Granule Params of Side Information
        private int par2_3_length_Gran1;
        private int bigVals_Gran1;
        private int globalGains_Gran1;


        /*
         * Byte Bits        Byte    Bits
         * 1    0-7         9       64-71               
         * 2    8-15        10      72-79               
         * 3    16-23       11      80-87          
         * 4    24-31       12      88-95
         * 5    32-39       13      96-103
         * 6    40-47       14      104-111
         * 7    48-55       15      112-119
         * 8    56-63       16      120-127
         */
         

        public SideInformation(bool isStereo, byte[] byteArr, int byteArrStartPos, int byteArrEndPos)
        {
            //SideInformation Size: 17 for mono, 32 for stereo. We will always use at least 17.
            this.byte1 = byteArr[byteArrStartPos];
            this.byte2 = byteArr[byteArrStartPos + 1];
            this.byte3 = byteArr[byteArrStartPos + 2];
            this.byte4 = byteArr[byteArrStartPos + 3];
            this.byte5 = byteArr[byteArrStartPos + 4];
            this.byte6 = byteArr[byteArrStartPos + 5];
            this.byte7 = byteArr[byteArrStartPos + 6];
            this.byte8 = byteArr[byteArrStartPos + 7];
            this.byte9 = byteArr[byteArrStartPos + 8];
            this.byte10 = byteArr[byteArrStartPos + 9];
            this.byte11 = byteArr[byteArrStartPos + 10];
            this.byte12 = byteArr[byteArrStartPos + 11];
            this.byte13 = byteArr[byteArrStartPos + 12];
            this.byte14 = byteArr[byteArrStartPos + 13];
            this.byte15 = byteArr[byteArrStartPos + 14];
            this.byte16 = byteArr[byteArrStartPos + 15];
            this.byte17 = byteArr[byteArrStartPos + 16];

            if (isStereo == true)
            {
                this.byte18 = byteArr[byteArrStartPos + 17];
                this.byte19 = byteArr[byteArrStartPos + 18];
                this.byte20 = byteArr[byteArrStartPos + 19];
                this.byte21 = byteArr[byteArrStartPos + 20];
                this.byte22 = byteArr[byteArrStartPos + 21];
                this.byte23 = byteArr[byteArrStartPos + 22];
                this.byte24 = byteArr[byteArrStartPos + 23];
                this.byte25 = byteArr[byteArrStartPos + 24];
                this.byte26 = byteArr[byteArrStartPos + 25];
                this.byte27 = byteArr[byteArrStartPos + 26];
                this.byte28 = byteArr[byteArrStartPos + 27];
                this.byte29 = byteArr[byteArrStartPos + 28];
                this.byte30 = byteArr[byteArrStartPos + 29];
                this.byte31 = byteArr[byteArrStartPos + 30];
                this.byte32 = byteArr[byteArrStartPos + 31];
            }

            if (isStereo == false) //Single Channel Mode
            {
                byteLength = 17;
                scfsiBits = (byte) ((byte2 & 0x03) | ((byte3 >> 6) & 0x03)); //Bits 15 - 18
                GetFlaggedBitsSCFSI(scfsiBits, false);

                par2_3_length_Gran1 = GetPar2_3_Length(false, byte3, byte4, byte5, byte6); //Bits 19 - 30 in Granule 1
                bigVals_Gran1 = GetBigVals(false, byte3, byte4, byte5, byte6); //Bits 31 - 39 in Granule 1
                globalGains_Gran1 = GetGlobalGains(false, byte6, byte7, byte8); //Bits 40 - 47 in Granule 1
            }
            else //Every Other Channel Mode
            {
                byteLength = 32;
                scfsiBits = (byte)((byte2 & 0x0F) | ((byte3 >> 4) & 0x0F)); //Bits 13 - 20
                GetFlaggedBitsSCFSI(scfsiBits, true);

                par2_3_length_Gran1 = GetPar2_3_Length(true, byte3, byte4, byte5, byte6); //Bits 21 - 44 in Granule 1
                bigVals_Gran1 = GetBigVals(true, byte5, byte6, byte7, byte8); //Bits 45 - 62 in Granule 1
                globalGains_Gran1 = GetGlobalGains(true, byte8, byte7, byte8); // Bits 63 - 78 in Granule 1
            }

            mainDataBegin = GetMainDataBegin(byte1, byte2);
        }

        private int GetGlobalGains(bool isStereo, byte b1, byte b2, byte b3)
        {
            int outcome = 0;
            if(isStereo == false)
            {
                outcome = (byte)((b1 & 0x01) | ((b2 >> 1) & 0x7F));
            }
            else
            {
                outcome = (int)((b1 & 0x01) | b2 | ((b3 >> 1) & 0x7F));
            }

            return outcome;
        }

        private int GetBigVals(bool isStereo, byte b1, byte b2, byte b3, byte b4)
        {
            int outcome = 0;

            if(isStereo == false)
            {
                outcome = (byte)((b1 & 0x01) | ((b2 >> 1) & 0x0F));
            }
            else
            {
                outcome = (int)((b1 & 0x03) | b2 | b3| ((b4 >> 7) & 0x01));
            }

            return outcome;
        }

        private int GetPar2_3_Length(bool isStereo, byte b1, byte b2, byte b3, byte b4)
        {
            int outcome = 0;
            if (isStereo == false)
            {
                outcome = (int)((b1 & 0x1F) | ((b2 >> 1) & 0x7F));
            }
            else
            {
                outcome = (int)((b1 & 0x07) | b2 | b3 | ((b4 >> 3) & 0x1F));
            }

            return outcome;
        }

        private void GetFlaggedBitsSCFSI(byte tempByte, bool isStereo)
        {
            if (isStereo == true) //Stereo
            {
                scfsiBitFlags = new int[8];

                scfsiBitFlags[0] = (tempByte >> 7 & 0x01);
                scfsiBitFlags[1] = (tempByte >> 6 & 0x01);
                scfsiBitFlags[2] = (tempByte >> 5 & 0x01);
                scfsiBitFlags[3] = (tempByte >> 4 & 0x01);
                scfsiBitFlags[4] = (tempByte >> 3 & 0x01);
                scfsiBitFlags[5] = (tempByte >> 2 & 0x01);
                scfsiBitFlags[6] = (tempByte >> 1 & 0x01);
                scfsiBitFlags[7] = (tempByte & 0x01);
            }
            else //Mono
            {
                scfsiBitFlags = new int[4];

                scfsiBitFlags[0] = (tempByte >> 3 & 0x01);
                scfsiBitFlags[1] = (tempByte >> 2 & 0x01);
                scfsiBitFlags[2] = (tempByte >> 1 & 0x01);
                scfsiBitFlags[3] = (tempByte & 0x01);
            }
            
        }


        private int GetMainDataBegin(byte b1, byte b2)
        {
            //9 bits long, so first 2 bytes will always be needed.

            short temp = b1;
            short outcome = (short)(temp << 1 | ((b2 >> 7) & 0x01));

            return outcome;
        }
    }
}

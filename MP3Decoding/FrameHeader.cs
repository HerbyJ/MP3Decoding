using System;

namespace MP3Decoding
{
    class FrameHeader
    {
        private bool isStereo;
        private bool isCopyright;
        private bool isOriginal;
        private bool isValidFrame;
        private bool isPadded;

        private byte byte1;
        private byte byte2;
        private byte byte3;
        private byte byte4;

        private int frameLength;
        private int mpegVersion;
        private int mpegLayer;
        private int bitRate;
        private int sampleRate;
        private int padding;

        private string frameHeaderBytes;


        public FrameHeader(byte byte1, byte byte2, byte byte3, byte byte4)
        {
            this.byte1 = byte1;
            this.byte2 = byte2;
            this.byte3 = byte3;
            this.byte4 = byte4;

            frameHeaderBytes = byte1 + " " + byte2 + " " + byte3 + " " + byte4;

            mpegVersion = GetMpegVersion(((byte2 >> 4) & 0x01), ((byte2 >> 3) & 0x01));

            mpegLayer = GetLayer(((byte2 >> 2) & 0x01), ((byte2 >> 1) & 0x01));

            bitRate = GetBitRate(mpegVersion, mpegLayer, ((byte3 >> 4) & 0x0F));

            sampleRate = GetSampleRate(((byte3 >> 3) & 0x01), ((byte3 >> 2) & 0x01), mpegVersion);

            padding = (byte3 >> 1) & 0x01;

            isPadded = padding != 0;

            isStereo = GetStereo(((byte4 >> 5) & 0x01), ((byte4 >> 4) & 0x01));

            isCopyright = ((byte4 >> 3) & 0x01) != 0;

            isOriginal = ((byte4 >> 2) & 0x01) != 0;

            frameLength = (int)Math.Floor((144 * ((bitRate * 1000.0) / (sampleRate)) + padding));

            isValidFrame = GetValidFrame(bitRate, sampleRate);
        }

        private bool GetValidFrame(int bitRate, int sampleRate)
        {
            if(bitRate > 0 && sampleRate > 0)
            {
                return true;
            }

            return false;
        }

        private bool GetStereo(int val1, int val2)
        {
            int outcome =  (byte)(val2 | (val1 << 1));

            if (outcome == 0 || outcome == 1 || outcome == 2)
            {
                return true;
            }
            else if (outcome == 3)
            {
                return false;
            }

            return false;
        }




        private int GetMpegVersion(int val1, int val2)
        {
            return (byte)(val2 | (val1 << 1));
        }





        private int GetLayer(int val1, int val2)
        {
            return (byte)(val2 | (val1 << 1));
        }






        private int GetBitRate(int mpegVer, int layer, int bitRateBits)
        {
            if (mpegVer == 3 && layer == 3) //MPEG Version 1, Layer 1
            {
                switch (bitRateBits)
                {
                    case 0: return 0;
                    case 1: return 32;
                    case 2: return 64;
                    case 3: return 96;
                    case 4: return 128;
                    case 5: return 160;
                    case 6: return 192;
                    case 7: return 224;
                    case 8: return 256;
                    case 9: return 288;
                    case 10: return 320;
                    case 11: return 352;
                    case 12: return 384;
                    case 13: return 416;
                    case 14: return 448;
                    case 15: return -1;
                    default: return -1;
                }
            }

            if (mpegVer == 3 && layer == 2) //MPEG Version 1, Layer 2
            {
                switch (bitRateBits)
                {
                    case 0: return 0;
                    case 1: return 32;
                    case 2: return 48;
                    case 3: return 56;
                    case 4: return 64;
                    case 5: return 80;
                    case 6: return 96;
                    case 7: return 112;
                    case 8: return 128;
                    case 9: return 160;
                    case 10: return 192;
                    case 11: return 224;
                    case 12: return 256;
                    case 13: return 320;
                    case 14: return 384;
                    case 15: return -1;
                    default: return -1;
                }
            }

            if (mpegVer == 3 && layer == 1) //MPEG Version 1, Layer 3
            {
                switch (bitRateBits)
                {
                    case 0: return 0;
                    case 1: return 32;
                    case 2: return 40;
                    case 3: return 48;
                    case 4: return 56;
                    case 5: return 67;
                    case 6: return 80;
                    case 7: return 96;
                    case 8: return 112;
                    case 9: return 128;
                    case 10: return 160;
                    case 11: return 192;
                    case 12: return 224;
                    case 13: return 256;
                    case 14: return 320;
                    case 15: return -1;
                    default: return -1;
                }
            }

            if (mpegVer == 2 && layer == 3) //MPEG Version 2, Layer 1
            {
                switch (bitRateBits)
                {
                    case 0: return 0;
                    case 1: return 32;
                    case 2: return 48;
                    case 3: return 56;
                    case 4: return 64;
                    case 5: return 80;
                    case 6: return 96;
                    case 7: return 112;
                    case 8: return 128;
                    case 9: return 144;
                    case 10: return 160;
                    case 11: return 176;
                    case 12: return 192;
                    case 13: return 224;
                    case 14: return 256;
                    case 15: return -1;
                    default: return -1;
                }
            }

            if (mpegVer == 2 && (layer == 2 || layer == 1)) //MPEG Version 2, Layer 2 or Layer 3
            {
                switch (bitRateBits)
                {
                    case 0: return 0;
                    case 1: return 8;
                    case 2: return 16;
                    case 3: return 24;
                    case 4: return 32;
                    case 5: return 40;
                    case 6: return 48;
                    case 7: return 56;
                    case 8: return 64;
                    case 9: return 80;
                    case 10: return 96;
                    case 11: return 112;
                    case 12: return 128;
                    case 13: return 144;
                    case 14: return 160;
                    case 15: return -1;
                    default: return -1;
                }
            }

            return -1;
        }




        private int GetSampleRate(int val1, int val2, int mpegVer)
        {
            int sampleBits = (byte)(val2 | (val1 << 1));

            if (mpegVer == 3) //MPEG Version 1
            {
                switch (sampleBits)
                {
                    case 0: return 44100;
                    case 1: return 48000;
                    case 2: return 32000;
                    case 3: return -1;
                    default: return -1;
                }
            }

            if (mpegVer == 2) //MPEG Version 2
            {
                switch (sampleBits)
                {
                    case 0: return 22050;
                    case 1: return 24000;
                    case 2: return 16000;
                    case 3: return -1;
                    default: return -1;
                }
            }

            if (mpegVer == 0) //MPEG Version 2.5
            {
                switch (sampleBits)
                {
                    case 0: return 11025;
                    case 1: return 12000;
                    case 2: return 8000;
                    case 3: return -1;
                    default: return -1;
                }
            }

            return -1;
        }

        public bool IsValidFrame { get => isValidFrame; set => isValidFrame = value; }
        public int FrameLength { get => frameLength; set => frameLength = value; }
        public string FrameHeaderBytes { get => frameHeaderBytes; set => frameHeaderBytes = value; }
        public int MpegVersion { get => mpegVersion; set => mpegVersion = value; }
        public int MpegLayer { get => mpegLayer; set => mpegLayer = value; }
        public int BitRate { get => bitRate; set => bitRate = value; }
        public int SampleRate { get => sampleRate; set => sampleRate = value; }
        public bool IsStereo { get => isStereo; set => isStereo = value; }
        public bool IsCopyright { get => isCopyright; set => isCopyright = value; }
        public bool IsOriginal { get => isOriginal; set => isOriginal = value; }
        public bool IsPadded { get => isPadded; set => isPadded = value; }
    }
}

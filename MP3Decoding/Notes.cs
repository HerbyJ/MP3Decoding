using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Decoding
{
    class Notes
    {
        /****************************************************************************************************************************
         * This class holds all the NOTES and TERMINOLOGY learned and used throughout this Project.
         * 
         * Project PDF Reference 1:   B:/C Sharp Projects/MP3Decoding/mp3.pdf
         * Project PDF Reference 2:   B:/C Sharp Projects/MP3Decoding/mp3_1.pdf
         * 
         * NOTES:
         * 
         * //Console.WriteLine("Shift 7: " + (b2 >> 7 & 0x01));
         * //Console.WriteLine("Shift 6: " + (b2 >> 6 & 0x01));
         * //Console.WriteLine("Shift 5: " + (b2 >> 5 & 0x01));
         * //Console.WriteLine("Shift 4: " + (b2 >> 4 & 0x01));
         * //Console.WriteLine("Shift 3: " + (b2 >> 3 & 0x01));
         * //Console.WriteLine("Shift 2: " + (b2 >> 2 & 0x01));
         * //Console.WriteLine("Shift 1: " + (b2 >> 1 & 0x01));
         * //Console.WriteLine("Shift 0: " + (b2 & 0x01));
         * 
         * 
         * **** SECTION 1- AUDIO FRAME HEADER ***************************************************************************************
         * DEFINITIONS:
         *  Samples - bytes??? No clear definition
         *  
         *  Frame Size - The number of Samples contained in a frame.
         *               Layer I always contains 384, Layers II & III contain 1152.
         * 
         *  Frame Length - The length of a frame when compressed. Measured in byte slots.
         *                 One slot is 4 Bytes long in Layer I, and 1 Byte long for Layers II & III.
         *                 Frame Length can change due to padding or bitrate switching.
         *                 To attain Frame Length:
         *                      Frame Length = 144 * (Bitrate/SampleRate) + Padding
         * 
         * **** Each frame is made up of 1152 Samples, and there is always a header attached to each frame in the MP3 file.
         *      Content in the header and side information portions of the frame are important in decoding the MP3 data.
         * 
         * **** MP3's allow for variable bit rates. This allows for lower bitrates in frames where it will not reduce sound quality.
         *      This allows for better compression rates while keeping high quality of sound.
         * 
         * **** Frame Header Layout: Refer to "PDF Reference 1" Section 5.1.1 for a visual layout, and bit for bit details in the header.
         * 
         * **** False headers aka False Syncs are common in MP3's. To ensure you attained an accurate header, perform the calculations 
         *      to attain the frame length (data is in the frame header), and validate the next couple frames headers match.
         *      
         * **** Frames may have a CRC check. The CRC is 16 bits long, and follows the frame header and is before the audio data.
         *      We can use the CRC to possibly find the size of the frame header.
         *      
         * 
         * 
         * 
         * **** SECTION 2- SIDE INFORMATION *****************************************************************************************
         * 
         * DEFINITIONS:
         *  Main Data Begin - It is a pointer that points to the beginning of the main data (audio data).
         *                  - 9 bits long, & is a negative offset in bytes from the first byte of the audio sync word. (It jumps backwards)
         *                  - Total bytes it can point up to are: 2^9 - 1 = 511 bytes in front of the header.
         *                  - If main data value is 0, then main data follows immediately after the side information.
         *                  
         * SCFSI (Scale Factor Selection Information) - Dictates the scale factor bands for 4 band groups.                 
         * 
         * **** Side Information contains the data that allows us to decode the MP3 file correctly. It contains 5 different sections:
         *      1) Main Data Begin
         *      2) Private Bits         //NOT GOING TO BE USED IN THIS PROGRAM
         *      3) SCFSI
         *      4) Side Info for Granule 1
         *      5) Side Info for Granule 2
         * 
         * **** Single Channel MP3's have 17 bytes of side information. Refer to Tables 6 & 7 in section 5.2 of "PDF Reference 1" for format.
         * 
         * **** Dual Channel MP3's have 32 bytes of side information. Refer to Tables 6 & 7 in section 5.2 of "PDF Reference 1" for format.
         * 
         * **** SCFSI portion contains 4 bits (mono) or 8 bits (Stereo) for scalefactor bands. Each bit represents a scalefactor.
         *      For example, if all bits are flagged (1111), then all scalefactor bands are used. (Refer to Table 5.9 in "PDF Reference 2").
         *      For Stereo, if a bit is flagged (= 1) then that means the next channel will contain the same scalefactor.
         *      However, I chose to simply utilize all 8 bits of the stereo SFSCI, instead of validating the first 4 bits, and 
         *      correlating them to the last 4 bits. 
         * 
         * **** There are 2 Granules in each Frame. We will only need 1. Mono uses 59 bits, while stereo is double that, at 118.
         * 
         * LEFT OFF ON SCALEFAC_COMPRESS
        ****************************************************************************************************************************/
    }
}

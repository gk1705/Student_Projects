using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TES_TriangleEscapeSimulator
{
    //Needed for the filestream;
    [Serializable]
    public struct HighScoreData
    {
        public string[] playerName;
        public float[] time;
        public int[] distance;

        public int count;

        public HighScoreData(int count)
        {
            playerName = new string[count];
            time = new float[count];
            distance = new int[count];

            this.count = count;
        }
    }

    public class HighScore
    {
        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public static void SaveHighScores(HighScoreData data, string filename)
        {
            //path of the save game;
            string fullpath = filename;

            //Open, create file if necessary;
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try
            {
                //Convert the obj. to XML data and put it in the stream;
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                stream.Close();
            }
        }

        public static HighScoreData LoadHighScores(string filename)
        {
            HighScoreData data;
            
            //Path of the save game;
            string fullpath = filename;

            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate,
            FileAccess.Read);
            try
            {

                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                //Deserializing corrupts the xml file - therefor sadly not yet of use;
                data = (HighScoreData)serializer.Deserialize(stream);
            }
            finally
            {
                // Close the file
                stream.Close();
            }

            return (data);
        }
    }
    
}

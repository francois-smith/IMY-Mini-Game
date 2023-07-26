/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Runtime.SaveSystem
{
    [Serializable]
    internal class SaveData
    {
        public int score;

        public SaveData(int score)
        {
            this.score = score;
        }
    }
    
    public class SaveManager : MonoBehaviour
    {
        private int _highScore;

        public void SaveData(int score)
        {
            var destination = Application.persistentDataPath + "/save.dat";
            var file = File.Exists(destination) ? File.OpenWrite(destination) : File.Create(destination);
            var data = new SaveData(score);
            
            var bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }
        
        public int GetHighScore()
        {
            return _highScore;
        }

        public void LoadData()
        {
            var destination = Application.persistentDataPath + "/save.dat";
            FileStream file;

            if(File.Exists(destination)) file = File.OpenRead(destination);
            else
            {
                SaveData(0);
                return;
            }

            var bf = new BinaryFormatter();
            var data = (SaveData)bf.Deserialize(file);
            file.Close();

            _highScore = data.score;
        }
    }
}

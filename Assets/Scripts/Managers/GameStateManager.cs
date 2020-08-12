using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/*
namespace ProjectR.SaveGame
{

    [Serializable]
    public class HeroStats
    {
        public int _baseHealth;
        public int _baseMaxHealth;
        public int _baseStrength;
        public int _baseAgility;
        public int _baseSpirituality;
        public int _baseArmor;
        public string _name;
        public string _description;
        public HeroArchetype _mainArchetype;
        public HeroArchetype _subArchetype;
        public Hero.HeroType _type;

        public int weaponID, weaponType;
        public int armorID, armorType;


        public int _level;
        public int _experience;
        public int _maxExperience;


        public HeroStats()
        {

        }
    }

    public class GameStateManager : MonoBehaviour
    {
        private string savePath;

        private void Start()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string finalpath = path+  "/" + Application.companyName + "/" + Application.productName;
            savePath = finalpath.Replace("\\","/");
#endif
#if !UNITY_STANDALONE_WIN && !UNITY_EDITOR
            savePath = Application.persistentDataPath;
#endif
        }


        public void Save()
        {


            Directory.CreateDirectory(Path.GetDirectoryName(savePath + "/gamesave.save"));
            GameInit g = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameInit>();
            GameState state = g.getState();
            state.packHeroes();


            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(savePath + "/gamesave.save");
            bf.Serialize(file, state);
            file.Close();
        }

        public void Load()
        {
            if (File.Exists(savePath + "/gamesave.save"))
            {

                // 2
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(savePath + "\\gamesave.save", FileMode.Open);
                GameState state = (GameState)bf.Deserialize(file);
                file.Close();

                GameInit g = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameInit>();
                g.setState(state);
            }
           


        }
    }
}*/

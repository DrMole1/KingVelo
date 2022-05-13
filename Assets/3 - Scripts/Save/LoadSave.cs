using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoadSave
{
    public class LoadSave
    {
        // ============================= //

        // Meilleur_Joueur_01 : STRING
        // Meilleur_Joueur_02 : STRING
        // Meilleur_Joueur_03 : STRING
        // Meilleur_Score_01 : INT
        // Meilleur_Score_02 : INT
        // Meilleur_Score_03 : INT
        // Meilleur_Avatar_01 : STRING
        // Meilleur_Avatar_02 : STRING
        // Meilleur_Avatar_03 : STRING

        // Casque_Joueur : INT
        // Armature_Joueur : INT
        // Sonnette_Joueur : INT
        // Pneus_Joueur : INT
        // Pieces_Joueur : INT

        // Visage : INT
        // Peau : INT
        // Bouche : INT
        // Nez : INT
        // Yeux : INT
        // Cheveux : INT

        // 00_Casque : BOOL (tableau)
        // 00_Armature : BOOL (tableau)
        // 00_Sonnette : BOOL (tableau)
        // 00_Pneus : BOOL (tableau)

        // Volume_Musique : INT
        // Volume_Son : INT

        // Pseudo_Joueur : STRING
        // Score_Joueur : INT
        // Combinaison_Avatar : STRING

        // ============================= //

        // ================================================================ Meilleurs Joueurs ================================================================

        static public string LoadBestPlayerAtRank(int _rank)
        {
            string nameSave = "Meilleur_Joueur_0" + _rank.ToString();
            return PlayerPrefs.GetString(nameSave, "");
        }

        static public int LoadBestScoreAtRank(int _rank)
        {
            string nameSave = "Meilleur_Score_0" + _rank.ToString();
            return PlayerPrefs.GetInt(nameSave, 0);
        }

        static public string LoadCombinationAtRank(int _rank)
        {
            string nameSave = "Meilleur_Avatar_0" + _rank.ToString();
            return PlayerPrefs.GetString(nameSave, "");
        }

        // ================================================================ Eléments équipés ================================================================

        static public int LoadEquipedHelmet()
        {
            return PlayerPrefs.GetInt("Casque_Joueur", 0);
        }

        static public int LoadEquipedBike()
        {
            return PlayerPrefs.GetInt("Armature_Joueur", 0);
        }

        static public int LoadEquipedKlaxon()
        {
            return PlayerPrefs.GetInt("Sonnette_Joueur", 0);
        }

        static public int LoadEquipedWheels()
        {
            return PlayerPrefs.GetInt("Pneus_Joueur", 0);
        }

        static public int LoadCurrentCoins()
        {
            return PlayerPrefs.GetInt("Pieces_Joueur", 0);
        }

        // ================================================================ Personnalisation Avatar ================================================================

        static public int LoadFace()
        {
            return PlayerPrefs.GetInt("Visage", 0);
        }

        static public int LoadSkin()
        {
            return PlayerPrefs.GetInt("Peau", 0);
        }

        static public int LoadMouth()
        {
            return PlayerPrefs.GetInt("Bouche", 0);
        }

        static public int LoadNose()
        {
            return PlayerPrefs.GetInt("Nez", 0);
        }

        static public int LoadEyes()
        {
            return PlayerPrefs.GetInt("Yeux", 0);
        }

        static public int LoadHair()
        {
            return PlayerPrefs.GetInt("Cheveux", 0);
        }

        // ================================================================ Items Achetés au Shop ================================================================

        static public int LoadBoughtHelmet(int _id)
        {
            string nameSave = _id.ToString() + "_Casque";

            return PlayerPrefs.GetInt(nameSave, 0);
        }

        static public int LoadBoughtBike(int _id)
        {
            string nameSave = _id.ToString() + "_Armature";

            return PlayerPrefs.GetInt(nameSave, 0);
        }

        static public int LoadBoughtKlaxon(int _id)
        {
            string nameSave = _id.ToString() + "_Sonnette";

            return PlayerPrefs.GetInt(nameSave, 0);
        }

        static public int LoadBoughtWheels(int _id)
        {
            string nameSave = _id.ToString() + "_Pneus";

            return PlayerPrefs.GetInt(nameSave, 0);
        }

        // ================================================================ Paramètres ================================================================

        static public float LoadMusicVolume()
        {
            return PlayerPrefs.GetFloat("VolumeMusique", 1f);
        }

        static public float LoadSoundVolume()
        {
            return PlayerPrefs.GetFloat("VolumeSon", 1f);
        }

        // =================================================================== Joueur ==================================================================

        static public string LoadPlayerPseudo()
        {
            return PlayerPrefs.GetString("Pseudo_Joueur", " ");
        }

        static public int LoadPlayerScore()
        {
            return PlayerPrefs.GetInt("Score_Joueur", 0);
        }

        static public int[] LoadCombination()
        {
            string combination =  PlayerPrefs.GetString("Combinaison_Avatar", "");
            int[] elements = new int[6];

            for(int i = 0; i < elements.Length; i++)
            {
                elements[i] = combination[i];
            }

            return elements;
        }
    }
}
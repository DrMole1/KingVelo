using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save
{
    public class Save
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

        static public void SaveStartElements()
        {
            SaveBoughtHelmet(0);
            SaveBoughtBike(0);
            SaveBoughtKlaxon(0);
            SaveBoughtWheels(0);
        }

        // ================================================================ Meilleurs Joueurs ================================================================

        static public void SaveBestPlayerAtRank(string _pseudo, int _rank)
        {
            if(_rank == 0 || _rank > 3) { Debug.Log("error : _SaveBestPlayerAtRank"); return; }

            string nameSave = "Meilleur_Joueur_0" + _rank.ToString();
            PlayerPrefs.SetString(nameSave, _pseudo);
        }

        static public void SaveBestScoreAtRank(int _score, int _rank)
        {
            if (_rank == 0 || _rank > 3) { Debug.Log("error : _SaveBestScoreAtRank"); return; }

            string nameSave = "Meilleur_Score_0" + _rank.ToString();
            PlayerPrefs.SetInt(nameSave, _score);
        }

        static public void SaveBestCombinationAtRank(string _combination, int _rank)
        {
            if (_rank == 0 || _rank > 3) { Debug.Log("error : _SaveBestCombinationAtRank"); return; }

            string nameSave = "Meilleur_Avatar_0" + _rank.ToString();
            PlayerPrefs.SetString(nameSave, _combination);
        }

        // ================================================================ Eléments équipés ================================================================

        static public void SaveEquipedHelmet(int _helmet)
        {
            PlayerPrefs.SetInt("Casque_Joueur", _helmet);
        }

        static public void SaveEquipedBike(int _bike)
        {
            PlayerPrefs.SetInt("Armature_Joueur", _bike);
        }

        static public void SaveEquipedKlaxon(int _klaxon)
        {
            PlayerPrefs.SetInt("Sonnette_Joueur", _klaxon);
        }

        static public void SaveEquipedWheels(int _wheels)
        {
            PlayerPrefs.SetInt("Pneus_Joueur", _wheels);
        }

        static public void SaveCurrentCoins(int _coins)
        {
            PlayerPrefs.SetInt("Pieces_Joueur", _coins);
        }

        // ================================================================ Personnalisation Avatar ================================================================

        static public void SaveFace(int _face)
        {
            PlayerPrefs.SetInt("Visage", _face);
        }

        static public void SaveSkin(int _skin)
        {
            PlayerPrefs.SetInt("Peau", _skin);
        }

        static public void SaveMouth(int _mouth)
        {
            PlayerPrefs.SetInt("Bouche", _mouth);
        }

        static public void SaveNose(int _nose)
        {
            PlayerPrefs.SetInt("Nez", _nose);
        }

        static public void SaveEyes(int _eyes)
        {
            PlayerPrefs.SetInt("Yeux", _eyes);
        }

        static public void SaveHair(int _hair)
        {
            PlayerPrefs.SetInt("Cheveux", _hair);
        }

        // ================================================================ Items Achetés au Shop ================================================================

        static public void SaveBoughtHelmet(int _id)
        {
            string nameSave = _id.ToString() + "_Casque";

            PlayerPrefs.SetInt(nameSave, 1);
        }

        static public void SaveBoughtBike(int _id)
        {
            string nameSave = _id.ToString() + "_Armature";

            PlayerPrefs.SetInt(nameSave, 1);
        }

        static public void SaveBoughtKlaxon(int _id)
        {
            string nameSave = _id.ToString() + "_Sonnette";

            PlayerPrefs.SetInt(nameSave, 1);
        }

        static public void SaveBoughtWheels(int _id)
        {
            string nameSave = _id.ToString() + "_Pneus";

            PlayerPrefs.SetInt(nameSave, 1);
        }

        // ================================================================ Paramètres ================================================================

        static public void SaveMusicVolume(float _volume)
        {
            PlayerPrefs.SetFloat("VolumeMusique", _volume);
        }

        static public void SaveSoundVolume(float _volume)
        {
            PlayerPrefs.SetFloat("VolumeSon", _volume);
        }

        // =================================================================== Joueur ==================================================================

        static public void SavePlayerPseudo(string _pseudo)
        {
            PlayerPrefs.SetString("Pseudo_Joueur", _pseudo);
        }

        static public void SavePlayerScore(int _score)
        {
            PlayerPrefs.GetInt("Score_Joueur", _score);
        }

        static public void SaveCombination(string _combination)
        {
            PlayerPrefs.SetString("Combinaison_Avatar", _combination);
        }
    }
}
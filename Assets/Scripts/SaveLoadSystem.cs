using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSystem : MonoBehaviour 
{
    private static string filePath = Application.persistentDataPath + "/highscore.dat";

    public static void SaveHighscore(int score, int layer)
    {
        try
        {
            // Create or open the file for appending
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                // Write the two integer values separated by a space and followed by a newline
                writer.WriteLine($"{score} €$\tL{layer}");
            }
        }
        catch (IOException e)
        {
            Debug.LogError("Error writing to file: " + e.Message);
        }
    }

    public static string[] ReadHighscores()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            // Sort the array in descending order based on the score values
            Array.Sort(lines, (a, b) =>
            {
                // Split the lines and parse the scores
                int scoreA = int.Parse(a.Split(' ')[0]);
                int scoreB = int.Parse(b.Split(' ')[0]);

                // Compare and sort in descending order
                return scoreB.CompareTo(scoreA);
            });

            return lines;
        }

        return null; // File not found or error
    }

}
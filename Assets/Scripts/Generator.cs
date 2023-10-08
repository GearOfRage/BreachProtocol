using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    private int matrixSize = 7;

    private string[] possibleCombinations =
    {
        "9B", "DD", "55", "1C", "FF", "7A",
        "8B", "BD", "E9", "33", "4A", "47",
        "5C", "A3", "61", "88", "F7", "AE",
    };

    public List<string> selectedCombinations = new List<string>();

    public void GenerateAll()
    {
        GenerateMatrix();

        Utility.DestroyAllChildren(GameMaster._instance.sequencesHolder);
        foreach (GameObject item in GameMaster._instance.sequencesPrefabs)
        {
            GenerateSequences(item);
        }

        foreach (Sequence item in GameMaster._instance.availableSequences)
        {
            item.GenerateSequenceValues();
        }
    }

    public void GenerateMatrix()
    {
        Utility.DestroyAllChildren(GameMaster._instance.matrixHolder);
        
        int rnd = Random.Range(5, 6);
        List<int> rndIndexes = new List<int>();

        for (int i = rnd; i > 0; i--)
        {
            int index = Random.Range(0, possibleCombinations.Length);
            if (rndIndexes.Contains(index))
            {
                i++;
            }
            else
            {
                rndIndexes.Add(index);
            }
        }

        foreach (var item in rndIndexes)
        {
            selectedCombinations.Add(possibleCombinations[item]);
        }

        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                GameObject newMatrixValueObj = Instantiate(GameMaster._instance.matrixValuePrefab,
                    GameMaster._instance.matrixHolder.transform);
                MatrixValue newMatrixValue = newMatrixValueObj.GetComponent<MatrixValue>();
                newMatrixValue.value = selectedCombinations[Random.Range(0, selectedCombinations.Count)];
                newMatrixValue.OnClickEvent += GameMaster._instance.HandleMatrixClick;
                newMatrixValue.row = i;
                newMatrixValue.column = j;
                newMatrixValue.UpdateInstance();
                GameMaster._instance.matrix[i, j] = newMatrixValueObj;
            }
            
        }
        //Debug.Log("Matrix generation is done!");
    }

    public void GenerateSequences(GameObject sequence)
    {
        GameObject newSequence =
            Instantiate(sequence, GameMaster._instance.sequencesHolder.transform);

        Sequence seqComp = newSequence.GetComponent<Sequence>();
        GameMaster._instance.availableSequences.Add(seqComp);

        Utility.DestroyAllChildren(seqComp.valueHolderObj);
        
        //Debug.Log(seqComp.seqName + " generation is done!");
    }
}
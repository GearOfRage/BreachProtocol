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
    [SerializeField]private int matrixSize = 7;

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

        foreach (Sequence item in GameMaster._instance.activeSequences)
        {
            item.GenerateSequenceValues();
        }
    }

    public void GenerateMatrix()
    {
        Utility.DestroyAllChildren(GameMaster._instance.matrixHolder);

        //Picking unique value combination
        int rnd = Random.Range(5, 7);
        int[] rndIndexes = new int[rnd];

        for (int i = 0; i < rnd; i++)
        {
            int index = Random.Range(0, possibleCombinations.Length);
            if (rndIndexes.Contains(index))
            {
                i--;
            }
            else
            {
                rndIndexes[i] = index;
            }
        }

        selectedCombinations.Clear();
        foreach (int item in rndIndexes)
        {
            selectedCombinations.Add(possibleCombinations[item]);
        }

        //Building fake matrix(array). Ensuring playability.
        
        float evenlyDistributedPercentage = 0.5f; // Percentage of first array to be evenly distributed
        int resultArraySize = matrixSize * matrixSize; // Size of the result array
        
        // Calculate the number of elements from the original array to be evenly distributed
        int evenlyDistributedCount = Mathf.FloorToInt(evenlyDistributedPercentage * resultArraySize);

        // Calculate the number of elements to be randomly distributed
        int randomlyDistributedCount = resultArraySize - evenlyDistributedCount;

        // Create arrays for even and random distribution
        string[] evenlyDistributedValues = new string[evenlyDistributedCount];
        string[] randomlyDistributedValues = new string[randomlyDistributedCount];

        // Fill the evenly distributed values array
        for (int i = 0; i < evenlyDistributedCount; i++)
        {
            evenlyDistributedValues[i] = selectedCombinations[i % selectedCombinations.Count];
        }

        // Fill the randomly distributed values array
        for (int i = 0; i < randomlyDistributedCount; i++)
        {
            int randomIndex = Random.Range(0, selectedCombinations.Count);
            randomlyDistributedValues[i] = selectedCombinations[randomIndex];
        }

        // Combine the even and random distribution arrays into the result array
        string[] resultArray = new string[resultArraySize];
        System.Array.Copy(evenlyDistributedValues, resultArray, evenlyDistributedCount);
        System.Array.Copy(randomlyDistributedValues, 0, resultArray, evenlyDistributedCount, randomlyDistributedCount);

        //Shuffling of values
        
        int n = resultArray.Length;
        for (int i = 0; i < n; i++)
        {
            int randomIndex = Random.Range(i, n);
            (resultArray[i], resultArray[randomIndex]) = (resultArray[randomIndex], resultArray[i]); //swap via deconstruction
        }
        
        //Instantiating game objects
        int counter = 0;
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                GameObject newMatrixValueObj = Instantiate(GameMaster._instance.matrixValuePrefab,
                    GameMaster._instance.matrixHolder.transform);
                MatrixValue newMatrixValue = newMatrixValueObj.GetComponent<MatrixValue>();
                newMatrixValue.OnClickEvent += GameMaster._instance.HandleMatrixClick;
                newMatrixValue.row = i;
                newMatrixValue.column = j;
                newMatrixValue.UpdateInstance(resultArray[counter]);
                GameMaster._instance.matrix[i, j] = newMatrixValueObj;

                counter++;
            }
        }
        //Debug.Log("Matrix generation is done!");
    }

    public void GenerateSequences(GameObject sequence)
    {
        GameObject newSequence =
            Instantiate(sequence, GameMaster._instance.sequencesHolder.transform);

        Sequence seqComp = newSequence.GetComponent<Sequence>();
        GameMaster._instance.activeSequences.Add(seqComp);

        Utility.DestroyAllChildren(seqComp.valueHolderObj);

        //Debug.Log(seqComp.seqName + " generation is done!");
    }
    
    
    public void GenerateSecurityProtocols(List<GameObject> securityProtocolsPrefabs)
    {
        int rndIndex = Random.Range(0, securityProtocolsPrefabs.Count);
        GameObject newSecurityProtocol =
            Instantiate(securityProtocolsPrefabs[rndIndex], GameMaster._instance.securityProtocolsHolder.transform);
        
        SecurityProtocol secComp = newSecurityProtocol.GetComponent<SecurityProtocol>();
        GameMaster._instance.activeSecurityProtocols.Add(secComp);
        securityProtocolsPrefabs.RemoveAt(rndIndex);
    }
}
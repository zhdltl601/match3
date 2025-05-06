using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    private const int k_y = 7;

    private readonly Node[][] arr = new Node[k_y][];    //jagged
    private Node[] allArr;                              //one dimension
    [SerializeField] private Node nodePrefab;
    [SerializeField] private Transform spawnTransform;
    private void OnEnable()
    {
        const int k_row = 15;
        int[] initArray = new int[k_y]
        {
            k_row,
            k_row,
            k_row,
            k_row,
            k_row,
            k_row,
            k_row
        };
        Task _ = Initialization(initArray);
    }
    private void OnDisable()
    {
        KillAllNode();
    }

    public async Task Initialization(IEnumerable<int> initArray)
    {
        Debug.Assert(initArray != null, "initArry is null");

        int arrayLength = 0;
        int arrayIndex = 0;
        IEnumerator<int> enumerator = initArray.GetEnumerator();
        while (enumerator.MoveNext())
        {
            int item = enumerator.Current;
            arr[arrayIndex] = new Node[item];
            arrayLength += item;

            arrayIndex++;
        }

        Debug.Assert(arrayLength != 0, "length is zero");

        AsyncInstantiateOperation<Node> task = InstantiateAsync(nodePrefab, arrayLength, transform.position, Quaternion.identity);

        await task;

        int x = 0;
        int y = 0;

        Node[] nodes = task.Result;
        allArr = nodes;

        Debug.Assert(nodes.Length == arrayLength, "array length doesn't match");

        for (int i = 0; i < nodes.Length; i++)
        {
            if (y >= arr[x].Length)
            {
                x++;
                y = 0;
            }

            await Awaitable.WaitForSecondsAsync(0.01f);

            Node item = nodes[i];

            Vector3 spawnPosition = this.spawnTransform.position;
            Vector3 position = new Vector3(y, -x) + spawnPosition;
            Quaternion rotation = Quaternion.identity;
            item.transform.SetPositionAndRotation(position, rotation);

            arr[x][y] = item;

            item.Initialization();

            const int min = (int)EColor.None + 1;
            const int max = (int)EColor.End;
            EColor color = (EColor)UnityEngine.Random.Range(min, max);
            item.SetColor(color);

            y++;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            KillAllNode();
        }
    }
    private void KillAllNode()
    {
        for (int i = 0; i < allArr.Length; i++)
        {
            ref Node currentNode = ref allArr[i];
            if (currentNode == null) continue;
            Destroy(currentNode.gameObject);
            currentNode = null;
        }
    }
    private void OnDestroy()
    {
        //todo : early exit when quitting
        if (allArr != null)
        {
            KillAllNode();
        }
    }
}

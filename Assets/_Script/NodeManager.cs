using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    private const int k_y = 7;

    private readonly Node[][] arr = new Node[k_y][];      //jagged
    private Node[] allArr;                              //one dimension
    [SerializeField] private Node nodePrefab;
    private void OnEnable()
    {
        int[] initArray = new int[k_y]
        {
            15,
            15,
            15,
            15,
            15,
            15,
            15
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

        AsyncInstantiateOperation<Node> task = InstantiateAsync(nodePrefab, arrayLength);

        await task;

        int x = 0;
        int y = 0;

        Node[] nodes = task.Result;
        allArr = nodes;

        Debug.Assert(nodes.Length == arrayLength, "what?");

        for (int i = 0; i < nodes.Length; i++)
        {
            if (y >= arr[x].Length)
            {
                x++;
                y = 0;
            }

            Node item = nodes[i];

            Vector3 position = new Vector3(y, -x) + transform.position;
            Quaternion rotation = Quaternion.identity;
            item.transform.SetPositionAndRotation(position, rotation);

            arr[x][y] = item;

            item.Initialization();
            const int min = (int)EColor.None + 1;
            const int max = (int)EColor.End;
            item.SetColor((EColor)UnityEngine.Random.Range(min, max));

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
        if(allArr != null)
        {
            KillAllNode();
        }
    }
}

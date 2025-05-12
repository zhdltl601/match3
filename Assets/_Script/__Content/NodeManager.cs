using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
public class NodeManager : MonoSingleton<NodeManager>
{
    private const int k_y = 7;

    private static readonly Node[][] nodeArray = new Node[k_y][];   //jagged        // note : y, x
    private Node[] allNodeArr;                                      //one dimension // note : do i need this?

    [Header("Settings")]
    [SerializeField] private Node nodePrefab;
    [SerializeField] private Transform spawnTransform;
    #region Getters
    public Vector3 GetNodePivot => spawnTransform.position;
    public IReadOnlyList<IReadOnlyList<Node>> GetAllNode => nodeArray;
    public static Node GetNodeFromArray(int x, int y)
    {
        Node result;
        bool flagValid = IsNodePositionValid(x, y);
        result = flagValid ?
            nodeArray[y][x] :
            null;

        return result;
    }
    #endregion

    private void OnEnable()
    {
        const int k_row = 3;//15;
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
    public static bool IsNodePositionValid(Node node)
    {
        return IsNodePositionValid(node.X, node.Y);
    }
    public static bool IsNodePositionValid(int x, int y)
    {
        bool result = y >= 0 &&
            y < nodeArray.Length &&
            x >= 0 &&
            x < nodeArray[y].Length;
        return result;
    }
    public static void SetNode(int x, int y, Node node)
    {
        nodeArray[y][x] = node;
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
            nodeArray[arrayIndex] = new Node[item];
            arrayLength += item;

            arrayIndex++;
        }

        Debug.Assert(arrayLength != 0, "length is zero");

        AsyncInstantiateOperation<Node> task = InstantiateAsync(nodePrefab, arrayLength, transform.position, Quaternion.identity);

        await task;

        int x = 0;
        int y = 0;

        Node[] nodes = task.Result;
        allNodeArr = nodes;

        Debug.Assert(nodes.Length == arrayLength, "array length doesn't match");

        Vector3 spawnPosition = GetNodePivot;
        for (int i = 0; i < arrayLength; i++)
        {
            if (x >= nodeArray[y].Length)
            {
                y++;
                x = 0;
            }

            await Awaitable.WaitForSecondsAsync(0.013f);

            Node item = nodes[i];
            item.name = i.ToString(); // todo : remove

            Vector3 position = new Vector3(x, y);
            item.SetPosition(x, y, position + spawnPosition);
            item.Initialization();

            nodeArray[y][x] = item;

            const int k_min = (int)EColor.None + 1;
            const int k_max = (int)EColor.End;
            EColor color = (EColor)Random.Range(k_min, k_max);
            item.SetColor(color);

            x++;
        }
    }
    //todo : incomplete
    public void KillNode(int x, int y)
    {
        Node a = nodeArray[y][x];
        a.gameObject.SetActive(false);
    }
    private void KillAllNode()
    {
        for (int i = 0; i < allNodeArr.Length; i++)
        {
            ref Node currentNode = ref allNodeArr[i];
            if (currentNode == null) continue;
            Destroy(currentNode.gameObject);
            currentNode = null;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        //todo : early exit when quitting
        if (allNodeArr != null)
        {
            KillAllNode();
        }
    }
}

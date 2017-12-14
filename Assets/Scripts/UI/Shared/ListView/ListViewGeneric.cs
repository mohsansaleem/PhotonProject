using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(ScrollRect))]
/// <summary>
/// List view generic.
/// </summary>
public class ListViewGeneric<M, V> : MonoBehaviour where V : ListNodeGeneric<M>
{
    [Header("ListView References")]
    [SerializeField]
    [Tooltip("ListView Reference.")]
    CanvasGroup canvasGroup;
    [SerializeField]
    [Tooltip("Container of the Rows.")]
    ScrollRect scrollView;
    [SerializeField]
    [Tooltip("Prefab for the ListRow.")]
    V rowPrefab;

    [Header("ListView Properties")]
    [Tooltip("Uncheck to remove the Views that are pooled from the previous list.")]
    [SerializeField]
    bool shouldPoolExtraRows;
    [SerializeField]
    [Tooltip("Check it to add the new row to the start.")]
    bool addNewToStart;
    [SerializeField]
    [Range(0, 500)]
    [Tooltip("Limit the total number of nodes. Set sezo for infinite.")]
    int nodesLimit;

    List<M> dataList = new List<M>();
    List<V> rowsViews = new List<V>();
    List<V> rowsPool = new List<V>();


    [ContextMenu("Optimise")]
    void Optimise()
    {
        while (dataList.Count < rowsViews.Count)
            DestroyViewNodeAt(addNewToStart ? 0 : dataList.Count);
    }

    public void DestroyViewNodeAt(int index)
    {
        Destroy(rowsViews[index].gameObject);
        rowsViews.RemoveAt(index);
    }

    public V AddNewRow(M rowData, bool addToStart = false)
    {

        V newRoomRow = null;
        if (rowsPool.Count > 0)
        {
            newRoomRow = rowsPool[0];
            rowsPool.RemoveAt(0);
            newRoomRow.Active = true;
        }
        else
            newRoomRow = Instantiate(rowPrefab, scrollView.content);

        newRoomRow.Data = rowData;
        rowsViews.Add(newRoomRow);

        if (addToStart)
            newRoomRow.transform.SetAsFirstSibling();

        // Applying Limit.
        if (nodesLimit != 0 && rowsViews.Count > nodesLimit)
        {
            DestroyViewNodeAt(addToStart ? rowsViews.Count - 1 : 0);
            DataList.RemoveAt(addToStart ? DataList.Count - 1 : 0);
        }

        return newRoomRow;
    }

    public void RemoveNodeAt(int index)
    {
        if (shouldPoolExtraRows)
        {
            rowsPool.Add(rowsViews[index]);
            rowsViews[index].Active = false;
            rowsViews.RemoveAt(index);
        }
        else
        {
            DestroyViewNodeAt(index);
            rowsViews.RemoveAt(index);
        }
    }

    #region Properties

    public List<M> DataList
    {
        get
        {
            return dataList;
        }

        set
        {
            dataList = value;

            int childCount = rowsViews.Count;
            int inputRoomsCount = dataList == null ? 0 : dataList.Count;

            int index;

            // Setting Data to Existing Rows.
            for (index = 0; index < childCount && index < inputRoomsCount && (nodesLimit == 0 || index < nodesLimit); index++)
            {
                rowsViews[index].Data = dataList[index];
                rowsViews[index].Active = true;
            }

            // Adding Remaining Rows.
            for (; index < inputRoomsCount && (nodesLimit == 0 || index < nodesLimit); index++)
                AddNewRow(dataList[index], addNewToStart);


            if (shouldPoolExtraRows)
            {
                // Pooling the Extra Rows.
                for (; index < childCount; index++)
                {
                    rowsPool.Add(rowsViews[index]);
                    rowsViews[index].Active = false;
                    rowsViews.RemoveAt(index);
                }
            }
            else
                Optimise();


        }
    }

    public bool Visible
    {
        get
        {
            return canvasGroup.alpha == 1f;
        }

        set
        {
            canvasGroup.alpha = (value ? 1f : 0f);
        }
    }

    public bool Enabled
    {
        get
        {
            return canvasGroup.interactable;
        }
        set
        {
            canvasGroup.interactable = value;
        }
    }
    #endregion Properties
}

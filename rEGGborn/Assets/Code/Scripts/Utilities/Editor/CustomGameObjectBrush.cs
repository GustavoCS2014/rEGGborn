#region Assembly Unity.2D.Tilemap.Extras.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// D:\Unity Projects\Github\rEGGborn\rEGGborn\Library\ScriptAssemblies\Unity.2D.Tilemap.Extras.Editor.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[HelpURL("https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@latest/index.html?subfolder=/manual/GameObjectBrush.html")]
[CustomGridBrush(true, false, false, "GameObject Brush")]
public class GameObjectBrush : GridBrushBase
{
    [Serializable]
    internal class HiddenGridLayout
    {
        public Vector3 cellSize = Vector3.one;

        public Vector3 cellGap = Vector3.zero;

        public GridLayout.CellLayout cellLayout;

        public GridLayout.CellSwizzle cellSwizzle;
    }

    [Serializable]
    public class BrushCell
    {
        [SerializeField]
        private GameObject m_GameObject;

        [SerializeField]
        private Vector3 m_Offset = Vector3.zero;

        [SerializeField]
        private Vector3 m_Scale = Vector3.one;

        [SerializeField]
        private Quaternion m_Orientation = Quaternion.identity;

        public GameObject gameObject
        {
            get
            {
                return m_GameObject;
            }
            set
            {
                m_GameObject = value;
            }
        }

        public Vector3 offset
        {
            get
            {
                return m_Offset;
            }
            set
            {
                m_Offset = value;
            }
        }

        public Vector3 scale
        {
            get
            {
                return m_Scale;
            }
            set
            {
                m_Scale = value;
            }
        }

        public Quaternion orientation
        {
            get
            {
                return m_Orientation;
            }
            set
            {
                m_Orientation = value;
            }
        }

        public override int GetHashCode()
        {
            return ((((gameObject != null) ? gameObject.GetInstanceID() : 0) * 33 + offset.GetHashCode()) * 33 + scale.GetHashCode()) * 33 + orientation.GetHashCode();
        }


    }

    [SerializeField, HideInInspector]
    private BrushCell[] m_Cells;

    [SerializeField, HideInInspector]
    private Vector3Int m_Size;

    [SerializeField, HideInInspector]
    private Vector3Int m_Pivot;

    [SerializeField]
    [HideInInspector]
    private bool m_CanChangeZPosition;

    [SerializeField]
    [HideInInspector]
    internal HiddenGridLayout hiddenGridLayout = new HiddenGridLayout();

    [HideInInspector]
    public GameObject hiddenGrid;

    [HideInInspector]
    public Vector3 m_Anchor = new Vector3(0.5f, 0.5f, 0f);

    public Vector3Int size
    {
        get
        {
            return m_Size;
        }
        set
        {
            m_Size = value;
            SizeUpdated();
        }
    }

    public Vector3Int pivot
    {
        get
        {
            return m_Pivot;
        }
        set
        {
            m_Pivot = value;
        }
    }

    public BrushCell[] cells => m_Cells;

    public int cellCount
    {
        get
        {
            if (m_Cells == null)
            {
                return 0;
            }

            return m_Cells.Length;
        }
    }

    public int sizeCount => m_Size.x * m_Size.y * m_Size.z;

    public bool canChangeZPosition
    {
        get
        {
            return m_CanChangeZPosition;
        }
        set
        {
            m_CanChangeZPosition = value;
        }
    }

    public GameObjectBrush()
    {
        Init(Vector3Int.one, Vector3Int.zero);
        SizeUpdated();
    }

    private void OnEnable()
    {
        hiddenGrid = new GameObject();
        hiddenGrid.name = "(Paint on SceneRoot)";
        hiddenGrid.hideFlags = HideFlags.HideAndDontSave;
        hiddenGrid.transform.position = Vector3.zero;
        Grid grid = hiddenGrid.AddComponent<Grid>();
        grid.cellSize = hiddenGridLayout.cellSize;
        grid.cellGap = hiddenGridLayout.cellGap;
        grid.cellSwizzle = hiddenGridLayout.cellSwizzle;
        grid.cellLayout = hiddenGridLayout.cellLayout;
    }

    private void OnDisable()
    {
        UnityEngine.Object.DestroyImmediate(hiddenGrid);
    }

    private void OnValidate()
    {
        if (m_Size.x < 0)
        {
            m_Size.x = 0;
        }

        if (m_Size.y < 0)
        {
            m_Size.y = 0;
        }

        if (m_Size.z < 0)
        {
            m_Size.z = 0;
        }
    }

    public void Init(Vector3Int size)
    {
        Init(size, Vector3Int.zero);
        SizeUpdated();
    }

    public void Init(Vector3Int size, Vector3Int pivot)
    {
        m_Size = size;
        m_Pivot = pivot;
        SizeUpdated();
    }

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        Vector3Int position2 = position - pivot;
        BoundsInt position3 = new BoundsInt(position2, m_Size);
        BoxFill(gridLayout, brushTarget, position3);
    }

    private void PaintCell(GridLayout grid, Vector3Int position, Transform parent, BrushCell cell)
    {
        if (!(cell.gameObject == null) && GetObjectInCell(grid, parent, position, m_Anchor, cell.offset) == null)
        {
            SetSceneCell(grid, parent, position, cell.gameObject, cell.offset, cell.scale, cell.orientation, m_Anchor);
        }
    }

    public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        Vector3Int position2 = position - pivot;
        BoundsInt position3 = new BoundsInt(position2, m_Size);
        GetGrid(ref gridLayout, ref brushTarget);
        BoxErase(gridLayout, brushTarget, position3);
    }

    private void EraseCell(GridLayout grid, Vector3Int position, Transform parent, BrushCell cell)
    {
        ClearSceneCell(grid, parent, position, cell);
    }

    public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
        GetGrid(ref gridLayout, ref brushTarget);
        foreach (Vector3Int item in position.allPositionsWithin)
        {
            Vector3Int vector3Int = item - position.min;
            BrushCell cell = m_Cells[GetCellIndexWrapAround(vector3Int.x, vector3Int.y, vector3Int.z)];
            PaintCell(gridLayout, item, (brushTarget != null) ? brushTarget.transform : null, cell);
        }
    }

    public override void BoxErase(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
        GetGrid(ref gridLayout, ref brushTarget);
        foreach (Vector3Int item in position.allPositionsWithin)
        {
            Vector3Int vector3Int = item - position.min;
            BrushCell cell = m_Cells[GetCellIndexWrapAround(vector3Int.x, vector3Int.y, vector3Int.z)];
            EraseCell(gridLayout, item, (brushTarget != null) ? brushTarget.transform : null, cell);
        }
    }

    public override void FloodFill(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        Debug.LogWarning("FloodFill not supported");
    }

    public override void Rotate(RotationDirection direction, GridLayout.CellLayout layout)
    {
        Vector3Int vector3Int = m_Size;
        BrushCell[] array = m_Cells.Clone() as BrushCell[];
        size = new Vector3Int(vector3Int.y, vector3Int.x, vector3Int.z);
        foreach (Vector3Int item in new BoundsInt(Vector3Int.zero, vector3Int).allPositionsWithin)
        {
            int x = ((direction == RotationDirection.Clockwise) ? (vector3Int.y - item.y - 1) : item.y);
            int y = ((direction == RotationDirection.Clockwise) ? item.x : (vector3Int.x - item.x - 1));
            int cellIndex = GetCellIndex(x, y, item.z);
            int cellIndex2 = GetCellIndex(item.x, item.y, item.z, vector3Int.x, vector3Int.y, vector3Int.z);
            m_Cells[cellIndex] = array[cellIndex2];
        }

        int x2 = ((direction == RotationDirection.Clockwise) ? (vector3Int.y - pivot.y - 1) : pivot.y);
        int y2 = ((direction == RotationDirection.Clockwise) ? pivot.x : (vector3Int.x - pivot.x - 1));
        pivot = new Vector3Int(x2, y2, pivot.z);
        Quaternion quaternion = Quaternion.Euler(0f, 0f, (direction != 0) ? 90f : (-90f));
        BrushCell[] array2 = m_Cells;
        for (int i = 0; i < array2.Length; i++)
        {
            array2[i].orientation *= quaternion;
        }
    }

    public override void Flip(FlipAxis flip, GridLayout.CellLayout layout)
    {
        if (flip == FlipAxis.X)
        {
            FlipX();
        }
        else
        {
            FlipY();
        }
    }

    public override void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pivot)
    {
        Reset();
        UpdateSizeAndPivot(new Vector3Int(position.size.x, position.size.y, 1), new Vector3Int(pivot.x, pivot.y, 0));
        GetGrid(ref gridLayout, ref brushTarget);
        foreach (Vector3Int item in position.allPositionsWithin)
        {
            PickCell(brushPosition: new Vector3Int(item.x - position.x, item.y - position.y, 0), position: item, grid: gridLayout, parent: (brushTarget != null) ? brushTarget.transform : null, withoutAnchor: true);
        }
    }

    private void PickCell(Vector3Int position, Vector3Int brushPosition, GridLayout grid, Transform parent, bool withoutAnchor = false)
    {
        GameObject gameObject = null;
        if (!withoutAnchor)
        {
            gameObject = GetObjectInCell(grid, parent, position, m_Anchor, Vector3.zero);
        }

        if (gameObject == null)
        {
            gameObject = GetObjectInCell(grid, parent, position, Vector3.zero, Vector3.zero);
        }

        Vector3 anchorRatio = GetAnchorRatio(grid, m_Anchor);
        Vector3 vector = grid.CellToLocalInterpolated(position);
        Vector3 vector2 = grid.CellToLocalInterpolated(anchorRatio);
        Vector3 vector3 = grid.LocalToWorld(vector + vector2);
        if (gameObject != null)
        {
            UnityEngine.Object correspondingObjectFromSource = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
            if ((bool)correspondingObjectFromSource)
            {
                SetGameObject(brushPosition, (GameObject)correspondingObjectFromSource);
            }
            else
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
                gameObject2.hideFlags = HideFlags.HideAndDontSave;
                gameObject2.SetActive(value: false);
                SetGameObject(brushPosition, gameObject2);
            }

            SetOffset(brushPosition, gameObject.transform.position - vector3);
            SetScale(brushPosition, gameObject.transform.localScale);
            SetOrientation(brushPosition, gameObject.transform.localRotation);
        }
    }

    public override void MoveStart(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
        Reset();
        UpdateSizeAndPivot(new Vector3Int(position.size.x, position.size.y, 1), Vector3Int.zero);
        GetGrid(ref gridLayout, ref brushTarget);
        Transform parent = ((brushTarget != null) ? brushTarget.transform : null);
        foreach (Vector3Int item in position.allPositionsWithin)
        {
            Vector3Int brushPosition = new Vector3Int(item.x - position.x, item.y - position.y, 0);
            PickCell(item, brushPosition, gridLayout, parent);
            BrushCell cell = m_Cells[GetCellIndex(brushPosition)];
            ClearSceneCell(gridLayout, parent, item, cell);
        }
    }

    public override void MoveEnd(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
    {
        GetGrid(ref gridLayout, ref brushTarget);
        Paint(gridLayout, brushTarget, position.min);
        Reset();
    }

    private void GetGrid(ref GridLayout gridLayout, ref GameObject brushTarget)
    {
        if (brushTarget == hiddenGrid)
        {
            brushTarget = null;
        }

        if (brushTarget != null)
        {
            GridLayout component = brushTarget.GetComponent<GridLayout>();
            if (component != null)
            {
                gridLayout = component;
            }
        }
    }

    public void Reset()
    {
        BrushCell[] array = m_Cells;
        foreach (BrushCell brushCell in array)
        {
            if (brushCell.gameObject != null && !EditorUtility.IsPersistent(brushCell.gameObject))
            {
                UnityEngine.Object.DestroyImmediate(brushCell.gameObject);
            }

            brushCell.gameObject = null;
        }

        UpdateSizeAndPivot(Vector3Int.one, Vector3Int.zero);
    }

    private void FlipX()
    {
        BrushCell[] array = m_Cells.Clone() as BrushCell[];
        foreach (Vector3Int item in new BoundsInt(Vector3Int.zero, m_Size).allPositionsWithin)
        {
            int x = m_Size.x - item.x - 1;
            int cellIndex = GetCellIndex(x, item.y, item.z);
            int cellIndex2 = GetCellIndex(item);
            m_Cells[cellIndex] = array[cellIndex2];
        }

        int x2 = m_Size.x - pivot.x - 1;
        pivot = new Vector3Int(x2, pivot.y, pivot.z);
        FlipCells(ref m_Cells, new Vector3(-1f, 1f, 1f));
    }

    private void FlipY()
    {
        BrushCell[] array = m_Cells.Clone() as BrushCell[];
        foreach (Vector3Int item in new BoundsInt(Vector3Int.zero, m_Size).allPositionsWithin)
        {
            int y = m_Size.y - item.y - 1;
            int cellIndex = GetCellIndex(item.x, y, item.z);
            int cellIndex2 = GetCellIndex(item);
            m_Cells[cellIndex] = array[cellIndex2];
        }

        int y2 = m_Size.y - pivot.y - 1;
        pivot = new Vector3Int(pivot.x, y2, pivot.z);
        FlipCells(ref m_Cells, new Vector3(1f, -1f, 1f));
    }

    private static void FlipCells(ref BrushCell[] cells, Vector3 scale)
    {
        BrushCell[] array = cells;
        foreach (BrushCell obj in array)
        {
            obj.scale = Vector3.Scale(obj.scale, scale);
        }
    }

    public void UpdateSizeAndPivot(Vector3Int size, Vector3Int pivot)
    {
        m_Size = size;
        m_Pivot = pivot;
        SizeUpdated();
    }

    public void SetGameObject(Vector3Int position, GameObject go)
    {
        if (ValidateCellPosition(position))
        {
            m_Cells[GetCellIndex(position)].gameObject = go;
        }
    }

    public void SetOffset(Vector3Int position, Vector3 offset)
    {
        if (ValidateCellPosition(position))
        {
            m_Cells[GetCellIndex(position)].offset = offset;
        }
    }

    public void SetOrientation(Vector3Int position, Quaternion orientation)
    {
        if (ValidateCellPosition(position))
        {
            m_Cells[GetCellIndex(position)].orientation = orientation;
        }
    }

    public void SetScale(Vector3Int position, Vector3 scale)
    {
        if (ValidateCellPosition(position))
        {
            m_Cells[GetCellIndex(position)].scale = scale;
        }
    }

    public void SetBrushCell(BrushCell[] cells)
    {
        m_Cells = cells;
    }

    public int GetCellIndex(Vector3Int brushPosition)
    {
        return GetCellIndex(brushPosition.x, brushPosition.y, brushPosition.z);
    }

    public int GetCellIndex(int x, int y, int z)
    {
        return x + m_Size.x * y + m_Size.x * m_Size.y * z;
    }

    public int GetCellIndex(int x, int y, int z, int sizex, int sizey, int sizez)
    {
        return x + sizex * y + sizex * sizey * z;
    }

    public int GetCellIndexWrapAround(int x, int y, int z)
    {
        return x % m_Size.x + m_Size.x * (y % m_Size.y) + m_Size.x * m_Size.y * (z % m_Size.z);
    }

    private GameObject GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position, Vector3 anchor, Vector3 offset)
    {
        GameObject[] array = null;
        int num;
        if (parent == null)
        {
            Scene activeScene = SceneManager.GetActiveScene();
            array = activeScene.GetRootGameObjects();
            num = activeScene.rootCount;
        }
        else
        {
            num = parent.childCount;
        }

        Vector3 anchorRatio = GetAnchorRatio(grid, anchor);
        Vector3 vector = grid.CellToLocalInterpolated(anchorRatio);
        for (int i = 0; i < num; i++)
        {
            Transform transform = ((array == null) ? parent.GetChild(i) : array[i].transform);
            Vector3Int vector3Int = grid.LocalToCell(grid.WorldToLocal(transform.position) - vector - offset);
            if (position == vector3Int)
            {
                return transform.gameObject;
            }
        }

        return null;
    }

    private bool ValidateCellPosition(Vector3Int position)
    {
        if (position.x < 0 || position.x >= size.x || position.y < 0 || position.y >= size.y || position.z < 0 || position.z >= size.z)
        {
            throw new ArgumentException($"Position {position} is an invalid cell position. Valid range is between [{Vector3Int.zero}, {size}).");
        }

        return true;
    }

    internal void SizeUpdated(bool keepContents = false)
    {
        OnValidate();
        Array.Resize(ref m_Cells, sizeCount);
        foreach (Vector3Int item in new BoundsInt(Vector3Int.zero, m_Size).allPositionsWithin)
        {
            if (keepContents || m_Cells[GetCellIndex(item)] == null)
            {
                m_Cells[GetCellIndex(item)] = new BrushCell();
            }
        }
    }

    private static void SetSceneCell(GridLayout grid, Transform parent, Vector3Int position, GameObject go, Vector3 offset, Vector3 scale, Quaternion orientation, Vector3 anchor)
    {
        if (go == null)
        {
            return;
        }

        GameObject gameObject;
        if (PrefabUtility.IsPartOfPrefabAsset(go))
        {
            gameObject = (GameObject)PrefabUtility.InstantiatePrefab(go, (parent != null) ? parent.root.gameObject.scene : SceneManager.GetActiveScene());
            gameObject.transform.parent = parent;
        }
        else
        {
            gameObject = UnityEngine.Object.Instantiate(go, parent);
            gameObject.name = go.name;
            gameObject.SetActive(value: true);
            Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].enabled = true;
            }
        }

        gameObject.hideFlags = HideFlags.None;
        Undo.RegisterCreatedObjectUndo(gameObject, "Paint GameObject");
        Vector3 anchorRatio = GetAnchorRatio(grid, anchor);
        gameObject.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(position) + grid.CellToLocalInterpolated(anchorRatio));
        gameObject.transform.localRotation = orientation;
        gameObject.transform.localScale = scale;
        gameObject.transform.Translate(offset);
    }

    private static Vector3 GetAnchorRatio(GridLayout grid, Vector3 cellAnchor)
    {
        Vector3 cellSize = grid.cellSize;
        Vector3 vector = cellSize + grid.cellGap;
        vector.x = (Mathf.Approximately(0f, vector.x) ? 1f : vector.x);
        vector.y = (Mathf.Approximately(0f, vector.y) ? 1f : vector.y);
        vector.z = (Mathf.Approximately(0f, vector.z) ? 1f : vector.z);
        return new Vector3(cellAnchor.x * cellSize.x / vector.x, cellAnchor.y * cellSize.y / vector.y, cellAnchor.z * cellSize.z / vector.z);
    }

    private void ClearSceneCell(GridLayout grid, Transform parent, Vector3Int position, BrushCell cell)
    {
        GameObject objectInCell = GetObjectInCell(grid, parent, position, m_Anchor, cell.offset);
        if (objectInCell != null)
        {
            Undo.DestroyObjectImmediate(objectInCell);
        }
    }

    public override int GetHashCode()
    {
        int num = 0;
        BrushCell[] array = cells;
        foreach (BrushCell brushCell in array)
        {
            num = num * 33 + brushCell.GetHashCode();
        }

        return num;
    }

    internal void UpdateHiddenGridLayout()
    {
        Grid component = hiddenGrid.GetComponent<Grid>();
        hiddenGridLayout.cellSize = component.cellSize;
        hiddenGridLayout.cellGap = component.cellGap;
        hiddenGridLayout.cellSwizzle = component.cellSwizzle;
        hiddenGridLayout.cellLayout = component.cellLayout;
    }
}
#if false // Decompilation log
'285' items in cache
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Data\NetStandard\compat\2.1.0\shims\netfx\mscorlib.dll'
------------------
Resolve: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Data\NetStandard\compat\2.1.0\shims\netfx\System.dll'
------------------
Resolve: 'UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Data\Managed\UnityEngine\UnityEditor.CoreModule.dll'
------------------
Resolve: 'Unity.2D.Tilemap.Extras, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Unity.2D.Tilemap.Extras, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'D:\Unity Projects\Github\rEGGborn\rEGGborn\Library\ScriptAssemblies\Unity.2D.Tilemap.Extras.dll'
------------------
Resolve: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll'
------------------
Resolve: 'UnityEngine.TilemapModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.TilemapModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Data\Managed\UnityEngine\UnityEngine.TilemapModule.dll'
------------------
Resolve: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Data\NetStandard\compat\2.1.0\shims\netfx\System.Core.dll'
------------------
Resolve: 'UnityEngine.IMGUIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.IMGUIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Data\Managed\UnityEngine\UnityEngine.IMGUIModule.dll'
------------------
Resolve: 'UnityEngine.GridModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.GridModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Data\Managed\UnityEngine\UnityEngine.GridModule.dll'
------------------
Resolve: 'Unity.2D.Tilemap.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Unity.2D.Tilemap.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'D:\Unity Projects\Github\rEGGborn\rEGGborn\Library\ScriptAssemblies\Unity.2D.Tilemap.Editor.dll'
------------------
Resolve: 'UnityEngine.TextRenderingModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.TextRenderingModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Data\Managed\UnityEngine\UnityEngine.TextRenderingModule.dll'
------------------
Resolve: 'UnityEngine.ImageConversionModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.ImageConversionModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Data\Managed\UnityEngine\UnityEngine.ImageConversionModule.dll'
------------------
Resolve: 'Microsoft.Win32.Registry, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'Microsoft.Win32.Registry, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Data\NetStandard\ref\2.1.0\netstandard.dll'
------------------
Resolve: 'System.Security.Principal.Windows, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Security.Principal.Windows, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'System.Security.AccessControl, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Security.AccessControl, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Could not find by name: 'System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
#endif

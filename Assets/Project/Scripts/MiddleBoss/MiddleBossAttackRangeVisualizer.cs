using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MiddleBossAttackRangeVisualizer : MonoBehaviour
{
    public Color color = new Color(1f, 0f, 0f, 0.7f);
    public string showState = "Attack";
    public Animator animator;
    public MiddleBossMove middleBossMove;
    PolygonCollider2D poly;
    MeshFilter mf;
    MeshRenderer mr;

    private Vector2[] originalPoints;       // 元のポリゴンコライダー頂点を保持

    void Awake()
    {
        poly = GetComponent<PolygonCollider2D>();
        if (poly == null) Debug.Log("nullや");

        // 可視化用の子オブジェクトを生成
        var go = new GameObject("PolyVis");         // 可視化用の子オブジェクトを作る
        go.transform.SetParent(transform, false);   // 親と同じローカル空間で扱う
        mf = go.AddComponent<MeshFilter>();
        mr = go.AddComponent<MeshRenderer>();

        var mat = new Material(Shader.Find("Standard"));     // 半透明にするためにSprites/Defaultを使う
        mat.color = color;

        // Standard シェーダを透明モードに切り替え
        mat.SetFloat("_Mode", 3);                                   // Transparent モード（半透明で描画される）
        mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);           // ScrAlpha：マテリアルの色のα値
        mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);   // ブレンドの背景の色の係数を指定。OneMinusSrcAlpha：1 - α の値
        mat.SetInt("_ZWrite", 0);                                   // 奥行きを無効化
        mat.DisableKeyword("_ALPHATEST_ON");                        // ALPHATEST：透明かどうかゼロイチ判定、今回は無効
        mat.EnableKeyword("_ALPHABLEND_ON");                        // ALPHABLEND：前景と背景を混ぜる、半透明を可能にするフラグ
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");                 // 特殊なアルファブレンドを無効化
        mat.renderQueue = 3000;                                     // 描画順番を指定。3000 = Transparent（半透明用）

        // カリングオフにしたい場合、下のコードを有効化
        //mat.SetInt("_Cull", (int)CullMode.Off);

        mr.sharedMaterial = mat;
        mr.shadowCastingMode = ShadowCastingMode.Off;
        mr.receiveShadows = false;
        mr.enabled = false;             // 初期は非表示（アニメに合わせて表示）        
    }

    void Start()
    {
        if (poly != null && poly.pathCount > 0)
            originalPoints = poly.GetPath(0);           // 最初の形を保存

        RebuildMesh();
    }

    void Update()
    {
        if (mr == null || animator == null)
        {
            Debug.Log("nullじゃな");
            return;
        }

        mr.enabled = animator.GetCurrentAnimatorStateInfo(0).IsName(showState);     // アニメーションが再生されてる時はtrue
        if (mr.enabled)
            Debug.Log("表示中");
        else if (!mr.enabled)
            Debug.Log("非表示");

        UpdateColliderAndMesh(middleBossMove.flip);
    }

    // Meshを作る
    public void RebuildMesh()
    {
        if (poly == null || mf == null)
            return;
        if (poly.pathCount == 0)
            return;

        Vector2[] pts = poly.GetPath(0);        // 頂点配列を取得

        // poly.offsetを反映（必要なら）
        Vector2 offset = poly.offset;
        for (int i = 0; i < pts.Length; i++)
            pts[i] += offset;

        pts = MakeClockwise(pts);               // 頂点を時計回りに直す

        // originalPointsを更新して保存する
        //originalPoints = pts;

        //Mesh mesh = poly.CreateMesh(true, false);
        // CreatMeshがうまくいかなかったから手動で変換
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[pts.Length];
        for (int i = 0; i < pts.Length; i++)
            vertices[i] = new Vector3(pts[i].x, pts[i].y, 0.01f);       // ポリゴンの頂点座標を取得

        // 塗る三角形を作る
        List<int> tris = Triangulate(pts);
        if (tris == null || tris.Count < 3)
        {
            Debug.LogWarning("生成されてない、もしくは三角形がない");
            return;
        }

        mesh.vertices = vertices;
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();          // 各頂点ごとの法線ベクトルを再計算する（Meshの面がどの向きかを判定する）
        mesh.RecalculateBounds();           // Meshの境界ボックスを再計算する（描画最適化や当たり判定用の境界ボックス）
                                            // 手動でMeshを生成した時、この2つを書かないと正しく表示されないことがある。

        // 差し替え
        if (mf.sharedMesh != null)
            Destroy(mf.sharedMesh);     // メモリリーク防止
        mf.sharedMesh = mesh;


        Debug.Log("頂点数：" + mesh.vertexCount);
        Debug.Log("三角形数：" + mesh.triangles.Length / 3);
    }

    // MiddleBossの向きに合わせてMeshとColliderの向きを変える
    private void UpdateColliderAndMesh(bool flipX)
    {
        Vector2[] currentPoints = new Vector2[originalPoints.Length];
        for (int i = 0; i < originalPoints.Length; i++)
        {
            Vector2 p = originalPoints[i];
            if (flipX)
                p.x = -p.x;
            currentPoints[i] = p;
        }

        // 向きを変えた頂点を時計回りにする
        currentPoints = MakeClockwise(currentPoints);

        // コライダーを更新
        poly.SetPath(0, currentPoints);

        // メッシュを更新
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[currentPoints.Length];
        for (int i = 0; i < currentPoints.Length; i++)
            vertices[i] = new Vector3(currentPoints[i].x, currentPoints[i].y, 0.01f);

        List<int> tris = Triangulate(currentPoints);
        mesh.vertices = vertices;
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // 差し替え
        if (mf.sharedMesh != null)
            Destroy(mf.sharedMesh);     // メモリリーク防止
        mf.sharedMesh = mesh;
    }

    List<int> Triangulate(Vector2[] points)
    {
        List<int> indices = new List<int>();
        List<int> verts = new List<int>();
        for (int i = 0; i < points.Length; i++)
            verts.Add(i);

        int safety = 0;
        while (verts.Count > 3 && safety++ < 10000)
        {
            bool earFound = false;
            for (int i = 0; i < verts.Count; i++)
            {
                int prev = verts[(i - 1 + verts.Count) % verts.Count];
                int curr = verts[i];
                int next = verts[(i + 1) % verts.Count];

                if (!InConvex(points[prev], points[curr], points[next]))
                    continue;

                // 内側に点が含まれてないかチェック
                bool hasPointInside = false;
                for (int j = 0; j < verts.Count; j++)
                {
                    if (verts[j] == prev || verts[j] == curr || verts[j] == next)
                        continue;
                    if (PointInTriangle(points[verts[j]], points[prev], points[curr], points[next]))
                    {
                        hasPointInside = true;
                        break;
                    }
                }

                if (!hasPointInside)
                {
                    indices.Add(prev);
                    indices.Add(curr);
                    indices.Add(next);
                    verts.RemoveAt(i);
                    earFound = true;
                    break;
                }

            }

            if (!earFound)
            {
                Debug.LogWarning("耳が見つからなかった->頂点順序や凹みの可能性あり");
                break;
            }
        }

        if (verts.Count == 3)
        {
            indices.Add(verts[0]);
            indices.Add(verts[1]);
            indices.Add(verts[2]);
        }

        return indices;
    }

    // 3点が凸かどうか
    private static bool InConvex(Vector2 a, Vector2 b, Vector2 c)
    {
        return Vector3.Cross(b - a, c - b).z < 0f;
    }

    // 点が三角形の内部にあるか
    private static bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
        float s1 = Sign(p, a, b);
        float s2 = Sign(p, b, c);
        float s3 = Sign(p, c, a);

        bool b1 = s1 < 0.0f;
        bool b2 = s2 < 0.0f;
        bool b3 = s3 < 0.0f;

        return (b1 == b2) && (b2 == b3);
    }

    // 計算用関数
    private static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }

    // PolygonCollider2Dの頂点を時計回りにする
    private Vector2[] MakeClockwise(Vector2[] pts)
    {
        // 時計回りチェック
        float sum = 0f;
        for (int i = 0; i < pts.Length; i++)
        {
            Vector2 p1 = pts[i];
            Vector2 p2 = pts[(i + 1) % pts.Length];
            sum += (p1.x * p2.y) - (p2.x * p1.y);
        }
        // 反時計周りの時は反転
        if (sum > 0)
        {
            System.Array.Reverse(pts);
        }
        return pts;
    }


    // Meshの表示/非表示のタイミング
    public void ShowMesh()
    {
        mr.enabled = true;
    }
    public void HideMesh()
    {
        mr.enabled = false;
    }
}

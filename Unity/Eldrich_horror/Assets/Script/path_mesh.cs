using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class path_mesh : MonoBehaviour {

    public List<Vector3> _Vertex = new List<Vector3>();
    public List<int> _Tri = new List<int>();
    public List<Vector2> _UV = new List<Vector2>();
    private Mesh mesh;

    //Cell간의 Gap입니다. 5by5짜리 텍스쳐니까 간단히 1/5 = 0.2!
    private float _cellGap = 0.2f;
    //cell들의 Index를 저장합니다.
    public List<Vector2> _cell = new List<Vector2>();

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;


        _Vertex.Add(new Vector3(x- 0.5f, y, z + 0.5f));
        _Vertex.Add(new Vector3(x + 0.5f, y, z + 0.5f));
        _Vertex.Add(new Vector3(x + 0.5f, y , z - 0.5f));
        _Vertex.Add(new Vector3(x- 0.5f, y , z - 0.5f));

        _Tri.Add(0);
        _Tri.Add(1);
        _Tri.Add(3);
        _Tri.Add(1);
        _Tri.Add(2);
        _Tri.Add(3);

        //셀은 2개만 사용할겁니다. 0,0에 있는 검은색과 0,1에 있는 흰색입니다.
        _cell.Add(new Vector2(0, 0));
        _cell.Add(new Vector2(0, 1));

        //텍스쳐의 좌표를 UV에 넣습니다.
        //만약 List 인덱스 0번이라면 (0,0), (0.2,0), (0, 0.2), (0.2, 0.2)의 값을 가지겠죠?
        _UV.Add(new Vector2(_cellGap * _cell[1].x, _cellGap * _cell[1].y + _cellGap));
        _UV.Add(new Vector2(_cellGap * _cell[1].x + _cellGap, _cellGap * _cell[1].y + _cellGap));
        _UV.Add(new Vector2(_cellGap * _cell[1].x + _cellGap, _cellGap * _cell[1].y));
        _UV.Add(new Vector2(_cellGap * _cell[1].x, _cellGap * _cell[1].y));

        mesh.Clear();
        mesh.vertices = _Vertex.ToArray();
        mesh.triangles = _Tri.ToArray();
        //UV값을 넣습니다.
        mesh.uv = _UV.ToArray();
        ;
        mesh.RecalculateNormals();


    }
}

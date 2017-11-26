using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Make_Path : MonoBehaviour {
    public BezierPath path = null;
    public List<Vector3> _Vertex = new List<Vector3>();
    public List<int> _Tri = new List<int>();
    public List<Vector2> _UV = new List<Vector2>();
    private Mesh mesh;

    //Cell간의 Gap입니다. 5by5짜리 텍스쳐니까 간단히 1/5 = 0.2!
    private float _cellGap = 0.2f;
    //cell들의 Index를 저장합니다.
    public List<Vector2> _cell = new List<Vector2>();

    // Use this for initialization
    void Start () {
        mesh = GetComponent<MeshFilter>().mesh;

        float t = 0;
        Vector3 temp = new Vector3();
        Vector3 temp2 = new Vector3();
        Vector3 anglevector = new Vector3();
        Vector3 up = new Vector3(0,0,0.8f);
        Vector3 down = new Vector3(0,0,-0.8f);
        Quaternion v3Rotation = Quaternion.Euler(0f, -90f, 0f);  // 회전각 
        Quaternion r_v3Rotation = Quaternion.Euler(0f, 90f, 0f);  // 회전각

        //셀은 2개만 사용할겁니다. 0,0에 있는 검은색과 0,1에 있는 흰색입니다.
        _cell.Add(new Vector2(0, 0));
        _cell.Add(new Vector2(0, 1));

        

        while (true)
        {
            temp = path.GetPositionByT(t) - transform.position;
            temp2 = path.GetPositionByT(t+0.1f) - transform.position;
            
			if (t + 0.1 > path.points.Count - 1) {
				temp = path.GetPositionByT (t - 0.1f) - transform.position;
				temp2 = path.GetPositionByT (t) - transform.position;
			}

			anglevector = temp2 - temp;
			up = Vector3.Normalize(v3Rotation * anglevector)*0.5f;
			down = Vector3.Normalize(r_v3Rotation * anglevector)*0.5f;




            _Vertex.Add(temp +up);
            _Vertex.Add(temp + down);

            t += 0.1f;
            if (t > path.points.Count - 1)
                break;
        }
        for(int i=0;i<(int)t*10-1;++i)
        {
            _Tri.Add(2*i+0);
            _Tri.Add(2 * i+2);
            _Tri.Add(2 * i+1);
            _Tri.Add(2 * i+2);
            _Tri.Add(2 * i+3);
            _Tri.Add(2 * i+1);

        }

		_cellGap = (1.0f/(float)_Vertex.Count);
        for (int i = 0; i < _Vertex.Count/4; ++i)
        {
			_UV.Add(new Vector2(_cellGap * _cell[1].x + _cellGap * i*4 , _cellGap * _cell[1].y + _cellGap+ _cellGap * i*4));
			_UV.Add(new Vector2(_cellGap * _cell[1].x + _cellGap+ _cellGap * i*4 , _cellGap * _cell[1].y + _cellGap+ _cellGap * i*4));
			_UV.Add(new Vector2(_cellGap * _cell[1].x + _cellGap+ _cellGap * i*4 , _cellGap * _cell[1].y+ _cellGap * i*4));
			_UV.Add(new Vector2(_cellGap * _cell[1].x+ _cellGap * i*4   , _cellGap * _cell[1].y+ _cellGap * i*4));
        }
        mesh.Clear();
        mesh.vertices = _Vertex.ToArray();
        mesh.triangles = _Tri.ToArray();
        //UV값을 넣습니다.
        mesh.uv = _UV.ToArray();        
        mesh.RecalculateNormals();
    }
	
}

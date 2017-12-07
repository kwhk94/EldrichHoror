using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class chracter_number : NetworkBehaviour
{
    public List<GameObject> chracter_name;
    public string chracter_name_str;
    [SyncVar(hook = "OnChangeName")]
    public int current_number ;
    public int max_number ;

    public Text m_text;

   
    public void Awake()
    {
        max_number = chracter_name.Count-1;
        //캐릭터리스트 이름추가, 첫이름 넣기        
        chracter_name_str = chracter_name[0].name;
    }


    public void change_number()
    {
        if (!isServer)
            return;

        ++current_number;
        if (current_number > max_number)
        {
            current_number = 0;
        }
    }

    void OnChangeName(int current_number)
    {
        if (current_number > max_number)
        {
            current_number = 0;
        }
        m_text.text = chracter_name[current_number].name;
    }


}

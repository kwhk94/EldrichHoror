using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : Photon.PunBehaviour,IPunObservable {
    public static PlayerUI instance;
    public List<Sprite> imagelist;
    public Image hp;
    public Image mp;
    public Sprite[] hplist;
    public Sprite[] mplist;
    public int hpnumber;
    public int mpnumber;
    private RectTransform rectTransform;
	private PhotonView photonview;
	private Image image;


	void Start () {
        instance = this;
        SettingComponent ();
		//플레이어번호와 캐릭터 번호,이름을 넘긴다
		if (photonview.isMine) {
			int num = LobbyPlayerlist.Instance.current_player_number;
			int chracter_num = LobbyPlayerlist.Instance.current_chracter_number;
			string playername = PhotonNetwork.player.NickName;
			photonview.RPC ("SetPlayerUI", PhotonTargets.All, num, chracter_num, playername);
            hpnumber = Player.instance.playerdata.hp;
            hp.sprite = hplist[hpnumber-1];
            mpnumber = Player.instance.playerdata.mp;
            mp.sprite = mplist[mpnumber-1];
        }
	}

	private void SettingComponent()
	{
		photonview = GetComponent<PhotonView> ();		
		image = GetComponent<Image> ();
		rectTransform = GetComponent<RectTransform> ();
	}


	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
            stream.SendNext(this.hpnumber);
            stream.SendNext(this.mpnumber);
        }
		else
		{
            int temp;
            temp = (int)stream.ReceiveNext();
            if(temp == 0)
                hp.sprite = null;
            else if (this.hpnumber != temp)
            {
                this.hpnumber = temp;
                hp.sprite = hplist[temp-1];
            }
            temp = (int)stream.ReceiveNext();
            if (temp == 0)
                mp.sprite = null;
            else if (this.mpnumber != temp)
            {
                this.mpnumber = temp;
                mp.sprite = mplist[temp-1];
            }
        }
	}



	[PunRPC]
	public void SetPlayerUI(int num,int chracternum,string playername)
	{
		if (!image)
			SettingComponent ();
		transform.parent = GameObject.Find ("Panel").transform;
		image.sprite = imagelist [chracternum];
		GetComponentInChildren<Text>().text = playername;
		rectTransform.anchorMax = new Vector2 (0.05f, 0.1f*num+0.4f);
		rectTransform.anchorMin = new Vector2 (0.0f, 0.1f*num+0.3f);
		rectTransform.pivot = new Vector2 (0.5f, 0.5f);
		rectTransform.offsetMin = new Vector2(0.0f, 0.0f);
		rectTransform.offsetMax = new Vector2(-0.0f, -0.0f);

	}
}

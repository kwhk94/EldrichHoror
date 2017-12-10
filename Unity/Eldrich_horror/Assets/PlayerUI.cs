using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : Photon.PunBehaviour,IPunObservable {
	public List<Sprite> imagelist;
	private PhotonView photonview;
	private Image image;
	void Start () {
		photonview = GetComponent<PhotonView> ();		
		image = GetComponent<Image> ();
		//플레이어번호와 캐릭터 번호,이름을 넘긴다
		if (photonview.isMine) {
			int num = LobbyPlayerlist.Instance.current_player_number;
			int chracter_num = LobbyPlayerlist.Instance.current_chracter_number;
			string playername = PhotonNetwork.player.NickName;
			photonview.RPC ("SetPlayerUI", PhotonTargets.All, num, chracter_num, playername);
		}

	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{			
		}
		else
		{			
		}
	}



	[PunRPC]
	public void SetPlayerUI(int num,int chracternum,string playername)
	{
		transform.parent = GameObject.Find ("Panel").transform;

		GetComponent<Image>().sprite = imagelist [chracternum];
		GetComponent<RectTransform> ().position += new Vector3 (0, -80 * num, 0);
		GetComponentInChildren<Text> ().text = playername;
	}
}

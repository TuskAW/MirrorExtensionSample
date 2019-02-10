// ========================================================
// ModifiedNetworkDiscovery.cs

// Reference:
// http://motoyama.hateblo.jp/entry/unet-networkdiscovery
// ========================================================
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;

namespace MirrorExtension
{
    public class ModifiedNetworkDiscovery : NetworkDiscovery
    {
        // ブロードキャスト受信時に呼ばれる関数
        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            Mirror.NetworkManager manager = Mirror.NetworkManager.singleton;

            // 既にサーバーまたはクライアントとしてネットワーク接続済みの場合は何もしない
            if (manager.isNetworkActive){ return; }

            // ブロードキャストの送信元IPアドレスを接続先としてセット
            manager.networkAddress = fromAddress.Replace("::ffff:", "");

            // クライアントとして起動する
            manager.StartClient();

            // ブロードキャストの受信をやめる
            StartCoroutine(StopBroadcastCoroutine());
        }

        // ブロードキャストの受信をやめる。
        // OnReceivedBroadcast内でStopBroadcast()を呼ぶと、エラーになってしまうため、
        // 次のフレームで呼ばれるように、コルーチンを使っている。
        IEnumerator StopBroadcastCoroutine()
        {
            yield return new WaitForEndOfFrame();

            // ブロードキャストの送受信をやめる
            StopBroadcast();
        }
    }
}
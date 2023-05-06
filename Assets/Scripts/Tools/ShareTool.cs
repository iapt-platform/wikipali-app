using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using cn.sharesdk.unity3d;
//using LitJson;
public class ShareTool : MonoBehaviour
{

//    //public GUISkin demoSkin;
//    public ShareSDK ssdk;

//    public MobSDK mobsdk;

//    // Use this for initialization
//    void Start()
//    {
//        ssdk = gameObject.GetComponent<ShareSDK>();
//        ssdk.authHandler = OnAuthResultHandler;
//        ssdk.shareHandler = OnShareResultHandler;
//        ssdk.showUserHandler = OnGetUserInfoResultHandler;
//        ssdk.getFriendsHandler = OnGetFriendsResultHandler;
//        ssdk.followFriendHandler = OnFollowFriendResultHandler;
//        mobsdk = gameObject.GetComponent<MobSDK>();
//#if UNITY_ANDROID
//        ShareSDKRestoreScene.setRestoreSceneListener(OnRestoreScene);

//#elif UNITY_IPHONE
//		mobsdk.getPolicy = OnFollowGetPolicy;
//        ssdk.wxRequestHandler = GetWXRequestTokenResultHandler;
//        ShareSDKRestoreScene.setRestoreSceneListener(OnRestoreScene);
//#endif

//    }
//    public void Share()
//    {
//        ShareContent content = new ShareContent();
//        //Facebook分享图片
//        ShareContent fbShareParams = new ShareContent();
//        fbShareParams.SetText("text");
//        fbShareParams.SetImageUrl("http://git.oschina.net/alexyu.yxj/MyTmpFiles/raw/master/kmk_pic_fld/small/107.JPG");
//        fbShareParams.setFacebookShareType(FacebookShareType.Native);
//        fbShareParams.SetShareType(ContentType.Image);
//        content.SetShareContentCustomize(PlatformType.Facebook, fbShareParams);


//        //Instagram分享图片
//        ShareContent insShareParams = new ShareContent();
//        insShareParams.SetImageUrl("http://git.oschina.net/alexyu.yxj/MyTmpFiles/raw/master/kmk_pic_fld/small/107.JPG");
//        insShareParams.SetMenuX(0);
//        insShareParams.SetMenuY(0);
//        content.SetShareContentCustomize(PlatformType.Instagram, insShareParams);
//        string imgFilePath = "";
//        //微信好友
//        ShareContent wechatFriend = new ShareContent();
//        wechatFriend.SetImagePath(imgFilePath);
//        content.SetShareContentCustomize(PlatformType.WeChat, wechatFriend);
//        ssdk.ShareContent(PlatformType.WeChat, content);
//        //微信朋友圈
//        ShareContent wechatMoments = new ShareContent();
//        wechatMoments.SetImagePath(imgFilePath);
//        content.SetShareContentCustomize(PlatformType.WeChatMoments, wechatMoments);
//        //QQ好友
//        ShareContent qqFriend = new ShareContent();
//        qqFriend.SetImagePath(imgFilePath);
//        content.SetShareContentCustomize(PlatformType.QQ, qqFriend);
//        //QQ空间
//        ShareContent qZone = new ShareContent();
//        qZone.SetImagePath(imgFilePath);
//        content.SetShareContentCustomize(PlatformType.QZone, qZone);
//        //新浪微博
//        ShareContent sinaWeibo = new ShareContent();
//        sinaWeibo.SetImagePath(imgFilePath);
//        content.SetShareContentCustomize(PlatformType.SinaWeibo, sinaWeibo);
//        //??豆瓣

//    }

//    public void CommonShare(PlatformType platform, string imgFilePath)
//    {
//        return;
//        ShareContent content = new ShareContent();
//        content.SetImagePath(imgFilePath);
//        content.SetShareContentCustomize(platform, content);
//        ssdk.ShareContent(platform, content);
//    }
//    void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
//    {
//        if (state == ResponseState.Success)
//        {
//            if (result != null && result.Count > 0)
//            {
//                print("authorize success !" + "Platform :" + type + "result:" + MiniJSON.jsonEncode(result));
//            }
//            else
//            {
//                print("authorize success !" + "Platform :" + type);
//            }
//        }
//        else if (state == ResponseState.Fail)
//        {
//#if UNITY_ANDROID
//            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//#endif
//        }
//        else if (state == ResponseState.Cancel)
//        {
//            print("cancel !");
//        }
//    }

//    void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
//    {
//        if (state == ResponseState.Success)
//        {
//            print("get user info result :");
//            print(MiniJSON.jsonEncode(result));
//            print("AuthInfo:" + MiniJSON.jsonEncode(ssdk.GetAuthInfo(type)));
//            print("Get userInfo success !Platform :" + type);
//        }
//        else if (state == ResponseState.Fail)
//        {
//#if UNITY_ANDROID
//            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//#endif
//        }
//        else if (state == ResponseState.Cancel)
//        {
//            print("cancel !");
//        }
//    }

//    void OnShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
//    {
//        if (state == ResponseState.Success)
//        {
//            print("share successfully - share result :");
//            print(MiniJSON.jsonEncode(result));
//        }
//        else if (state == ResponseState.Fail)
//        {
//#if UNITY_ANDROID
//            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//#endif
//        }
//        else if (state == ResponseState.Cancel)
//        {
//            print("cancel !");
//        }
//    }

//    void OnGetFriendsResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
//    {
//        if (state == ResponseState.Success)
//        {
//            print("get friend list result :");
//            print(MiniJSON.jsonEncode(result));
//        }
//        else if (state == ResponseState.Fail)
//        {
//#if UNITY_ANDROID
//            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//#endif
//        }
//        else if (state == ResponseState.Cancel)
//        {
//            print("cancel !");
//        }
//    }

//    void OnFollowFriendResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
//    {
//        if (state == ResponseState.Success)
//        {
//            print("Follow friend successfully !");
//        }
//        else if (state == ResponseState.Fail)
//        {
//#if UNITY_ANDROID
//            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//#endif
//        }
//        else if (state == ResponseState.Cancel)
//        {
//            print("cancel !");
//        }
//    }

//    public static void OnRestoreScene(RestoreSceneInfo scene)
//    {
//        Hashtable customParams = scene.customParams;
//        if (customParams != null)
//        {
//            Debug.Log("[sharesdk-unity-Demo]OnRestoreScen(). path:" + scene.path.ToString() + ", params:" + scene.customParams.toJson());
//        }
//        else
//        {
//            Debug.Log("[sharesdk-unity-Demo]OnRestoreScen(). path:" + scene.path.ToString() + ", params:null");
//        }

//        //根据scene开发者自己处理场景转换
//        //SceneManager.LoadScene("SceneA");
//    }


//#if UNITY_IPHONE

//    public static void GetWXRequestTokenResultHandler(String authcode, sendWXRequestToken send)
//    {
//        Debug.Log("[GetWXRequestTokenResultHandler:" + authcode);
//        send("11", "22");
//    }

//    public static void GetWXRefreshTokenResultHandler(String uid, sendWXRefreshToken send)
//    {
//        send("11");
//    }
//#endif
//    //隐私协议回调
//    public static void OnFollowGetPolicy(string url)
//    {
//        Debug.Log("[OnFollowGetPolicy:" + url);
//    }
}

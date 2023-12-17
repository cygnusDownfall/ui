using System.Threading.Tasks;
using TMPro;
using Unity.Services.Vivox;
using Unity.Services.Vivox.AudioTaps;
using UnityEngine;
public class ChatSystem : SingletonPersistent<ChatSystem>
{

    public string UserDisplayName = "";
    [SerializeField] GameObject rosterItemTemplate;
    public Transform rosterItemContainer;
    [SerializeField] GameObject textChatContentTemplate;
    public Transform textChatContentContainer;
    public TMP_InputField inputTypingText;
    #region lifeCycleVivox
    public async Task InitializeAsync()
    {
        await VivoxService.Instance.InitializeAsync();
    }
    public async Task LoginToVivoxAsync()
    {
        LoginOptions options = new LoginOptions();
        options.DisplayName = UserDisplayName;
        options.EnableTTS = true;
        await VivoxService.Instance.LoginAsync(options);
    }
    public async Task JoinEchoChannelAsync(string chanelName = "Lobby")
    {
        try
        {
            chanelName = UIFunctionSystem.Instance.joincode;
        }
        catch
        {
            Debug.Log("khong the load joincode tu UI");
        }
        string channelToJoin = chanelName;
        await VivoxService.Instance.JoinEchoChannelAsync(channelToJoin, ChatCapability.TextAndAudio);
    }

    public async Task LeaveEchoChannelAsync(string chanelName = "Lobby")
    {
        try
        {
            chanelName = UIFunctionSystem.Instance.joincode;
        }
        catch
        {
            Debug.Log("khong the load joincode tu UI");
        }
        string channelToLeave = chanelName;
        await VivoxService.Instance.LeaveChannelAsync(channelToLeave);
    }
    public void LogoutOfVivox()
    {
        _ = VivoxService.Instance.LogoutAsync();
    }


    private void SendMessage(string channelName, string message)
    {
        _ = VivoxService.Instance.SendChannelTextMessageAsync(channelName, message);
    }

    private void BindSessionEvents(bool doBind)
    {
        VivoxService.Instance.ChannelMessageReceived += onChannelMessageReceived;
        if (doBind)
        {
            VivoxService.Instance.ParticipantAddedToChannel += onParticipantAddedToChannel;
            VivoxService.Instance.ParticipantRemovedFromChannel += onParticipantRemovedFromChannel;
        }
        else
        {
            VivoxService.Instance.ParticipantAddedToChannel -= onParticipantAddedToChannel;
            VivoxService.Instance.ParticipantRemovedFromChannel -= onParticipantRemovedFromChannel;
        }
    }

    private void onChannelMessageReceived(VivoxMessage message)
    {
        string messageText = message.MessageText;
        string senderID = message.SenderPlayerId;
        string senderDisplayName = message.SenderDisplayName;
        string messageChannel = message.ChannelName;
        HandleUIMessage((messageText, messageChannel, senderID, senderDisplayName));
    }



    #endregion

    #region inChannelAction
    private void onParticipantAddedToChannel(VivoxParticipant participant)
    {
        ///RosterItem is a class intended to store the participant object, and reflect events relating to it into the game's UI.
        ///It is a sample of one way to use these events, and is detailed just below this snippet.
        //RosterItem newRosterItem = new RosterItem();

    }

    private void onParticipantRemovedFromChannel(VivoxParticipant participant)
    {

    }

    bool mute = false;
    public void toogleMuteSeft()
    {
        if (mute)
            VivoxService.Instance.UnmuteInputDevice();
        else
            VivoxService.Instance.MuteInputDevice();
        mute = !mute;
    }
    #endregion
    /// <summary>
    /// xu ly tin nhan nahn duoc len UI 
    /// </summary>
    /// <param name="message">(messageText,messageChannel,senderID,senderDisplayName)</param>
    void HandleUIMessage((string, string, string, string) message)
    {
        addMessageUI(message.Item4, message.Item1);
    }

    /// <summary>
    /// for local and remote message
    /// </summary>
    /// <param name="author"></param>
    /// <param name="message"></param>
    public void addMessageUI(string author, string message)
    {
        var textChatContentTF = Instantiate(textChatContentTemplate, textChatContentContainer).transform;
        textChatContentTF.Find("Name").GetComponent<TMP_Text>().text = author;
        textChatContentTF.Find("Value").GetComponent<TMP_Text>().text = message;
    }
    #region main
    public void SendMessage()
    {

        SendMessage(UIFunctionSystem.Instance.joincode, inputTypingText.text);
    }
    public async Task startSystem()
    {
        Debug.Log("init vivox");
        await InitializeAsync();
        Debug.Log("init done vivox");
        await LoginToVivoxAsync();
        BindSessionEvents(true);
    }
    public async void endSystem()
    {
        await LeaveEchoChannelAsync();
        LogoutOfVivox();
        BindSessionEvents(false);
    }
    #endregion

    #region mono
    private void Start()
    {
        gameObject.AddComponent<VivoxChannelAudioTap>();
    }
    #endregion
}
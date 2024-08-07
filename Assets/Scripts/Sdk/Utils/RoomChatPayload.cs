using System;
using System.Collections;
using System.Collections.Generic;

public class Messages
{
    public string channel_id;
    public int code;
    public string content;
    public DateTime create_time;
    public string message_id;
    public bool persistent;
    public string room_name;
    public string sender_id;
    public DateTime update_time;
    public string username;

}

// payload template for room's chat
public class RoomChatPayload
{
    public string cacheable_cursor;
    public IList<Messages> messages;

}
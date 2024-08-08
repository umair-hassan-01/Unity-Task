using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class LeaderBoardData
{
    public List<Record> records;
    public List<object> ownerRecords;
    public object nextCursor;
    public object prevCursor;
    public int rankCount;
}

[Serializable]
public class Metadata
{
    public string username;
}

[Serializable]
public class Record
{
    public int subscore;
    public int maxNumScore;
    public Metadata metadata;
    public int updateTime;
    public string leaderboardId;
    public string ownerId;
    public string username;
    public int score;
    public int numScore;
    public int createTime;
    public object expiryTime;
    public int rank;
}


// leaderboard data payload
[Serializable]
public class Payload
{
    public bool success;
    public LeaderBoardData data;
    public string message;
}


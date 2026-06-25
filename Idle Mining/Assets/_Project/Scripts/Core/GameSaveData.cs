using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public double Gold = 0;
    public string LastPlayTime = "";

    public List<string> MineIds = new List<string>();
    public List<int> MineLevels = new List<int>();
}

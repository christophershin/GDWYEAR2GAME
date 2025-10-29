using System.Drawing;
using UnityEngine;

public class EntitiesClass: MonoBehaviour
{

    private string teamID;


    void Start()
    {
        teamID = this.gameObject.ToString();
    }

    


    public string TeamID()
    {
        return teamID;
    }

    public void SetTeamID(string id)
    {
        teamID = id;
    }

}

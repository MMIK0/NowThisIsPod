using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceController : MonoBehaviour
{

    private GameManagerScript GMS;
    public int lapsInRace;
    public Text LapInfoText;
    public Text CheckpointInfoText;
    public Text RaceOverText;

    private int nextCheckpointNumber;
    private int checkpointCount;
    private int lapCount;
    private float lapStartTime;
    private bool isRaceActive;
    // Laptimes get stored in a list
    // Kierrosajat tallennetaan listaan
    private List<float> lapTimes = new List<float>();
    private Checkpoint activeCheckpoint;

    
    void Start()
    {  
        StartCoroutine(CountDown(4.20));
    }
     IEnumerator CountDown(double seconds) {

        double count = seconds;
        while (count > 0) { 
         lapStartTime = Time.time;
        yield return new WaitForSeconds(1);
        count --;
        }
        isRaceActive = true; 
        nextCheckpointNumber = 0;
        lapCount = 0;
        checkpointCount = this.transform.childCount;
        
        // Assign each of the checkpoints its own number in order in Hierarchy
        // Annetaan jokaiselle tarkastuspisteelle sen järjestysnumero hierarkian järjestyksen mukaan
        
         
        for (int i = 0; i < checkpointCount; i++)
        {
            Checkpoint cp = transform.GetChild(i).GetComponent<Checkpoint>();
            cp.checkpointNumber = i;
            cp.isActiveCheckpoint = false;
        }
    
     
     
           StartRace();
      
    }
     
         
    // Update is called once per frame
    void Update()
    {
    
        if(isRaceActive) {
        
            
            LapInfoText.text = TimeParser(Time.time - lapStartTime);
            CheckpointInfoText.text = ("CHECKPOINT " + (nextCheckpointNumber + 1) + " / " + checkpointCount + "\nLAP " + (lapCount + 1) + " / " + lapsInRace);
        }
        else
        {
            LapInfoText.text = "";
            CheckpointInfoText.text = "";
            RaceOverText.color = Color.HSVToRGB(Mathf.Abs(Mathf.Sin(Time.time)), 1, 1);
        }
    }

    public void StartRace()
    {
        
            
         
        activeCheckpoint = transform.GetChild(nextCheckpointNumber).GetComponent<Checkpoint>();
        activeCheckpoint.isActiveCheckpoint = true;
        lapStartTime = Time.time;
        }
    

    public void CheckpointPassed()
    {
         
            
         
        activeCheckpoint.isActiveCheckpoint = false;
        nextCheckpointNumber++;
        if (nextCheckpointNumber < checkpointCount)
        {
            activeCheckpoint = transform.GetChild(nextCheckpointNumber).GetComponent<Checkpoint>();
            activeCheckpoint.isActiveCheckpoint = true;
        }
        // If a lap was finished, we enter the new lap, and the checkpoint-counter is reset
        // Jos kierros loppui, mennään seuraavalle kierrokselle ja nollataan tarkastuspistelaskuri
        else
        {
            // Add the laptime to the list of laptimes
            // Lisätään kierrosaika kierrosaikojen listaan
            lapTimes.Add(Time.time - lapStartTime);
            lapCount++;
            // Reset the lap timer
            // Nollataan kierrosaika
            lapStartTime = Time.time;
            nextCheckpointNumber = 0;
            // If the finished lap wasn't the last lap
            // Jos päätetty kierros ei ollut viimeinen kierros
            if (lapCount < lapsInRace)
            {
                activeCheckpoint = transform.GetChild(nextCheckpointNumber).GetComponent<Checkpoint>();
                activeCheckpoint.isActiveCheckpoint = true;
            }
            // If final lap, end the game and calculate results
            // Jos viimeinen kierros, päätä peli ja laske tulokset
            else
            {
                isRaceActive = false;
                float raceTotalTime = 0.0f;
                float fastestLapTime = lapTimes[0];
                for (int i = 0; i < lapsInRace; i++)
                {
                    // Compare the laptimes to pick fastest
                    // Vertaa kierrosaikoja nopeimman löytämiseksi
                    if (lapTimes[i] < fastestLapTime)
                    {
                        fastestLapTime = lapTimes[i];
                    }
                    // Count total time
                    // Laske kokonaisaika
                    raceTotalTime += lapTimes[i];
                }
                RaceOverText.text = "RACE COMPLETE!\n\nTotal Time:" + TimeParser(raceTotalTime) + "\nBest Lap: " + TimeParser(fastestLapTime);
            }
        }
    }

    public string TimeParser(float time)
    {
       
        float minutes = Mathf.Floor((time) / 60);
        float seconds = Mathf.Floor((time) % 60);
        float msecs = Mathf.Floor(((time) * 100) % 100);

        return (minutes.ToString() + ":" + seconds.ToString("00") + ":" + msecs.ToString("00"));

         }
}

        


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSImulation : MonoBehaviour
{
    public static ControllerSImulation Instance;
    public normalparticle[] normalparticles = new normalparticle[13];
    public DisplayGame simulationWater;
    public DisplayGame1 simulationFire;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        normalparticles[0] = new normalparticle("Water",false);
        normalparticles[1] = new normalparticle("Fire",false);
        normalparticles[2] = new normalparticle("Vine",false);
        normalparticles[3] = new normalparticle("Lightning", false);
        normalparticles[4] = new normalparticle("FlammableGases", false);
        normalparticles[5] = new normalparticle("Eraser", false);
        normalparticles[6] = new normalparticle("Sunshine", false);
        normalparticles[7] = new normalparticle("Seed", false);
        normalparticles[8] = new normalparticle("Virus", false);
        normalparticles[9] = new normalparticle("Alcohol", false);
        normalparticles[10] = new normalparticle("Blackhole", false);
        normalparticles[11] = new normalparticle("Nuclearbomb", false);
        normalparticles[12] = new normalparticle("Oxy", false);
     
    }
 
    private void Update()
    {
     
       // neu water duoc bat
       foreach (var particle in normalparticles)
        {
            if (particle.statusOn)
            {
                // chay ham mo phong doi tuong
                CheckSimulationWhichOn(particle.name);
            }
        }
    }
   void CheckSimulationWhichOn(string name)
    {
        switch (name)
        {
            case "Water":
                simulationWater.OnUpdate();
                break;
            case "Fire":
                simulationFire.OnUpdate();
                break;
            case "Vine": 
                
                break;
            case "Lightning":
                
                break;
            case "FlammableGases": 
                
                break;
            case "Eraser": 
                
                break;
            case "Sunshine":
                
                break;
            case "Seed":
                
                break;
            case "Virus":
                
                break;
            case "Alcohol":
                
                break;
            case "Blackhole":
                
                break;
            case "Nuclearbomb": 
                
                break;
            case "Oxy":
                
                break;

        }
      
    }

}
public struct normalparticle
{
   public string name;
   public bool statusOn;
    public normalparticle(string thisName, bool thisStatusOn)
    {
        name = thisName;
        statusOn = thisStatusOn;
    }

}
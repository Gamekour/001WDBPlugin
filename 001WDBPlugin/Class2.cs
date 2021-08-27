using System;
using Exiled.API.Interfaces;
using System.ComponentModel;

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;

    [Description("Controls the chance of a SCP 001 round. Set to 0 to disable and 100 to always have an SCP 001 round.")]
    public float Chance { get; set; } = 10f;

    [Description("Controls the chance of replacing a regular spawn wave with zombies during an SCP 001 round")]
    public float ZombieChance { get; set; } = 10f;

    [Description("Controls whether or not CI can survive on surface without becoming zombies")]
    public bool CISuit { get; set; } = false;

    [Description("Controls whether or to override spawn ticket system")]
    public bool OverrideTickets { get; set; } = true;

}

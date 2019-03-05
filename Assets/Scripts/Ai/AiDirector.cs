using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDirector {
    public bool fully_rotated = false;
    public bool hunting = false;
    public bool hunted = false;
    public bool capture = false;
    public bool returning = false;
    public bool in_contact = false;
    public bool freeze = false;
    public bool saving = false;
    public bool beingSaved = false;
    public float last_recorded_velocity;
}
